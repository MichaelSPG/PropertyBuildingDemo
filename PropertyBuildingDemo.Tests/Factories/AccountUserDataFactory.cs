using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;
using System.Text.RegularExpressions;

namespace PropertyBuildingDemo.Tests.Factories
{
    /// <summary>
    /// Factory class for generating user-related data for testing purposes.
    /// </summary>
    public static class AccountUserDataFactory
    {
        /// <summary>
        /// Creates a valid test user registration DTO with predefined values.
        /// </summary>
        /// <returns>A UserRegisterDto instance with valid test data.</returns>
        public static UserRegisterDto CreateValidTestUserForRegister()
        {
            return new UserRegisterDto
            {
                DisplayName = "Charles Junior",
                Email = "charles.junior@someemail.com",
                IdentificationNumber = 1230129,
                Password = "C@1213f"
            };
        }

        /// <summary>
        /// Generates a list of valid test user registration DTOs with unique email addresses and identification numbers.
        /// </summary>
        /// <param name="count">The number of user registration DTOs to generate.</param>
        /// <returns>A list of UserRegisterDto instances with valid test data.</returns>
        public static List<UserRegisterDto> GetValidTestUserForRegisterList(int count)
        {
            var random = new Random();
            var userDtos = new List<UserRegisterDto>();

            string[] firstNames = { "Charles", "Alice", "Bob", "Emily", "David", "Sarah" };
            string[] lastNames = { "Junior", "Smith", "Johnson", "Brown", "Williams" };
            string[] domains = { "example.com", "someemail.com", "testmail.com", "mailprovider.com" };

            var usedEmails = new HashSet<string>();
            var usedIdentificationNumbers = new HashSet<int>();

            while (userDtos.Count < count)
            {
                var displayName = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";
                var email = $"{firstNames[random.Next(firstNames.Length)].ToLower()}.{lastNames[random.Next(lastNames.Length)].ToLower()}@{domains[random.Next(domains.Length)]}";
                var identificationNumber = random.Next(1000, 9999999);
                var password = GenerateRandomValidPassword(random);

                // Ensure email and identification number are unique
                if (!usedEmails.Contains(email) && !email.ToLower().Contains(CreateValidTestUserForRegister().Email.ToLower()))
                {
                    var userDto = new UserRegisterDto
                    {
                        DisplayName = displayName,
                        Email = email,
                        IdentificationNumber = identificationNumber,
                        Password = password
                    };

                    userDtos.Add(userDto);
                    usedEmails.Add(email);
                    usedIdentificationNumbers.Add(identificationNumber);
                }
            }
            usedIdentificationNumbers.Clear();
            usedEmails.Clear();
            return userDtos;
        }

        /// <summary>
        /// Generates a list of invalid test user registration DTOs by changing passwords of valid users.
        /// </summary>
        /// <param name="count">The number of invalid user registration DTOs to generate.</param>
        /// <returns>A list of UserRegisterDto instances with invalid passwords.</returns>
        public static List<UserRegisterDto> GetInvalidTestUserForRegisterList(int count)
        {
            var random = new Random();
            List<UserRegisterDto> list = GetValidTestUserForRegisterList(count);
            foreach (var item in list)
            {
                item.Password = GenerateInvalidRandomPassword(random);
            }
            return list;
        }

        /// <summary>
        /// Generates a random valid password based on predefined patterns.
        /// </summary>
        /// <param name="random">A Random instance for generating random data.</param>
        /// <returns>A random valid password.</returns>
        public static string GenerateRandomValidPassword(Random random)
        {
            string password;
            do
            {
                int passwordLength = random.Next(6, 15); // Password length between 6 and 10
                password = new string(Enumerable.Repeat(GetAllowedCharacters(), passwordLength)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (!IsValidPassword(password, TestConstants.PasswordPattern));

            return password;
        }

        /// <summary>
        /// Checks if a given password matches a specified pattern.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="pattern">The pattern to match against.</param>
        /// <returns>True if the password matches the pattern; otherwise, false.</returns>
        public static bool IsValidPassword(string password, string pattern)
        {
            return Regex.IsMatch(password, pattern);
        }

        /// <summary>
        /// Gets a string containing all allowed characters for password generation.
        /// </summary>
        /// <returns>A string containing all allowed characters for password generation.</returns>
        public static string GetAllowedCharacters()
        {
            return TestConstants.UppercaseChars + TestConstants.LowercaseChars + TestConstants.DigitChars + TestConstants.SymbolChars;
        }

        /// <summary>
        /// Generates a random invalid password with only lowercase characters.
        /// </summary>
        /// <param name="random">A Random instance for generating random data.</param>
        /// <returns>A random invalid password with only lowercase characters.</returns>
        public static string GenerateInvalidRandomPassword(Random random)
        {
            var passwordLength = random.Next(6, 11); // Password length between 6 and 10
            var password = new string(Enumerable.Repeat(TestConstants.LowercaseChars, passwordLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }

        /// <summary>
        /// Creates a UserDto instance with custom properties.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <param name="username">The user's display name.</param>
        /// <param name="email">The user's email address.</param>
        /// <returns>A UserDto instance with custom properties.</returns>
        public static UserDto CreateUserWithCustomProperties(string id, string username, string email)
        {
            return new UserDto
            {
                Id = id,
                DisplayName = username,
                Email = email,
            };
        }
    }
}
