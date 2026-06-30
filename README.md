# ChillPay Operation Logs API

Standalone .NET 8 Web API for the central **Chillpay Operation Log Registry** (Module **1–14**, Merchant=**2**, Settings=**3**) in the shared ChillPay SQL Server database.

## Architecture

```
chillpay-operation-logs-api/
├── src/dotnet8/
│   ├── ChillPay.OperationLogsApi-NET8.sln
│   ├── ChillPay.Core/
│   │   ├── Authentications/                 # ApiKey auth (แบบ payout)
│   │   ├── Constants/OperationLogs/
│   │   ├── Domains/Data/                      # OperationLogsDbContext + IDbContext
│   │   ├── Domains/Entities/OperationLogs/
│   │   ├── Domains/Repositories/              # GenericRepository + OperationLogs/
│   │   ├── Models/OperationLogs/
│   │   └── Services/OperationLogs/
│   └── ChillPay.OperationLogs.Api/
│       ├── Controllers/V1/                    # BaseController + ChillpayOperationLogController
│       └── Helpers/AutoMapperProfile.cs
└── docs/Chillpay-Operation-Logs.md   # เอกสาร + SQL script §7.4
```

Layering: **Controller (V1) → Service → GenericRepository → DbContext** — สอดคล้อง `chillpay-payout-api-dev`

### Database

| Table | View | Deploy script |
|-------|------|---------------|
| `ChillpayOperationLogs` (+ `MerchantId`) | `VW_ChillpayOperationLogs` (`SELECT b.*` + `RefTypeText`) | [`Table`](docs/sql/ChillpayOperationLogs-Table.sql) · [`Index`](docs/sql/ChillpayOperationLogs-Index.sql) · [`View`](docs/sql/ChillpayOperationLogs-View.sql) · [`Deploy`](docs/sql/ChillpayOperationLogs-Deploy.sql) |

Search filter ร้านค้า: `merchantId[]` — แบบเดียวกับ PayOut (ดู [§5.6](docs/Chillpay-Operation-Logs.md#56-เปรียบเทียบ-merchantid-กับ-payout))

### API routes

| Purpose | Route prefix |
|---------|--------------|
| Registry read/write | `api/v1/chillpayoperationlogs/*` |

All endpoints require the `CHILLPAY-TOKEN` header configured in `WebAppSetting`.

## Running locally

```bash
cd src/dotnet8
dotnet build
dotnet run --project ChillPay.OperationLogs.Api
```

- Swagger: http://localhost:8135/swagger
- Health check: http://localhost:8135/healthz

## CI/CD (GitLab)

โครงสร้างตาม `payout-api` / `merchant-api`:

| ไฟล์ | หน้าที่ |
|------|--------|
| `.gitlab-ci.yml` | variables + stages |
| `git-templates/sit-gitlab-ci.yml` | SIT build/deploy |
| `git-templates/sandbox-gitlab-ci.yml` | Sandbox build/deploy |
| `git-templates/prod-gitlab-ci.yml` | Beta + Prod build/deploy |
| `deploy/configs/{env}/` | `appsettings` + `web.config` ต่อ env |

### Tag สำหรับ deploy

| Env | Tag ตัวอย่าง |
|-----|-------------|
| SIT | `sit-v1.0.0` |
| Sandbox | `sandbox-v1.0.0` |
| Beta / Prod | `prod-v1.0.0` |

### Runner variables (ตั้งบน GitLab Runner)

- `NUGET_PACKAGES_DIRECTORY`, `NUGET_CONFIG_PATH`
- `APPPOOLNAME_NET8_LB1`, `IIS_WEBSITE_NAME_LB1`
- `DEPLOY_FOLDER_NET8_LB1_SIT`, `DEPLOY_FOLDER_NET8_LB1_SANDBOX`, `DEPLOY_FOLDER_NET8_LB1_BETA`, `DEPLOY_FOLDER_NET8_LB1_PROD`
- Env var บน server: `OPERATION_LOGS_CHILLPAY_TOKEN`, connection string ตาม `ConnectionStrings.SqlDbConnection`

Runner tag: `chillpay-operation-logs-api` (+ `sit2` / `sandbox` / `prod`)

### Publish profile

CI ใช้ `Properties/PublishProfiles/DEPLOY.pubxml` (`win-x64`, output `deploy/publish`) ผ่าน:

```bash
dotnet publish .\src\dotnet8\ChillPay.OperationLogs.Api\ChillPay.OperationLogs.Api.csproj -c Release -o .\deploy\publish /p:PublishProfile=DEPLOY.pubxml
```

### Git repository

```powershell
cd D:\chillpay-operation-logs-api
git init
git remote add origin <gitlab-repo-url>
git add .
git commit -m "Initial commit: ChillPay Operation Logs API"
git push -u origin main
```

สร้างโปรเจกต์บน GitLab ก่อน แล้วตั้ง runner tag + CI variables ตามตารางด้านบน

## Chillpay Operation Logs — เอกสาร

เอกสารหลัก (registry, search, UI, writer, **SQL script §7.4**): [docs/Chillpay-Operation-Logs.md](docs/Chillpay-Operation-Logs.md)

| Artifact | Path |
|----------|------|
| เอกสารรวม + SQL script | `docs/Chillpay-Operation-Logs.md` (§7.4) |
| C# constants | `ChillPay.Core/Constants/OperationLogs/OperationLogConstants.cs` — enum + validate + `AllModuleTypes` |
| API | `api/v1/chillpayoperationlogs/*` |
