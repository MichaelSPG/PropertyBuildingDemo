using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    public static class PropertyBuildingDataFactory
    {
        public static OwnerDto CreateValidTestOwnerDto()
        {
            byte[] buffer = new byte[201];
            Utilities.Random.NextBytes(buffer);
            return new OwnerDto()
            {
                BirthDay = DateTime.Now.AddYears(-20),
                Address = GenerateValidRandomAddress(),
                Name = "Alfred Newman",
                Photo = buffer,
            };
        }
        public static List<OwnerDto> CreateValidTestOwnerList(int count)
        {
            var random = new Random();
            var ownerDtos = new List<OwnerDto>();

            string[] firstNames = { "Alice", "Bob", "Charlie", "David", "Emma", "Frank", "Grace", "Henry", "Ivy", "Jack" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Davis", "Wilson", "Miller", "Anderson", "Martinez", "Garcia", "Jackson" };


            var usedNames = new HashSet<string>();

            while (ownerDtos.Count < count)
            {
                var displayName = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";

                var password = GenerateRandomByteArray();

                // Ensure email and identification number are unique
                if (!usedNames.Contains(displayName))
                {
                    var owner = new OwnerDto
                    {
                        Name = displayName,
                        BirthDay = GenerateValidAgeRandomDate(),
                        Photo = GenerateRandomByteArray(),
                        Address = GenerateValidRandomAddress()
                    };

                    ownerDtos.Add(owner);
                    usedNames.Add(displayName);
                }
            }
            usedNames.Clear();
            return ownerDtos;
        }

        private static HashSet<string> generatedNames = new HashSet<string>();

        public static OwnerDto CreateRandomOwner()
        {
            var owner = new OwnerDto()
            {
                BirthDay = GenerateValidAgeRandomDate(),
                Address = GenerateValidRandomAddress(),
                Name = GenerateUniqueRandomName(),
                Photo = GenerateRandomByteArray()
            };

            return owner;
        }

        public static DateTime GenerateValidAgeRandomDate()
        {
            int years = Utilities.Random.Next(18, 99);
            DateTime start = DateTime.Now.AddYears(-years); // Adjust the range as needed
            int randomDay = Utilities.Random.Next(365);

            start = start.AddDays(randomDay);
            return start;
        }

        private static string GenerateValidRandomAddress()
        {
            string[] streets = { "Main St", "Elm St", "Maple Ave", "Oak Ln", "Cedar Rd" };
            string[] cities = { "New York", "Los Angeles", "Chicago", "Houston", "Miami" };

            string randomStreet = streets[Utilities.Random.Next(streets.Length)];
            string randomCity = cities[Utilities.Random.Next(cities.Length)];

            return $"{randomStreet}, {randomCity}";
        }

        private static string GenerateUniqueRandomName()
        {
            string[] firstNames = { "Alice", "Bob", "Charlie", "David", "Emma", "Frank", "Grace", "Henry", "Ivy", "Jack" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Davis", "Wilson", "Miller", "Anderson", "Martinez", "Garcia", "Jackson" };

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
