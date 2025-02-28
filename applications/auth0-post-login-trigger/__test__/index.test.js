const { onExecutePostLogin } = require("../src");

const mockAppMetadataUpdate = jest.fn().mockReturnValue(true);

function getMockApiProp() {
    return {
        user: {
            setAppMetadata: mockAppMetadataUpdate
        }
    }
}

function getMockEventProp() {
    return {
        secrets: {
            DYNAMICS_CLIENT_ID: 'dynamics_client_id',
            DYNAMICS_CLIENT_SECRET: 'dynamics_client_secret',
            DYNAMICS_RESOURCE_URL: 'https://dynamics_resource_url',
            DYNAMICS_TENANT_ID: 'dynamics_tenant_id'
        },
        user: {
            email: 'email_address',
            app_metadata: {
                dynamics_contact_id: ''
            },
            user_id: 'user_id'
        }
    }
}

function determineResponse(happyMessage, unhappyMessage, requestCount, totalTransientErrors) {
    if (totalTransientErrors === 0) {
        return Promise.resolve({
            ok: true,
            json: () => Promise.resolve(happyMessage),
        })
    }
    if (requestCount <= totalTransientErrors) {
        return Promise.resolve({
            ok: false,
            status: 403,
            text: () => Promise.resolve(unhappyMessage)
        })
    }
    return Promise.resolve({
        ok: true,
        json: () => Promise.resolve(happyMessage),
    })
}

function mockFetchSuccess() {
    global.fetch = jest.fn().mockImplementation((url, _args, _headers) => {
        if (url.includes('/v9.2/v1_CreateContactWithAuth0')) {
            return Promise.resolve({
                ok: true,
                json: () => Promise.resolve({
                    contactId: 'a_contact_id' 
                }),
            })
        }
        return Promise.resolve({
            ok: true,
            json: () => Promise.resolve({
                access_token: 'token' 
            }),
        })
    })
}

function mockFetchTransientErrors(numberOfTransientTokenErrors = 0, numberOfTransientContactErrors = 0) {
    let numberOfTokenRequests = 0;
    let numberOfContactRequests = 0;

    global.fetch = jest.fn().mockImplementation((url, _args, _headers) => {
        if (url.includes('/v9.2/v1_CreateContactWithAuth0')) {
            numberOfContactRequests++ 

            const contactSuccessPath = {
                contactId: 'a_contact_id' 
            } 
            const contactErrorPath = 'failed to create contact.'

            return determineResponse(
                contactSuccessPath, 
                contactErrorPath, 
                numberOfContactRequests, 
                numberOfTransientContactErrors
            )
        }
        numberOfTokenRequests++

        const tokenHappyPath = {
            access_token: 'token' 
        }
        const tokenUnhappyPath = 'failed to create token.'

        return determineResponse(
            tokenHappyPath, 
            tokenUnhappyPath, 
            numberOfTokenRequests, 
            numberOfTransientTokenErrors
        )
    })
}

function mockFetchBadRequest(isBadTokenRequest, isBadContactRequest) {
    const tokenStatusCode = isBadTokenRequest ? 400 : 200
    const contactStatusCode = isBadContactRequest ? 400 :200

    global.fetch = jest.fn().mockImplementation((url, _args, _headers) => {
        if (url.includes('/v9.2/v1_CreateContactWithAuth0')) {
            return Promise.resolve({
                ok: !isBadContactRequest,
                status: contactStatusCode,
                text: () => Promise.resolve('unhappy.'),
                json: () => Promise.resolve({
                    contactId: 'a_contact_id' 
                }),
            })
        }
        return Promise.resolve({
            ok: !isBadTokenRequest,
            status: tokenStatusCode,
            text: () => Promise.resolve('unhappy.'),
            json: () => Promise.resolve({
                access_token: 'token' 
            }),
        })
    })
}

