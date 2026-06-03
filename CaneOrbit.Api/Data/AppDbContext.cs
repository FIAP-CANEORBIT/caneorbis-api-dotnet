using CaneOrbis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CaneOrbis.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<DispositivoIot> DispositivosIot { get; set; }
        public DbSet<LeituraSensor> LeiturasSensor { get; set; }
        public DbSet<DadoSatelite> DadosSatelite { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DispositivoIot>(entity =>
            {
                entity.ToTable("T_ORB_DISPOSITIVO_IOT");

                entity.HasKey(e => e.IdDispositivo);

                entity.Property(e => e.IdDispositivo)
                    .HasColumnName("ID_DISPOSITIVO");
            });

            modelBuilder.Entity<LeituraSensor>(entity =>
            {
                entity.ToTable("T_ORB_LEITURA_SENSOR");

                entity.HasKey(e => e.IdLeitura);

                entity.Property(e => e.IdLeitura)
                    .HasColumnName("ID_LEITURA");

                entity.Property(e => e.IdDispositivo)
                    .HasColumnName("ID_DISPOSITIVO")
                    .IsRequired();

                entity.Property(e => e.VlUmidadeSolo)
                    .HasColumnName("VL_UMIDADE_SOLO")
                    .HasColumnType("NUMBER(10,2)")
                    .IsRequired();

                entity.Property(e => e.VlTemperatura)
                    .HasColumnName("VL_TEMPERATURA")
                    .HasColumnType("NUMBER(10,2)")
                    .IsRequired();

                entity.Property(e => e.VlPhSolo)
                    .HasColumnName("VL_PH_SOLO")
                    .HasColumnType("NUMBER(4,2)");

                entity.Property(e => e.DtLeitura)
                    .HasColumnName("DT_LEITURA")
                    .HasColumnType("TIMESTAMP");

                entity.HasOne(e => e.DispositivoIot)
                    .WithMany(e => e.LeiturasSensor)
                    .HasForeignKey(e => e.IdDispositivo);
            });

            modelBuilder.Entity<DadoSatelite>(entity =>
            {
                entity.ToTable("T_ORB_DADO_SATELITE");

                entity.HasKey(e => e.IdDadoSatelite);

                entity.Property(e => e.IdDadoSatelite)
                    .HasColumnName("ID_DADO_SATELITE");

                entity.Property(e => e.IdDispositivo)
                    .HasColumnName("ID_DISPOSITIVO")
                    .IsRequired();

                entity.Property(e => e.VlNdvi)
                    .HasColumnName("VL_NDVI")
                    .HasColumnType("NUMBER(10,4)");

                entity.Property(e => e.VlPrecipitacao)
                    .HasColumnName("VL_PRECIPITACAO")
                    .HasColumnType("NUMBER(10,2)");

                entity.Property(e => e.VlTemperaturaAr)
                    .HasColumnName("VL_TEMPERATURA_AR")
                    .HasColumnType("NUMBER(10,2)");

                entity.Property(e => e.DsCondicaoClima)
                    .HasColumnName("DS_CONDICAO_CLIMA")
                    .HasMaxLength(50);

                entity.Property(e => e.DtColeta)
                    .HasColumnName("DT_COLETA")
                    .HasColumnType("TIMESTAMP");

                entity.HasOne(e => e.DispositivoIot)
                    .WithMany(e => e.DadosSatelite)
                    .HasForeignKey(e => e.IdDispositivo);
            });
        }
    }
}