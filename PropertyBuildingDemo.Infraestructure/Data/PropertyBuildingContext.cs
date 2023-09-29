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
    public class PropertyBuildingContext : ApiAuthorizationDbContext<AppUser>
    {
        //private readonly ICurrentUserService _currentUserService;
        public PropertyBuildingContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions/*, ICurrentUserService currentUserService*/)
            : base(options, operationalStoreOptions)
        {
            //_currentUserService = currentUserService;
        }

        public DbSet<Property> Property { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<PropertyImage> PropertyImage { get; set; }
        public DbSet<PropertyTrace> PropertyTrace { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            //foreach (var entry in ChangeTracker.Entries<BaseAuditableEntityDB>().ToList())
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            entry.Entity.CreatedBy = _currentUserService?.UserName ?? "System";
            //            break;
            //        case EntityState.Modified:
            //            entry.Entity.ModifiedBy = _currentUserService?.UserName ?? "System";
            //            break;
            //    }
            //}

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
