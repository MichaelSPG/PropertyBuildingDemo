using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Threading;
using System.Diagnostics;

namespace PropertyBuildingDemo.Tests.IntegrationTests.PerformanceFixtures
{
    public class BenchmarkingTests : PropertyBaseTest
    {
        
        [SetUp]
        public async Task Setup()
        {
            await SetupValidRegistrationUser();
            await SetupPropertyTest();
        }

        public async Task PerformInsertionOfPropertyData(int minImages = 0, int minTraces = 0, int maxImages = 0, int maxTraces = 0 )
        {
            int countImages = minImages;
            int countTraces = minTraces;

            if (maxTraces > minTraces)
            {
                countTraces = Utilities.Random.Next(minTraces, maxTraces);
            }
            if (maxImages > minImages)
            {
                countTraces = Utilities.Random.Next(minImages, maxImages);
            }

            var currentProperty =
                PropertyBuildingDataFactory.GenerateRandomValidProperties(1,
                    GetValidOwnerIdList(), 0, false, countImages, countTraces).FirstOrDefault();

            var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.Insert}",
                Is.EqualTo(HttpStatusCode.OK), currentProperty);
            Utilities.ValidateApiResultData_ExpectedSuccess(result);
        }

        [Test]
        public async Task MeasureInsertionPerformance_NoTracesOrImages()
        {
            int iterations = 2000;
            int maxMilliseconds = 11000;
            // Measure the execution time of the method
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                
                // Simulate a time-consuming operation
                // Replace this with the code you want to measure
                await PerformInsertionOfPropertyData();
            }
            stopwatch.Stop();

            Console.WriteLine($"Total time was {stopwatch.ElapsedMilliseconds}, Average of {stopwatch.Elapsed.TotalMilliseconds / iterations} millis");

            // Check if the execution time is within the allowed limit
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, maxMilliseconds);
        }

        [Test]
        public async Task MeasureInsertionPerformance_NoImages()
        {
            const int iterations = 200;
            const int maxMilliseconds = 2000;
            // Measure the execution time of the method
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
            {
                // Simulate a time-consuming operation
                // Replace this with the code you want to measure
                await PerformInsertionOfPropertyData(0, 1);
                
            }
            stopwatch.Stop();

            Console.WriteLine($"Total time was {stopwatch.ElapsedMilliseconds}, Average of {stopwatch.Elapsed.TotalMilliseconds / iterations} millis");

            // Check if the execution time is within the allowed limit
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, maxMilliseconds);

            
        }
    }
}