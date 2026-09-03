# Delivery Report

**Outcome:** `failed`

## Completion metrics

- Implementation files: 64
- Expected implementation files: 95
- Blocking items: 0/16 done
- Failed items: 3
- Requirements: 0/0 met
- Fidelity: 0.0%
- Abort reason: Terminal infrastructure/configuration failure for 3

## Outstanding findings

- UI/UX fidelity mismatch in `frontend/src/styles/tokens.ts`: approved Figma token values are missing (fonts: other). Replace generic/default tokens with the exact design-contract values and keep every component/test aligned to this file.
- Contract defines GET /api/audit-logs but no matching backend route found
- Contract defines GET /api/auth/profile but no matching backend route found
- Contract defines GET /api/auth/roles but no matching backend route found
- Contract defines GET /api/bank-accounts but no matching backend route found
- Contract defines POST /api/bank-accounts but no matching backend route found
- Contract defines DELETE /api/bank-accounts/{id} but no matching backend route found
- Contract defines GET /api/bank-accounts/{id} but no matching backend route found
- Contract defines PUT /api/bank-accounts/{id} but no matching backend route found
- Contract defines GET /api/bank-statements but no matching backend route found
- Contract defines POST /api/bank-statements/upload but no matching backend route found
- Contract defines DELETE /api/bank-statements/{id} but no matching backend route found
- Contract defines GET /api/bank-statements/{id} but no matching backend route found
- Contract defines POST /api/cash-flow/calculate but no matching backend route found
- Contract defines GET /api/cash-flow/projections but no matching backend route found
- Contract defines GET /api/dashboard/alerts but no matching backend route found
- Contract defines GET /api/dashboard/recent-transactions but no matching backend route found
- Contract defines GET /api/dashboard/summary but no matching backend route found
- Contract defines GET /api/expected-collections but no matching backend route found
- Contract defines POST /api/expected-collections but no matching backend route found
- Contract defines DELETE /api/expected-collections/{id} but no matching backend route found
- Contract defines GET /api/expected-collections/{id} but no matching backend route found
- Contract defines PUT /api/expected-collections/{id} but no matching backend route found
- Contract defines GET /api/payment-requests but no matching backend route found
- Contract defines POST /api/payment-requests but no matching backend route found
- Contract defines GET /api/payment-requests/{id} but no matching backend route found
- Contract defines PUT /api/payment-requests/{id} but no matching backend route found
- Contract defines POST /api/payment-requests/{id}/approve but no matching backend route found
- Contract defines GET /api/payment-requests/{id}/history but no matching backend route found
- Contract defines POST /api/payment-requests/{id}/mark-paid but no matching backend route found
- Contract defines POST /api/payment-requests/{id}/reject but no matching backend route found
- Contract defines POST /api/reconciliation/auto-match but no matching backend route found
- Contract defines GET /api/reconciliation/differences but no matching backend route found
- Contract defines PUT /api/reconciliation/manual-match but no matching backend route found
- Contract defines GET /api/reconciliation/status but no matching backend route found
- Contract defines PUT /api/reconciliation/unmatch but no matching backend route found
- Contract defines GET /api/reports/cash-flow/excel but no matching backend route found
- Contract defines GET /api/reports/cash-flow/pdf but no matching backend route found
- Contract defines GET /api/reports/reconciliation/pdf but no matching backend route found
- Contract defines GET /api/subsidiaries but no matching backend route found
- Contract defines POST /api/subsidiaries but no matching backend route found
- Contract defines DELETE /api/subsidiaries/{id} but no matching backend route found
- Contract defines GET /api/subsidiaries/{id} but no matching backend route found
- Contract defines PUT /api/subsidiaries/{id} but no matching backend route found
- Contract defines GET /health but no matching backend route found
- Contract defines GET /metrics but no matching backend route found
- Functional requirement not implemented: **Azure AD SSO Authentication**
Implementation approach: Microsoft Identity Platform integration with OIDC
Missing file(s) that must be created/completed:
  - `frontend/src/hooks/useAuth.ts`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Dashboard Consolidado de Flujo de Caja**
Implementation approach: Dashboard API endpoint returning aggregated balances, inflows/outflows by subsidiary
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/DashboardController.cs`
  - `backend/src/Services/DashboardService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Vista por filial y grupo**
Implementation approach: Subsidiary filter parameter on all dashboard endpoints, subsidiary selector UI component
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/SubsidiarySelector/SubsidiarySelector.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Carga de cartolas bancarias CSV/OFX**
Implementation approach: File upload endpoint accepting multipart/form-data, CSV parser service
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/BankStatementsController.cs`
  - `backend/src/Services/BankStatementService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Matching semiautomático**
Implementation approach: Auto-match algorithm comparing amounts and dates within tolerance
Missing file(s) that must be created/completed:
  - `backend/src/Services/ReconciliationService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Revisión manual de diferencias**
Implementation approach: Manual match/unmatch endpoints for operator review
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/ReconciliationController.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Módulo de Pagos a Proveedores**
Implementation approach: Full CRUD for payment requests with status workflow
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/PaymentRequestsController.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Flujo de aprobación multinivel**
Implementation approach: Status transitions: pending → pending_approval_gerente → pending_approval_cfo → approved
Missing file(s) that must be created/completed:
  - `backend/src/Services/PaymentRequestService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Proyección de Flujo de Caja 30/60/90 días**
