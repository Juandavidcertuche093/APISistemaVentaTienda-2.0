using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            var mysqlVersion = new MySqlServerVersion(new Version(8, 3, 0));//Cambia la versión según tu servidor MySQL
            services.AddDbContext<MyDbContext>(Options =>
            {
                Options.UseMySql(configuration.GetConnectionString("cadenaMySQL"), mysqlVersion);
            });

            ////Repositorios
            //services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IVentaRepository, VentaRepository>();
            //services.AddScoped<ICompraRepository, CompraRepository>();

            ////agregamos la dependencia de los mapeos
            //services.AddAutoMapper(typeof(AutoMapperProfile));

            ////agragamos las dependencias de los servicios o logica de negocio
            //services.AddScoped<IRolService, RolService>();
            //services.AddScoped<IUsuarioService, UsuarioService>();
            //services.AddScoped<ICategoriaService, CategoriaService>();
            //services.AddScoped<IImagenProductoService, ImagenProductoService>();
            //services.AddScoped<IProductoService, ProductoService>();
            //services.AddScoped<IVentaService, VentaService>();
            //services.AddScoped<ICompraService, CompraService>();
            //services.AddScoped<IDashboardService, DashboardService>();
            //services.AddScoped<IMenuService, MenuService>();
            //services.AddScoped<IProveedorService, ProveedorService>();
            //services.AddScoped<IProductoEmpaqueService, ProductoEmpaqueService>();
            //services.AddScoped<IPresentacionService, PresentacionService>();
        }
    }
}
