using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;



namespace WPF_LoginForm.Model
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("name=MyDbContext") { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Credito> Creditos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Creditos)
                .WithRequired(cr => cr.Cliente)
                .HasForeignKey(cr => cr.IdCliente);

            modelBuilder.Entity<Venta>()
                .HasOptional(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.IdCliente);

            modelBuilder.Entity<DetalleVenta>()
                .HasRequired(dv => dv.Venta)
                .WithMany(v => v.DetalleVentas)
                .HasForeignKey(dv => dv.IdVenta);

            modelBuilder.Entity<DetalleVenta>()
                .HasRequired(dv => dv.Producto)
                .WithMany()
                .HasForeignKey(dv => dv.IdProducto);

            modelBuilder.Entity<Producto>()
                .HasOptional(p => p.Inventario)
                .WithRequired(i => i.Producto);

            modelBuilder.Entity<DetalleCompra>()
                .HasRequired(dc => dc.Compra)
                .WithMany(c => c.DetalleCompras)
                .HasForeignKey(dc => dc.IdCompra);

            base.OnModelCreating(modelBuilder);
        }

    }
}
