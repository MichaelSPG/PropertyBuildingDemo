using Microsoft.AspNetCore.Http;
using PropertyBuildingDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Duende.IdentityServer.Models.IdentityResources;

namespace PropertyBuildingDemo.Application.Services
{
    //public class CurentUserService : ICurrentUserService
    //{
    //    public CurentUserService(IHttpContextAccessor httpContextAccessor)
    //    {
    //        UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    //        UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    //        Role = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    //        Claims = httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
    //    }
    //    public string UserId { get; }
    //    public string UserName { get; }
    //    public List<KeyValuePair<string, string>> Claims { get; set; }
    //    public string Role { get; }
    //}
}
