
# Azure Deployment

- Create a resource group (e.g. "bytebox")
- Create an azure sql instance
- Create an app service plan
- Create an app service
- Go to Settings > Configuration and enable `SCM Basic Auth Publishing Credentials` and `FTP Basic Auth Publishing Credentials`. Then set `FTP State` as `All Allowed`
- Download app service's publish profile and set it as repository secrets
- Set `dotnet /home/site/wwwroot/ByteBox.FileStore.API.dll` as startup command
- Set all environment variables
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

# AWS Deployment

- Add Dockerfile in the project
- Go to `ECR` and create a repository
- Install AWS CLI
- Authenticate your docker client to your registry by the command given in ECR repository
- Go to the root folder of the project. For this project it is `ByteBox.FileStore` folder
- Build the docker image by the command `docker build -t bytebox-filestore . -f src/ByteBox.FileStore.API/Dockerfile` from the root folder
- Tag the image by the command `docker tag bytebox-filestore:latest <aws-account-id>.dkr.ecr.<region>.amazonaws.com/bytebox-filestore:latest`
- Push the image by the command `docker push <aws-account-id>.dkr.ecr.<region>.amazonaws.com/bytebox-filestore:latest`
- Go to `ECS` and create a cluster with name `ByteBoxFileStoreCluster` and keep rest of the things as default
- Now create a task definition with name `ByteBoxFileStoreTaskDef`. Map port as per you mentioned in Dockerfile. And add all the environment variables.
- Go to your created Cluster again. And run the task definition you created.
- Go to `Networking` tab of your task. Go to security group. And add a new inbound rule with port you mentioned in Dockerfile so that your public IP can be accessed from anywhere.
