namespace PropertyBuildingDemo.Tests.Helpers
{
    /// <summary>
    /// Constants class containing various test-related constants.
    /// </summary>
    public class TestConstants
    {
        /// <summary>
        /// String of lowercase characters.
        /// </summary>
        public const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Base path for API endpoints.
        /// </summary>
        public const string BaseApiPath = "/Api";

        /// <summary>
        /// Regular expression pattern for valid passwords.
        /// </summary>
        public const string PasswordPattern = @"^(?=.{5,25}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{"":;'?/>.<,])(?!.*\s).*$";

        /// <summary>
        /// String of uppercase characters.
        /// </summary>
        public const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// String of digit characters.
        /// </summary>
        public const string DigitChars = "0123456789";

        /// <summary>
        /// String of symbol characters.
        /// </summary>
        public const string SymbolChars = "!@#$%^&*()_+}{\":;'?/>.<,.";

        public const string ValidExpiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjU5YWUxOGVmLWNjNDEtNDc1YS04MDM3LTE2Mjg5NjYyOTk5ZSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImNocmlzdG9waGVyOTJAc3R3bmV0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL21vYmlsZXBob25lIjoiIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy91c2VyZGF0YSI6IjEyMzU5MjMzIiwiZXhwIjoxNjk1OTc2NTI1LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTQwLyIsImF1ZCI6ImNocmlzdG9waGVyOTJAc3R3bmV0LmNvbSJ9.HoS0bvsoiOtYaIpcmTEGQlnXQam9e3VpulGKGPxmY8E";

    }
}
