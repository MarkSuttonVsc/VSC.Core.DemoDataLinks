
using Microsoft.EntityFrameworkCore;
using VSC.Core.DataModel;
using VSC.Core.DemoDataLinks.Model;

namespace VSC.Core.DemoDataLinks.Data;

public partial class DemoDatabaseContext : DbContext
{
    public DemoDatabaseContext()
    {
    }

    public DemoDatabaseContext(DbContextOptions<DemoDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContactData> ContactData { get; set; }

    public virtual DbSet<ContactGroup> ContactGroups { get; set; }

    public virtual DbSet<ContactType> ContactTypes { get; set; }

    public virtual DbSet<Interest> Interests { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonalInterest> PersonalInterests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactData>(entity =>
        {
            entity.HasKey(e => e.ContactDataId);

            entity.Property(e => e.ContactDataId).ValueGeneratedNever();
            entity.Property(e => e.Detail).HasMaxLength(500);
            entity.Property(e => e.Inserted).HasColumnType("datetime");
            entity.Property(e => e.Updated).HasColumnType("datetime");

            entity.HasOne(d => d.ContactType).WithMany(p => p.ContactData)
                .HasForeignKey(d => d.ContactTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContactData_ContactTypes");

            entity.HasOne(d => d.Person).WithMany(p => p.ContactData)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContactData_People");
        });

        modelBuilder.Entity<ContactGroup>(entity =>
        {
            entity.Property(e => e.ContactGroupId).ValueGeneratedNever();
            entity.Property(e => e.Inserted).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Updated).HasColumnType("datetime");
        });

        modelBuilder.Entity<ContactType>(entity =>
        {
            entity.Property(e => e.ContactTypeId).ValueGeneratedNever();
            entity.Property(e => e.Inserted).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Updated).HasColumnType("datetime");
        });

        modelBuilder.Entity<Interest>(entity =>
        {
            entity.Property(e => e.InterestId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Inserted).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Updated).HasColumnType("datetime");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.PersonId).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Inserted).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Updated).HasColumnType("datetime");

            entity.HasOne(d => d.ContactGroup).WithMany(p => p.People)
                .HasForeignKey(d => d.ContactGroupId)
                .HasConstraintName("FK_People_ContactGroups");
        });

        modelBuilder.Entity<PersonalInterest>(entity =>
        {
            entity.HasIndex(e => new { e.PersonId, e.InterestId }, "UQ_PersonalInterests").IsUnique();

            entity.Property(e => e.PersonalInterestId).ValueGeneratedNever();
            entity.Property(e => e.Inserted).HasColumnType("datetime");
            entity.Property(e => e.Updated).HasColumnType("datetime");

            entity.HasOne(d => d.Interest).WithMany(p => p.PersonalInterests)
                .HasForeignKey(d => d.InterestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalInterests_Interests");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonalInterests)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalInterests_People");
        });

        

        

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
