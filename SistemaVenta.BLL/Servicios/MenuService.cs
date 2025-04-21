using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;//Se usa para interactuar con los datos de los usuarios (Usuario).
        private readonly IGenericRepository<Menurol> _menurolRepositorio;//Permite consultar la relación entre menús y roles (Menurol).
        private readonly IGenericRepository<Menu> _menuRepositorio;//Maneja los datos relacionados con los menús.
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Usuario> usuarioRepositorio, IGenericRepository<Menurol> menurolRepositorio, IGenericRepository<Menu> menuRepositorio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _menurolRepositorio = menurolRepositorio;
            _menuRepositorio = menuRepositorio;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> Lista(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = await _usuarioRepositorio.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<Menurol> tbMenuRol = await _menurolRepositorio.Consultar();
            IQueryable<Menu> tbMenu = await _menuRepositorio.Consultar();

            try
            {
                // Obtener menús según el rol del usuario
                var tbResultado = (from u in tbUsuario
                                   join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                   join m in tbMenu on mr.IdMenu equals m.IdMenu
                                   select m).ToList();

                // Armar jerarquía
                var menuPadres = tbResultado
                    .Where(m => m.IdPadre == null)
                    .Select(m => new MenuDTO
                    {
                        IdMenu = m.IdMenu,
                        Nombre = m.Nombre,
                        Icono = m.Icono,
                        Url = m.Url,
                        Submenus = tbResultado
                            .Where(sm => sm.IdPadre == m.IdMenu)
                            .Select(sm => new MenuDTO
                            {
                                IdMenu = sm.IdMenu,
                                Nombre = sm.Nombre,
                                Icono = sm.Icono,
                                Url = sm.Url
                            }).ToList()
                    }).ToList();

                return menuPadres;
            }
            catch
            {
                throw;
            }
        }
    }
}
