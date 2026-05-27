# Deploy Azure
## prepare:
+ install azd CLI
+ from root folder run:

```bash
azd init

```
+ deploy with command:

```bash
azd up
```
## Notice 
+ the web frontend need to update `appsettings.Production.json` API URL
+ the web frontend should deploy under static web via docker file


# CI/CD with github:
- chạy lệnh bên đưới để lấy azure credentials, sau đó dán vào secret của github repository
  - Name: AZURE_CREDENTIALS
  - Value: output của lệnh bên dưới
az ad sp create-for-rbac --name "github-actions-rsa" --role contributor --scopes /subscriptions/33ec68a6-22a4-4ae4-84da-fa8a82cc7694/resourceGroups/rg-blazor-app-to-azure --sdk-auth

- cấp quyền đọc cho github có thể deploy:
az role assignment create \
  --assignee "tên-hoặc-appId-của-github-actions-rsa" \
  --role "Reader" \
  --scope "/subscriptions/33ec68a6-22a4-4ae4-84da-fa8a82cc7694"

  