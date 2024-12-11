using ExpressYourself.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpressYourself.Infrastructure.Persistence;

public partial class ExpressYourselfContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<IpAddress> Ipaddresses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ThreeLetterCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TwoLetterCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<IpAddress>(entity =>
        {
            entity.ToTable("IPAddresses");

            entity.HasIndex(e => e.Ip, "IX_IPAddresses").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Country).WithMany(p => p.IpAddresses)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IPAddresses_Countries");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
