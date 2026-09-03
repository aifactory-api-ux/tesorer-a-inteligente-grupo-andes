# Arquitectura — Tesorería Inteligente Grupo Andes

## Visión General

Plataforma web modular para la gestión centralizada de la liquidez de Grupo Andes Capital, desplegada exclusivamente en Microsoft Azure con autenticación vía Azure AD (SSO).

## Stack Tecnológico

| Capa | Tecnología |
|------|-----------|
| Frontend | React 18, TypeScript 5, Vite 5, React Router DOM 6 |
| Backend | C# .NET 8, ASP.NET Core 8 |
| Base de Datos | Azure Database for PostgreSQL Flexible Server |
| Almacenamiento | Azure Storage Accounts (Blob) |
| Secretos | Azure Key Vault |
| Monitoreo | Azure Monitor + Application Insights |
| CI/CD | Azure DevOps Pipelines |
| Contenedores | Docker (desarrollo local) |

## Arquitectura de la Solución

```
┌─────────────────────────────────────────────┐
│                Azure AD                       │
│           (Autenticación SSO)                 │
└──────────────┬──────────────────────────────┘
               │
┌──────────────▼──────────────────────────────┐
│         Azure App Service                    │
│  ┌─────────────┐    ┌──────────────────┐   │
│  │   Frontend   │    │     Backend      │   │
│  │   (React)    │    │   (.NET 8 API)   │   │
│  │   :80        │    │     :5000        │   │
│  └─────────────┘    └────────┬─────────┘   │
└──────────────────────────────┼──────────────┘
                               │
              ┌────────────────┼────────────────┐
              │                │                │
    ┌─────────▼────┐  ┌───────▼─────┐  ┌──────▼──────┐
    │  PostgreSQL   │  │    Azure    │  │    Azure    │
    │   Flexible   │  │    Key      │  │   Monitor    │
    │    Server    │  │    Vault    │  │             │
    └──────────────┘  └────────────┘  └─────────────┘
```

## Estructura de la Base de Datos

### Entidades Principales

- **Users** — Usuarios mapeados desde Azure AD con roles locales
- **Subsidiaries** — Filiales del grupo empresarial
- **BankAccounts** — Cuentas bancarias por filial
- **BankStatements** — Cartolas bancarias (encabezado)
- **BankStatementLines** — Líneas de movimiento de cartola
- **ExpectedCollections** — Cobros esperados registrados manualmente
- **PaymentRequests** — Solicitudes de pago a proveedores
- **ApprovalHistory** — Historial de aprobaciones (trazabilidad)
- **CashFlowProjections** — Proyecciones de caja 30/60/90 días
- **AuditLogs** — Log de auditoría inmutable

### Relaciones

```
Subsidiary (1) ──< BankAccount (N)
BankAccount (1) ──< BankStatement (N)
BankStatement (1) ──< BankStatementLine (N)
Subsidiary (1) ──< ExpectedCollection (N)
Subsidiary (1) ──< PaymentRequest (N)
PaymentRequest (1) ──< ApprovalHistory (N)
Subsidiary (1) ──< CashFlowProjection (N)
User (1) ──< AuditLog (N)
```

## Módulos Funcionales

| Módulo | Descripción |
|--------|-------------|
| Dashboard | Vista consolidada de flujo de caja por filial y grupo |
| Conciliación Bancaria | Carga de cartolas CSV/OFX, matching semiautomático |
| Pagos a Proveedores | Creación y aprobación multinivel de solicitudes |
| Proyección de Caja | Cálculo de proyecciones a 30/60/90 días |
| Cobros Esperados | Registro manual de ingresos anticipated |
| Reportes | Exportación PDF y Excel para directorio |
| Administración | Gestión de usuarios, permisos y auditoría |

## Seguridad

- Cifrado en tránsito: TLS 1.2+ (Azure App Service)
- Cifrado en reposo: Transparent Data Encryption (PostgreSQL)
- Secretos: Azure Key Vault
- Autenticación: Azure AD SSO (OIDC)
- Auditoría: Log inmutable de todos los movimientos

## Patrones de Diseño

- **API REST**: Endpoints `/api/*` con respuestas JSON
- **Repository Pattern**: Acceso a datos vía repositorios
- **Service Layer**: Lógica de negocio en servicios
- **Middleware**: Logging, auditoría y manejo de errores
- **DTOs**: Objetos de transferencia validados con FluentValidation

## Convenciones de Código

- **Commits**: Mensajes en español, tono neutro
- **Nomenclatura**: PascalCase para entidades, camelCase para campos
- **Comentarios**: Lo necesario, sin exceso
- **Validación**: FluentValidation en backend, React Hook Form + Zod en frontend
