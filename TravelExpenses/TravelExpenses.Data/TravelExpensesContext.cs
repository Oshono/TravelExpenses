using TravelExpenses.Core;
using Microsoft.EntityFrameworkCore;

namespace TravelExpenses.Data
{
    public class TravelExpensesContext: DbContext
    {
        public TravelExpensesContext(DbContextOptions<TravelExpensesContext> options)
            : base(options)
        {

        }

        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Empresas> CatEmpresas { get; set; }
        public DbSet<Departamentos> CatDepartamentos { get; set; }
        public DbSet<Gastos> CatGastos { get; set; }
        public DbSet<Comprobante> Comprobante { get; set; }
        public DbSet<Concepto> Concepto { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Destinos> Destinos { get; set; }
        public DbSet<CentroCosto> CatCentroCostos { get; set; }
        public DbSet<CentroCostoEmpresa> CentroCosto_Empresa { get; set; }
        public DbSet<Ciudades> Ciudades { get; set; }
        public DbSet<Paises> Paises { get; set; }
        public DbSet<Moneda> CatMonedas { get; set; }
        public DbSet<Politica> Politica { get; set; }
        public DbSet<PoliticaDetalle> PoliticaDetalle { get; set; }
        public DbSet<CatProdServSAT> CatProdServSAT { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CentroCostoEmpresa>().HasKey(table => new {
                table.ClaveCentroCosto,
                table.RFC
            });

            builder.Entity<PoliticaDetalle>().HasKey(table => new {
                table.IdPolitica,
                table.IdGasto
            });
        }
    }
}
