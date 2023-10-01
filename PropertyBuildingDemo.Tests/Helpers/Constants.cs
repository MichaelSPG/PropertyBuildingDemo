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

        public struct AccountEnpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/Account";
            public const string Login = $"{BaseEndpoint}/Login";
            public const string Register = $"{BaseEndpoint}/Register";
            public const string ExistsEmail = $"{BaseEndpoint}/ExistsEmail";
            public const string CurrentUser = $"{BaseEndpoint}/CurrentUser";
        }

        public struct OwnerEnpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/Owner";
            public const string List = $"{BaseEndpoint}/List";
            public const string Insert = $"{BaseEndpoint}/Insert";
            public const string ById = $"{BaseEndpoint}/ById";
        }

        public struct PropertyImageEnpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/PropertyImage";
            public const string List = $"{BaseEndpoint}/List";
        }

        public struct PropertyTraceEnpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/PropertyTrace";
            public const string List = $"{BaseEndpoint}/List";
        }

        public struct PropertyBuildingEnpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/PropertyBuilding";
            public const string ListBy = $"{BaseEndpoint}/ListBy";
            public const string ById = $"{BaseEndpoint}/ById";
            //public const string ListBy = $"{BaseEndpoint}/ListBy";
            //public const string ListBy = $"{BaseEndpoint}/ListBy";
            //public const string ListBy = $"{BaseEndpoint}/ListBy";
        }
    }
}
