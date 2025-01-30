using Microsoft.EntityFrameworkCore;
using WargaGrpc.Models;

namespace WargaGrpc.Data;

public class WargaContext : DbContext
{
    public WargaContext(DbContextOptions<WargaContext> options) : base(options)
    {
    }

    public DbSet<WargaModel> Warga { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WargaModel>(entity =>
        {
            entity.ToTable("warga");
            entity.HasKey(e => e.IdWarga);
            entity.Property(e => e.IdWarga).HasColumnName("id_warga");
            entity.Property(e => e.Nik).HasColumnName("nik").HasMaxLength(16);
            entity.Property(e => e.NamaLengkap).HasColumnName("nama_lengkap").HasMaxLength(100);
            entity.Property(e => e.TempatLahir).HasColumnName("tempat_lahir").HasMaxLength(50);
            entity.Property(e => e.JenisKelamin).HasColumnName("jenis_kelamin").HasMaxLength(10);
            entity.Property(e => e.Alamat).HasColumnName("alamat");
        });
    }
}