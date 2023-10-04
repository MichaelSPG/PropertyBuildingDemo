namespace PropertyBuildingDemo.Tests.Helpers.Config
{
    /// <summary>
    /// An interface representing the basic structure of an API endpoint URL.
    /// </summary>
    public interface IEndpointUrl
    {
        string List { get; }      // URL for listing resources
        string Insert { get; }    // URL for inserting a new resource
        string Update { get; }    // URL for updating an existing resource
        string Delete { get; }    // URL for deleting a resource
        string ById { get; }      // URL for retrieving a resource by its unique identifier
    }

    /// <summary>
    /// A struct that defines API endpoint URLs related to account management.
    /// </summary>
    public struct AccountEndpoint
    {
        public const string BaseEndpoint = $"{TestConstants.BaseApiPath}/Account";
        public const string Login = $"{BaseEndpoint}/Login";             // URL for user login
        public const string Register = $"{BaseEndpoint}/Register";       // URL for user registration
        public const string ExistsEmail = $"{BaseEndpoint}/ExistsEmail"; // URL for checking if an email exists
        public const string CurrentUser = $"{BaseEndpoint}/CurrentUser"; // URL for retrieving the current user
    }

    /// <summary>
    /// A struct that defines API endpoint URLs related to owner resources.
    /// </summary>
    public struct OwnerEndpoint : IEndpointUrl
    {
        public OwnerEndpoint()
        {
        }

        public const string BaseEndpoint = $"{TestConstants.BaseApiPath}/Owner";
        public string List { get; } = $"{BaseEndpoint}/List";
        public string Insert { get; } = $"{BaseEndpoint}/Insert";
        public string Update { get; } = $"{BaseEndpoint}/Update";
        public string Delete { get; } = $"{BaseEndpoint}/Delete";
        public string ById { get; } = $"{BaseEndpoint}/ById";
    }

    /// <summary>
    /// A struct that defines API endpoint URLs related to property image resources.
    /// </summary>
    public struct PropertyImageEndpoint : IEndpointUrl
    {
        public const string BaseEndpoint = $"{TestConstants.BaseApiPath}/PropertyImage";

        public PropertyImageEndpoint()
        {
        }

        public string List { get; } = $"{BaseEndpoint}/List";
        public string Insert { get; } = $"{BaseEndpoint}/Insert";
        public string Update { get; } = $"{BaseEndpoint}/Update";
        public string Delete { get; } = $"{BaseEndpoint}/Delete";
        public string ById { get; } = $"{BaseEndpoint}/ById";
    }

    /// <summary>
    /// A struct that defines API endpoint URLs related to property trace resources.
    /// </summary>
    public struct PropertyTraceEndpoint : IEndpointUrl
    {
        public const string BaseEndpoint = $"{TestConstants.BaseApiPath}/PropertyTrace";

        public PropertyTraceEndpoint()
        {
        }

        public string List { get; } = $"{BaseEndpoint}/List";
        public string Insert { get; } = $"{BaseEndpoint}/Insert";
        public string Update { get; } = $"{BaseEndpoint}/Update";
        public string Delete { get; } = $"{BaseEndpoint}/Delete";
        public string ById { get; } = $"{BaseEndpoint}/ById";
    }

    /// <summary>
    /// A struct that defines API endpoint URLs related to property building resources.
    /// </summary>
    public struct PropertyBuildingEnpoint
    {
        public const string BaseEndpoint = $"{TestConstants.BaseApiPath}/PropertyBuilding";
        public const string ListBy = $"{BaseEndpoint}/ListBy";
        public const string ById = $"{BaseEndpoint}/ById";

        public static string Update { get; set; } = $"{BaseEndpoint}/Update";
        public static string Insert { get; set; } = $"{BaseEndpoint}/Create";
        public static string AddImage { get; set; } = $"{BaseEndpoint}/AddImageFromProperty";

        public const string ChangePrice = $"{BaseEndpoint}/ChangePrice";
    }

}