Implementation approach: Calculation algorithm: current_balance + expected_collections - scheduled_payments
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/CashFlowController.cs`
  - `backend/src/Services/CashFlowService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Alertas de liquidez negativa**
Implementation approach: Dashboard alerts when projected_balance < 0
Missing file(s) that must be created/completed:
  - `backend/src/Services/DashboardService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Gestión de Cobros Esperados**
Implementation approach: CRUD operations for expected_collections table
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/ExpectedCollectionsController.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Reportes PDF y Excel**
Implementation approach: Report generation using QuestPDF or ClosedXML
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/ReportsController.cs`
  - `backend/src/Services/ReportService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Trazabilidad completa (audit log)**
Implementation approach: Middleware capturing all entity changes, immutable log table
Missing file(s) that must be created/completed:
  - `backend/src/Middleware/AuditMiddleware.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Navegación Principal sidebar**
Implementation approach: Navigation component with logo, module links, subsidiary selector in header
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Navigation/Navigation.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Botones CTA (Call to Action)**
Implementation approach: Button component with primary/secondary/positive/negative variants
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Button/Button.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Tarjetas (Cards)**
Implementation approach: Card component for dashboard metrics and payment details
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Card/Card.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Tablas de Datos**
Implementation approach: DataTable component with fixed headers, alternating rows, monospace numeric columns
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/DataTable/DataTable.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Formularios**
Implementation approach: Form component with React Hook Form + Zod validation
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Form/Form.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Alertas y Mensajes de Estado**
Implementation approach: Alert component with success/error/warning/info types
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Alert/Alert.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Modales/Diálogos**
Implementation approach: Modal component for confirmations and detail views
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Modal/Modal.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Filtros y Controles de Búsqueda**
Implementation approach: Filter component for date ranges, subsidiaries, status
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Filter/Filter.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Selector de Filial/Grupo**
Implementation approach: SubsidiarySelector component with "All" option
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/SubsidiarySelector/SubsidiarySelector.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Página Login**
Implementation approach: Login page with Azure AD SSO button
Missing file(s) that must be created/completed:
  - `frontend/src/pages/Login/Login.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Página Dashboard**
Implementation approach: Dashboard page with summary cards, recent transactions, alerts
Missing file(s) that must be created/completed:
  - `frontend/src/pages/Dashboard/Dashboard.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Página Conciliación Bancaria**
Implementation approach: Reconciliation page with statement list, match interface, difference review
Missing file(s) that must be created/completed:
  - `frontend/src/pages/Reconciliation/Reconciliation.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Indicadores de conciliación**
Implementation approach: Cards showing: movements reconciled, pending review, differences detected, amount reconciled
Missing file(s) that must be created/completed:
  - `frontend/src/pages/Reconciliation/Reconciliation.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Footer con trazabilidad**
Implementation approach: Footer component showing "Tesorería Inteligente Grupo Andes" and sync timestamp
Missing file(s) that must be created/completed:
  - `frontend/src/components/layout/Footer.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Functional requirement not implemented: **Métricas Azure Monitor**
Implementation approach: Application Insights SDK integrated
Missing file(s) that must be created/completed:
  - `backend/src/Api/Program.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- Fidelity verifier unavailable: Fidelity verifier unavailable: fidelity response does not contain assessments[]
- Ítem no completado: Foundation — shared types, interfaces, DB schema, configuration (1/2)
- Ítem no completado: Foundation — shared types, interfaces, DB schema, configuration (2/2)
- Ítem no completado: Backend — Authentication & User Management (Azure AD SSO) (1/2)
- Ítem no completado: Backend — Authentication & User Management (Azure AD SSO) (2/2)
- Ítem no completado: Backend — Bank Reconciliation
- Ítem no completado: Backend — Payment Requests & Cash Flow
- Ítem no completado: Frontend — Foundation (Tokens, Layout, Routing, Base Components) (1/2)
- Ítem no completado: Frontend — Foundation (Tokens, Layout, Routing, Base Components) (2/2)
- Ítem no completado: Frontend — Authentication Flow (Login Azure AD)
- Ítem no completado: Frontend — Dashboard Consolidado de Flujo de Caja
- Ítem no completado: Frontend — Conciliación Bancaria Page
- Ítem no completado: Frontend — Payment Requests Page
- Ítem no completado: Infrastructure & Deployment (Docker, Azure DevOps) (1/2)
- Ítem no completado: Infrastructure & Deployment (Docker, Azure DevOps) (2/2)
- Ítem no completado: Frontend — Additional Pages (Collections, Reports)
- [project] UI/UX fidelity mismatch in `frontend/src/styles/tokens.ts`: approved Figma token values are missing (fonts: other). Replace generic/default tokens with the exact design-contract values and keep every component/test aligned to this file.
- [project] Functional requirement not implemented: **Azure AD SSO Authentication**
Implementation approach: Microsoft Identity Platform integration with OIDC
Missing file(s) that must be created/completed:
  - `frontend/src/hooks/useAuth.ts`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Dashboard Consolidado de Flujo de Caja**
