namespace Common.Exceptions;

/// <summary>
/// System messages for inner app events
/// </summary>
public static class CoreMessages
{
    public const string RollbackFailed = "Rollback operation on DB context has failed";
    public const string CommitFailed = "commit operation on DB context has failed";

    public const string DbContextInUseAlready =
        "DB context is already performing operation, nested transactions are not allowed";

    public const string AnonymousSaveNotAllowed = "Anonymous changes to data are not allowed!";

    // {0} entity type {1} entity id
    public const string EntityAlreadyExists = "Entity type '{0}' name '{1}' with companyId '{2}' already exists";
    public const string EntityNotFound = "'{0}' with id '{1}' was not found";
    public const string EntityNotAbleToDelete = "No able to delete '{0}' with id '{1}'";

    public const string EntityNotAbleToDeleteManyToMany =
        "No able to delete entity '{0}', with connection ['{1}':'{2}']";

    public const string EntityNotAbleToUpdate = "No able to update '{0}' with id '{1}'";

    public const string EntityNotAbleToUpdateManyToMany =
        "No able to setup entity '{0}', with connection ['{1}':'{2}']";

    public const string EntityConnectionAlreadyExists =
        "There is already connection between entity '{0}', with connection ['{1}':'{2}']";

    public const string EntityEmptyList = "There are no '{0}' found in database";
    public const string EntityNotFoundToUpdate = "Cannot find entity to update!";

    public const string EntityReplacedInUpdate =
        "Existing entity is replaced by another instance during update, wrong order of definitions???";

    public const string EntityExistsInCreate =
        "New entity should be created in create process, but instance is already assigned, wrong order of definitions???";

    public const string TranslatorWrongVersioningUsage =
        "Call UseDataContextCreate or UseDataContextUpdate before UseVersioning definition!";

    public const string UseAddCollectionInstead =
        "Use AddCollection method for IEnumerable types. Translator '{0} <-> {1}', between property types {2} and {3}'";

    public const string IncorrectUsageTranslation =
        "Incorrect input types for AddNavigationOneMany, must be from Instance to Collection. Translator '{0} <-> {1}', between property types {2} and {3}'";

    public const string DbSetNotFound = "Requested DbSet '{0}' not found!";
    public const string EntityHasNoPrimaryKey = "Entity {0} has not primary key defined!";

    public const string MainPublishedEntityLocked =
        "Cannot update previous (Published) version if newer version (Modified) exists!";

    public const string CannotProcessThisVersion =
        "Cannot update entity which has non-allowed publishing status! Entity Status: {0}";

    public const string AsyncActionCancelled = "Action cancelled.";
    public const string ItemInCacheNotFound = "Item '{0}' not found in cache of type '{1}'";
    public const string TypeIsNotSupportedTypeCache = "Type of '{0}' is not supported as type for TypeCache.";
    public const string DuplicateKeyInData = "Type of '{0}' in contains duplicate key.";
    public const string DuplicateKeyInCache = "Cache of '{0}' type duplicate key exception: {1}";
    public const string DuplicateKeyInDataDetails = "Type of '{0}' in contains duplicate keys '{1}'.";
    public const string BadExpressionProperty = "Invalid expression, unable to extract property name";

    // -- Mediator
    public const string CannotProcessActionHandlerParams =
        "Method is not suitable for mediator pattern. Only one parameter accepting class type model of IDaphneMediatorRequestQuery or IDaphneMediatorRequestCommand is required.";

    public const string CannotProcessActionHandlerParamsDetail = "Class '{0}', action: {1}";

    public const string CannotProcessActionHandlerClassType =
        "Method is not compatible with class type, i.e. query action in command service or command action in query service.";

    public const string CannotProcessActionHandlerClassTypeDetail = "Class '{0}', action: {1}, request type {2}";

    public const string NoActionHandlerForRequestModel =
        "There is no action handler registered accepting model of '{0}' type.";

    public const string NoUniqueActionHandlerForRequestModel =
        "More than one action handler found accepting model of '{0}' type.";

    public const string UndefinedRequestModel = "Request cannot be null";

    public const string ActionHandlerReturnTypeNotSupported =
        "Return type '{0}' of action handler '{1}' is not supported.";

    // Authentication
    public const string AuthenticationOptionInvalid = "Invalid authentication option '{0}'";
    public const string AuthenticatorNotAvailable = "Authenticator '{0}' specified for user '{1}' is not available.";

    public const string BadAuthorizationName =
        "Signed user: '{0}', EntityType: '{1}', IdentityName '{2}' with RequestDetail ['{3}']";

    public const string NoActiveCompany = "User '{0}' has no active company assigned";
    public const string NotAbleToGetAuthUser = "Not able to get authenticated user to set default company for group";
    public const string NotAbleToDeleteUser = "Not able to delete user with '{0}'";
    public const string NoCompanyAssigned = "User '{0}' has no company assigned";
    public const string NoUserNameAssigned = "There is no valid username assigned in token";
    public const string NoValidCertificatesAssigned = "There is no certificate assigned to user";

    public const string AlreadyAuthenticated =
        "User is already authenticated using a certificate. Cannot authenticate using JWT";

    public const string PasswordNotSettable =
        "User '{0}' has not settable password, because it is not supported by protocol '{1}'";

    public const string WrongActiveCompany =
        "User '{0}' has wrong company assigned, active company is not in user's list.";

    public const string NoValidToken = "There is no valid token of requested bearer.";
    public const string UserDoesntExists = "There is no available user with ID '{0}'.";
    
    public const string UserAccountNotExists = "There is no available account for user '{0}'.";

    public const string UserAccountTypeDifferent =
        "Loaded account for user '{0}' is for different authentication provider. Current '{1}', needed '{2}'.";

    public const string NonExistingActiveCompany =
        "Requested switch to active company failed, such company ID '{0}' is not company of user '{1}'.";

    // -- Messaging
    public const string ForeignKeyNotFound = "Foreign key for navigation '{0}' not found.";


    // -- Paging
    public const string PagingWrongPage = "Page number must be equal or higher than zero.";
    public const string PagingWrongPageSize = "PageSize number must be higher than zero.";

    public const string BothSortParametersRequired =
        "Both sort parameter and sort direction must be provided together.";

    public const string SortParametersDoesntMatch = "Sort parameter does not match any property of the entity.";
    public const string WrongSortDirection = "Sort direction must be 'asc' or 'desc'.";

    public const string BothFilterParametersRequired =
        "Both filter parameter and filter value must be provided together.";

    public const string FilterParametersDoesntMatch = "Filter parameter does not match any property of the entity.";

    public const string ExpressionTypeNotSupported = "The expression type '{0}' is not supported.";
    public const string NotSupportedDbType = "The type '{0}' is not supported to be mapped to direct DB access.";
    public const string ValueCannotBeNull = "The value assigned to database operation cannot be null.";

    public const string ExpressionIsNotValidFormat =
        "The expression is not of valid format, it must be column selector compared with simple value (or simple non-nullable variable). IE. 'int number; i => i.Number > number;'";

    public const string ContextActionCancelled = "Action cancelled by invoking cancellation token";
    public const string ContextTooManyClients = "Too many clients connected, connection refused";
}