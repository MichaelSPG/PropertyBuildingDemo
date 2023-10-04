using NUnit.Framework.Internal;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Diagnostics;
using System.Net;
using PropertyBuildingDemo.Tests.Helpers.Config;
using static PropertyBuildingDemo.Tests.Helpers.TestEndpoint;
using static PropertyBuildingDemo.Tests.Helpers.Config.BenchmarkingConfig;

namespace PropertyBuildingDemo.Tests.IntegrationTests.PerformanceFixtures
{
    /// <summary>
    /// NUnit test class for measuring performance and testing various data insertion/querying scenarios.
    /// </summary>
    public class BenchmarkingTests : PropertyBaseTest
    {
        [OneTimeSetUp]
        public async Task Setup()
        {
            await SetupValidRegistrationUser();
            await SetupPropertyTest(false);
            await InsertEntityDtoListWithApi<OwnerDto>(GetEndpoint<OwnerDto>().Insert, OwnerValidList);

            ValidTestEntityCount = 100;

            PropertyValidList = PropertyBuildingDataFactory.PropertyDataFactory.CreateValidEntityDtoList(ValidTestEntityCount, 
                OwnerValidList.FirstOrDefault().IdOwner).ToList();
            PropertyValidList = await InsertListOfEntity<Property, PropertyDto>(PropertyValidList);
        }

        /// <summary>
        /// Performs the insertion of owner data into the API.
        /// </summary>
        /// <param name="iterationIndex">The index of the iteration for tracking purposes.</param>
        /// <param name="maxFileSize">The maximum file size for owner data.</param>
        public async Task PerformInsertionOfOwnerData(int iterationIndex, int maxFileSize)
        {
            // Create a random owner with the specified maximum file size.
            var owner = OwnerDataFactory.CreateRandomOwner(maxFileSize, maxFileSize);

            // Make an API POST request to insert the owner data.
            var result = await HttpApiClient.MakeApiPostRequestAsync<Owner>(
                $"{GetEndpoint<OwnerDto>().Insert}",
                Is.EqualTo(HttpStatusCode.OK), owner);

            // Validate the API result for expected success.
            Utilities.ValidateApiResult_ExpectedSuccess(result);
        }

        /// <summary>
        /// Performs the insertion of property image data into the API.
        /// </summary>
        /// <param name="iterationIndex">The index of the iteration for tracking purposes.</param>
        /// <param name="imageMaxSize">The maximum image file size for property image data.</param>
        public async Task PerformInsertionOfPropertyImageData(int iterationIndex, int imageMaxSize)
        {
            // Create a random property image with the specified image maximum size.
            var image = PropertyImageDataFactory.CreateRandomPropertyImage(
                PropertyValidList.FirstOrDefault().IdProperty, imageMaxSize, imageMaxSize);

            // Make an API POST request to insert the property image data.
            var result = await HttpApiClient.MakeApiPostRequestAsync<Owner>(
                $"{GetEndpoint<PropertyImageDto>().Insert}",
                Is.EqualTo(HttpStatusCode.OK), image);

            // Validate the API result for expected success.
            Utilities.ValidateApiResult_ExpectedSuccess(result);
        }

        /// <summary>
        /// Performs the insertion of property trace data into the API.
        /// </summary>
        /// <param name="iterationIndex">The index of the iteration for tracking purposes.</param>
        public async Task PerformInsertionOfPropertyTraceData(int iterationIndex)
        {
            // Create a random property trace for the first property in the PropertyValidList.
            var trace = PropertyTraceDataFactory.CreateRandomPropertyTrace(
                PropertyValidList.FirstOrDefault().IdProperty);

            // Make an API POST request to insert the property trace data.
            var result = await HttpApiClient.MakeApiPostRequestAsync<Owner>(
                $"{GetEndpoint<PropertyTraceDto>().Insert}",
                Is.EqualTo(HttpStatusCode.OK), trace);

            // Validate the API result for expected success.
            Utilities.ValidateApiResult_ExpectedSuccess(result);
        }

