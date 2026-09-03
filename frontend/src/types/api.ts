import type {
  Subsidiary,
  BankAccount,
  BankStatement,
  BankStatementLine,
  ExpectedCollection,
  PaymentRequest,
  ApprovalHistory,
  CashFlowProjection,
  DashboardSummary,
  ReconciliationStatus,
  User,
  Alert,
  PaginatedResult,
} from './index';

export interface LoginRequest {
  code: string;
  redirectUri: string;
}

export interface LoginResponse {
  token: string;
  user: User;
}

export interface BankAccountCreate {
  subsidiaryId: string;
  bankName: string;
  accountNumber: string;
  accountType: 'corriente' | 'ahorro' | 'vista';
  currency: string;
}

export interface BankAccountUpdate {
  bankName?: string;
  accountNumber?: string;
  accountType?: 'corriente' | 'ahorro' | 'vista';
  currency?: string;
  isActive?: boolean;
}

export interface BankStatementUpload {
  bankAccountId: string;
  statementDate: string;
  file: File;
}

export interface BankStatementUploadResponse {
  id: string;
  bankAccountId: string;
  statementDate: string;
  fileName: string;
  importStatus: 'pending' | 'processing' | 'completed' | 'error';
  totalCredits: number;
  totalDebits: number;
  finalBalance: number;
  lineCount: number;
}

export interface AutoMatchRequest {
  lineIds: string[];
}

export interface ManualMatchRequest {
  lineId: string;
  matchedWithId: string;
}

export interface ExpectedCollectionCreate {
  subsidiaryId: string;
  customerName: string;
  amount: number;
  expectedDate: string;
  notes?: string;
}

export interface ExpectedCollectionUpdate {
  customerName?: string;
  amount?: number;
  expectedDate?: string;
  actualDate?: string;
  status?: 'pending' | 'received' | 'overdue' | 'cancelled';
  notes?: string;
}

export interface PaymentRequestCreate {
  subsidiaryId: string;
  vendorName: string;
  description?: string;
  amount: number;
  currency: string;
  requestDate: string;
  dueDate?: string;
}

export interface PaymentRequestUpdate {
  vendorName?: string;
  description?: string;
  amount?: number;
  currency?: string;
  dueDate?: string;
}

export interface ApprovalAction {
  comments?: string;
}

export interface RejectAction {
  reason: string;
}

export interface CashFlowProjectionRequest {
  subsidiaryId: string | null;
  days: 30 | 60 | 90;
  date: string;
}

export interface AuditLogQuery {
  entityType?: string;
  entityId?: string;
  userId?: string;
  startDate?: string;
  endDate?: string;
  page?: number;
  pageSize?: number;
}

export interface DashboardFilters {
  subsidiaryId?: string | null;
  startDate?: string;
  endDate?: string;
}

export interface ReconciliationFilters {
  bankAccountId?: string;
  statementDate?: string;
  status?: 'pending' | 'reconciled' | 'difference';
}

export interface PaymentRequestFilters {
  subsidiaryId?: string;
  status?: string;
  startDate?: string;
  endDate?: string;
  minAmount?: number;
  maxAmount?: number;
}

export interface ReportRequest {
  type: 'cash_flow' | 'reconciliation' | 'payments' | 'collections';
  format: 'pdf' | 'excel';
  subsidiaryId?: string;
  startDate?: string;
  endDate?: string;
}

export type {
  Subsidiary,
  BankAccount,
  BankStatement,
  BankStatementLine,
  ExpectedCollection,
  PaymentRequest,
  ApprovalHistory,
  CashFlowProjection,
  DashboardSummary,
  ReconciliationStatus,
  User,
  Alert,
  PaginatedResult,
};
