using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiEscapeRank.Helpers;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace ApiEscapeRank.Controladores
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PartidasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PartidasController(MySQLDbcontext contexto, IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _contexto = contexto;
            _configuration = configuration;
        }

        /// <summary>Obtener todas las partidas</summary>
        /// <response code="200">Partidas devueltas</response>
        /// <response code="404">No se encuentran partidas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Partida>>> GetPartidas()
        {
            List<Partida> partidas = await _contexto.GetPartidas().ToListAsync();

            if (partidas == null)
            {
                return NotFound();
            }

            return partidas;
        }

        /// <summary>Obtener todas las partidas de un usuario</summary>
        /// <param name="id">Id de usuario</param>
        /// <response code="200">Partidas de usuario devueltas</response>
        /// <response code="404">No se encuentran partidas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<List<Partida>>> GetPartidasUsuario(int id)
        {
            List<Partida> partidasUsuario = await _contexto.GetPartidasUsuario(id).ToListAsync();

            if (partidasUsuario == null)
            {
                return NotFound();
            }

            return partidasUsuario;
        }

        /// <summary>Obtener todas las partidas de un equipo</summary>
        /// <param name="id">Id de equipo</param>
        /// <response code="200">Partidas de equipo devueltas</response>
        /// <response code="404">No se encuentran partidas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("equipo/{id}")]
        public async Task<ActionResult<List<Partida>>> GetPartidasEquipo(int id)
        {
            List<Partida> partidasEquipo = await _contexto.GetPartidasEquipo(id).ToListAsync();

            if (partidasEquipo == null)
            {
                return NotFound();
            }

            return partidasEquipo;
        }

        /// <summary>Obtener todas las partidas jugadas en una sala</summary>
        /// <param name="id">Id de sala</param>
        /// <response code="200">Partidas en sala devueltas</response>
        /// <response code="404">No se encuentran partidas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("sala/{id}")]
        public async Task<ActionResult<List<Partida>>> GetPartidasSala(string id)
        {
            List<Partida> partidasSala = await _contexto.GetPartidasSala(id).ToListAsync();

            if (partidasSala == null)
            {
                return NotFound();
            }

            return partidasSala;
        }

        /// <summary>Obtener una partida por su id</summary>
        /// <param name="id">Id de partida</param>
        /// <response code="200">Partida devuelta</response>
        /// <response code="404">No se encuentra partida</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Partida>> GetPartida(int id)
        {
            Partida partida = await _contexto.GetPartida(id).FirstOrDefaultAsync();

            if (partida == null)
            {
                return NotFound();
            }

            return partida;
        }

        /// <summary>Modificar una partida por su id</summary>
        /// <param name="id">Id de partida a modificar</param>
        /// <param name="partida">Partida modificada</param>
        /// <response code="200">Partida modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra partida</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPartida(int id, Partida partida)
        {
            if (id != partida.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(partida).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartidaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>Añadir una nueva partida</summary>
        /// <param name="req">Partida</param>
        /// <response code="200">Partida añadida</response>
        /// <response code="409">Partida ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult> PostPartida(PartidaRequest req)
        {

            string Imagen = await GestionarFoto(req.Foto);
            if (!string.IsNullOrEmpty(Imagen))
            {
                Partida partidaNueva = new Partida
                {
                    Minutos = req.Minutos,
                    Segundos = req.Segundos,
                    SalaId = req.Sala.Id,
                    EquipoId = req.Equipo.Id,
                    Fecha = req.Fecha.Value,
                    Imagen = Imagen

                };

                List<Usuario> miembros = await _contexto.GetUsuariosEquipo(req.Equipo.Id)
                        .Include(p => p.Perfil).ToListAsync();

                foreach (Usuario miembro in miembros)
                {
                    if (miembro.Perfil != null)
                    {
                        miembro.Perfil.NumeroPartidas += 1;

                        if (int.Parse(req.Minutos) == int.Parse(req.Sala.Duracion) && int.Parse(req.Segundos) == 0
                            || int.Parse(req.Minutos) < int.Parse(req.Sala.Duracion))
                        {
                            miembro.Perfil.PartidasGanadas += 1;
                        }
                        else
                        {
                            miembro.Perfil.PartidasPerdidas += 1;
                        }
                    }
                }

                _contexto.Entry(req.Equipo).State = EntityState.Modified;

                _contexto.Partidas.Add(partidaNueva);

                Noticia noti = CrearNoticiaPartida(req, miembros,Imagen);

                if (noti != null)
                {
                    _contexto.Noticias.Add(noti);
                }
                else
                {
                    return NotFound();
                }

                try
                {
                    await _contexto.SaveChangesAsync();

                    return Ok();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>Borrar una partida</summary>
        /// <param name="id">Id de partida</param>
        /// <response code="200">Partida borrada</response>
        /// <response code="404">No se encuentra partida</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePartida(int id)
        {
            Partida partida = await _contexto.Partidas.FindAsync(id);

            if (partida == null)
            {
                return NotFound();
            }

            _contexto.Partidas.Remove(partida);
            await _contexto.SaveChangesAsync();

            return Ok();
        }

        //Comprobar una la partida existe
        private bool PartidaExists(int id)
        {
            return _contexto.Partidas.Any(e => e.Id == id);
        }

        //Gestionar foto de la partida la partida
        private async Task<string> GestionarFoto(byte[] foto)
        {
            string nombre = null;

            bool result = false;

            if (foto != null && foto.Length > 0)
            {
                nombre = DateTime.Now.Ticks.ToString() + ".jpg";
#if DEBUG
                if (!result)
                {
                    string rutaFotos = _configuration.GetSection("AppSettings").GetSection("RutaImagenesPartidasLocal").Value;
                    try
                    {
                        await System.IO.File.WriteAllBytesAsync(_env.ContentRootPath + rutaFotos + nombre, foto);
                    }
                    catch
                    {
                        nombre = "";
                    }
                }
            }
#else
                result = await StorageHelper.PostFotoAStorage(_configuration, foto, nombre);
            }
            if (!result)
            {
                nombre = "";
            }
#endif
            return nombre;
        }

        //Crear una noticia sobre una partida
        private Noticia CrearNoticiaPartida(PartidaRequest req, List<Usuario> miembros,string imagen)
        {
            if (!string.IsNullOrEmpty(imagen))
            {
                string cadenaMiembros = "";

                for (int i = 0; i < miembros.Count; i++)
                {
                    cadenaMiembros += miembros[i].Nick;

                    if (i == miembros.Count - 2)
                    {
                        cadenaMiembros += " y ";
                    }
                    else if (i == miembros.Count - 1)
                    {
                        cadenaMiembros += " ";
                    }
                    else
                    {
                        cadenaMiembros += ", ";
                    }
                }

                Noticia noticia = new Noticia
                {
                    Imagen = imagen,
                    Titular = "Se ha jugado en " + req.Sala.Nombre,
                    TextoCorto = "El equipo: " + req.Equipo.Nombre + " ha jugado una nueva partida en " + req.Sala.Companyia.Ciudad.Nombre + ".",
                    TextoLargo = "¡" + cadenaMiembros + "han realizado un tiempo de " + req.Minutos + " minutos con " + req.Segundos + " segundos! " +
                    "Los equipos de EscapeRank y " + req.Sala.Companyia.Nombre + " os estamos muy agradecidos.",
                    EquipoId = req.Equipo.Id
                };

                return noticia;
            }
            else
            {
                return null;
            }
        }

    }
}
