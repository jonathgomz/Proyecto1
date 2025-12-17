using SistemaDeEncuestas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SistemaDeEncuestas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Encuesta> Encuestas { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<OpcionPregunta> OpcionesPregunta { get; set; }
        public DbSet<RespuestaUsuario> RespuestasUsuario { get; set; }
        public DbSet<RespuestaDetalle> RespuestasDetalle { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Encuesta>().ToTable("Encuesta");
            modelBuilder.Entity<Pregunta>().ToTable("Pregunta");
            modelBuilder.Entity<OpcionPregunta>().ToTable("OpcionPregunta");
            modelBuilder.Entity<RespuestaUsuario>().ToTable("RespuestaUsuario");
            modelBuilder.Entity<RespuestaDetalle>().ToTable("RespuestaDetalle");
        }
    }
}