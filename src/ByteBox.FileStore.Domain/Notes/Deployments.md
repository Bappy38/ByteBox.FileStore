
# Azure Deployment

- Create a resource group (e.g. "bytebox")
- Create an azure sql instance
- Create an app service plan
- Create an app service
- 

---

### **Steps to Generate Azure Credentials from the Azure Portal**

#### **1. Create a Service Principal in Azure AD**
1. Go to the [Azure portal](https://portal.azure.com/).
2. In the left-hand menu, select **Azure Active Directory**.
3. Under **Manage**, select **App registrations**.
4. Click **+ New registration**.
5. Fill in the details:
   - **Name**: Provide a name for the app registration (e.g., `github-actions-sp`).
   - **Supported account types**: Select **Accounts in this organizational directory only** (single tenant).
   - Leave the **Redirect URI** field blank.
6. Click **Register**.

---

#### **2. Create a Client Secret**
1. After creating the app registration, go to the **Certificates & secrets** section.
2. Under **Client secrets**, click **+ New client secret**.
3. Provide a description (e.g., `github-actions-secret`) and choose an expiration period.
4. Click **Add**.
5. Copy the **client secret value** (you won’t be able to retrieve it later).

---

#### **3. Assign a Role to the Service Principal**
1. Go to the **Subscriptions** section in the Azure portal.
2. Select the subscription where your App Service is located.
3. Under **Access control (IAM)**, click **+ Add** and select **Add role assignment**.
4. Assign the **Contributor** role:
   - **Role**: Contributor.
   - **Assign access to**: User, group, or service principal.
   - **Select**: Search for the app registration name (e.g., `github-actions-sp`) and select it.
5. Click **Save**.

---

#### **4. Gather Required Information**
You’ll need the following details to configure GitHub Actions:
- **Client ID**: Found in the **Overview** section of the app registration.
- **Client Secret**: The value you copied when creating the client secret.
- **Tenant ID**: Found in the **Overview** section of Azure Active Directory.
- **Subscription ID**: Found in the **Subscriptions** section of the Azure portal.

---

#### **5. Format the Azure Credentials**
Combine the information into a JSON object like this:
```json
{
  "clientId": "<client-id>",
  "clientSecret": "<client-secret>",
  "subscriptionId": "<subscription-id>",
  "tenantId": "<tenant-id>",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

---

#### **6. Add Azure Credentials to GitHub Secrets**
1. Go to your GitHub repository.
2. Navigate to **Settings > Secrets and variables > Actions**.
3. Click **New repository secret**.
4. Name the secret `AZURE_CREDENTIALS`.
5. Paste the JSON object into the **Value** field.
6. Click **Add secret**.

---

### **Use the Credentials in GitHub Actions**
Now you can use the `AZURE_CREDENTIALS` secret in your GitHub Actions workflow. Here’s an example:

```yaml
name: Deploy to Azure App Service

on:
  push:
    branches:
      - main

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
      # Check out the repository
      - name: Checkout code
        uses: actions/checkout@v3

      # Set up .NET (or your runtime)
      - name: Set up .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '6.0.x'  # Replace with your .NET version

      # Build the project
      - name: Build project
        run: dotnet build --configuration Release

      # Publish the project
      - name: Publish project
        run: dotnet publish -c Release -o ./publish

      # Deploy to Azure App Service
      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: '<your-app-service-name>'  # Replace with your App Service name
          slot-name: 'production'  # Optional: Specify a deployment slot
          publish-profile: ${{ secrets.AZURE_CREDENTIALS }}
```

---
