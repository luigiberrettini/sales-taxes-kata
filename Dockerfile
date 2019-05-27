FROM mcr.microsoft.com/dotnet/core/sdk
RUN apt-get update && \
    apt-get install -y --no-install-recommends jq && \
    rm -rf /var/lib/apt/lists/*