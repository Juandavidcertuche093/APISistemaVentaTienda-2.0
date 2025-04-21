using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DAL.Repositorio.Contrato
{
    public interface IGenericRepository<TModel> where TModel : class //esto nos sirve para poder manejar todos los modelos de forma generica
    {
        Task<TModel?> Obtener(Expression<Func<TModel, bool>> filtro);//obtener un menu, un rol o un usurio etc
        Task<TModel> Crear(TModel modelo);
        Task<TModel> Editar(TModel modelo);
        Task<TModel> Eliminar(TModel modelo);
        Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>>? filtro = null);//Usa expresiones lambda para permitir consultas filtradas 
    }
}

//metodos para trabajar con todos los modelos o tablas de la base de datos 
// en este caso el contrato seria IGenericRepositor para interactuar con todos los modelos o tablas
