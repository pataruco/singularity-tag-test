const qs = require('querystring')

exports.onExecutePostLogin = async (event, api) => {
    try {
        const { user, secrets } = validateEvent(event);

        if (user.contactId) {
            console.log('Contact has already been created in Dynamics.')
            return;
        }
    
        const token = await getDynamicsAccessToken(secrets.dynamics)
    
        const contactId = await createDynamicsContact(secrets.dynamics.resourceUrl, user, token)
    
        console.log('Updating user metadata in Auth0...')
    
        api.user.setAppMetadata('dynamics_contact_id', contactId)
        
        console.log('Updated user metadata in Auth0...', { contactId });
    }
    catch (err) {
        console.error('Error occurred whilst processing login event...', err)
    }
};

function validateEvent(event) {
    const dynamicsClientId = event.secrets?.DYNAMICS_CLIENT_ID;
    const dynamicsClientSecret = event.secrets?.DYNAMICS_CLIENT_SECRET;
    const dynamicsResource = event.secrets?.DYNAMICS_RESOURCE_URL;
    const dynamicsTenantId = event.secrets?.DYNAMICS_TENANT_ID;

    if (!dynamicsClientId || !dynamicsClientSecret || !dynamicsResource || !dynamicsTenantId) {
        throw new Error('Dynamics secrets not configured.')
    }

    // Extract user information
    const email = event.user.email;
    const contactId = event.user.app_metadata?.dynamics_contact_id
    const auth0Id = event.user.user_id;

    if (!email || !auth0Id) {
        throw new Error('Missing required user fields: email or user ID');
    }

    return {
        secrets: {
            dynamics: {
                clientId: dynamicsClientId,
                clientSecret: dynamicsClientSecret,
                resourceUrl: dynamicsResource,
                tenantId: dynamicsTenantId
            },
        },
        user: {
            email,
            auth0Id,
            contactId
        }
    }
}

/**
 * @param { string } dynamicsResource - Base URL for dynamics instance to be connecting to. 
 * @param {{ email: string, auth0Id: string, contactId?: string}} userDetails - User details from Auth0.
 * @param { string } token - Token for authenticating with Dynamics. 
*/
async function createDynamicsContact(dynamicsResource, userDetails, token) {
    const {
        email,
        auth0Id
    } = userDetails;

    console.log("Creating contact in Dynamics...")

    const dynamicsUrl = `${dynamicsResource}/api/data/v9.2/v1_CreateContactWithAuth0`;
    const response = await fetchRetry(
        dynamicsUrl,
        {
            method: 'POST',
            headers: {
                Authorization: `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ auth0Id, email })
        }
    )

    if (!response.contactId) {
        console.error('No contact ID returned by Dynamics');
        throw new Error('Unexpected response from Dynamics, no contact ID returned.')
    }

    console.log('Created contact in Dynamics...');
    return response.contactId;
}

/**
 * @param {{tenantId: string, clientId: string, clientSecret: string, resourceUrl: string}} dynamicsSecrets - Dynamics secrets received in event.
*/
const getDynamicsAccessToken = async (dynamicsSecrets) => {
    const {
        clientId,
        clientSecret,
        tenantId,
        resourceUrl
    } = dynamicsSecrets;

    console.log("Getting Dynamics 365 token...")

    const tokenUrl = `https://login.microsoftonline.com/${tenantId}/oauth2/token`;
    const requestBody = qs.stringify({
        client_id: clientId,
        client_secret: clientSecret,
        grant_type: 'client_credentials',
        resource: resourceUrl
    });

    const response = await fetchRetry(tokenUrl, {
        method: 'POST',
        body: requestBody,
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
    })

    if (!response.access_token) {
        throw new Error("Access token not in response.")
    }

    console.log('Received Dynamics 365 token...');
    return response.access_token;
};

function wait(delay) {
    return new Promise((resolve) => setTimeout(resolve, delay));
}

async function fetchRetry(url, options, delayMs = 100, retryAttempts = 3) {
    const res = await fetch(url, options)

    if (res.ok) {
        return await res.json()
    }

    if (res.status === 400) {
        console.error(`Received 400 from ${url}, exiting...`)
        const badRequestErr = await res.text()
        throw new Error(badRequestErr)
    }

    const triesLeft = retryAttempts - 1;

    if (!triesLeft) {
        console.error(`Exhausted retry attempts, last recevied ${res.status} from ${url}, exiting...`)
        const failedRequestRes = await res.text()
        throw new Error(failedRequestRes)
    }

    console.warn(`Received ${res.status} from ${url}, attempting again in ${delayMs}ms...`)

    await wait(delayMs);
    return await fetchRetry(url, options, delayMs, triesLeft)
}