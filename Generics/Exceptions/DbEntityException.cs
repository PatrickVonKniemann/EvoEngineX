namespace Generics.Exceptions;

/// <summary>
/// Exception. Selected entity is not found in database
/// </summary>
public class DbEntityException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbEntityNotFoundException"/> class.
    /// </summary>
    /// <param name="message"></param>
    protected DbEntityException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbEntityNotFoundException"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public DbEntityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception. Selected entity is not found in database
/// </summary>
public class DbEntityNotFoundException : DbEntityException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbEntityNotFoundException"/> class.
    /// </summary>
    /// <param name="entityType"></param>
    public DbEntityNotFoundException(string entityType) : base(string.Format(CoreMessages.EntityNotFound,
        entityType, Guid.Empty))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbEntityNotFoundException"/> class.
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="entityId"></param>
    public DbEntityNotFoundException(string entityType, Guid entityId) : base(
        string.Format(CoreMessages.EntityNotFound, entityType, entityId))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbEntityNotFoundException"/> class.
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="identifier"></param>
    public DbEntityNotFoundException(string entityType, object identifier) : base(
        string.Format(CoreMessages.EntityNotFound, entityType, identifier))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbEntityNotFoundException"/> class.
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="entityId"></param>
    public DbEntityNotFoundException(string entityType, string entityId) : base(
        string.Format(CoreMessages.EntityNotFound, entityType, entityId))
    {
    }
}

/// <summary>
/// Exception. Selected entity is not found in database
/// </summary>
public class DbEntityAlreadyExistsException : DbEntityException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbEntityAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="userName"></param>
    /// <param name="companyId"></param>
    public DbEntityAlreadyExistsException(string entityType, string userName, Guid companyId) : base(
        string.Format(CoreMessages.EntityAlreadyExists, entityType, userName, companyId))
    {
    }
}

/// <summary>
/// Exception. Selected entity is not found in database
/// </summary>
public abstract class DbEntityEmptyListException : DbEntityException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbEntityNotFoundException"/> class.
    /// </summary>
    /// <param name="entityType"></param>
    protected DbEntityEmptyListException(string entityType) : base(string.Format(CoreMessages.EntityEmptyList,
        entityType))
    {
    }
}

/// <summary>
/// Exception. Target entity cannot be replaced in database
/// </summary>
public class DbEntityReplacingException : DbEntityException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbEntityReplacingException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public DbEntityReplacingException(string message) : base(message)
    {
    }
}

/// <summary>
/// Exception. Translated entity already exists before creation
/// </summary>
public class DbEntityExistsInCreateException : DbEntityException
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public DbEntityExistsInCreateException(string message) : base(message)
    {
    }
}