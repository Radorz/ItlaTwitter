using System;
using ItlaTwitter.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ItlaTwitter.Models
{
    public partial class itlaTwitterContext : DbContext
    {
        public itlaTwitterContext()
        {
        }
       
        public itlaTwitterContext(DbContextOptions<itlaTwitterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Amigos> Amigos { get; set; }
        public virtual DbSet<Comentario> Comentario { get; set; }
        public virtual DbSet<Publicaciones> Publicaciones { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<PublicacionesViewModel> Publicaciones2 { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("cc");
            }
            optionsBuilder.EnableSensitiveDataLogging();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amigos>(entity =>
            {
                entity.Property(e => e.Id)
                                   .HasColumnName("id")
                                   .ValueGeneratedOnAdd();

                entity.Property(e => e.Idenvia).HasColumnName("idenvia");

                entity.Property(e => e.Idrecibe).HasColumnName("idrecibe");

                entity.HasOne(d => d.IdenviaNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Idenvia)
                    .HasConstraintName("FK_Amigos_envia");

                entity.HasOne(d => d.IdrecibeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Idrecibe)
                    .HasConstraintName("FK_Amigo_recibe");
            });

            modelBuilder.Entity<Comentario>(entity =>
            {

                entity.Property(e => e.Contenido)
                    .HasColumnName("contenido")
                    .HasMaxLength(1000)
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Idpublicacion).HasColumnName("idpublicacion");

                entity.Property(e => e.Idusuario).HasColumnName("idusuario");
            });

            modelBuilder.Entity<Publicaciones>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Contenido)
                    .HasMaxLength(1000)
                    .IsFixedLength();

                entity.Property(e => e.Idusuario).HasColumnName("idusuario");

                entity.HasOne(d => d.IdusuarioNavigation)
                    .WithMany(p => p.Publicaciones)
                    .HasForeignKey(d => d.Idusuario)
                    .HasConstraintName("FK_Publicaciones_Usuarios");
                entity.Property(e => e.Fecha).HasColumnName("Fecha");

            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Estado)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Telefono)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.Usuario)
                    .HasMaxLength(15)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
