using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace ApiEscapeRank.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>Llamada por defecto en la carga de la API</summary>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Bienvenidos a EscapeAPI" };
        }
    }
}
