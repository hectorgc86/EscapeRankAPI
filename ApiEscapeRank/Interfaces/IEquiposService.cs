using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiEscapeRank.Modelos;

namespace ApiEscapeRank.Interfaces
{
    public interface IEquiposService
    {
        public Task<ActionResult<IEnumerable<Equipo>>> GetEquipos();
        public Task<ActionResult<Equipo>> GetEquipo(int id);
    }
}
