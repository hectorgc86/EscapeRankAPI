using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ApiEscapeRank.Modelos;

namespace ApiEscapeRank.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Bienvenidos a EscapeAPI" };
        }
    }
}
