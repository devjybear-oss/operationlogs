# ChillPay Operation Logs API

Standalone .NET 8 Web API for the central **Chillpay Operation Log Registry** (Module **1–14**, Merchant=**2**, Settings=**3**) in the shared ChillPay SQL Server database.

**เว็บ docs (GitHub Pages):** https://devjybear-oss.github.io/operationlogs/

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
└── docs/Chillpay-Operation-Logs.md   # เอกสาร + SQL script
```

Layering: **Controller (V1) → Service → GenericRepository → DbContext** — สอดคล้อง `chillpay-payout-api-dev`

### Database

| Table | View | Deploy script |
|-------|------|---------------|
| `ChillpayOperationLogs` (+ `MerchantId`) | `VW_ChillpayOperationLogs` | [`Table`](docs/sql/ChillpayOperationLogs-Table.sql) · [`Index`](docs/sql/ChillpayOperationLogs-Index.sql) · [`View`](docs/sql/ChillpayOperationLogs-View.sql) · [`Deploy`](docs/sql/ChillpayOperationLogs-Deploy.sql) |

Search filter ร้านค้า: `merchantId[]` — แบบเดียวกับ PayOut

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

Runner tag: `chillpay-operation-logs-api` (+ `sit2` / `sandbox` / `prod`)

## เอกสาร

| Artifact | Path |
|----------|------|
| เอกสารหลัก (source) | `docs/Chillpay-Operation-Logs.md` |
| Doc site (build) | `python docs/build-site.py` → `docs/site/` |
| GitHub Pages (root) | `index.html`, `registry.html`, … |
| C# constants | `src/dotnet8/ChillPay.Core/Constants/OperationLogs/OperationLogConstants.cs` |
