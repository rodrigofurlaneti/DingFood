namespace DingFood.Domain.Exceptions;

public sealed class TenantAccessException() : Exception("O recurso não pertence à empresa ativa.");
