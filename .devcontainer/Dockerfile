FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN apt-get update \
    && apt-get install -y zip \
    && dotnet new -i Amazon.Lambda.Templates \
    && dotnet tool install -g Amazon.Lambda.Tools \
    && apt-get -y clean \
    && rm -rf /var/lib/apt/lists/* \
    && echo "export PATH=\$PATH:/root/.dotnet/tools" >> ~/.bash_profile \
    && . ~/.bash_profile