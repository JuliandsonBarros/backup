using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sisat.Models
{
    public partial class SisatContext : DbContext
    {
        public SisatContext()
        {
        }

        public SisatContext(DbContextOptions<SisatContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Conveniados> Conveniados { get; set; } = null!;
        public virtual DbSet<ConvenioProjeto> ConvenioProjeto { get; set; } = null!;
        public virtual DbSet<PacotesAtualizacoes> PacotesAtualizacoes { get; set; } = null!;
        public virtual DbSet<Projetos> Projetos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("data Source=CPADSI39\\SQLEXPRESS;Initial Catalog=Sis_Atualizacoes; Integrated Security=True;");
                optionsBuilder.UseSqlServer("data Source=HOME_PC\\SQLEXPRESS;Initial Catalog=Sis_Atualizacoes; Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conveniados>(entity =>
            {
                entity.HasKey(e => e.IdConveniado)
                    .HasName("PK__Convenia__24F606D1F4D4B101");
            });

            modelBuilder.Entity<ConvenioProjeto>(entity =>
            {
                entity.HasKey(e => new { e.IdCon, e.IdProj })
                    .HasName("PK__Convenio__1D45BDBD0E36289A");

                entity.HasOne(d => d.IdConNavigation)
                    .WithMany(p => p.ConvenioProjeto)
                    .HasForeignKey(d => d.IdCon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_CONVENIADO");
            });

            modelBuilder.Entity<PacotesAtualizacoes>(entity =>
            {
                entity.HasKey(e => e.IdPacote)
                    .HasName("PK__Pacotes___B2C7136C9DD61923");

                entity.HasOne(d => d.IdProjNavigation)
                    .WithMany(p => p.PacotesAtualizacoes)
                    .HasForeignKey(d => d.IdProj)
                    .HasConstraintName("FK_ID_PROJETO");
            });

            modelBuilder.Entity<Projetos>(entity =>
            {
                entity.HasKey(e => e.IdProjeto)
                    .HasName("PK__Projetos__6701DEA94F9BE6A0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
