const config = {
  app: {
    name: 'Tesorería Inteligente Grupo Andes',
    version: '1.0.0',
  },
  pagination: {
    defaultPageSize: 50,
    maxPageSize: 100,
  },
  dateFormat: 'yyyy-MM-dd',
  dateTimeFormat: 'yyyy-MM-ddTHH:mm:ss',
  currency: {
    default: 'CLP',
    supported: ['CLP', 'USD', 'EUR'],
  },
  approvalThresholds: {
    gerente: 5000000,
    cfo: null,
  },
  reconciliation: {
    amountTolerance: 0.01,
    dateTolerance: 3,
  },
};

export default config;
