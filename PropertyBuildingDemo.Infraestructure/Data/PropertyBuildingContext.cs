using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using System.Reflection;

namespace PropertyBuildingDemo.Infrastructure.Data
{
    public class PropertyBuildingContext : DbContext
    {
        public PropertyBuildingContext(DbContextOptions<PropertyBuildingContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyTrace> PropertyTraces { get; set; }
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
            //            //entry.Entity.Created = DateTime.Now;
            //            entry.Entity.CreatedBy = _currentUserService?.UserName ?? "System";
            //            break;
            //        case EntityState.Modified:
            //            //entry.Entity.LastModified = DateTime.Now;
            //            entry.Entity.ModifiedBy = _currentUserService?.UserName ?? "System";
            //            break;
            //    }
            //}

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
