using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Tests.Helpers
{
    public partial class Utilities
    {
        /// <summary>
        /// A static class that provides methods for generating random data.
        /// </summary>
        public static class RandomGenerators
        {
            /// <summary>
            /// Generates a random byte array with a specified length range.
            /// </summary>
            /// <param name="min">The minimum length of the byte array.</param>
            /// <param name="max">The maximum length of the byte array.</param>
            /// <returns>A random byte array within the specified length range.</returns>
            public static byte[] GenerateRandomByteArray(int min = 512, int max = 1024)
            {
                int num = Random.Next(min, max);
                byte[] buffer = new byte[num];
                Random.NextBytes(buffer);
                return buffer;
            }

            /// <summary>
            /// Generates a random valid address in the format "Street, City".
            /// </summary>
            /// <returns>A random valid address string.</returns>
            public static string GenerateValidRandomAddress()
            {
                string[] streets = { "Main St", "Elm St", "Maple Ave", "Oak Ln", "Cedar Rd" };
                string[] cities = { "New York", "Los Angeles", "Chicago", "Houston", "Miami" };

                var randomStreet = streets[Random.Next(streets.Length)];
                var randomCity = cities[Random.Next(cities.Length)];

                return $"{randomStreet}, {randomCity}";
            }

            /// <summary>
            /// Generates a unique random name combining a random first name and last name.
            /// </summary>
            /// <returns>A unique random full name string.</returns>
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

            /// <summary>
            /// Generates a random date of birth for a valid age.
            /// </summary>
            /// <returns>A random date of birth that corresponds to a valid age.</returns>
            public static DateTime GenerateValidAgeRandomDate()
            {
                var years = Random.Next(18, 99);
                var start = DateTime.Now.AddYears(-years); // Adjust the range as needed
                var randomDay = Random.Next(365);

                start = start.AddDays(-randomDay);

                return start;
            }

            /// <summary>
            /// Generates a random decimal number within a specified range.
            /// </summary>
            /// <param name="minValue">The minimum value of the decimal number.</param>
            /// <param name="maxValue">The maximum value of the decimal number.</param>
            /// <returns>A random decimal number within the specified range.</returns>
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

            /// <summary>
            /// Generates a random date in the past within a specified number of years.
            /// </summary>
            /// <param name="maxYearsAgo">The maximum number of years ago for the generated date.</param>
            /// <returns>A random date in the past within the specified range.</returns>
            public static DateTime GenerateRandomDateInPast(int maxYearsAgo = 50)
            {
                int daysAgo = Random.Next(1, maxYearsAgo * 365);
                DateTime currentDate = DateTime.Now;
                DateTime randomDate = currentDate.AddDays(-daysAgo);
                return randomDate;
            }

            /// <summary>
            /// Generates a random boolean value (true or false).
            /// </summary>
            /// <returns>A random boolean value.</returns>
            public static bool GenerateRandomBool()
            {
                // Generate a random integer (0 or 1) and convert it to a boolean value
                return Random.Next(2) == 1;
            }

            /// <summary>
            /// Generates a random building name by combining a random prefix and suffix.
            /// </summary>
            /// <returns>A random building name string.</returns>
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
