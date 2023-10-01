using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Tests.Factories
{
    public static class PropertyBuildingDataFactory
    {
        public static OwnerDto CreateValidTestOwnerDto()
        {
            return new OwnerDto()
            {
                BirthDay = DateTime.Now.AddYears(-20),
                Address = "st aca 233, av 34 # 44",
                Name = "Alfred Newman",
                Photo = new byte[]{2, 4, 3},
            };
        }
        public static List<OwnerDto> CreateValidTestOwnerList(int count)
        {
            var userDtos = new List<OwnerDto>();

            while (userDtos.Count < count)
            {
                var owner = CreateRandomOwner();
                // Ensure email and identification number are unique
                if (!userDtos.Any(x => x.Name.Equals(owner.Name)))
                {
                    userDtos.Add(owner);
                }
            }

            return userDtos;
        }
        
        private static HashSet<string> generatedNames = new HashSet<string>();

        public static OwnerDto CreateRandomOwner()
        {
            var owner = new OwnerDto()
            {
                BirthDay = GenerateRandomDate(),
                Address = GenerateRandomAddress(),
                Name = GenerateUniqueRandomName(),
                Photo = GenerateRandomByteArray()
            };

            return owner;
        }

        private static DateTime GenerateRandomDate()
        {
            DateTime start = DateTime.Now.AddYears(-30); // Adjust the range as needed
            int range = (DateTime.Today - start).Days;
            return start.AddDays(Utilities.Random.Next(range));
        }

        private static string GenerateRandomAddress()
        {
            string[] streets = { "Main St", "Elm St", "Maple Ave", "Oak Ln", "Cedar Rd" };
            string[] cities = { "New York", "Los Angeles", "Chicago", "Houston", "Miami" };

            string randomStreet = streets[Utilities.Random.Next(streets.Length)];
            string randomCity = cities[Utilities.Random.Next(cities.Length)];

            return $"{randomStreet}, {randomCity}";
        }

        private static string GenerateUniqueRandomName()
        {
            string[] firstNames = { "Alice", "Bob", "Charlie", "David", "Emma" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Davis", "Wilson" };

            string randomFirstName;
            string randomLastName;
            string fullName;

            do
            {
                randomFirstName = firstNames[Utilities.Random.Next(firstNames.Length)];
                randomLastName = lastNames[Utilities.Random.Next(lastNames.Length)];
                fullName = $"{randomFirstName} {randomLastName}";
            } while (!generatedNames.Add(fullName)); // Ensure the name is unique

            return fullName;
        }

        public static byte[] GenerateRandomByteArray()
        {
            //int num = Utilities.Random.Next(512, 1024);
            byte[] buffer = new byte[1243];
            Utilities.Random.NextBytes(buffer);
            return buffer;
        }
    }
}
