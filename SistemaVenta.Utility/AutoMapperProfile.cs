using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Model.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.Utility
{
    public class AutoMapperProfile : Profile //profile le pertecen a AutoMapper
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion

            #region Proveedor
            CreateMap<Proveedor, ProveedorDTO>()
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo.HasValue && origen.EsActivo.Value == 1UL ? 1 : 0)
                );

            CreateMap<ProveedorDTO, Proveedor>()
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo == 1)
                );
            #endregion

            #region Categoria
            CreateMap<Categoria, CategoriaDTO>()
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo.HasValue && origen.EsActivo.Value == 1UL ? 1 : 0)
                );

            CreateMap<CategoriaDTO, Categoria>()
                .ForMember(destino =>
                  destino.EsActivo,
                  opt => opt.MapFrom(origen => origen.EsActivo == 1)
               );
            #endregion

            #region imagenProducto
            CreateMap<ImagenProducto, ImagenProductoDTO>().ReverseMap();
            #endregion

            #region presentacion
            CreateMap<Presentacion, PresentacionDTO>()
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo.HasValue && origen.EsActivo.Value == 1UL ? 1 : 0)
                );
            CreateMap<PresentacionDTO, Presentacion>()
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo == 1)
                );
            #endregion

            #region Producto_empaque
            CreateMap<ProductoEmpaque, ProductoEmpaqueDTO>()
                .ForMember(destino =>
                    destino.DescripcionProducto,
                    opt => opt.MapFrom(origen => origen.IdProductoNavigation != null ? origen.IdProductoNavigation.Nombre : "")

                )
                .ForMember(destino =>
                    destino.DescripcionPresentacion,
                    opt => opt.MapFrom(origen => origen.IdPresentacionNavigation != null ? origen.IdPresentacionNavigation.Nombre : "")
                )
                .ForMember(destino =>
                    destino.PrecioVenta,
                    opt => opt.MapFrom(origen => origen.PrecioVenta.HasValue
                        ? Convert.ToString(origen.PrecioVenta.Value, new CultureInfo("es-COL"))
                        : string.Empty) // O un valor por defecto como "0.00"
                )
                .ForMember(destino =>
                    destino.PrecioCompra,
                    opt => opt.MapFrom(origen => origen.PrecioCompra.HasValue
                        ? Convert.ToString(origen.PrecioCompra.Value, new CultureInfo("es-COL"))
                        : string.Empty) // O un valor por defecto como "0.00"
                )
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo.HasValue && origen.EsActivo.Value == 1UL ? 1 : 0)
                );


            CreateMap<ProductoEmpaqueDTO, ProductoEmpaque>()
                .ForMember(destino =>
                    destino.IdProductoNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(destino =>
                    destino.IdPresentacionNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(destino =>
                   destino.PrecioVenta,
                   opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioVenta, new CultureInfo("es-COL")))
                )
                .ForMember(destino =>
                   destino.PrecioCompra,
                   opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioCompra, new CultureInfo("es-COL")))
                )
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo == 1)
                );

            #endregion

            #region Usuario

            //Esto te permite incluir información del rol en el DTO sin necesidad de exponer toda la entidad Rol. Solo te interesa el nombre del rol, y AutoMapper se encarga de extraerl
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destino =>
                    destino.RolDescripcion,
                    opt => opt.MapFrom(origen => origen.IdRolNavigation != null ? origen.IdRolNavigation.Nombre : "")
                )
                //En este caso, transformar un valor binario de ulong a un valor de entero (1 o 0) para que sea más fácil de manejar en el frontend o en otros procesos.
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo.HasValue && origen.EsActivo.Value == 1UL ? 1 : 0)
                );

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino => destino.IdRolNavigation, opt => opt.Ignore())
                .ForMember(destino => destino.EsActivo, opt => opt.MapFrom(origen => (ulong?)origen.EsActivo));

            //es similar al primer mapeo asigna el nombre del rol a RolDescripcion representa la informacion minima para la sesion (como un login) esto es util para determinar los permisos del usurio
            CreateMap<Usuario, SesionDTO>()
                .ForMember(destino =>
                    destino.RolDescripcion,
                    opt => opt.MapFrom(origen => origen.IdRolNavigation != null ? origen.IdRolNavigation.Nombre : "")
                );


            //En la mayoría de los casos, cuando estás mapeando desde un DTO a una entidad, no necesitas mapear relaciones complejas como entidades completas. Solo el IdRol es suficiente.
            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino =>
                    destino.IdRolNavigation,
                    opt => opt.Ignore()
                )
                //Esto es útil para transformar los valores binarios manejados en la UI (1 o 0) a un formato más adecuado para la base de datos o el backend
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo == 1)
                 );

            #endregion

            #region Producto
            CreateMap<Producto, ProductoDTO>()
                .ForMember(destino =>
                    destino.DescripcionCategoria,
                    opt => opt.MapFrom(origen => origen.IdCategoriaNavigation != null ? origen.IdCategoriaNavigation.Nombre : "")
                )
                .ForMember(destino =>
                    destino.NombreProveedor,
                    opt => opt.MapFrom(origen => origen.IdProveedorNavigation != null ? origen.IdProveedorNavigation.Nombre : "")
                )
                .ForMember(destino =>
                    destino.NombreImagen,
                    opt => opt.MapFrom(origen => origen.IdImagenNavigation != null ? origen.IdImagenNavigation.NombreArchivo : "")
                )
                .ForMember(destino => destino.RutaImagen,
                    opt => opt.MapFrom(origen => origen.IdImagenNavigation != null ? origen.IdImagenNavigation.Ruta.TrimStart('/') : "")
                )                
                .ForMember(destino =>
                   destino.EsActivo,
                   opt => opt.MapFrom(origen => origen.EsActivo.HasValue && origen.EsActivo.Value == 1UL ? 1 : 0)
                );


            CreateMap<ProductoDTO, Producto>()
               .ForMember(destino =>
                   destino.IdCategoriaNavigation,
                   opt => opt.Ignore()
               )
               .ForMember(destino =>
                   destino.IdProveedorNavigation,
                   opt => opt.Ignore()
               )
               .ForMember(destino =>
                   destino.IdImagenNavigation,
                   opt => opt.Ignore()
               )               
               .ForMember(destino =>
                  destino.EsActivo,
                  opt => opt.MapFrom(origen => origen.EsActivo == 1)
               );
            #endregion

            #region Venta
            CreateMap<Venta, VentaDTO>()
             .ForMember(destino =>
                destino.UsuarioDescripcion,
                opt => opt.MapFrom(origen => origen.IdUsuarioNavigation != null ? origen.IdUsuarioNavigation.NombreCompleto : "")
             )
             .ForMember(destino =>
                destino.TotalTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Total, new CultureInfo("es-COL")))
             )
            //en este caso se convierte de una DateTime(fecha) a una cadena string
            .ForMember(destino =>
                destino.FechaRegistro,
                opt => opt.MapFrom(origen => origen.FechaRegistro.HasValue
                    ? origen.FechaRegistro.Value.ToString("dd/MM/yyyy")
                    : string.Empty)
             ); // O un valor por defecto como "N/A"


            //en este caso se donvierte una cadema string a un valor decimal
            CreateMap<VentaDTO, Venta>()
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-COL")))
                )
                .ForMember(destino =>
                   destino.IdUsuarioNavigation,
                   opt => opt.Ignore()
                );
            #endregion

            #region DetalleVenta
            CreateMap<Detalleventa, DetalleVentaDTO>()
                .ForMember(destino =>
                    destino.DescripcionProductoEmpaque,
                    opt => opt.MapFrom(origen => origen.IdProductoEmpaqueNavigation != null ? origen.IdProductoEmpaqueNavigation.IdProductoNavigation.Nombre : "")
                )
                .ForMember(destino =>
                    destino.PrecioTexto,
                    opt => opt.MapFrom(origen => origen.Precio.HasValue
                    ? Convert.ToString(origen.Precio.Value, new CultureInfo("es-COL"))
                    : string.Empty)
                 )
                .ForMember(destino =>
                    destino.TotalTexto,
                    opt => opt.MapFrom(origen => origen.Total.HasValue
                    ? Convert.ToString(origen.Total.Value, new CultureInfo("es-COL"))
                    : string.Empty)
                 );


            CreateMap<DetalleVentaDTO, Detalleventa>()
                .ForMember(destino =>
                    destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-COL")))
                )
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-COL")))
                )
                .ForMember(destino =>
                   destino.IdProductoEmpaqueNavigation,
                   opt => opt.Ignore()
                );
            #endregion

            #region Compra
            CreateMap<Compra, CompraDTO>()
                .ForMember(destino =>
                    destino.NombreProveedor,
                    opt => opt.MapFrom(origen => origen.IdProveedorNavigation != null ? origen.IdProveedorNavigation.Nombre : "")
                )
                .ForMember(destino =>
                    destino.TotalTexto,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total, new CultureInfo("es-COL")))
                )
                .ForMember(destino =>
                destino.FechaCompra,
                opt => opt.MapFrom(origen => origen.FechaCompra.HasValue
                    ? origen.FechaCompra.Value.ToString("dd/MM/yyyy")
                    : string.Empty)
             ); // O un valor por defecto como "N/A"

            // Mapeo inverso: CompraDTO a Compra
            CreateMap<CompraDTO, Compra>()
                .ForMember(destino =>
                    destino.IdCompra,
                    opt => opt.Ignore()) // Ignorar el Id si es autogenerado

                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-COL")))
                );
            #endregion

            #region DetalleCompra
            CreateMap<Detallecompra, DetalleCompraDTO>()
                .ForMember(destino =>
                    destino.DescripcionProductoEmpaque,
                    opt => opt.MapFrom(origen => origen.IdProductoEmpaqueNavigation != null ? origen.IdProductoEmpaqueNavigation.IdProductoNavigation.Nombre : "")
                )
                .ForMember(destino =>
                    destino.PrecioTexto,
                    opt => opt.MapFrom(origen => origen.PrecioUnitario.HasValue
                        ? Convert.ToString(origen.PrecioUnitario.Value, new CultureInfo("es-COL"))
                        : string.Empty)
                )
                .ForMember(destino =>
                    destino.TotalTexto,
                    opt => opt.MapFrom(origen => origen.Subtotal.HasValue
                        ? Convert.ToString(origen.Subtotal.Value, new CultureInfo("es-COL"))
                        : string.Empty)
                );

            // Mapeo inverso: DetalleCompraDTO a Detallecompra
            CreateMap<DetalleCompraDTO, Detallecompra>()
                //.ForMember(destino =>
                //    destino.IdDetallecompra,
                //    opt => opt.Ignore()) // Ignorar el Id si es autogenerado

                .ForMember(destino =>
                    destino.PrecioUnitario,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-COL")))
                )
                .ForMember(destino =>
                    destino.Subtotal,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-COL")))
                );
            #endregion

            #region Reporte
            CreateMap<Detalleventa, ReporteDTO>()
                .ForMember(destino =>
                    destino.FechaRegistro,
                    opt => opt.MapFrom(origen =>
                        origen.IdProductoEmpaqueNavigation != null && origen.IdProductoEmpaqueNavigation.FechaRegistro.HasValue
                        ? origen.IdProductoEmpaqueNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy")
                        : string.Empty)  // Puedes usar un valor por defecto si es null
                )
                .ForMember(destino =>
                    destino.Presentacion,
                    opt => opt.MapFrom(origen =>
                        origen.IdProductoEmpaqueNavigation != null
                        ? origen.IdProductoEmpaqueNavigation.IdPresentacionNavigation.Nombre
                        : string.Empty)
                )
                .ForMember(destino =>
                    destino.NumeroVenta,
                    opt => opt.MapFrom(origen =>
                        origen.IdVentaNavigation != null
                        ? origen.IdVentaNavigation.NumVenta
                        : string.Empty))  // Puedes usar un valor por defecto si es null

                .ForMember(destino =>
                    destino.TipoPago,
                    opt => opt.MapFrom(origen =>
                           origen.IdVentaNavigation != null
                           ? origen.IdVentaNavigation.TipoPago
                           : string.Empty)
                 )
                 .ForMember(destino =>
                    destino.TotalVenta,
                    opt => opt.MapFrom(origen =>
                        origen.IdVentaNavigation != null && origen.IdVentaNavigation.Total.HasValue
                        ? Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-COL"))
                        : string.Empty) // Valor por defecto si es null
                  )
                 .ForMember(destino =>
                     destino.NombreProducto,
                     opt => opt.MapFrom(origen =>
                          origen.IdProductoEmpaqueNavigation != null
                          ? origen.IdProductoEmpaqueNavigation.IdProductoNavigation.Nombre
                          : string.Empty) // Valor por defecto si es null
                  )
                 .ForMember(destino =>
                       destino.Precio,
                       opt => opt.MapFrom(origen =>
                           origen.Precio.HasValue
                           ? Convert.ToString(origen.Precio.Value, new CultureInfo("es-COL"))
                           : string.Empty) // Valor por defecto si es null
                  )
                 .ForMember(destino =>
                       destino.Total,
                       opt => opt.MapFrom(origen =>
                           origen.Total.HasValue
                           ? Convert.ToString(origen.Total.Value, new CultureInfo("es-COL"))
                           : string.Empty) // Valor por defecto si es null
                 )
                 .ForMember(destino =>
                      destino.Usuario,
                      opt => opt.MapFrom(origen => origen.IdVentaNavigation != null ? origen.IdVentaNavigation.IdUsuarioNavigation.NombreCompleto : "")
                 );

            #endregion
        }
    }
}
