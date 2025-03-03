
# Azure Deployment

- Create a resource group (e.g. "bytebox")
- Create an azure sql instance
- Create an app service plan
- Create an app service
- Go to Settings > Configuration and enable `SCM Basic Auth Publishing Credentials` and `FTP Basic Auth Publishing Credentials`. Then set `FTP State` as `All Allowed`
- Download app service's publish profile and set it as repository secrets
- Implement deployment pipeline which will use this publish profile for deployment

```yml

name: Deploy to Azure App Service (Linux)

on:
  push:
    branches:
      - main

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v3

      - name: Set up .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'

      - name: Build project
        run: dotnet build --configuration Release

      - name: Publish project
        run: dotnet publish -c Release -o ./publish

      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: 'ByteBox-FileStore'
          slot-name: 'production'
          publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}

```
