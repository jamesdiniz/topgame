using System.Collections.Generic;
using TopGame.Core.Domain;
using TopGame.Core.Domain.Pontuacao;

namespace TopGame.Core.Infrastructure
{
    public interface IJogoRepository
    {
        Jogo GetByToken(string token);
        IEnumerable<Premiacao> GetPremios(int id);
        IEnumerable<PontuacaoApp> GetRanking(int id);
    }
}