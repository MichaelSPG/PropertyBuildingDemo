using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities.Identity;

namespace PropertyBuildingDemo.Tests.Factories
{
    public static class TokenDataFactory
    {
        public static TokenResponse CreateExpiredTokenResponse()
        {
            return new TokenResponse()
            {
                RefreshToken = Guid.NewGuid().ToString(),
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjU5YWUxOGVmLWNjNDEtNDc1YS04MDM3LTE2Mjg5NjYyOTk5ZSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImNocmlzdG9waGVyOTJAc3R3bmV0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL21vYmlsZXBob25lIjoiIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy91c2VyZGF0YSI6IjEyMzU5MjMzIiwiZXhwIjoxNjk1OTc2NTI1LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTQwLyIsImF1ZCI6ImNocmlzdG9waGVyOTJAc3R3bmV0LmNvbSJ9.HoS0bvsoiOtYaIpcmTEGQlnXQam9e3VpulGKGPxmY8E",
                TokenExpiryTime = DateTime.UtcNow,
            };
        }

        public static TokenResponse CreateCorruptedTokenResponseValue() 
        {
            return new TokenResponse()
            {
                RefreshToken = Guid.NewGuid().ToString(),
                Token = "eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWl",
                TokenExpiryTime = null,
            };
        }
        public static TokenRequest CreateTokenRequestCustom(string userName = "", string password = "")
        {
            return new TokenRequest()
            {
                Username = userName,
                Password = password
            };
        }
    }
}
