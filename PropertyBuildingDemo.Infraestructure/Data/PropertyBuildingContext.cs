using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Domain.Interfaces;
using System.Reflection;

namespace PropertyBuildingDemo.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework DbContext for the PropertyBuilding application.
    /// </summary>
    public class PropertyBuildingContext : ApiAuthorizationDbContext<AppUser>
    {
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBuildingContext"/> class.
        /// </summary>
        /// <param name="options">The DbContext options.</param>
        /// <param name="operationalStoreOptions">The IdentityServer operational store options.</param>
        /// <param name="currentUserService">The current user service for tracking user information.</param>
        public PropertyBuildingContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService)
            : base(options, operationalStoreOptions)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Property> Property { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<PropertyImage> PropertyImage { get; set; }
        public DbSet<PropertyTrace> PropertyTrace { get; set; }

        /// <summary>
        /// Configures the model using configurations from the current assembly.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .Property(p => p.InternalCode)
                .HasDefaultValue(Guid.NewGuid().ToString());

            modelBuilder.Entity<PropertyImage>()
                .HasOne(i => i.Property)
                .WithMany(p => p.PropertyImages)
                .HasForeignKey(i => i.IdProperty);

            modelBuilder.Entity<PropertyTrace>()
                .HasOne(i => i.Property)
                .WithMany(p => p.PropertyTraces)
                .HasForeignKey(i => i.IdProperty);

            // INDICES

            modelBuilder.Entity<Owner>()
                .HasIndex(p => p.Name);
            modelBuilder.Entity<Owner>()
                .HasIndex(p => p.IdOwner);

            modelBuilder.Entity<Property>()
                .HasIndex(p => p.IdProperty);
            modelBuilder.Entity<Property>()
                .HasIndex(p => p.Name);
            modelBuilder.Entity<Property>()
                .HasIndex(p => p.IdOwner);

            modelBuilder.Entity<PropertyTrace>()
                .HasIndex(p => p.Name);
            modelBuilder.Entity<PropertyTrace>()
                .HasIndex(p => p.IdPropertyTrace);
            modelBuilder.Entity<PropertyTrace>()
                .HasIndex(p => p.IdProperty);

            modelBuilder.Entity<PropertyImage>()
                .HasIndex(p => p.IdPropertyImage);
            modelBuilder.Entity<PropertyImage>()
                .HasIndex(p => p.IdProperty);


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Configures the DbContext options to use lazy loading proxies.
        /// </summary>
        /// <param name="optionsBuilder">The DbContext options builder.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Saves changes to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An integer representing the number of state entries written to the database.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntityDb>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService?.UserName ?? "System";
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _currentUserService?.UserName ?? "System";
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        
        ///// <summary>
        ///// Retrieves the count of properties owned by a specific owner identified by their ID.
        ///// </summary>
        ///// <param name="ownerId">The ID of the owner for whom the property count is retrieved.</param>
        ///// <returns>
        ///// A list of tuples containing the owner's ID and the count of properties they own.
        ///// Each tuple is represented as (IdOwner, PropertyCount).
        ///// </returns>
        //[DbFunction("dbo", "GetOwnersWithPropertyCounts")]
        //public virtual List<(long IdOwner, int PropertyCount)> GetOwnersWithPropertyCounts(long ownerId)
        //{
        //    var ownersWithPropertyCounts = Property
        //        .Where(p => p.IdOwner == ownerId)
        //        .GroupBy(p => p.IdOwner)
        //        .Select(g => new
        //        {
        //            IdOwner = g.Key,
        //            PropertyCount = g.Count()
        //        })
        //        .ToList()
        //        .Select(result => ((long)result.IdOwner, result.PropertyCount))
        //        .ToList();

        //    return ownersWithPropertyCounts;
        //}
        // commented, just to illustrate I'm able to create stores procedures, native in EFCore
    }
}
