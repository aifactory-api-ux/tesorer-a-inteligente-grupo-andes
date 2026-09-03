# DEVELOPMENT PLAN: Tesorería Inteligente Grupo Andes

## 1. ARCHITECTURE OVERVIEW

**Project:** Tesorería Inteligente Grupo Andes - Plataforma centralizada para gestión de tesorería corporativa

### Components
- **Backend:** ASP.NET Core 8.0 con Entity Framework Core 8.x + Npgsql
- **Database:** Azure Database for PostgreSQL Flexible Server
- **Frontend:** React 18 + TypeScript + Vite
- **Authentication:** Azure AD SSO via Microsoft Identity Platform
- **Storage:** Azure Storage Accounts (Blob para cartolas bancarias)
- **Deployment:** Azure App Service con Azure DevOps CI/CD

### Data Model
- **Users:** Gestión de usuarios con roles (analista, gerente, cfo, auditor, admin)
- **Subsidiaries:** Filiales de Grupo Andes Capital
- **BankAccounts:** Cuentas bancarias por filial
- **BankStatements:** Cartolas bancarias con estados de importación
- **BankStatementLines:** Movimientos individuales de cartolas
- **ExpectedCollections:** Cobros esperados
- **PaymentRequests:** Solicitudes de pago con workflow de aprobación multinivel
- **ApprovalHistory:** Historial de aprobaciones
- **CashFlowProjections:** Proyecciones de caja 30/60/90 días
- **AuditLogs:** Trazabilidad completa

### API Structure
- `/api/auth/*` - Autenticación Azure AD
- `/api/subsidiaries/*` - Gestión de filiales
- `/api/bank-accounts/*` - Cuentas bancarias
- `/api/bank-statements/*` - Cartolas y movimientos
- `/api/expected-collections/*` - Cobros esperados
- `/api/payment-requests/*` - Solicitudes de pago
- `/api/cash-flow/*` - Proyecciones de flujo de caja
- `/api/audit/*` - Logs de auditoría

### Folder Structure
```
project-root/
├── backend/
│   ├── src/
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Models/
│   │   ├── DTOs/
│   │   ├── Data/
│   │   └── Program.cs
│   ├── Tests/
│   └── Dockerfile
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── hooks/
│   │   ├── services/
│   │   ├── styles/
│   │   └── App.tsx
│   └── Dockerfile
└── docker-compose.yml
```

---

## 2. ACCEPTANCE CRITERIA

1. **Usuario puede autenticarse via Azure AD SSO** - Login funcional con Azure AD, redirección correcta, obtención de token JWT
2. **Dashboard muestra flujo de caja consolidado** - Vista agregada de saldos por filial, indicadores de conciliación, alertas de proyección
3. **Conciliación bancaria funciona** - Carga de cartolas (CSV), auto-conciliación de movimientos, revisión de diferencias
4. **Workflow de aprobación multinivel opera** - Solicitudes de pago fluyen por estados (pending → pending_approval_gerente → pending_approval_cfo → approved/rejected)
5. **Proyecciones de caja calculan** - Proyecciones automáticas a 30/60/90 días con alertas de liquidez negativa
6. **Trazabilidad completa** - Logs de auditoría registran todas las acciones de usuarios
7. **Despliegue zero-touch** - `./run.sh` levanta toda la plataforma sin pasos manuales

---

## 3. EXECUTABLE ITEMS

### ITEM 1: Foundation — shared types, interfaces, DB schema, configuration
**Goal:** Create all shared code that other items will import including TypeScript types, C# models, database schema, and environment validation.

