using TopGame.Core.Domain;

namespace TopGame.Core.Infrastructure
{
    public interface IJogadorService
    {
        Jogador GetById(int id);
        Jogador GetByToken(string token);
        Jogador GetByDocumento(string documento);
        Jogador Add(Jogador jogador);
        JogadorToken GetToken(int id, string token);
    }
}