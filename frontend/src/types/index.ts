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

export interface Alert {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  title: string;
  message: string;
  entityId?: string;
  createdAt: string;
}

export interface PaginatedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}