**Files to create:**
- backend/src/Models/Entities/*.cs (create) - Entity Framework Core entities: User, Subsidiary, BankAccount, BankStatement, BankStatementLine, ExpectedCollection, PaymentRequest, ApprovalHistory, CashFlowProjection, AuditLog
- backend/src/Models/Enums/*.cs (create) - Enums: UserRole, AccountType, StatementImportStatus, CollectionStatus, PaymentRequestStatus, ApprovalAction, ProjectionDays
- backend/src/DTOs/*.cs (create) - DTOs for API requests/responses
- backend/src/Data/AppDbContext.cs (create) - Entity Framework DbContext configuration
- backend/src/Data/Migrations/*.cs (create) - EF Core migrations for PostgreSQL
- backend/src/Configuration/*.cs (create) - Configuration classes for Azure AD, Key Vault, Storage
- backend/src/appsettings.json (create) - App settings template
- frontend/src/types/index.ts (create) - TypeScript interfaces matching C# entities
- frontend/src/types/api.ts (create) - API response types
- frontend/src/config/index.ts (create) - Environment configuration
- frontend/src/styles/tokens.ts (create) - Design tokens from UI/UX contract (colors, typography, spacing, radii, shadows)
- frontend/src/styles/global.css (create) - Global styles with design tokens
- docs/architecture.md (create) - System architecture documentation

**Dependencies:** None

**Validation:** 
- `dotnet build` compiles backend without errors
- `npm run build` compiles frontend without TypeScript errors
- Database migrations can be generated

**Role:** role-tl (technical_lead)

---

### ITEM 2: Backend — Authentication & User Management (Azure AD SSO)
**Goal:** Implement Azure AD authentication, user role management, and subsidiary access control.

**Files to create:**
- backend/src/Controllers/AuthController.cs (create) - Azure AD login, token validation, user info endpoint
- backend/src/Controllers/UsersController.cs (create) - CRUD for users, role assignment
- backend/src/Controllers/SubsidiariesController.cs (create) - CRUD for subsidiaries
- backend/src/Services/AuthService.cs (create) - Azure AD token validation, user claims extraction
- backend/src/Services/UserService.cs (create) - User management business logic
- backend/src/Services/SubsidiaryService.cs (create) - Subsidiary business logic
- backend/src/Middleware/RoleAuthorizationMiddleware.cs (create) - Role-based authorization
- backend/src/Program.cs (create) - Configure Azure AD, JWT, EF Core, CORS
- backend/src/Properties/launchSettings.json (create) - Development launch settings

**Dependencies:** Item 1

**Validation:**
- Azure AD authentication flow works (redirect, token, user creation)
- Role-based authorization blocks unauthorized endpoints
- API returns proper 401/403 for unauthenticated/unauthorized requests

**Role:** role-be (backend_developer)

---

### ITEM 3: Backend — Bank Accounts & Statements (Cartolas)
**Goal:** Implement bank account management and bank statement (cartola) import with line processing.

**Files to create:**
- backend/src/Controllers/BankAccountsController.cs (create) - CRUD for bank accounts
- backend/src/Controllers/BankStatementsController.cs (create) - Upload cartolas, list statements, get lines
- backend/src/Services/BankAccountService.cs (create) - Bank account business logic
- backend/src/Services/BankStatementService.cs (create) - Cartola import, parsing CSV/OFX, line extraction
- backend/src/Services/BlobStorageService.cs (create) - Azure Blob Storage for cartola files
- backend/src/Validators/BankStatementValidator.cs (create) - FluentValidation for statement import

**Dependencies:** Item 1

**Validation:**
- Bank accounts can be created, listed, filtered by subsidiary
- Cartolas can be uploaded via multipart/form-data
- Statement lines are parsed and stored correctly
- File is saved to Azure Blob Storage

**Role:** role-be (backend_developer)

---

### ITEM 4: Backend — Bank Reconciliation
**Goal:** Implement automatic and manual bank reconciliation with difference detection.

**Files to create:**
- backend/src/Controllers/ReconciliationController.cs (create) - Reconciliation operations, match/unmatch
- backend/src/Services/ReconciliationService.cs (create) - Auto-conciliation algorithm, match logic
- backend/src/Models/DTOs/ReconciliationDTOs.cs (create) - DTOs for reconciliation operations

**Dependencies:** Item 1, Item 3

**Validation:**
- Auto-conciliation matches transactions by amount + date + reference
- Manual reconciliation allows user to link unmatched items
- Reconciliation status updates correctly (is_reconciled flag)
- Difference detection identifies unmatched credits/debits

**Role:** role-be (backend_developer)

---

### ITEM 5: Backend — Payment Requests & Cash Flow
**Goal:** Implement payment request workflow with multi-level approval and cash flow projections.

**Files to create:**
- backend/src/Controllers/PaymentRequestsController.cs (create) - CRUD, approval actions
- backend/src/Controllers/ExpectedCollectionsController.cs (create) - CRUD for expected collections
- backend/src/Controllers/CashFlowController.cs (create) - Projections 30/60/90 days
- backend/src/Services/PaymentRequestService.cs (create) - Approval workflow, status transitions
- backend/src/Services/ExpectedCollectionService.cs (create) - Collection management
- backend/src/Services/CashFlowProjectionService.cs (create) - Projection calculations
- backend/src/Services/AuditService.cs (create) - Audit log creation

**Dependencies:** Item 1, Item 2

**Validation:**
- Payment request approval flows through correct status transitions
- Cash flow projections calculate correctly based on collections + payments
- All actions create audit log entries
- Projections generate alerts for negative liquidity scenarios

**Role:** role-be (backend_developer)

---

### ITEM 6: Frontend — Foundation (Tokens, Layout, Routing, Base Components)
**Goal:** Implement design system tokens, main layout, routing, and reusable base components.

**Files to create:**
- frontend/src/styles/tokens.ts (create) - Design tokens: colors, typography, spacing, radii, shadows (verbatim from UIUX contract)
- frontend/src/styles/global.css (create) - Global CSS with design tokens
- frontend/src/components/layout/MainLayout.tsx (create) - Main layout with sidebar navigation
- frontend/src/components/layout/Topbar.tsx (create) - Top bar with brand, subsidiary selector, profile
- frontend/src/components/ui/Button.tsx (create) - Primary, secondary, positive, negative button variants
- frontend/src/components/ui/Card.tsx (create) - Card container component
- frontend/src/components/ui/Table.tsx (create) - Data table with sortable headers
- frontend/src/components/ui/Input.tsx (create) - Form input with validation
- frontend/src/components/ui/Select.tsx (create) - Dropdown select component
- frontend/src/components/ui/Modal.tsx (create) - Modal dialog component
- frontend/src/components/ui/Alert.tsx (create) - Alert banner component
- frontend/src/components/ui/FilterBar.tsx (create) - Filter and search controls
- frontend/src/components/ui/SubsidiarySelector.tsx (create) - Global subsidiary/group selector
- frontend/src/App.tsx (create) - Main app with React Router setup
- frontend/src/main.tsx (create) - Entry point
- frontend/src/api/client.ts (create) - Axios client with auth interceptor

**Dependencies:** Item 1

**Validation:**
- Design tokens match UIUX contract exactly
- Layout renders with sidebar, topbar, content area
- All base components render with proper styling
- Router navigates between pages

**Role:** role-fe (frontend_developer)

---

### ITEM 7: Frontend — Authentication Flow (Login Azure AD)
**Goal:** Implement Azure AD SSO login flow and protected route handling.

**Files to create:**
- frontend/src/pages/Login.tsx (create) - Login page with Azure AD button (matches Figma Login frame)
- frontend/src/pages/Callback.tsx (create) - OAuth callback handler
- frontend/src/hooks/useAuth.ts (create) - Auth context and token management
- frontend/src/contexts/AuthContext.tsx (create) - Auth provider with Azure AD
- frontend/src/components/auth/ProtectedRoute.tsx (create) - Route guard component
- frontend/src/config/auth.ts (create) - Azure AD configuration

**Dependencies:** Item 1, Item 6

**Validation:**
- Login page displays Azure AD login button
- Clicking redirects to Azure AD
- Callback processes token and stores in context
- Protected routes redirect to login if not authenticated

**Role:** role-fe (frontend_developer)

---

### ITEM 8: Frontend — Dashboard Consolidado de Flujo de Caja
**Goal:** Implement cash flow dashboard with consolidated view, metrics cards, and projection charts.

**Files to create:**
- frontend/src/pages/Dashboard.tsx (create) - Main dashboard page (matches Figma Dashboard frame)
- frontend/src/components/dashboard/SummaryCards.tsx (create) - KPI cards: saldo total, ingresos, egresos
- frontend/src/components/dashboard/SubsidiaryBreakdown.tsx (create) - Per-subsidiary breakdown table
- frontend/src/components/dashboard/ProjectionChart.tsx (create) - 30/60/90 day projection chart (Recharts)
- frontend/src/components/dashboard/ReconciliationStatus.tsx (create) - Reconciliation status indicators
- frontend/src/components/dashboard/Alerts.tsx (create) - Alerts for negative projections, pending approvals
- frontend/src/services/dashboardApi.ts (create) - API calls for dashboard data

**Dependencies:** Item 1, Item 6, Item 7

**Validation:**
- Dashboard loads with all KPI cards populated
- Projection chart displays 30/60/90 day forecast
- Subsidiary breakdown shows per-filial totals
- Alerts appear for negative liquidity scenarios

**Role:** role-fe (frontend_developer)

---

### ITEM 9: Frontend — Conciliación Bancaria Page
**Goal:** Implement bank reconciliation page with statement upload, movement matching, and difference review.

**Files to create:**
- frontend/src/pages/Reconciliation.tsx (create) - Reconciliation page (matches Figma Conciliación frame)
- frontend/src/components/reconciliation/StatementUploader.tsx (create) - File upload for cartolas
- frontend/src/components/reconciliation/MovementTable.tsx (create) - Table with bank statement lines
- frontend/src/components/reconciliation/MatchControls.tsx (create) - Manual matching UI
- frontend/src/components/reconciliation/DifferencePanel.tsx (create) - Unmatched items panel
- frontend/src/components/reconciliation/SummaryPanel.tsx (create) - Reconciliation stats: reconciled, pending, differences
- frontend/src/services/reconciliationApi.ts (create) - API calls for reconciliation

**Dependencies:** Item 1, Item 6, Item 7

**Validation:**
- Cartola upload accepts CSV files
- Movement table displays all lines with reconciliation status
- Manual match/unmatch operations work
- Summary shows correct counts and amounts

**Role:** role-fe (frontend_developer)

---

### ITEM 10: Frontend — Payment Requests Page
**Goal:** Implement payment request management with approval workflow.

**Files to create:**
- frontend/src/pages/PaymentRequests.tsx (create) - Payment requests list and detail page
- frontend/src/components/payments/PaymentRequestList.tsx (create) - Table of payment requests with status
- frontend/src/components/payments/PaymentRequestForm.tsx (create) - Create/edit payment request form
- frontend/src/components/payments/ApprovalActions.tsx (create) - Approve/reject buttons with comments
- frontend/src/components/payments/ApprovalHistory.tsx (create) - Timeline of approval actions
- frontend/src/services/paymentApi.ts (create) - API calls for payment requests

**Dependencies:** Item 1, Item 6, Item 7

**Validation:**
- Payment request list shows all requests with status
- Create form submits correctly
- Approve/reject actions trigger workflow
- Approval history displays all actions

**Role:** role-fe (frontend_developer)

---

### ITEM 11: Infrastructure & Deployment (Docker, Azure DevOps)
**Goal:** Complete Docker orchestration, Azure configuration, and deployment pipeline.

**Files to create:**
- docker-compose.yml (create) - All services: backend, frontend, PostgreSQL, Redis
- backend/Dockerfile (create) - Multi-stage .NET 8 build, non-root user, port 8080
- frontend/Dockerfile (create) - Multi-stage Node build + nginx, port 80
- .env.example (create) - All environment variables with descriptions
- .gitignore (create) - Standard .NET + React ignores
- .dockerignore (create) - Build context exclusions
- run.sh (create) - Validates Docker, builds, starts, waits healthy, prints URL
- README.md (create) - Prerequisites, clone, run, test instructions
- azure-pipelines.yml (create) - Azure DevOps CI/CD pipeline
- azure/main.bicep (create) - Azure infrastructure as code (App Service, PostgreSQL, Storage, Key Vault)

**Dependencies:** All previous items

**Validation:**
- `./run.sh` completes without errors
- All services start and report healthy
- Frontend accessible at localhost:3000
- Backend API responds at localhost:8080

**Role:** role-devops (devops_support)

---

### ITEM 12: Frontend — Additional Pages (Collections, Reports)
**Goal:** Implement expected collections management and reports export functionality.

**Files to create:**
- frontend/src/pages/Collections.tsx (create) - Expected collections management page
- frontend/src/components/collections/CollectionForm.tsx (create) - Add/edit collection form
- frontend/src/components/collections/CollectionList.tsx (create) - List with status filters
- frontend/src/pages/Reports.tsx (create) - Reports page with export options
- frontend/src/components/reports/ExportButtons.tsx (create) - PDF/Excel export buttons
- frontend/src/services/collectionApi.ts (create) - API calls for collections
- frontend/src/services/reportApi.ts (create) - API calls for reports

**Dependencies:** Item 1, Item 6, Item 7

**Validation:**
- Collections page lists expected collections
- Form allows adding/editing collections with status
- Reports page offers PDF and Excel export
- Exports generate downloadable files

**Role:** role-fe (frontend_developer)