function mockFetchBadResponse(isTokenBad, isContactBad) {
    global.fetch = jest.fn().mockImplementation((url, _args, _headers) => {
        if (url.includes('/v9.2/v1_CreateContactWithAuth0')) {
            const contactResponse = isContactBad ? {
                notAContactId: 'contactId'
            } : {
                contactId: 'a_contact_id' 
            } 
            return Promise.resolve({
                ok: true,
                status: 200,
                json: () => Promise.resolve(contactResponse),
            })
        }

        const tokenResponse = isTokenBad ? { notAToken: 'token' } : {
            access_token: 'token' 
        }
        return Promise.resolve({
            ok: true,
            status: 200,
            json: () => Promise.resolve(tokenResponse),
        })
    })
}

describe("Post Login Action", () => {
    beforeEach(() => {
        jest.clearAllMocks()
    })

    describe('Validate Event' , () => {
        test.each([
            [undefined, "defined"],
            ["defined", undefined],
            [undefined, undefined]
        ])("stops execution when email is %s and auth0id is %s", async (userId, email) => {
            mockFetchSuccess()
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            const mutatedEvent = {
                ...mEvent,
                user: {
                    ...mEvent.user,
                    user_id: userId,
                    email
                }
            }
    
            await onExecutePostLogin(mutatedEvent, mApi)

            expect(fetch).toHaveBeenCalledTimes(0)
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(0)
        })
    
        test.each([
            [undefined, undefined, undefined, undefined],
            ["defined", undefined, undefined, undefined],
            [undefined, "defined", undefined, undefined],
            [undefined, undefined, "defined", undefined],
            [undefined, undefined, undefined, "defined"],
            ["defined", "defined", undefined, undefined],
            ["defined", "defined", "defined", undefined],
            [undefined, "defined", "defined", undefined],
            [undefined, "defined", "defined", "defined"],
            [undefined, undefined, "defined", "defined"],
            ["defined", undefined, "defined", "defined"],
            ["defined", undefined, undefined, "defined"],
            [undefined, "defined", "defined", undefined],
            [undefined, "defined", undefined, "defined"],
            ["defined", "defined", undefined, "defined"],
            ["defined", undefined, "defined", "defined"],
        ])("stops execution when dynamics (clientId: %s, clientSecret: %s, resourceUrl: %s, tenantId: %s)", async (clientId, clientSecret, resourceUrl, tenantId) => {
            mockFetchSuccess()
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            const mutatedEvent = {
                ...mEvent,
                secrets: {
                    ...mEvent.secrets,
                    DYNAMICS_CLIENT_ID: clientId,
                    DYNAMICS_CLIENT_SECRET: clientSecret,
                    DYNAMICS_RESOURCE_URL: resourceUrl,
                    DYNAMICS_TENANT_ID: tenantId
                }
            }
    
            await onExecutePostLogin(mutatedEvent, mApi)

            expect(fetch).toHaveBeenCalledTimes(0)
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(0)
        })
    })

    describe('Error Handling', () => { 
        test("stops execution when unable to generate dynamics token (receives recurrent non-400/200 status codes 3 times)", async () => {
            mockFetchTransientErrors(3)
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            await onExecutePostLogin(mEvent, mApi)
    
            expect(fetch).toHaveBeenCalledTimes(3)
            expect(fetch).toHaveBeenNthCalledWith(
                1,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                2,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                3,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
    
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(0)
        })
    
        test("stops execution when unable to create contact (receives recurrent non-400/200 status codes 3 times)", async () => {
            mockFetchTransientErrors(0,3)
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            await onExecutePostLogin(mEvent, mApi)
    
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(0);
            expect(fetch).toHaveBeenCalledTimes(4)
            expect(fetch).toHaveBeenNthCalledWith(
                1,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                2,
                `${mEvent.secrets.DYNAMICS_RESOURCE_URL}/api/data/v9.2/v1_CreateContactWithAuth0`, 
                {
                    method: 'POST',
                    body: JSON.stringify({
                        auth0Id: mEvent.user.user_id,
                        email: mEvent.user.email,
                    }),
                    headers: {
                        Authorization: 'Bearer token',
                        'Content-Type': 'application/json'
                    }
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                3,
                `${mEvent.secrets.DYNAMICS_RESOURCE_URL}/api/data/v9.2/v1_CreateContactWithAuth0`, 
                {
                    method: 'POST',
                    body: JSON.stringify({
                        auth0Id: mEvent.user.user_id,
                        email: mEvent.user.email,
                    }),
                    headers: {
                        Authorization: 'Bearer token',
                        'Content-Type': 'application/json'
                    }
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                4,
                `${mEvent.secrets.DYNAMICS_RESOURCE_URL}/api/data/v9.2/v1_CreateContactWithAuth0`, 
                {
                    method: 'POST',
                    body: JSON.stringify({
                        auth0Id: mEvent.user.user_id,
                        email: mEvent.user.email,
                    }),
                    headers: {
                        Authorization: 'Bearer token',
                        'Content-Type': 'application/json'
                    }
                }
            )
        })
    
        test('stops execution and does not retry 400 error when generating token', async () => {
            mockFetchBadRequest(true, false)
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            await onExecutePostLogin(mEvent, mApi)
    
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(0)
    
            expect(fetch).toHaveBeenCalledTimes(1)
            expect(fetch).toHaveBeenNthCalledWith(
                1,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
        })
    
        test('stops execution and does not retry 400 error when creating contact', async () => {
            mockFetchBadRequest(false, true)
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            await onExecutePostLogin(mEvent, mApi)
    
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(0)
            expect(fetch).toHaveBeenCalledTimes(2)
            expect(fetch).toHaveBeenNthCalledWith(
                1,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                2,
                `${mEvent.secrets.DYNAMICS_RESOURCE_URL}/api/data/v9.2/v1_CreateContactWithAuth0`, 
                {
                    method: 'POST',
                    body: JSON.stringify({
                        auth0Id: mEvent.user.user_id,
                        email: mEvent.user.email,
                    }),
                    headers: {
                        Authorization: 'Bearer token',
                        'Content-Type': 'application/json'
                    }
                }
            )
        })
    
        test('retries transient errors when generating token (non-400/200 error that occurs less than 3 times)', async () => {
            mockFetchTransientErrors(1,0)
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            await onExecutePostLogin(mEvent, mApi)
    
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(1)
    
            expect(fetch).toHaveBeenCalledTimes(3)
            expect(fetch).toHaveBeenNthCalledWith(
                1,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                2,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                3,
                `${mEvent.secrets.DYNAMICS_RESOURCE_URL}/api/data/v9.2/v1_CreateContactWithAuth0`, 
                {
                    method: 'POST',
                    body: JSON.stringify({
                        auth0Id: mEvent.user.user_id,
                        email: mEvent.user.email,
                    }),
                    headers: {
                        Authorization: 'Bearer token',
                        'Content-Type': 'application/json'
                    }
                }
            )
        })
    
        test('retries transient errors when creating contact (non-400/200 error that occurs less than 3 times)', async () => {
            mockFetchTransientErrors(0,1)
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            await onExecutePostLogin(mEvent, mApi)
    
            expect(fetch).toHaveBeenCalledTimes(3)
            expect(fetch).toHaveBeenNthCalledWith(
                1,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                2,
                `${mEvent.secrets.DYNAMICS_RESOURCE_URL}/api/data/v9.2/v1_CreateContactWithAuth0`, 
                {
                    method: 'POST',
                    body: JSON.stringify({
                        auth0Id: mEvent.user.user_id,
                        email: mEvent.user.email,
                    }),
                    headers: {
                        Authorization: 'Bearer token',
                        'Content-Type': 'application/json'
                    }
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                3,
                `${mEvent.secrets.DYNAMICS_RESOURCE_URL}/api/data/v9.2/v1_CreateContactWithAuth0`, 
                {
                    method: 'POST',
                    body: JSON.stringify({
                        auth0Id: mEvent.user.user_id,
                        email: mEvent.user.email,
                    }),
                    headers: {
                        Authorization: 'Bearer token',
                        'Content-Type': 'application/json'
                    }
                }
            )
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(1)
        })
    
        test('stops execution if contact id not returned from dynamics when creating contact', async () => {
            mockFetchBadResponse(false,true)
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            await onExecutePostLogin(mEvent,mApi)
    
            expect(fetch).toHaveBeenCalledTimes(2)
            expect(fetch).toHaveBeenNthCalledWith(
                1,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
            expect(fetch).toHaveBeenNthCalledWith(
                2,
                `${mEvent.secrets.DYNAMICS_RESOURCE_URL}/api/data/v9.2/v1_CreateContactWithAuth0`, 
                {
                    method: 'POST',
                    body: JSON.stringify({
                        auth0Id: mEvent.user.user_id,
                        email: mEvent.user.email,
                    }),
                    headers: {
                        Authorization: 'Bearer token',
                        'Content-Type': 'application/json'
                    }
                }
            )
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(0)
        })

        test('stops execution if token not returned when generating token', async () => {
            mockFetchBadResponse(true,false)
            const mEvent = getMockEventProp()
            const mApi = getMockApiProp()
    
            await onExecutePostLogin(mEvent,mApi)
    
            expect(fetch).toHaveBeenCalledTimes(1)
            expect(fetch).toHaveBeenNthCalledWith(
                1,
                `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                    method: 'POST'
    
                }
            )
            expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(0)
        })
    })

    test("creates dynamics token with correct request payload", async () => {
        mockFetchSuccess()
        const mEvent = getMockEventProp()
        const mApi = getMockApiProp()

        await onExecutePostLogin(mEvent, mApi);

        expect(fetch).toHaveBeenNthCalledWith(
            1,
            `https://login.microsoftonline.com/${mEvent.secrets.DYNAMICS_TENANT_ID}/oauth2/token`,
            {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body:`client_id=${mEvent.secrets.DYNAMICS_CLIENT_ID}&client_secret=${mEvent.secrets.DYNAMICS_CLIENT_SECRET}&grant_type=client_credentials&resource=${encodeURIComponent(mEvent.secrets.DYNAMICS_RESOURCE_URL)}`,
                method: 'POST'

            }
        )
    })

    test('does not call dynamics or update user if event has dynamics_contact_id present', async () => {
        mockFetchSuccess()
        const mEvent = getMockEventProp()
        const mApi = getMockApiProp()

        mEvent.user.app_metadata.dynamics_contact_id = 'VALUE'

        await onExecutePostLogin(mEvent,mApi)

        expect(fetch).toHaveBeenCalledTimes(0)
        expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(0)
    })

    test("calls dynamics with correct request payload", async () => {
        mockFetchSuccess()
        const mEvent = getMockEventProp()
        const mApi = getMockApiProp()

        await onExecutePostLogin(mEvent, mApi);

        expect(fetch).toHaveBeenCalledTimes(2)
        expect(fetch).toHaveBeenNthCalledWith(
            2,
            `${mEvent.secrets.DYNAMICS_RESOURCE_URL}/api/data/v9.2/v1_CreateContactWithAuth0`, 
            {
                method: 'POST',
                body: JSON.stringify({
                    auth0Id: mEvent.user.user_id,
                    email: mEvent.user.email,
                }),
                headers: {
                    Authorization: 'Bearer token',
                    'Content-Type': 'application/json'
                }
            }
        )
    })

    test("calls metadata update function with correct parameters", async () => {
        mockFetchSuccess()
        const mEvent = getMockEventProp()
        const mApi = getMockApiProp()

        await onExecutePostLogin(mEvent, mApi);

        expect(mockAppMetadataUpdate).toHaveBeenCalledTimes(1)
        expect(mockAppMetadataUpdate).toHaveBeenCalledWith('dynamics_contact_id', 'a_contact_id');
    })
})