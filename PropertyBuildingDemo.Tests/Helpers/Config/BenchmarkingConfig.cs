using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Tests.Helpers.Config
{
    /// <summary>
    /// Configuration class for benchmarking various performance metrics.
    /// </summary>
    public class BenchmarkingConfig
    {
        /// <summary>
        /// Enumeration for valid file sizes.
        /// </summary>
        public enum EValidFileSize
        {
            Tiny,
            Small,
            Medium,
            Large,
        }

        /// <summary>
        /// Dictionary mapping file size enums to corresponding byte sizes.
        /// </summary>
        private static readonly Dictionary<EValidFileSize, int> SizeMapping = new()
        {
            { EValidFileSize.Tiny, 256 },
            { EValidFileSize.Small, 512 },
            { EValidFileSize.Medium, 1024 },
            { EValidFileSize.Large, 2048 },
        };

        /// <summary>
        /// Gets the byte size for a given file size enum.
        /// </summary>
        /// <param name="size">The file size enum.</param>
        /// <returns>The corresponding byte size.</returns>
        public static int GetValidFileSize(EValidFileSize size)
        {
            return SizeMapping.TryGetValue(size, out var fileSize) ? fileSize : 256;
        }

        /// <summary>
        /// Gets the file size enum for a given byte size.
        /// </summary>
        /// <param name="fileSize">The byte size.</param>
        /// <returns>The corresponding file size enum.</returns>
        public static EValidFileSize GetValidSizeEnum(int fileSize)
        {
            if (fileSize <= 256)
            {
                return EValidFileSize.Tiny;
            }
            else if (fileSize <= 512)
            {
                return EValidFileSize.Small;
            }
            else if (fileSize <= 1024)
            {
                return EValidFileSize.Medium;
            }
            else if (fileSize <= 2048)
            {
                return EValidFileSize.Large;
            }
            else
            {
                return EValidFileSize.Tiny; // Default value
            }
        }

        /// <summary>
        /// Constants for specific valid file sizes in bytes.
        /// </summary>
        public const int SmallValidFileSize = 256; // Represents a file of 256 bytes

        public const int MediumValidFileSize = 512; // Represents a file of 512 bytes
        public const int LargeValidFileSize = 1024; // Represents a file of 1024 bytes

        /// <summary>
        /// Nested class containing benchmarks for property image performance tests.
        /// </summary>
        public static class PropertyImagePerformance
        {
            /// <summary>
            /// Dictionary mapping file size enums to average image insertion times in milliseconds.
            /// </summary>
            public static readonly Dictionary<EValidFileSize, decimal> AverageTimeMapping = new()
            {
                { EValidFileSize.Tiny, 5 },
                { EValidFileSize.Small, 10 },
                { EValidFileSize.Medium, 20 },
                { EValidFileSize.Large, 30 },
            };
        }

        /// <summary>
        /// Nested class containing the benchmark for property trace insertion performance.
        /// </summary>
        public static class PropertyTracePerformance
        {
            /// <summary>
            /// Average insertion time for property traces in milliseconds.
            /// </summary>
            public const decimal AvgInsertionTime = 5;
        }

        /// <summary>
        /// Nested class containing benchmarks for owner performance tests.
        /// </summary>
        public static class OwnerPerformance
        {
            /// <summary>
            /// Dictionary mapping file size enums to average owner insertion times in milliseconds.
            /// </summary>
            public static readonly Dictionary<EValidFileSize, decimal> AverageTimeMapping = new()
            {
                { EValidFileSize.Tiny, 5.15M },
                { EValidFileSize.Small, 6 },
                { EValidFileSize.Medium, 7 },
                { EValidFileSize.Large, 9 },
            };
        }

        /// <summary>
        /// Nested class containing various performance benchmarks related to property tests.
        /// </summary>
        public static class PropertyPerformance
        {
            /// <summary>
            /// Average time for property retrieval in milliseconds.
            /// </summary>
            public const decimal AvgGetByTime = 14.2M;

            /// <summary>
            /// Average time for property insertion in milliseconds.
            /// </summary>
            public const decimal AvgInsertionTime = 10.4M;

            /// <summary>
            /// Reduction in time for image insertion (percentage).
            /// </summary>
            public const decimal ImageTimeReduction = 0.2M;

            /// <summary>
            /// Average time for insertion with property traces in milliseconds.
            /// </summary>
            public const decimal AvgWithTraceInsertionTime = 1.6M;

            /// <summary>
            /// Dictionary mapping file size enums to average image insertion times in milliseconds.
            /// </summary>
            public static readonly Dictionary<EValidFileSize, decimal> AverageImageInsertionTimeMapping = new()
            {
                { EValidFileSize.Tiny, 1.8M },
                { EValidFileSize.Small, 3.0M },
                { EValidFileSize.Medium, 4.7M },
                { EValidFileSize.Large, 6.0M },
            };

            /// <summary>
            /// Calculates the average time for property insertion based on file size, image count, and trace count.
            /// </summary>
            /// <param name="fileSize">The file size enum.</param>
            /// <param name="countImages">The count of images to insert.</param>
            /// <param name="countTraces">The count of property traces to insert.</param>
            /// <returns>The calculated average time in milliseconds.</returns>
            public static decimal ComputeAvgTimeOfProperty(EValidFileSize fileSize, int countImages, int countTraces)
            {
                var avg = AvgInsertionTime;
                avg += GetAvgOfFile(countImages, fileSize);
                avg += AvgWithTraceInsertionTime * countTraces;
                return avg;
            }

            /// <summary>
            /// Calculates the average time for inserting a specific number of images of a given file size.
            /// </summary>
            /// <param name="count">The count of images to insert.</param>
            /// <param name="size">The file size enum.</param>
            /// <returns>The calculated average time in milliseconds.</returns>
            public static decimal GetAvgOfFile(int count, EValidFileSize size)
            {
                decimal fileAvg = AverageImageInsertionTimeMapping[size];
                return count * fileAvg * ImageTimeReduction;
            }
        }
    }
}
