using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Tests.Helpers
{
    public partial class Utilities
    {
        public static class RandomGenerators
        {
            public static byte[] GenerateRandomByteArray(int min = 512, int max = 1024)
            {
                int num = Random.Next(min, max);
                byte[] buffer = new byte[num];
                Random.NextBytes(buffer);
                return buffer;
            }

            public static string GenerateValidRandomAddress()
            {
                string[] streets = { "Main St", "Elm St", "Maple Ave", "Oak Ln", "Cedar Rd" };
                string[] cities = { "New York", "Los Angeles", "Chicago", "Houston", "Miami" };

                var randomStreet = streets[Random.Next(streets.Length)];
                var randomCity = cities[Random.Next(cities.Length)];

                return $"{randomStreet}, {randomCity}";
            }

            public static string GenerateUniqueRandomName()
            {
                string[] firstNames =
                    { "Alice", "Bob", "Charlie", "David", "Emma", "Frank", "Grace", "Henry", "Ivy", "Jack" };
                string[] lastNames =
                {
                    "Smith", "Johnson", "Brown", "Davis", "Wilson", "Miller", "Anderson", "Martinez", "Garcia",
                    "Jackson"
                };

                var randomFirstName = firstNames[Random.Next(firstNames.Length)];
                var randomLastName = lastNames[Random.Next(lastNames.Length)];

                return $"{randomFirstName} {randomLastName}";
            }

            public static DateTime GenerateValidAgeRandomDate()
            {
                var years = Random.Next(18, 99);
                var start = DateTime.Now.AddYears(-years); // Adjust the range as needed
                var randomDay = Random.Next(365);

                start = start.AddDays(-randomDay);

                return start;
            }

            public static decimal GenerateRandomDecimal(decimal minValue, decimal maxValue)
            {
                if (minValue >= maxValue)
                {
                    throw new ArgumentException("minValue must be less than maxValue.");
                }

                decimal range = maxValue - minValue;
                decimal randomValue = (decimal)Random.NextDouble() * range + minValue;
                return decimal.Round(randomValue, 2); // Round to 2 decimal places if needed
            }

            public static DateTime GenerateRandomDateInPast(int maxYearsAgo = 50)
            {
                int daysAgo = Random.Next(1, maxYearsAgo * 365);
                DateTime currentDate = DateTime.Now;
                DateTime randomDate = currentDate.AddDays(-daysAgo);
                return randomDate;
            }

            public static bool GenerateRandomBool()
            {
                // Generate a random integer (0 or 1) and convert it to a boolean value
                return Random.Next(2) == 1;
            }

            public static string GenerateRandomBuildingName()
            {
                List<string> buildingPrefixes = new List<string>
                {
                    "Skyview", "Riverside", "Serenity", "Urban Oasis", "Harmony",
                    "Cityscape", "Emerald", "Beacon Ridge", "Golden Gate", "Sunset Shores",
                    "Pinecrest", "Mountain View", "Lakeside", "Parkside", "Meadowbrook",
                    "Horizon", "Coastal Crest", "Willowbrook", "Metropolitan View", "Azure Skies"
                };

                List<string> buildingSuffixes = new List<string>
                {
                    "Towers", "Residences", "Heights", "Apartments", "Gardens",
                    "Suites", "Penthouses", "Condos", "Estates", "Villas",
                    "Palms", "Lofts", "Haven", "Plaza", "Manor"
                };
                string prefix = buildingPrefixes[Random.Next(buildingPrefixes.Count)];
                string suffix = buildingSuffixes[Random.Next(buildingSuffixes.Count)];

                return $"{prefix} {suffix}";
            }
        }
    }
}
