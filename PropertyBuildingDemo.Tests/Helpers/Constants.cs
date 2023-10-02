using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.CompilerServices;

namespace PropertyBuildingDemo.Tests.Helpers
{
    public class TestConstants
    {

        public const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";

        public const string BaseApiPath = "/Api";
        public const string PasswordPattern = @"^(?=.{5,25}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{"":;'?/>.<,])(?!.*\s).*$";
        public const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string DigitChars = "0123456789";
        public const string SymbolChars = "!@#$%^&*()_+}{\":;'?/>.<,.";

        public struct AccountEndpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/Account";
            public const string Login = $"{BaseEndpoint}/Login";
            public const string Register = $"{BaseEndpoint}/Register";
            public const string ExistsEmail = $"{BaseEndpoint}/ExistsEmail";
            public const string CurrentUser = $"{BaseEndpoint}/CurrentUser";
        }

        public struct OwnerEndpoint : IEndpointUrl
        {
            public OwnerEndpoint()
            {
            }

            public const string BaseEndpoint = $"{BaseApiPath}/Owner";
            public string List    { get; } = $"{BaseEndpoint}/List";
            public string Insert  { get; } = $"{BaseEndpoint}/Insert";
            public string Update  { get; } = $"{BaseEndpoint}/Update";
            public string Delete  { get; } = $"{BaseEndpoint}/Delete";
            public string ById    { get; } = $"{BaseEndpoint}/ById";
        }

        public struct PropertyImageEndpoint : IEndpointUrl
        {
            public const string BaseEndpoint = $"{BaseApiPath}/PropertyImage";

            public PropertyImageEndpoint()
            {
            }

            public string List   { get; } = $"{BaseEndpoint}/List";
            public string Insert { get; } = $"{BaseEndpoint}/Insert";
            public string Update { get; } = $"{BaseEndpoint}/Update";
            public string Delete { get; } = $"{BaseEndpoint}/Delete";
            public string ById   { get; } = $"{BaseEndpoint}/ById";
        }

        public struct PropertyTraceEndpoint : IEndpointUrl
        {
            public const string BaseEndpoint = $"{BaseApiPath}/PropertyTrace";

            public PropertyTraceEndpoint()
            {
            }

            public string List { get; } = $"{BaseEndpoint}/List";
            public string Insert { get; } = $"{BaseEndpoint}/Insert";
            public string Update { get; } = $"{BaseEndpoint}/Update";
            public string Delete { get; } = $"{BaseEndpoint}/Delete";
            public string ById { get; } = $"{BaseEndpoint}/ById";
        }

        public struct PropertyBuildingEnpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/PropertyBuilding";
            public const string ListBy = $"{BaseEndpoint}/ListBy";
            public const string ById = $"{BaseEndpoint}/ById";

            public static string Update { get; set; } = $"{BaseEndpoint}/Update";
            public static string Insert { get; set; } = $"{BaseEndpoint}/Create";
            public static string AddImage { get; set; } = $"{BaseEndpoint}/AddImageFromProperty";

            public const string  ChangePrice = $"{BaseEndpoint}/ChangePrice";
        }

        public interface IEndpointUrl
        {
            string List { get; }
            string Insert{ get; }
            string Update { get; }
            string Delete { get; }
            string ById { get; }
        }

    }
}
