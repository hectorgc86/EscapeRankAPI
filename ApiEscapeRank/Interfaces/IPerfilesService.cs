using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiEscapeRank.Modelos;

namespace ApiEscapeRank
{
    public interface IPerfilesService
    {
        public Task<ActionResult<IEnumerable<Perfil>>> GetPerfiles();
        public Task<ActionResult<Perfil>> GetPerfil(int id);
    }
}
