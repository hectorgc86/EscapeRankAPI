using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        // GET: api/partidas
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

        // GET: api/partidas/usuario/5
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

        // GET: api/partidas/equipo/5
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

        // GET: api/partidas/sala/5
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

        // GET: api/partidas/5
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

        // PUT: api/partidas/5
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

        // POST: api/partidas
        [HttpPost]
        public async Task<ActionResult> PostPartida(PartidaRequest req)
        {
            Partida partidaNueva = new Partida
            {
                Minutos = req.Minutos,
                Segundos = req.Segundos,
                SalaId = req.Sala.Id,
                EquipoId = req.Equipo.Id,
                Fecha = req.Fecha.Value,
                Imagen = await GestionarFoto(req.Foto)
        };


        List<Usuario> miembros = await _contexto.GetUsuariosEquipo(req.Equipo.Id)
                .Include(p=>p.Perfil).ToListAsync();

            foreach (Usuario miembro in miembros)
            {
                if(miembro.Perfil != null)
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

            _contexto.Noticias.Add(await CrearNoticiaPartida(req, miembros));

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

        // DELETE: api/Partidas/5
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

        private bool PartidaExists(int id)
        {
            return _contexto.Partidas.Any(e => e.Id == id);
        }


        private async Task<string> GestionarFoto(byte[] foto)
        {
            string imagen = null;

            if (foto != null && foto.Length > 0)
            {
                imagen = DateTime.Now.Ticks.ToString() + ".jpg";

                string rutaFotos = _configuration.GetSection("AppSettings").GetSection("RutaImagenesPartidas").Value;

                await System.IO.File.WriteAllBytesAsync(_env.ContentRootPath + rutaFotos + imagen, foto);
            }

            return imagen;
        }

        private async Task<Noticia> CrearNoticiaPartida(PartidaRequest req, List<Usuario> miembros)
        {
            string imagen = await GestionarFoto(req.Foto);

            string cadenaMiembros = "";

            for (int i = 0;i < miembros.Count;i++)
            {
                cadenaMiembros += miembros[i].Perfil.Nombre;

                if (i == miembros.Count - 2)
                {
                    cadenaMiembros += " y ";
                }
                else if(i == miembros.Count - 1)
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
    }
}