        /// <summary>
        /// Performs the insertion of property data into the API.
        /// </summary>
        /// <param name="iterationIndex">The index of the iteration for tracking purposes.</param>
        /// <param name="minImages">The minimum number of images to associate with the property.</param>
        /// <param name="minTraces">The minimum number of traces to associate with the property.</param>
        /// <param name="imageBytesCount">The count of image bytes for property data.</param>
        public async Task PerformInsertionOfPropertyData(int iterationIndex, int minImages = 0, int minTraces = 0, int imageBytesCount = 0)
        {
            // Generate a random valid property with the specified parameters.
            var currentProperty = PropertyBuildingDataFactory.GenerateRandomValidProperties(1,
                GetValidOwnerIdList(), 0, false, minImages, minTraces, imageBytesCount).FirstOrDefault();

            // Make an API POST request to insert the property data.
            var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyDto>(
                $"{PropertyBuildingEnpoint.Insert}",
                Is.EqualTo(HttpStatusCode.OK), currentProperty);

            // Validate the API result for expected success.
            Utilities.ValidateApiResult_ExpectedSuccess(result);
        }

        /// <summary>
        /// Performs querying of property data using specified pagination arguments.
        /// </summary>
        /// <param name="index">The page index for pagination.</param>
        /// <param name="count">The page size for pagination.</param>
        public async Task PerformQueryingOfPropertyData(int index, int count)
        {
            // Create pagination arguments.
            DefaultQueryFilterArgs args = new DefaultQueryFilterArgs
            {
                PageSize = count,
                PageIndex = index
            };

            // Make an API POST request to retrieve property data with pagination.
            var result = await HttpApiClient.MakeApiPostRequestAsync<List<PropertyDto>>(
                $"{PropertyBuildingEnpoint.ListBy}",
                Is.EqualTo(HttpStatusCode.OK), args);

            // Validate the API result for expected success.
            Utilities.ValidateApiResult_ExpectedSuccess(result);
        }

        /// <summary>
        /// Measures the performance of a given insertion method asynchronously.
        /// </summary>
        /// <param name="insertionMethod">The insertion method to measure.</param>
        /// <param name="iterations">The number of iterations to perform.</param>
        /// <param name="avgMilliseconds">The expected average execution time in milliseconds.</param>
        public async Task MeasureFunction_PerformanceAsync(Func<int, Task> insertionMethod, int iterations, decimal avgMilliseconds)
        {
            // Measure the execution time of the insertion method
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                await insertionMethod(i);
            }
            stopwatch.Stop();

            Console.WriteLine($"Max Expected Time {avgMilliseconds * iterations} ms, Total time was {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Expected Avg {avgMilliseconds} ms, Result average of {stopwatch.Elapsed.TotalMilliseconds / iterations} ms");

