using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace PropertyBuildingDemo.Tests.Helpers
{
    public class TestConstants
    {
        public const string BaseApiPath = "/Api";


        public struct AccountEnpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/Account";
            public const string Login = $"{BaseEndpoint}/Login";
            public const string Register = $"{BaseEndpoint}/Register";
            public const string CheckEmail = $"{BaseEndpoint}/ExistsEmail";
            public const string CurrentUser = $"{BaseEndpoint}/CurrentUser";
        }

        public struct OwnerEnpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/Owner";
            public const string List = $"{BaseEndpoint}/List";
        }

        public struct PropertyEnpoint
        {
            public const string BaseEndpoint = $"{BaseApiPath}/Property";
            public const string List = $"{BaseEndpoint}/List";
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
        }
    }
}
