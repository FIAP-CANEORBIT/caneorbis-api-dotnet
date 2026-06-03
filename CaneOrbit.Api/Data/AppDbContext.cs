using CaneOrbis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CaneOrbis.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Propriedade> Propriedades { get; set; }
        public DbSet<Talhao> Talhoes { get; set; }
        public DbSet<DispositivoIot> DispositivosIot { get; set; }
        public DbSet<LeituraSensor> LeiturasSensor { get; set; }
        public DbSet<DadoSatelite> DadosSatelite { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("T_ORB_USUARIO");
                entity.HasKey(e => e.IdUsuario);

                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
                entity.Property(e => e.NmUsuario).HasColumnName("NM_USUARIO").HasMaxLength(100).IsRequired();
                entity.Property(e => e.DsEmail).HasColumnName("DS_EMAIL").HasMaxLength(100).IsRequired();
                entity.Property(e => e.DsSenhaHash).HasColumnName("DS_SENHA_HASH").HasMaxLength(255).IsRequired();
                entity.Property(e => e.DtCadastro)
    .HasColumnName("DT_CADASTRO")
    .HasColumnType("TIMESTAMP")
    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(e => e.DsEmail).IsUnique();
            });

            modelBuilder.Entity<Propriedade>(entity =>
            {
                entity.ToTable("T_ORB_PROPRIEDADE");
                entity.HasKey(e => e.IdPropriedade);

                entity.Property(e => e.IdPropriedade).HasColumnName("ID_PROPRIEDADE");
                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO").IsRequired();
                entity.Property(e => e.NmPropriedade).HasColumnName("NM_PROPRIEDADE").HasMaxLength(100).IsRequired();
                entity.Property(e => e.DsLocalizacao).HasColumnName("DS_LOCALIZACAO").HasMaxLength(150);
                entity.Property(e => e.VlAreaHectare).HasColumnName("VL_AREA_HECTARE").HasColumnType("NUMBER(10,2)");

                entity.HasOne(e => e.Usuario)
                    .WithMany(e => e.Propriedades)
                    .HasForeignKey(e => e.IdUsuario);
            });

            modelBuilder.Entity<Talhao>(entity =>
            {
                entity.ToTable("T_ORB_TALHAO");
                entity.HasKey(e => e.IdTalhao);

                entity.Property(e => e.IdTalhao).HasColumnName("ID_TALHAO");
                entity.Property(e => e.IdPropriedade).HasColumnName("ID_PROPRIEDADE").IsRequired();
                entity.Property(e => e.NmTalhao).HasColumnName("NM_TALHAO").HasMaxLength(100).IsRequired();
                entity.Property(e => e.VlAreaHectare).HasColumnName("VL_AREA_HECTARE").HasColumnType("NUMBER(10,2)");
                entity.Property(e => e.DsTipoSolo).HasColumnName("DS_TIPO_SOLO").HasMaxLength(80);
                entity.Property(e => e.DsCultura).HasColumnName("DS_CULTURA").HasMaxLength(80).HasDefaultValue("CANA_DE_ACUCAR");

                entity.HasOne(e => e.Propriedade)
                    .WithMany(e => e.Talhoes)
                    .HasForeignKey(e => e.IdPropriedade);
            });

            modelBuilder.Entity<DispositivoIot>(entity =>
            {
                entity.ToTable("T_ORB_DISPOSITIVO_IOT");
                entity.HasKey(e => e.IdDispositivo);

                entity.Property(e => e.IdDispositivo).HasColumnName("ID_DISPOSITIVO");
                entity.Property(e => e.IdTalhao).HasColumnName("ID_TALHAO").IsRequired();
                entity.Property(e => e.DsMacAddress).HasColumnName("DS_MAC_ADDRESS").HasMaxLength(17).IsRequired();
                entity.Property(e => e.NmApelido).HasColumnName("NM_APELIDO").HasMaxLength(50);
                entity.Property(e => e.VlLatitude).HasColumnName("VL_LATITUDE").HasColumnType("NUMBER(10,8)");
                entity.Property(e => e.VlLongitude).HasColumnName("VL_LONGITUDE").HasColumnType("NUMBER(11,8)");
                entity.Property(e => e.DsStatusDispositivo).HasColumnName("DS_STATUS_DISPOSITIVO").HasMaxLength(20).HasDefaultValue("ATIVO");
                entity.Property(e => e.DtInstalacao)
    .HasColumnName("DT_INSTALACAO")
    .HasColumnType("DATE")
    .HasDefaultValueSql("SYSDATE");

                entity.HasIndex(e => e.DsMacAddress).IsUnique();

                entity.HasOne(e => e.Talhao)
                    .WithMany(e => e.DispositivosIot)
                    .HasForeignKey(e => e.IdTalhao);
            });

            modelBuilder.Entity<LeituraSensor>(entity =>
            {
                entity.ToTable("T_ORB_LEITURA_SENSOR");
                entity.HasKey(e => e.IdLeitura);

                entity.Property(e => e.IdLeitura).HasColumnName("ID_LEITURA");
                entity.Property(e => e.IdDispositivo).HasColumnName("ID_DISPOSITIVO").IsRequired();
                entity.Property(e => e.VlUmidadeSolo).HasColumnName("VL_UMIDADE_SOLO").HasColumnType("NUMBER(10,2)").IsRequired();
                entity.Property(e => e.VlTemperatura).HasColumnName("VL_TEMPERATURA").HasColumnType("NUMBER(10,2)").IsRequired();
                entity.Property(e => e.VlPhSolo).HasColumnName("VL_PH_SOLO").HasColumnType("NUMBER(4,2)");
                entity.Property(e => e.DtLeitura)
    .HasColumnName("DT_LEITURA")
    .HasColumnType("TIMESTAMP")
    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.DispositivoIot)
                    .WithMany(e => e.LeiturasSensor)
                    .HasForeignKey(e => e.IdDispositivo);
            });

            modelBuilder.Entity<DadoSatelite>(entity =>
            {
                entity.ToTable("T_ORB_DADO_SATELITE");
                entity.HasKey(e => e.IdDadoSatelite);

                entity.Property(e => e.IdDadoSatelite).HasColumnName("ID_DADO_SATELITE");
                entity.Property(e => e.IdDispositivo).HasColumnName("ID_DISPOSITIVO").IsRequired();
                entity.Property(e => e.VlNdvi).HasColumnName("VL_NDVI").HasColumnType("NUMBER(10,4)");
                entity.Property(e => e.VlPrecipitacao).HasColumnName("VL_PRECIPITACAO").HasColumnType("NUMBER(10,2)");
                entity.Property(e => e.VlTemperaturaAr).HasColumnName("VL_TEMPERATURA_AR").HasColumnType("NUMBER(10,2)");
                entity.Property(e => e.DsCondicaoClima).HasColumnName("DS_CONDICAO_CLIMA").HasMaxLength(50);
                entity.Property(e => e.DtColeta)
    .HasColumnName("DT_COLETA")
    .HasColumnType("TIMESTAMP")
    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.DispositivoIot)
                    .WithMany(e => e.DadosSatelite)
                    .HasForeignKey(e => e.IdDispositivo);
            });
        }
    }
}