            // Check if the execution time is within the allowed limit
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, iterations * avgMilliseconds);
        }


        /// <summary>
        /// Measures the performance of property insertion.
        /// </summary>
        /// <param name="iterations">The number of iterations to perform.</param>
        /// <param name="maxImages">The maximum number of images for property insertion.</param>
        /// <param name="maxTraces">The maximum number of traces for property insertion.</param>
        /// <param name="fileSize">The size of image files to be inserted.</param>
        [Test]
        [TestCase(20,  0, 0, EValidFileSize.Tiny)] // Test case: No Traces or Images
        [TestCase(20,  0, 5, EValidFileSize.Tiny)]  // Test case: No Images
        [TestCase(20,  5, 0, EValidFileSize.Tiny)] // Test case: No Traces
        [TestCase(20,  7, 7, EValidFileSize.Tiny)] // Test case: Images and traces
        [TestCase(20, 0, 5, EValidFileSize.Small)]  // Test case: No Images
        [TestCase(20, 5, 0, EValidFileSize.Small)] // Test case: No Traces
        [TestCase(20, 7, 7, EValidFileSize.Small)] // Test case: Images and traces
        [TestCase(20, 0, 5, EValidFileSize.Medium)]  // Test case: No Images
        [TestCase(20, 5, 0, EValidFileSize.Medium)] // Test case: No Traces
        [TestCase(20, 7, 7, EValidFileSize.Medium)] // Test case: Images and traces
        [TestCase(20, 0, 5, EValidFileSize.Large)]  // Test case: No Images
        [TestCase(20, 5, 0, EValidFileSize.Large)] // Test case: No Traces
        [TestCase(20, 7, 7, EValidFileSize.Large)] // Test case: Images and traces
        public async Task Measure_Property_InsertionPerformance(
            int iterations, int maxImages, int maxTraces, EValidFileSize fileSize)
        {
            decimal avgMilliseconds = PropertyPerformance.ComputeAvgTimeOfProperty(fileSize, maxImages, maxTraces);

            await MeasureFunction_PerformanceAsync(
                (int iterationIndex) => PerformInsertionOfPropertyData(iterationIndex, maxImages, maxTraces, GetValidFileSize(fileSize)),
                iterations, avgMilliseconds);
        }

        /// <summary>
        /// Measures the performance of owner data insertion with different iterations and file sizes.
        /// </summary>
        /// <param name="iterations">The number of iterations to perform.</param>
        /// <param name="fileSize">The size of files for owner data insertion.</param>
        [Test]
        [TestCase(100, EValidFileSize.Tiny)]
        [TestCase(200, EValidFileSize.Tiny)]
        [TestCase(500, EValidFileSize.Tiny)]
        [TestCase(1000, EValidFileSize.Tiny)]
        [TestCase(2000, EValidFileSize.Tiny)]
        [TestCase(100, EValidFileSize.Medium)]
        [TestCase(200, EValidFileSize.Medium)]
        [TestCase(500, EValidFileSize.Medium)]
        [TestCase(500, EValidFileSize.Large)]
        public async Task Measure_Owner_InsertionPerformance(int iterations, EValidFileSize fileSize)
        {
            // Measure the performance of owner data insertion.
            await MeasureFunction_PerformanceAsync(
                (int iterationIndex) => PerformInsertionOfOwnerData(iterationIndex, GetValidFileSize(fileSize)),
                iterations, OwnerPerformance.AverageTimeMapping[fileSize]);
        }

        /// <summary>
        /// Measures the performance of querying property data with different iterations.
        /// </summary>
        /// <param name="iterations">The number of iterations to perform.</param>
        [Test]
        [TestCase(20)]
        [TestCase(30)]
        [TestCase(40)]
        [TestCase(100)]
        public async Task Measure_Property_GetByPerformance(int iterations)
        {
            // Measure the performance of querying property data.
            await MeasureFunction_PerformanceAsync(
                (int iterationIndex) => PerformQueryingOfPropertyData(iterationIndex, ValidTestEntityCount / iterations),
                iterations, PropertyPerformance.AvgGetByTime);
        }

        /// <summary>
        /// Measures the performance of property trace data insertion with different iterations.
        /// </summary>
        /// <param name="iterations">The number of iterations to perform.</param>
        [Test]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(40)]
        [TestCase(100)]
        [TestCase(200)]
        [TestCase(500)]
        [TestCase(1000)]
        public async Task Measure_PropertyTrace_InsertPerformance(int iterations)
        {
            // Measure the performance of property trace data insertion.
            await MeasureFunction_PerformanceAsync(
                PerformInsertionOfPropertyTraceData,
                iterations, PropertyTracePerformance.AvgInsertionTime);
        }

        /// <summary>
        /// Measures the performance of property image data insertion with different iterations and file sizes.
        /// </summary>
        /// <param name="iterations">The number of iterations to perform.</param>
        /// <param name="fileSize">The size of files for property image data insertion.</param>
        [Test]
        [TestCase(10, EValidFileSize.Tiny)]
        [TestCase(50, EValidFileSize.Tiny)]
        [TestCase(100, EValidFileSize.Tiny)]
        [TestCase(200, EValidFileSize.Tiny)]
        [TestCase(500, EValidFileSize.Tiny)]
        [TestCase(1000, EValidFileSize.Tiny)]
        [TestCase(2000, EValidFileSize.Tiny)]
        public async Task Measure_PropertyImage_InsertionPerformance(int iterations, EValidFileSize fileSize)
        {
            // Measure the performance of property image data insertion.
            await MeasureFunction_PerformanceAsync(
                (int iterationIndex) => PerformInsertionOfPropertyImageData(iterationIndex, GetValidFileSize(fileSize)),
                iterations, PropertyImagePerformance.AverageTimeMapping[fileSize]);
        }
    }
    
}