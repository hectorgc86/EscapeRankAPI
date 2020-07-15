using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Modelos
{
    public partial class MySQLDbcontext : BaseDbContext
    {
        public MySQLDbcontext(DbContextOptions<MySQLDbcontext> options) : base(options) { }

        //Funciones Usuario

        public IQueryable<Usuario> GetUsuarios()
        {
            return Usuarios.Include(p => p.Perfil);
        }

        public IQueryable<Usuario> GetUsuariosEquipo(int equipoId)
        {
            string sqlString = "SELECT * FROM usuarios " +
                               "WHERE id IN (" +
                               "SELECT usuario_id FROM equipos_usuarios " +
                               "WHERE equipo_id = " + equipoId + ")";

            return Usuarios.FromSqlRaw(sqlString).Include(p=>p.Perfil);
        }

        public List<Amigo> GetAmigosUsuario(int usuarioId)
        {
            string consulta =
                "SELECT usuarios.* , usuarios_amigos.estado , perfiles.* " +
                "FROM usuarios, usuarios_amigos, perfiles " +
                "WHERE usuarios.id = usuarios_amigos.amigo_id " +
                "AND usuarios_amigos.amigo_id = perfiles.usuario_id " +
                "AND usuarios_amigos.usuario_id = " + usuarioId + " " +
                "AND usuarios_amigos.estado = 'aceptado' " +
                "OR " +
                "usuarios.id = usuarios_amigos.usuario_id " +
                "AND usuarios_amigos.usuario_id = perfiles.usuario_id " +
                "AND usuarios_amigos.amigo_id = " + usuarioId + " " +
                "AND(usuarios_amigos.estado = 'aceptado' OR usuarios_amigos.estado = 'pendiente')";

            return RawSqlQuery(consulta, x => new Amigo
            {
                Id = (int)x[0],
                Nick = (string)x[1],
                Email = (string)x[2],
                Activado = (bool)x[4],
                Estado = Enum.Parse<Estado>((string)x[5]),
                Perfil = new Perfil
                {
                    Id = (int)x[6],
                    Nombre = x[7] != DBNull.Value ? (string)x[7]: "",
                    Avatar = (string)x[10]
                }
            });
        }

        public IQueryable<Usuario> GetUsuario(int usuarioId)
        {
            return Usuarios.Include(p => p.Perfil).Where(i => i.Id == usuarioId);
        }

        //Funciones Equipo

        public IQueryable<Equipo> GetEquipos()
        {
            return Equipos;
        }

        public IQueryable<Equipo> GetEquiposUsuario(int usuarioId)
        {
            string sqlString = "SELECT * FROM equipos " +
                               "WHERE id " +
                               "IN(SELECT equipo_id " +
                               "FROM equipos_usuarios " +
                               "WHERE usuario_id = " + usuarioId + ") " +
                               "AND activado = 1";

            return Equipos.FromSqlRaw(sqlString);
        }

        public IQueryable<Equipo> GetEquipo(int equipoId)
        {
            return Equipos.Where(e => e.Id == equipoId);
        }

        //Funciones Sala

        public IQueryable<Sala> GetSalas(int offset)
        {
            return Salas.Include(c => c.Companyia)
                        .ThenInclude(ci => ci.Ciudad)
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasPromocionadas(int offset)
        {
            return Salas.Where(p => p.Promocionada == true)
                        .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasCategoria(string categoriaId, int offset)
        {
            return Salas.Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Where(s => s.SalasCategorias.Any(c => c.CategoriaId == categoriaId))
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasTematica(string tematicaId, int offset)
        {
            return Salas.Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Where(s => s.SalasTematicas.Any(t => t.TematicaId == tematicaId))
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasPublico(string publicoId, int offset)
        {
            return Salas.Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Where(s => s.SalasPublico.Any(p => p.PublicoId == publicoId))
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasDificultad(string dificultadId, int offset)
        {
            return Salas.Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Where(s => s.DificultadId == dificultadId)
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasFiltradas(int offset, string busqueda)
        {
            return Salas.Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Where(s => s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasCategoriaFiltradas(string categoriaId, int offset, string busqueda)
        {
            return Salas.Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Where(s => s.SalasCategorias.Any(c => c.CategoriaId == categoriaId) &&
                       s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasTematicaFiltradas(string tematicaId, int offset, string busqueda)
        {
            return Salas.Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Where(s => s.SalasTematicas.Any(t => t.TematicaId == tematicaId) &&
                       s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasPublicoFiltradas(string publicoId, int offset, string busqueda)
        {
            return Salas.Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Where(s => s.SalasPublico.Any(c => c.PublicoId == publicoId) &&
                       s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSalasDificultadFiltradas(string dificultadId, int offset, string busqueda)
        {
            return Salas.Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Where(s => s.DificultadId == dificultadId &&
                       s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                       s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
                        .Skip(offset)
                        .Take(10);
        }

        public IQueryable<Sala> GetSala(string salaId)
        {
            return Salas.Include(e => e.Dificultad)
                        .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                        .Include(sc => sc.SalasCategorias).ThenInclude(s => s.Categoria)
                        .Include(st => st.SalasTematicas).ThenInclude(t => t.Tematica)
                        .Include(sp => sp.SalasPublico).ThenInclude(p => p.Publico)
                        .Where(i => i.Id == salaId);
        }

        //Funciones Noticia

        public IQueryable<Noticia> GetNoticias()
        {
            return Noticias;
        }

        public IQueryable<Noticia> GetNoticiasUsuario(int usuarioId)
        {
            string sqlString = "SELECT * FROM noticias WHERE equipo_id" +
                 " IN(SELECT DISTINCT equipo_id FROM equipos_usuarios WHERE usuario_id " +
                 " IN(SELECT DISTINCT amigo_id FROM usuarios_amigos " +
                 " WHERE (usuario_id = " + usuarioId + " OR amigo_id = " + usuarioId + ")))" +
                 " OR promocionada = 1" +
                 " OR usuario_id = " + usuarioId + "" +
                 " ORDER by fecha DESC";

            return Noticias.FromSqlRaw(sqlString);
        }

        public IQueryable<Noticia> GetNoticia(int noticiaId)
        {
            return Noticias.Where(n => n.Id == noticiaId);
        }

        //Funciones Compañia

        public IQueryable<Companyia> GetCompanyias()
        {
            return Companyias;
        }

        public IQueryable<Companyia> GetCompanyia(string companyiaId)
        {
            return Companyias.Where(c=> c.Id == companyiaId);
        }

        //Funciones Partida

        public IQueryable<Partida> GetPartidas()
        {
            return Partidas;
        }

        public IQueryable<Partida> GetPartidasUsuario(int usuarioId)
        {
            string sqlString = "SELECT * from partidas " +
                               "WHERE equipo_id IN " +
                               "(SELECT equipo_id FROM equipos_usuarios " +
                               "WHERE usuario_id = " + usuarioId + ")";

            return Partidas.FromSqlRaw(sqlString)
                           .Include(s => s.Sala)
                           .Include(e => e.Equipo)
                           .OrderByDescending(f => f.Fecha);
        }

        public IQueryable<Partida> GetPartidasEquipo(int equipoId)
        {
            return Partidas.Where(ei => ei.EquipoId == equipoId).Include(s => s.Sala);
        }

        public IQueryable<Partida> GetPartidasSala(string salaId)
        {
            return Partidas.Where(si => si.SalaId == salaId)
                           .Include(s => s.Sala)
                           .Include(e => e.Equipo);
        }

        public IQueryable<Partida> GetPartida(int partidaId)
        {
            return Partidas.Where(p => p.Id == partidaId);
        }


        //Funciones Categoria

        public IQueryable<Categoria> GetCategorias()
        {
            string sqlString = "SELECT id ,tipo, COUNT(*) numero_salas " +
                               "FROM categorias, salas_categorias " +
                               "WHERE categorias.id = salas_categorias.categoria_id " +
                               "GROUP BY id, tipo " +
                               "ORDER BY numero_salas DESC";

            return Categorias.FromSqlRaw(sqlString);
        }

        public IQueryable<Categoria> GetCategoria(string categoriaId)
        {
            return Categorias.Where(t => t.Id == categoriaId);
        }

        //Funciones Temática

        public IQueryable<Tematica> GetTematicas()
        {
            string sqlString = "SELECT id ,tipo, COUNT(*) numero_salas " +
                               "FROM tematicas, salas_tematicas " +
                               "WHERE tematicas.id = salas_tematicas.tematica_id " +
                               "GROUP BY id, tipo " +
                               "ORDER BY numero_salas DESC";

            return Tematicas.FromSqlRaw(sqlString);
        }

        public IQueryable<Tematica> GetTematica(string tematicaId)
        {
            return Tematicas.Where(t => t.Id == tematicaId);
        }

        //Funciones Público

        public IQueryable<Publico> GetPublico()
        {
            string sqlString = "SELECT id ,tipo, COUNT(*) numero_salas " +
                               "FROM publico, salas_publico " +
                               "WHERE publico.id = salas_publico.publico_id " +
                               "GROUP BY id, tipo";

            return Publico.FromSqlRaw(sqlString);
        }

        public IQueryable<Publico> GetPublico(string publicoId)
        {
            return Publico.Where(t => t.Id == publicoId);
        }

        //Funciones Dificultad

        public IQueryable<Dificultad> GetDificultades()
        {
            string sqlString = "SELECT dificultades.id, tipo, COUNT(*) numero_salas " +
                               "FROM dificultades, salas " +
                               "WHERE dificultades.id = salas.dificultad_id " +
                               "GROUP BY dificultades.id, tipo";

            return Dificultades.FromSqlRaw(sqlString);
        }

        public IQueryable<Dificultad> GetDificultad(string dificultadId)
        {
            return Dificultades.Where(t => t.Id == dificultadId);
        }

        //Funciones Ciudad

        public IQueryable<Ciudad> GetCiudades()
        {
            return Ciudades;
        }

        public IQueryable<Ciudad> GetCiudad(string id)
        {
            return Ciudades.Where(c => c.Id == id);
        }



        //Función para ejecutar una query directamente a la DB y no a un DBSet en concreto

        public List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;

            Database.OpenConnection();

            using var result = command.ExecuteReader();
            var entities = new List<T>();

            while (result.Read())
            {
                entities.Add(map(result));
            }

            return entities;
        }

    }
}
