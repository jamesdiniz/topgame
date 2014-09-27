using System.Collections.Generic;
using TopGame.Core.Domain;

namespace TopGame.Core.Infrastructure.Services
{
    public interface IJogoService
    {
        Jogo GetByToken(string token);
        IEnumerable<Premiacao> GetPremios(int id);
    }
}