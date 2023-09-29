using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Infrastructure.Config
{
    public class ApplicationConfig
    {
        public string BaseUrl { get; set; }
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireInMinutes{ get; set; }
    }
}
