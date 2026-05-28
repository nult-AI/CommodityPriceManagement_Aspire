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
- chạy lệnh bên dưới tự động thực hiện theo: 
azd pipeline config

- tới step authenticate: chọn
SP + OIDC

- 