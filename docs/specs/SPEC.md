# SPEC.md

## 1. TECHNOLOGY STACK

### Backend
- **Runtime**: .NET 8 (C# 12)
- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.x with Npgsql (PostgreSQL driver)
- **Authentication**: Microsoft Identity Platform (Azure AD SSO)
- **API Documentation**: Swashbuckle (Swagger/OpenAPI)
- **Validation**: FluentValidation
- **Configuration**: Azure Key Vault integration via SecretManager

### Database
- **Engine**: Azure Database for PostgreSQL Flexible Server
- **Version**: PostgreSQL 15.x (managed by Azure)
- **Connection**: Npgsql Entity Framework Core provider

### Frontend
- **Framework**: React 18.x
- **Language**: TypeScript 5.x
- **Build Tool**: Vite 5.x
- **Routing**: React Router DOM 6.x
- **State Management**: React Context API + useReducer
- **HTTP Client**: Axios
- **Styling**: CSS Modules with Design Tokens (no Tailwind)
- **Forms**: React Hook Form + Zod validation
- **Date Handling**: date-fns
- **Charts/Visualization**: Recharts (for cash flow charts)

### Infrastructure & DevOps
- **Cloud Platform**: Microsoft Azure
- **Compute**: Azure App Service (Web Apps)
- **Storage**: Azure Storage Accounts (Blob Storage for bank statements)
- **Secrets**: Azure Key Vault
- **Monitoring**: Azure Monitor + Application Insights
- **CI/CD**: Azure DevOps Pipelines
- **Container**: Docker (for local development)

---

## 2. DATA CONTRACTS

### Database Entities (PostgreSQL)

Based on the functional requirements, the following entities are required. The Architect Database Schema Contract is minimal (`Entity { string id PK }`), so all additional schema is derived from requirements.

#### Users / Authentication
```sql
-- Azure AD users mapped to local roles
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    azure_ad_object_id VARCHAR(255) UNIQUE NOT NULL,
    email VARCHAR(255) NOT NULL,
    display_name VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL CHECK (role IN ('analista', 'gerente', 'cfo', 'auditor', 'admin')),
    subsidiary_id UUID REFERENCES subsidiaries(id),
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

#### Subsidiaries (Filiales)
```sql
CREATE TABLE subsidiaries (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    code VARCHAR(50) UNIQUE NOT NULL,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

#### Bank Accounts
```sql
CREATE TABLE bank_accounts (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    subsidiary_id UUID NOT NULL REFERENCES subsidiaries(id),
    bank_name VARCHAR(255) NOT NULL,
    account_number VARCHAR(50) NOT NULL,
    account_type VARCHAR(20) CHECK (account_type IN ('corriente', 'ahorro', 'vista')),
    currency VARCHAR(3) DEFAULT 'CLP',
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

#### Bank Statements (Cartolas)
```sql
CREATE TABLE bank_statements (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    bank_account_id UUID NOT NULL REFERENCES bank_accounts(id),
    statement_date DATE NOT NULL,
    file_name VARCHAR(255),
    file_path VARCHAR(500),
    total_credits DECIMAL(18,2) DEFAULT 0,
    total_debits DECIMAL(18,2) DEFAULT 0,
    final_balance DECIMAL(18,2) NOT NULL,
    import_status VARCHAR(20) DEFAULT 'pending' CHECK (import_status IN ('pending', 'processing', 'completed', 'error')),
    created_by UUID REFERENCES users(id),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(bank_account_id, statement_date)
);
```

#### Bank Statement Lines (Movimientos)
```sql
CREATE TABLE bank_statement_lines (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    bank_statement_id UUID NOT NULL REFERENCES bank_statements(id) ON DELETE CASCADE,
    line_number INTEGER NOT NULL,
    transaction_date DATE NOT NULL,
    description TEXT,
    reference VARCHAR(255),
    credit DECIMAL(18,2) DEFAULT 0,
    debit DECIMAL(18,2) DEFAULT 0,
    balance DECIMAL(18,2),
    is_reconciled BOOLEAN DEFAULT false,
    reconciled_with_id UUID REFERENCES bank_statement_lines(id),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

#### Expected Collections (Cobros Esperados)
```sql
CREATE TABLE expected_collections (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    subsidiary_id UUID NOT NULL REFERENCES subsidiaries(id),
    customer_name VARCHAR(255) NOT NULL,
    amount DECIMAL(18,2) NOT NULL,
    expected_date DATE NOT NULL,
    actual_date DATE,
    status VARCHAR(20) DEFAULT 'pending' CHECK (status IN ('pending', 'received', 'overdue', 'cancelled')),
    notes TEXT,
    created_by UUID REFERENCES users(id),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

#### Payment Requests (Solicitudes de Pago)
```sql
CREATE TABLE payment_requests (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    subsidiary_id UUID NOT NULL REFERENCES subsidiaries(id),
    vendor_name VARCHAR(255) NOT NULL,
    description TEXT,
    amount DECIMAL(18,2) NOT NULL,
    currency VARCHAR(3) DEFAULT 'CLP',
    request_date DATE NOT NULL,
    due_date DATE,
    status VARCHAR(30) DEFAULT 'pending' CHECK (status IN ('pending', 'pending_approval_gerente', 'pending_approval_cfo', 'approved', 'rejected', 'paid')),
    rejection_reason TEXT,
    created_by UUID REFERENCES users(id),
    approved_by UUID REFERENCES users(id),
    approved_at TIMESTAMP WITH TIME ZONE,
    payment_proof_path VARCHAR(500),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

#### Approval History
```sql
CREATE TABLE approval_history (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    payment_request_id UUID NOT NULL REFERENCES payment_requests(id),
    approver_id UUID NOT NULL REFERENCES users(id),
    action VARCHAR(20) NOT NULL CHECK (action IN ('approve', 'reject')),
    comments TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

#### Cash Flow Projections
```sql
CREATE TABLE cash_flow_projections (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    subsidiary_id UUID NOT NULL REFERENCES subsidiaries(id),
    projection_date DATE NOT NULL,
    projection_days INTEGER NOT NULL CHECK (projection_days IN (30, 60, 90)),
    projected_inflow DECIMAL(18,2) DEFAULT 0,
    projected_outflow DECIMAL(18,2) DEFAULT 0,
    projected_balance DECIMAL(18,2) NOT NULL,
    calculated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(subsidiary_id, projection_date, projection_days)
);
```

#### Audit Log (Trazabilidad)
```sql
CREATE TABLE audit_logs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES users(id),
    action VARCHAR(100) NOT NULL,
    entity_type VARCHAR(100) NOT NULL,
    entity_id UUID NOT NULL,
    old_values JSONB,
    new_values JSONB,
    ip_address VARCHAR(45),
    user_agent TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```

### Pydantic Models (Backend)

```python
# shared/models.py
from pydantic import BaseModel, EmailStr, Field
from typing import Optional, List
from datetime import date, datetime
from uuid import UUID
from enum import Enum

class UserRole(str, Enum):
    ANALISTA = "analista"
    GERENTE = "gerente"
    CFO = "cfo"
    AUDITOR = "auditor"
    ADMIN = "admin"

class SubsidiaryBase(BaseModel):
    name: str = Field(..., min_length=1, max_length=255)
    code: str = Field(..., min_length=1, max_length=50)

class SubsidiaryCreate(SubsidiaryBase):
    pass

class SubsidiaryResponse(SubsidiaryBase):
    id: UUID
    is_active: bool
    created_at: datetime
    
    class Config:
        from_attributes = True

class BankAccountBase(BaseModel):
    bank_name: str
    account_number: str
    account_type: str
    currency: str = "CLP"

class BankAccountCreate(BankAccountBase):
    subsidiary_id: UUID

class BankAccountResponse(BankAccountBase):
    id: UUID
    subsidiary_id: UUID
    is_active: bool
    created_at: datetime
    
    class Config:
        from_attributes = True

class BankStatementLineBase(BaseModel):
    line_number: int
    transaction_date: date
    description: Optional[str] = None
    reference: Optional[str] = None
    credit: Decimal = Field(default=0, ge=0)
    debit: Decimal = Field(default=0, ge=0)
    balance: Optional[Decimal] = None

class BankStatementLineResponse(BankStatementLineBase):
    id: UUID
    is_reconciled: bool
    reconciled_with_id: Optional[UUID] = None
    created_at: datetime
    
    class Config:
        from_attributes = True

class BankStatementBase(BaseModel):
    statement_date: date
    file_name: Optional[str] = None

class BankStatementCreate(BankStatementBase):
    bank_account_id: UUID

class BankStatementResponse(BankStatementBase):
    id: UUID
    bank_account_id: UUID
    file_path: Optional[str] = None
    total_credits: Decimal
    total_debits: Decimal
    final_balance: Decimal
    import_status: str
    created_by: Optional[UUID] = None
    created_at: datetime
    lines: List[BankStatementLineResponse] = []
    
    class Config:
        from_attributes = True

class ExpectedCollectionBase(BaseModel):
    customer_name: str
    amount: Decimal = Field(..., gt=0)
    expected_date: date
    notes: Optional[str] = None

class ExpectedCollectionCreate(ExpectedCollectionBase):
    subsidiary_id: UUID

class ExpectedCollectionUpdate(BaseModel):
    actual_date: Optional[date] = None
    status: Optional[str] = None
    notes: Optional[str] = None

class ExpectedCollectionResponse(ExpectedCollectionBase):
    id: UUID
    subsidiary_id: UUID
    actual_date: Optional[date] = None
    status: str
    created_by: Optional[UUID] = None
    created_at: datetime
    updated_at: datetime
    
    class Config:
        from_attributes = True

class PaymentRequestBase(BaseModel):
    vendor_name: str
    description: Optional[str] = None
    amount: Decimal = Field(..., gt=0)
    currency: str = "CLP"
    request_date: date
    due_date: Optional[date] = None

class PaymentRequestCreate(PaymentRequestBase):
    subsidiary_id: UUID

class PaymentRequestUpdate(BaseModel):
    status: Optional[str] = None
    rejection_reason: Optional[str] = None
    payment_proof_path: Optional[str] = None

class PaymentRequestResponse(PaymentRequestBase):
    id: UUID
    subsidiary_id: UUID
    status: str
    rejection_reason: Optional[str] = None
    created_by: Optional[UUID] = None
    approved_by: Optional[UUID] = None
    approved_at: Optional[datetime] = None
    payment_proof_path: Optional[str] = None
    created_at: datetime
    updated_at: datetime
    
    class Config:
        from_attributes = True

class ApprovalHistoryResponse(BaseModel):
    id: UUID
    payment_request_id: UUID
    approver_id: UUID
    action: str
    comments: Optional[str] = None
    created_at: datetime
    
    class Config:
        from_attributes = True

class CashFlowProjectionResponse(BaseModel):
    id: UUID
    subsidiary_id: UUID
    projection_date: date
    projection_days: int
    projected_inflow: Decimal
    projected_outflow: Decimal
    projected_balance: Decimal
    calculated_at: datetime
    
    class Config:
        from_attributes = True

class DashboardSummary(BaseModel):
    total_balance: Decimal
    total_inflow: Decimal
    total_outflow: Decimal
    reconciled_count: int
    pending_count: int
    difference_count: int
    by_subsidiary: List[SubsidiarySummary]

class SubsidiarySummary(BaseModel):
    subsidiary_id: UUID
    subsidiary_name: str
    balance: Decimal

class ReconciliationStatus(BaseModel):
    bank_statement_id: UUID
    total_lines: int
    reconciled_lines: int
    pending_lines: int
    difference_amount: Decimal

class UserResponse(BaseModel):
    id: UUID
    azure_ad_object_id: str
    email: str
    display_name: str
    role: UserRole
    subsidiary_id: Optional[UUID] = None
    is_active: bool
    
    class Config:
        from_attributes = True
```

### TypeScript Interfaces (Frontend)

```typescript
// frontend/src/types/models.ts

export type UserRole = 'analista' | 'gerente' | 'cfo' | 'auditor' | 'admin';

export interface Subsidiary {
  id: string;
  name: string;
  code: string;
  isActive: boolean;
  createdAt: string;
}

export interface BankAccount {
  id: string;
  subsidiaryId: string;
  bankName: string;
  accountNumber: string;
  accountType: 'corriente' | 'ahorro' | 'vista';
  currency: string;
  isActive: boolean;
  createdAt: string;
}

export interface BankStatementLine {
  id: string;
  lineNumber: number;
  transactionDate: string;
  description?: string;
  reference?: string;
  credit: number;
  debit: number;
  balance?: number;
  isReconciled: boolean;
  reconciledWithId?: string;
  createdAt: string;
}

export interface BankStatement {
  id: string;
  bankAccountId: string;
  statementDate: string;
  fileName?: string;
  filePath?: string;
  totalCredits: number;
  totalDebits: number;
  finalBalance: number;
  importStatus: 'pending' | 'processing' | 'completed' | 'error';
  createdBy?: string;
  createdAt: string;
  lines: BankStatementLine[];
}

export interface ExpectedCollection {
  id: string;
  subsidiaryId: string;
  customerName: string;
  amount: number;
  expectedDate: string;
  actualDate?: string;
  status: 'pending' | 'received' | 'overdue' | 'cancelled';
  notes?: string;
  createdBy?: string;
  createdAt: string;
  updatedAt: string;
}

export interface PaymentRequest {
  id: string;
  subsidiaryId: string;
  vendorName: string;
  description?: string;
  amount: number;
  currency: string;
  requestDate: string;
  dueDate?: string;
  status: 'pending' | 'pending_approval_gerente' | 'pending_approval_cfo' | 'approved' | 'rejected' | 'paid';
  rejectionReason?: string;
  createdBy?: string;
  approvedBy?: string;
  approvedAt?: string;
  paymentProofPath?: string;
  createdAt: string;
  updatedAt: string;
}

export interface ApprovalHistory {
  id: string;
  paymentRequestId: string;
  approverId: string;
  action: 'approve' | 'reject';
  comments?: string;
  createdAt: string;
}

export interface CashFlowProjection {
  id: string;
  subsidiaryId: string;
  projectionDate: string;
  projectionDays: 30 | 60 | 90;
  projectedInflow: number;
  projectedOutflow: number;
  projectedBalance: number;
  calculatedAt: string;
}

export interface DashboardSummary {
  totalBalance: number;
  totalInflow: number;
  totalOutflow: number;
  reconciledCount: number;
  pendingCount: number;
  differenceCount: number;
  bySubsidiary: SubsidiarySummary[];
}

export interface SubsidiarySummary {
  subsidiaryId: string;
  subsidiaryName: string;
  balance: number;
}

export interface ReconciliationStatus {
  bankStatementId: string;
  totalLines: number;
  reconciledLines: number;
  pendingLines: number;
  differenceAmount: number;
}

export interface User {
  id: string;
  azureAdObjectId: string;
  email: string;
  displayName: string;
  role: UserRole;
  subsidiaryId?: string;
  isActive: boolean;
}
```

---

## 3. API ENDPOINTS

### Authentication
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/auth/profile` | - | `UserResponse` |
| GET | `/api/auth/roles` | - | `{ roles: string[] }` |

### Subsidiaries
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/subsidiaries` | - | `SubsidiaryResponse[]` |
| GET | `/api/subsidiaries/{id}` | - | `SubsidiaryResponse` |
| POST | `/api/subsidiaries` | `SubsidiaryCreate` | `SubsidiaryResponse` |
| PUT | `/api/subsidiaries/{id}` | `SubsidiaryCreate` | `SubsidiaryResponse` |
| DELETE | `/api/subsidiaries/{id}` | - | `204` |

### Bank Accounts
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/bank-accounts` | `?subsidiaryId={uuid}` | `BankAccountResponse[]` |
| GET | `/api/bank-accounts/{id}` | - | `BankAccountResponse` |
| POST | `/api/bank-accounts` | `BankAccountCreate` | `BankAccountResponse` |
| PUT | `/api/bank-accounts/{id}` | `BankAccountCreate` | `BankAccountResponse` |
| DELETE | `/api/bank-accounts/{id}` | - | `204` |

### Bank Statements (Cartolas)
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/bank-statements` | `?bankAccountId={uuid}&startDate={date}&endDate={date}` | `BankStatementResponse[]` |
| GET | `/api/bank-statements/{id}` | - | `BankStatementResponse` |
| POST | `/api/bank-statements/upload` | `multipart/form-data` (file, bankAccountId, statementDate) | `BankStatementResponse` |
| DELETE | `/api/bank-statements/{id}` | - | `204` |

### Reconciliation (Conciliación)
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/reconciliation/status` | `?bankStatementId={uuid}` | `ReconciliationStatus` |
| POST | `/api/reconciliation/auto-match` | `BankStatementLineIds[]` | `{ matched: number }` |
| PUT | `/api/reconciliation/manual-match` | `{ lineId, matchedWithId }` | `BankStatementLineResponse` |
| PUT | `/api/reconciliation/unmatch` | `{ lineId }` | `BankStatementLineResponse` |
| GET | `/api/reconciliation/differences` | `?bankStatementId={uuid}` | `BankStatementLineResponse[]` |

### Expected Collections (Cobros Esperados)
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/expected-collections` | `?subsidiaryId={uuid}&status={status}&startDate={date}&endDate={date}` | `ExpectedCollectionResponse[]` |
| GET | `/api/expected-collections/{id}` | - | `ExpectedCollectionResponse` |
| POST | `/api/expected-collections` | `ExpectedCollectionCreate` | `ExpectedCollectionResponse` |
| PUT | `/api/expected-collections/{id}` | `ExpectedCollectionUpdate` | `ExpectedCollectionResponse` |
| DELETE | `/api/expected-collections/{id}` | - | `204` |

### Payment Requests (Pagos)
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/payment-requests` | `?subsidiaryId={uuid}&status={status}&startDate={date}&endDate={date}` | `PaymentRequestResponse[]` |
| GET | `/api/payment-requests/{id}` | - | `PaymentRequestResponse` |
| POST | `/api/payment-requests` | `PaymentRequestCreate` | `PaymentRequestResponse` |
| PUT | `/api/payment-requests/{id}` | `PaymentRequestUpdate` | `PaymentRequestResponse` |
| POST | `/api/payment-requests/{id}/approve` | `{ comments?: string }` | `PaymentRequestResponse` |
| POST | `/api/payment-requests/{id}/reject` | `{ reason: string }` | `PaymentRequestResponse` |
| POST | `/api/payment-requests/{id}/mark-paid` | `{ proofPath?: string }` | `PaymentRequestResponse` |
| GET | `/api/payment-requests/{id}/history` | - | `ApprovalHistoryResponse[]` |

### Cash Flow Projections
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/cash-flow/projections` | `?subsidiaryId={uuid}&days={30|60|90}&date={date}` | `CashFlowProjectionResponse` |
| POST | `/api/cash-flow/calculate` | `{ subsidiaryId?, days: 30|60|90, date }` | `CashFlowProjectionResponse` |

### Dashboard
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/dashboard/summary` | `?subsidiaryId={uuid}` | `DashboardSummary` |
| GET | `/api/dashboard/recent-transactions` | `?subsidiaryId={uuid}&limit={number}` | `BankStatementLineResponse[]` |
| GET | `/api/dashboard/alerts` | `?subsidiaryId={uuid}` | `Alert[]` |

### Reports (Exportación)
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/reports/cash-flow/pdf` | `?subsidiaryId={uuid}&startDate={date}&endDate={date}` | `Binary (PDF)` |
| GET | `/api/reports/cash-flow/excel` | `?subsidiaryId={uuid}&startDate={date}&endDate={date}` | `Binary (XLSX)` |
| GET | `/api/reports/reconciliation/pdf` | `?bankStatementId={uuid}` | `Binary (PDF)` |

### Audit Logs
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/api/audit-logs` | `?entityType={string}&entityId={uuid}&userId={uuid}&startDate={date}&endDate={date}&page={n}&pageSize={n}` | `{ items: AuditLog[], total, page, pageSize }` |

### Health & Metrics
| Method | Path | Request | Response |
|--------|------|---------|----------|
| GET | `/health` | - | `{ status: "healthy" }` |
| GET | `/metrics` | - | Prometheus format |

---

## 4. FILE STRUCTURE

```
tesoreria-inteligente/
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   │   ├── ui/
│   │   │   │   ├── Navigation/
│   │   │   │   │   ├── Navigation.tsx
│   │   │   │   │   └── Navigation.module.css
│   │   │   │   ├── Button/
│   │   │   │   │   ├── Button.tsx
│   │   │   │   │   └── Button.module.css
│   │   │   │   ├── Card/
│   │   │   │   │   ├── Card.tsx
│   │   │   │   │   └── Card.module.css
│   │   │   │   ├── DataTable/
│   │   │   │   │   ├── DataTable.tsx
│   │   │   │   │   └── DataTable.module.css
│   │   │   │   ├── Form/
│   │   │   │   │   ├── Form.tsx
│   │   │   │   │   └── Form.module.css
│   │   │   │   ├── Alert/
│   │   │   │   │   ├── Alert.tsx
│   │   │   │   │   └── Alert.module.css
│   │   │   │   ├── Modal/
│   │   │   │   │   ├── Modal.tsx
│   │   │   │   │   └── Modal.module.css
│   │   │   │   ├── Filter/
│   │   │   │   │   ├── Filter.tsx
│   │   │   │   │   └── Filter.module.css
│   │   │   │   └── SubsidiarySelector/
│   │   │   │       ├── SubsidiarySelector.tsx
│   │   │   │       └── SubsidiarySelector.module.css
│   │   │   └── layout/
│   │   │       ├── Topbar.tsx
│   │   │       ├── Sidebar.tsx
│   │   │       └── Footer.tsx
│   │   ├── pages/
│   │   │   ├── Login/
│   │   │   │   ├── Login.tsx
│   │   │   │   └── Login.module.css
│   │   │   ├── Dashboard/
│   │   │   │   ├── Dashboard.tsx
│   │   │   │   └── Dashboard.module.css
│   │   │   └── Reconciliation/
│   │   │       ├── Reconciliation.tsx
│   │   │       └── Reconciliation.module.css
│   │   ├── hooks/
│   │   │   ├── useAuth.ts
│   │   │   ├── useDashboard.ts
│   │   │   ├── useReconciliation.ts
│   │   │   ├── usePaymentRequests.ts
│   │   │   └── useCashFlow.ts
│   │   ├── services/
│   │   │   ├── api.ts
│   │   │   ├── auth.service.ts
│   │   │   ├── dashboard.service.ts
│   │   │   ├── reconciliation.service.ts
│   │   │   ├── payment.service.ts
│   │   │   └── cashflow.service.ts
│   │   ├── context/
│   │   │   ├── AuthContext.tsx
│   │   │   └── AppContext.tsx
│   │   ├── styles/
│   │   │   ├── tokens.ts
│   │   │   ├── global.css
│   │   │   └── variables.css
│   │   ├── types/
│   │   │   └── models.ts
│   │   ├── utils/
│   │   │   ├── formatters.ts
│   │   │   └── validators.ts
│   │   ├── App.tsx
│   │   ├── main.tsx
│   │   └── vite-env.d.ts
│   ├── index.html
│   ├── vite.config.ts
│   ├── tsconfig.json
│   ├── package.json
│   └── .env.example
├── backend/
│   ├── src/
│   │   ├── Api/
│   │   │   ├── Program.cs
│   │   │   ├── appsettings.json
│   │   │   └── appsettings.Development.json
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── SubsidiariesController.cs
│   │   │   ├── BankAccountsController.cs
│   │   │   ├── BankStatementsController.cs
│   │   │   ├── ReconciliationController.cs
│   │   │   ├── ExpectedCollectionsController.cs
│   │   │   ├── PaymentRequestsController.cs
│   │   │   ├── CashFlowController.cs
│   │   │   ├── DashboardController.cs
│   │   │   ├── ReportsController.cs
│   │   │   └── AuditLogsController.cs
│   │   ├── Services/
│   │   │   ├── Interfaces/
│   │   │   │   ├── IAuthService.cs
│   │   │   │   ├── ISubsidiaryService.cs
│   │   │   │   ├── IBankAccountService.cs
│   │   │   │   ├── IBankStatementService.cs
│   │   │   │   ├── IReconciliationService.cs
│   │   │   │   ├── IExpectedCollectionService.cs
│   │   │   │   ├── IPaymentRequestService.cs
│   │   │   │   ├── ICashFlowService.cs
│   │   │   │   ├── IDashboardService.cs
│   │   │   │   ├── IReportService.cs
│   │   │   │   └── IAuditLogService.cs
│   │   │   └── Implementations/
│   │   │       ├── AuthService.cs
│   │   │       ├── SubsidiaryService.cs
│   │   │       ├── BankAccountService.cs
│   │   │       ├── BankStatementService.cs
│   │   │       ├── ReconciliationService.cs
│   │   │       ├── ExpectedCollectionService.cs
│   │   │       ├── PaymentRequestService.cs
│   │   │       ├── CashFlowService.cs
│   │   │       ├── DashboardService.cs
│   │   │       ├── ReportService.cs
│   │   │       └── AuditLogService.cs
│   │   ├── Models/
│   │   │   ├── Entities/
│   │   │   │   ├── User.cs
│   │   │   │   ├── Subsidiary.cs
│   │   │   │   ├── BankAccount.cs
│   │   │   │   ├── BankStatement.cs
│   │   │   │   ├── BankStatementLine.cs
│   │   │   │   ├── ExpectedCollection.cs
│   │   │   │   ├── PaymentRequest.cs
│   │   │   │   ├── ApprovalHistory.cs
│   │   │   │   ├── CashFlowProjection.cs
│   │   │   │   └── AuditLog.cs
│   │   │   ├── DTOs/
│   │   │   │   ├── SubsidiaryDTOs.cs
│   │   │   │   ├── BankAccountDTOs.cs
│   │   │   │   ├── BankStatementDTOs.cs
│   │   │   │   ├── ExpectedCollectionDTOs.cs
│   │   │   │   ├── PaymentRequestDTOs.cs
│   │   │   │   └── DashboardDTOs.cs
│   │   │   └── Enums/
│   │   │       ├── UserRole.cs
│   │   │       ├── PaymentStatus.cs
│   │   │       └── CollectionStatus.cs
│   │   ├── Data/
│   │   │   ├── TesoreriaDbContext.cs
│   │   │   └── Migrations/
│   │   ├── Validators/
│   │   │   ├── SubsidiaryValidator.cs
│   │   │   ├── BankAccountValidator.cs
│   │   │   ├── ExpectedCollectionValidator.cs
│   │   │   └── PaymentRequestValidator.cs
│   │   ├── Configuration/
│   │   │   ├── AzureAdConfig.cs
│   │   │   ├── DatabaseConfig.cs
│   │   │   └── StorageConfig.cs
│   │   ├── Middleware/
│   │   │   ├── AuditMiddleware.cs
│   │   │   └── ErrorHandlingMiddleware.cs
│   │   └── Shared/
│   │       ├── Constants.cs
│   │       ├── Extensions.cs
│   │       └── Helper.cs
│   ├── tests/
│   │   └── Api.Tests/
│   └── TesoreriaApi.csproj
├── infrastructure/
│   ├── docker-compose.yml
│   ├── docker/
│   │   ├── backend.Dockerfile
│   │   └── frontend.Dockerfile
│   ├── azure/
│   │   ├── main.bicep
│   │   ├── app-service.bicep
│   │   ├── postgres.bicep
│   │   └── storage.bicep
│   └── monitoring/
│       └── applicationinsights.json
├── .env.example
├── .gitignore
├── README.md
└── SPEC.md
```

### PORT TABLE
| Service | Listening Port | Path |
|---------|-----------------|------|
| backend | 5000 | backend/src/Api/ |
| frontend | 5173 | frontend/ |

---

## 5. ENVIRONMENT VARIABLES

### Backend (.env / Azure App Service Configuration)

| Name | Type | Description | Example |
|------|------|-------------|---------|
| `AZURE_AD__CLIENT_ID` | string | Azure AD Application Client ID | `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` |
| `AZURE_AD__TENANT_ID` | string | Azure AD Tenant ID | `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` |
| `AZURE_AD__CLIENT_SECRET` | string | Azure AD Application Secret | `~xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx` |
| `AZURE_AD__CALLBACK_PATH` | string | OAuth callback path | `/signin-oidc` |
| `DATABASE__HOST` | string | PostgreSQL host | `tesoreria-db.postgres.database.azure.com` |
| `DATABASE__PORT` | int | PostgreSQL port | `5432` |
| `DATABASE__NAME` | string | Database name | `tesoreria_db` |
| `DATABASE__USERNAME` | string | Database username | `tesoreria_admin` |
| `DATABASE__PASSWORD` | string | Database password | `SecurePassword123!` |
| `STORAGE__CONNECTION_STRING` | string | Azure Storage connection string | `DefaultEndpointsProtocol=https;AccountName=...` |
| `STORAGE__CONTAINER_NAME` | string | Blob container for bank statements | `bank-statements` |
| `KEY_VAULT__URI` | string | Azure Key Vault URI | `https://tesoreria-kv.vault.azure.net/` |
| `APPINSIGHTS__CONNECTION_STRING` | string | Application Insights connection string | `InstrumentationKey=...` |
| `APPINSIGHTS__INSTRUMENTATION_KEY` | string | AI Instrumentation Key | `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` |
| `ASPNETCORE_ENVIRONMENT` | string | Environment | `Development` or `Production` |
| `ASPNETCORE_URLS` | string | Server URLs | `http://0.0.0.0:5000` |
| `CORS__ALLOWED_ORIGINS` | string | Comma-separated allowed origins | `https://tesoreria.azurewebsites.net` |
| `PAYMENT_APPROVAL_GERENTE_THRESHOLD` | decimal | Max amount for Gerente approval | `5000000` |
| `PAYMENT_APPROVAL_CFO_THRESHOLD` | decimal | Min amount requiring CFO approval | `5000001` |

### Frontend (.env)

| Name | Type | Description | Example |
|------|------|-------------|---------|
| `VITE_API_BASE_URL` | string | Backend API base URL | `https://tesoreria-api.azurewebsites.net/api` |
| `VITE_AZURE_AD_CLIENT_ID` | string | Azure AD Client ID | `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` |
| `VITE_AZURE_AD_AUTHORITY` | string | Azure AD Authority | `https://login.microsoftonline.com/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` |
| `VITE_AZURE_AD_REDIRECT_URI` | string | Redirect URI | `https://tesoreria.azurewebsites.net` |

---

## 6. IMPORT CONTRACTS

### Frontend

```typescript
// Token imports (MUST use these exact paths)
import { tokens } from './styles/tokens';
import './styles/global.css';

// Component imports
import { Navigation } from './components/ui/Navigation/Navigation';
import { Button } from './components/ui/Button/Button';
import { Card } from './components/ui/Card/Card';
import { DataTable } from './components/ui/DataTable/DataTable';
import { Form } from './components/ui/Form/Form';
import { Alert } from './components/ui/Alert/Alert';
import { Modal } from './components/ui/Modal/Modal';
import { Filter } from './components/ui/Filter/Filter';
import { SubsidiarySelector } from './components/ui/SubsidiarySelector/SubsidiarySelector';

// Hook imports
import { useAuth } from './hooks/useAuth';
import { useDashboard } from './hooks/useDashboard';
import { useReconciliation } from './hooks/useReconciliation';
import { usePaymentRequests } from './hooks/usePaymentRequests';
import { useCashFlow } from './hooks/useCashFlow';

// Service imports
import { authService } from './services/auth.service';
import { dashboardService } from './services/dashboard.service';
import { reconciliationService } from './services/reconciliation.service';
import { paymentService } from './services/payment.service';
import { cashflowService } from './services/cashflow.service';

// Context imports
import { AuthProvider } from './context/AuthContext';
import { AppProvider } from './context/AppContext';

// Type imports
import type { 
  Subsidiary, 
  BankAccount, 
  BankStatement, 
  ExpectedCollection, 
  PaymentRequest,
  DashboardSummary,
  User 
} from './types/models';
```

### Backend

```csharp
// Entity using statements
using Api.Models.Entities;
using Api.Models.DTOs;
using Api.Models.Enums;
using Api.Data;
using Api.Services.Interfaces;
using Api.Services.Implementations;
using Api.Configuration;
using Api.Validators;
using Api.Middleware;

// Dependency Injection in Program.cs
builder.Services.AddDbContext<TesoreriaDbContext>();
builder.Services.AddScoped<ISubsidiaryService, SubsidiaryService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IBankStatementService, BankStatementService>();
builder.Services.AddScoped<IReconciliationService, ReconciliationService>();
builder.Services.AddScoped<IExpectedCollectionService, ExpectedCollectionService>();
builder.Services.AddScoped<IPaymentRequestService, PaymentRequestService>();
builder.Services.AddScoped<ICashFlowService, CashFlowService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
```

---

## 7. FRONTEND STATE & COMPONENT CONTRACTS

### React Hooks

```typescript
// useAuth() → Auth state and methods
{
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: () => void;
  logout: () => void;
  error: string | null;
}

// useDashboard() → Dashboard data
{
  summary: DashboardSummary | null;
  recentTransactions: BankStatementLine[];
  alerts: Alert[];
  loading: boolean;
  error: string | null;
  refetch: () => void;
}

// useReconciliation() → Reconciliation operations
{
  bankStatements: BankStatement[];
  currentStatement: BankStatement | null;
  reconciliationStatus: ReconciliationStatus | null;
  loading: boolean;
  error: string | null;
  uploadStatement: (file: File, bankAccountId: string, date: string) => Promise<void>;
  autoMatch: (lineIds: string[]) => Promise<void>;
  manualMatch: (lineId: string, matchedWithId: string) => Promise<void>;
  unmatch: (lineId: string) => Promise<void>;
  getDifferences: (statementId: string) => Promise<void>;
}

// usePaymentRequests() → Payment operations
{
  paymentRequests: PaymentRequest[];
  currentRequest: PaymentRequest | null;
  loading: boolean;
  error: string | null;
  createRequest: (data: PaymentRequestCreate) => Promise<PaymentRequest>;
  updateRequest: (id: string, data: PaymentRequestUpdate) => Promise<PaymentRequest>;
  approveRequest: (id: string, comments?: string) => Promise<PaymentRequest>;
  rejectRequest: (id: string, reason: string) => Promise<PaymentRequest>;
  markAsPaid: (id: string, proofPath?: string) => Promise<PaymentRequest>;
}

// useCashFlow() → Cash flow projections
{
  projections: CashFlowProjection[];
  loading: boolean;
  error: string | null;
  calculateProjection: (subsidiaryId: string | null, days: 30 | 60 | 90, date: string) => Promise<void>;
}
```

### Component Props Interfaces

```typescript
// Navigation props
interface NavigationProps {
  onLogout: () => void;
  currentUser: User | null;
}

// Button props
interface ButtonProps {
  variant?: 'primary' | 'secondary' | 'positive' | 'negative';
  size?: 'sm' | 'md' | 'lg';
  disabled?: boolean;
  loading?: boolean;
  onClick?: () => void;
  type?: 'button' | 'submit' | 'reset';
  children: React.ReactNode;
  className?: string;
}

// Card props
interface CardProps {
  title?: string;
  children: React.ReactNode;
  className?: string;
  variant?: 'default' | 'highlighted';
}

// DataTable props
interface DataTableProps<T> {
  data: T[];
  columns: ColumnDef<T>[];
  loading?: boolean;
  onRowClick?: (row: T) => void;
  emptyMessage?: string;
  pageSize?: number;
}

interface ColumnDef<T> {
  key: keyof T;
  header: string;
  render?: (value: any, row: T) => React.ReactNode;
  align?: 'left' | 'center' | 'right';
  sortable?: boolean;
}

// Form props
interface FormProps<T> {
  onSubmit: (data: T) => void;
  defaultValues?: Partial<T>;
  validationSchema?: ZodSchema;
  children: React.ReactNode;
  className?: string;
}

// Alert props
interface AlertProps {
  type: 'success' | 'error' | 'warning' | 'info';
  title?: string;
  message: string;
  onClose?: () => void;
  className?: string;
}

// Modal props
interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  children: React.ReactNode;
  footer?: React.ReactNode;
  size?: 'sm' | 'md' | 'lg';
}

// Filter props
interface FilterProps {
  onFilterChange: (filters: FilterState) => void;
  initialValues?: FilterState;
  availableFields: FilterField[];
}

interface FilterState {
  [key: string]: any;
}

interface FilterField {
  key: string;
  label: string;
  type: 'text' | 'select' | 'date' | 'daterange' | 'number';
  options?: { value: string; label: string }[];
}

// SubsidiarySelector props
interface SubsidiarySelectorProps {
  value: string | null;
  onChange: (subsidiaryId: string | null) => void;
  subsidiaries: Subsidiary[];
  allowAll?: boolean;
  className?: string;
}
```

---

## 8. FILE EXTENSION CONVENTION

- **Frontend**: TypeScript project — all React components use `.tsx` extension
- **Entry Point**: `/src/main.tsx`
- **Styles**: CSS Modules (`.module.css`) alongside each component
- **Global Styles**: `/src/styles/global.css`
- **Backend**: C# — Controllers, Services, Models use `.cs` extension
- **Database Context**: Entity Framework Core with code-first migrations

---

## 9. DESIGN TOKENS

```typescript
// frontend/src/styles/tokens.ts

export const tokens = {
  colors: {
    'border-light': '#D1D5DB',
    'primary-dark': '#0B1F3A',
    'text-primary': '#0B1F3A',
    'neutral-white': '#FFFFFF',
    'text-secondary': '#4A5568',
    'accent-positive': '#C9A24B',
    'status-negative': '#C0392B',
    'status-positive': '#2E7D46',
    'neutral-light-gray': '#EEF0F3',
  },
  typography: {
    'font-family-base': "'Inter', sans-serif",
    'font-family-numeric': "'Space Mono', monospace",
    'font-size-h1': '2.5rem',
    'font-size-h2': '2rem',
    'font-size-h3': '1.5rem',
    'font-size-body-base': '1rem',
    'font-size-body-large': '1.125rem',
    'font-size-body-small': '0.875rem',
    'font-weight-bold': '700',
    'font-weight-medium': '500',
    'font-weight-regular': '400',
    'line-height-base': '1.5',
  },
  spacing: {
    'xs': '8px',
    'sm': '16px',
    'md': '24px',
    'lg': '32px',
    'xl': '48px',
    'xxl': '64px',
  },
  borderRadius: {
    'sm': '4px',
    'md': '6px',
    'lg': '10px',
    'full': '9999px',
  },
  shadows: {
    'sm': '0px 1px 3px rgba(0, 0, 0, 0.1)',
    'md': '0px 4px 6px rgba(0, 0, 0, 0.1)',
  },
  motion: {
    'easing-standard': 'ease-in-out',
    'transition-duration-fast': '150ms',
    'transition-duration-normal': '300ms',
  },
};
```

---

## 10. FUNCTIONAL REQUIREMENTS COVERAGE

| Requirement | Implementation | Files |
|-------------|----------------|-------|
| Azure AD SSO Authentication | Microsoft Identity Platform integration with OIDC | backend/src/Configuration/AzureAdConfig.cs, frontend/src/hooks/useAuth.ts |
| Dashboard Consolidado de Flujo de Caja | Dashboard API endpoint returning aggregated balances, inflows/outflows by subsidiary | backend/src/Controllers/DashboardController.cs, backend/src/Services/DashboardService.cs |
| Vista por filial y grupo | Subsidiary filter parameter on all dashboard endpoints, subsidiary selector UI component | frontend/src/components/ui/SubsidiarySelector/SubsidiarySelector.tsx |
| Saldos, ingresos/egresos, alertas | DashboardSummary DTO with breakdown by category, Alert array for liquidity warnings | backend/src/Models/DTOs/DashboardDTOs.cs |
| Carga de cartolas bancarias CSV/OFX | File upload endpoint accepting multipart/form-data, CSV parser service | backend/src/Controllers/BankStatementsController.cs, backend/src/Services/BankStatementService.cs |
| Matching semiautomático | Auto-match algorithm comparing amounts and dates within tolerance | backend/src/Services/ReconciliationService.cs |
| Revisión manual de diferencias | Manual match/unmatch endpoints for operator review | backend/src/Controllers/ReconciliationController.cs |
| Módulo de Pagos a Proveedores | Full CRUD for payment requests with status workflow | backend/src/Controllers/PaymentRequestsController.cs |
| Flujo de aprobación multinivel | Status transitions: pending → pending_approval_gerente → pending_approval_cfo → approved | backend/src/Services/PaymentRequestService.cs |
| Registro de comprobantes | payment_proof_path field, file upload for proof documents | backend/src/Models/Entities/PaymentRequest.cs |
| Motivos de rechazo | rejection_reason field, audit trail on rejection | backend/src/Models/Entities/PaymentRequest.cs, backend/src/Models/Entities/ApprovalHistory.cs |
| Proyección de Flujo de Caja 30/60/90 días | Calculation algorithm: current_balance + expected_collections - scheduled_payments | backend/src/Controllers/CashFlowController.cs, backend/src/Services/CashFlowService.cs |
| Alertas de liquidez negativa | Dashboard alerts when projected_balance < 0 | backend/src/Services/DashboardService.cs |
| Gestión de Cobros Esperados | CRUD operations for expected_collections table | backend/src/Controllers/ExpectedCollectionsController.cs |
| Reportes PDF y Excel | Report generation using QuestPDF or ClosedXML | backend/src/Controllers/ReportsController.cs, backend/src/Services/ReportService.cs |
| Gestión de Usuarios y Permisos | Azure AD roles mapped to local role enum, subsidiary-level access | backend/src/Models/Enums/UserRole.cs, backend/src/Controllers/AuthController.cs |
| Trazabilidad completa (audit log) | Middleware capturing all entity changes, immutable log table | backend/src/Middleware/AuditMiddleware.cs, backend/src/Models/Entities/AuditLog.cs |
| Diseño institucional (colores, tipografía) | tokens.ts with exact hex values from UI/UX contract | frontend/src/styles/tokens.ts |
| Navegación Principal sidebar | Navigation component with logo, module links, subsidiary selector in header | frontend/src/components/ui/Navigation/Navigation.tsx |
| Botones CTA (Call to Action) | Button component with primary/secondary/positive/negative variants | frontend/src/components/ui/Button/Button.tsx |
| Tarjetas (Cards) | Card component for dashboard metrics and payment details | frontend/src/components/ui/Card/Card.tsx |
| Tablas de Datos | DataTable component with fixed headers, alternating rows, monospace numeric columns | frontend/src/components/ui/DataTable/DataTable.tsx |
| Formularios | Form component with React Hook Form + Zod validation | frontend/src/components/ui/Form/Form.tsx |
| Alertas y Mensajes de Estado | Alert component with success/error/warning/info types | frontend/src/components/ui/Alert/Alert.tsx |
| Modales/Diálogos | Modal component for confirmations and detail views | frontend/src/components/ui/Modal/Modal.tsx |
| Filtros y Controles de Búsqueda | Filter component for date ranges, subsidiaries, status | frontend/src/components/ui/Filter/Filter.tsx |
| Selector de Filial/Grupo | SubsidiarySelector component with "All" option | frontend/src/components/ui/SubsidiarySelector/SubsidiarySelector.tsx |
| Página Login | Login page with Azure AD SSO button | frontend/src/pages/Login/Login.tsx |
| Página Dashboard | Dashboard page with summary cards, recent transactions, alerts | frontend/src/pages/Dashboard/Dashboard.tsx |
| Página Conciliación Bancaria | Reconciliation page with statement list, match interface, difference review | frontend/src/pages/Reconciliation/Reconciliation.tsx |
| Indicadores de conciliación | Cards showing: movements reconciled, pending review, differences detected, amount reconciled | frontend/src/pages/Reconciliation/Reconciliation.tsx |
| Footer con trazabilidad | Footer component showing "Tesorería Inteligente Grupo Andes" and sync timestamp | frontend/src/components/layout/Footer.tsx |
| Cifrado en tránsito | HTTPS enforced via Azure App Service TLS/SSL |
| Cifrado en reposo | Azure SQL Transparent Data Encryption (TDE) |
| Azure Key Vault | All secrets stored in KV, accessed via config injection |
| Métricas Azure Monitor | Application Insights SDK integrated | backend/src/Api/Program.cs |

---

## Notes

- **No Database Schema Override**: The Architect Database Schema Contract was minimal (`Entity { string id PK }`), so all schema is derived entirely from the functional requirements. No constraints were violated.
- **No External Integrations for V1.0**: Per requirements, no ERP, core banking, or payment gateway integrations are included.
- **Tests**: Test files are outside the user-approved scope per instructions.
- **Infrastructure**: Docker Compose included for local development; Azure infrastructure defined via Bicep templates.
- **Port Range**: Frontend runs on 5173 (Vite default), Backend on 5000. Both within the 21000-65000 range.