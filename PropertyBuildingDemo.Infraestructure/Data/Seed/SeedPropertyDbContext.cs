using PropertyBuildingDemo.Domain.Entities;
using System.Text;

namespace PropertyBuildingDemo.Infrastructure.Data.Seed
{
    public static class SeedPropertyDbContext
    {
        public static async Task SeedPropertyBuildingData(PropertyBuildingContext dbContext)
        {
            if (dbContext.Property.Any())
            {
                return;
            }
            var owners = new List<Owner>
            {
                new Owner
                {
                    Name = "John Doe",
                    Address = "123 Main Street",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 1"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1980, 5, 15)
                },
                new Owner
                {
                    Name = "Jane Smith",
                    Address = "456 Elm Street",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 2"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1992, 8, 27)
                },
                new Owner
                {
                    Name = "Michael Johnson",
                    Address = "789 Oak Avenue",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 3"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1975, 3, 8)
                },
                new Owner
                {
                    Name = "Sara Adams",
                    Address = "101 Pine Road",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 4"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1988, 11, 2)
                },
                new Owner
                {
                    Name = "Robert Wilson",
                    Address = "222 Cedar Lane",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 5"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1995, 7, 20)
                },
                new Owner
                {
                    Name = "Emily Brown",
                    Address = "333 Willow Street",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 6"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1983, 12, 10)
                },
                new Owner
                {
                    Name = "Daniel Lee",
                    Address = "444 Birch Avenue",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 7"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1990, 9, 17)
                },
                new Owner
                {
                    Name = "Olivia White",
                    Address = "555 Maple Road",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 8"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1987, 6, 5)
                },
                new Owner
                {
                    Name = "William Taylor",
                    Address = "666 Spruce Lane",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 9"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1979, 4, 30)
                },
                new Owner
                {
                    Name = "Sophia Harris",
                    Address = "777 Oak Avenue",
                    Photo = Encoding.UTF8.GetBytes("Sample Photo 10"), // Replace with an actual photo byte array
                    BirthDay = new DateTime(1986, 2, 15)
                }
            };

            var properties = new List<Property>();

            for (int i = 1; i <= 10; i++)
            {
                var owner = owners[i % owners.Count]; // Cycle through owners for each property

                properties.Add(new Property
                {
                    Name = $"Property {i}",
                    Address = $"Address {i}",
                    Price = 500000 + (i * 10000),
                    Year = (short)(2010 + i),
                    Owner = owner,
                    PropertyImages = new List<PropertyImage>
                    {
                        new PropertyImage
                        {
                            File = Encoding.UTF8.GetBytes($"Image Data {i}"),
                            Enabled = true
                        }
                    },
                    PropertyTraces = new List<PropertyTrace>
                    {
                        new PropertyTrace
                        {
                            DateSale = new DateTime(2023 + i, 5, 15),
                            Name = $"Sale {i}",
                            Value = 500000 + (i * 10000),
                            Tax = 5000 + (i * 100)
                        }
                    }
                });
            }
            dbContext.Property.AddRange(properties);
            await dbContext.SaveChangesAsync();
        }
    }
}
