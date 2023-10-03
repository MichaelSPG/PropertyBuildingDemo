using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace PropertyBuildingDemo.Tests.Factories
{
    public static class AccountUserDataFactory
    {
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

        public static bool IsValidPassword(string password, string pattern)
        {
            return Regex.IsMatch(password, pattern);
        }

        public static string GetAllowedCharacters()
        {
            return TestConstants.UppercaseChars + TestConstants.LowercaseChars + TestConstants.DigitChars + TestConstants.SymbolChars;
        }

        public static string GenerateInvalidRandomPassword(Random random)
        {
            int passwordLength = random.Next(6, 11); // Password length between 6 and 10
            string password = new string(Enumerable.Repeat(TestConstants.LowercaseChars, passwordLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }

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
