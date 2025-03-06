# base
FROM ubuntu:latest


# set the github runner version
ARG RUNNER_VERSION="2.319.1"

# update the base packages and add a non-sudo user
RUN apt-get update -y && apt-get upgrade -y && useradd -m docker

ARG DEBIAN_FRONTEND="noninteractive"

# install base dependencies
RUN apt-get install -y --no-install-recommends \
  curl jq build-essential libssl-dev unzip git software-properties-common

# Add microsoft repo for Linux packages
RUN add-apt-repository ppa:dotnet/backports

# add dotnet toolkit and runtime
RUN apt-get install -y dotnet-sdk-9.0 aspnetcore-runtime-9.0 dotnet-runtime-9.0
RUN dotnet dev-certs https --trust

# install Azure CLI
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

# install aks cli
RUN az aks install-cli

# insall helm
RUN curl -fsSL -o get_helm.sh https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3 &&  \
  chmod 700 get_helm.sh && \
  ./get_helm.sh

# cd into the user directory, download and unzip the github actions runner
RUN cd /home/docker && mkdir actions-runner && cd actions-runner \
  && curl -O -L https://github.com/actions/runner/releases/download/v${RUNNER_VERSION}/actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz \
  && tar xzf ./actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz

# install some additional dependencies
RUN chown -R docker ~docker && /home/docker/actions-runner/bin/installdependencies.sh

# copy over the start.sh script
COPY start.sh start.sh

# make the script executable
RUN chmod +x start.sh

# since the config and run script for actions are not allowed to be run by root,
# set the user to "docker" so all subsequent commands are run as the docker user
USER docker

# set the entrypoint to the start.sh script
ENTRYPOINT ["./start.sh"]
