using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface ICurrentUserService
    {
        public string UserId { get; }
        public string UserName { get; }
        public List<KeyValuePair<string, string>> Claims { get; set; }
        public string Role { get; }
    }
}