Implementation approach: Dashboard API endpoint returning aggregated balances, inflows/outflows by subsidiary
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/DashboardController.cs`
  - `backend/src/Services/DashboardService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Vista por filial y grupo**
Implementation approach: Subsidiary filter parameter on all dashboard endpoints, subsidiary selector UI component
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/SubsidiarySelector/SubsidiarySelector.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Carga de cartolas bancarias CSV/OFX**
Implementation approach: File upload endpoint accepting multipart/form-data, CSV parser service
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/BankStatementsController.cs`
  - `backend/src/Services/BankStatementService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Matching semiautomático**
Implementation approach: Auto-match algorithm comparing amounts and dates within tolerance
Missing file(s) that must be created/completed:
  - `backend/src/Services/ReconciliationService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Revisión manual de diferencias**
Implementation approach: Manual match/unmatch endpoints for operator review
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/ReconciliationController.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Módulo de Pagos a Proveedores**
Implementation approach: Full CRUD for payment requests with status workflow
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/PaymentRequestsController.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Flujo de aprobación multinivel**
Implementation approach: Status transitions: pending → pending_approval_gerente → pending_approval_cfo → approved
Missing file(s) that must be created/completed:
  - `backend/src/Services/PaymentRequestService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Proyección de Flujo de Caja 30/60/90 días**
Implementation approach: Calculation algorithm: current_balance + expected_collections - scheduled_payments
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/CashFlowController.cs`
  - `backend/src/Services/CashFlowService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Alertas de liquidez negativa**
Implementation approach: Dashboard alerts when projected_balance < 0
Missing file(s) that must be created/completed:
  - `backend/src/Services/DashboardService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Gestión de Cobros Esperados**
Implementation approach: CRUD operations for expected_collections table
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/ExpectedCollectionsController.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Reportes PDF y Excel**
Implementation approach: Report generation using QuestPDF or ClosedXML
Missing file(s) that must be created/completed:
  - `backend/src/Controllers/ReportsController.cs`
  - `backend/src/Services/ReportService.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Trazabilidad completa (audit log)**
Implementation approach: Middleware capturing all entity changes, immutable log table
Missing file(s) that must be created/completed:
  - `backend/src/Middleware/AuditMiddleware.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Navegación Principal sidebar**
Implementation approach: Navigation component with logo, module links, subsidiary selector in header
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Navigation/Navigation.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Botones CTA (Call to Action)**
Implementation approach: Button component with primary/secondary/positive/negative variants
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Button/Button.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Tarjetas (Cards)**
Implementation approach: Card component for dashboard metrics and payment details
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Card/Card.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Tablas de Datos**
Implementation approach: DataTable component with fixed headers, alternating rows, monospace numeric columns
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/DataTable/DataTable.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Formularios**
Implementation approach: Form component with React Hook Form + Zod validation
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Form/Form.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Alertas y Mensajes de Estado**
Implementation approach: Alert component with success/error/warning/info types
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Alert/Alert.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Modales/Diálogos**
Implementation approach: Modal component for confirmations and detail views
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Modal/Modal.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Filtros y Controles de Búsqueda**
Implementation approach: Filter component for date ranges, subsidiaries, status
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/Filter/Filter.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Selector de Filial/Grupo**
Implementation approach: SubsidiarySelector component with "All" option
Missing file(s) that must be created/completed:
  - `frontend/src/components/ui/SubsidiarySelector/SubsidiarySelector.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Página Login**
Implementation approach: Login page with Azure AD SSO button
Missing file(s) that must be created/completed:
  - `frontend/src/pages/Login/Login.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Página Dashboard**
Implementation approach: Dashboard page with summary cards, recent transactions, alerts
Missing file(s) that must be created/completed:
  - `frontend/src/pages/Dashboard/Dashboard.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Página Conciliación Bancaria**
Implementation approach: Reconciliation page with statement list, match interface, difference review
Missing file(s) that must be created/completed:
  - `frontend/src/pages/Reconciliation/Reconciliation.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Indicadores de conciliación**
Implementation approach: Cards showing: movements reconciled, pending review, differences detected, amount reconciled
Missing file(s) that must be created/completed:
  - `frontend/src/pages/Reconciliation/Reconciliation.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Footer con trazabilidad**
Implementation approach: Footer component showing "Tesorería Inteligente Grupo Andes" and sync timestamp
Missing file(s) that must be created/completed:
  - `frontend/src/components/layout/Footer.tsx`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
- [project] Functional requirement not implemented: **Métricas Azure Monitor**
Implementation approach: Application Insights SDK integrated
Missing file(s) that must be created/completed:
  - `backend/src/Api/Program.cs`

Instructions:
1. Create each missing file with complete, production-ready implementation.
2. Wire it into the service that owns it (add imports, register routes, etc.).
3. Do NOT create stub or placeholder files — implement the full logic.
4. Verify the file exists on disk after writing.
