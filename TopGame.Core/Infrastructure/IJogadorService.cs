using TopGame.Core.Domain;

namespace TopGame.Core.Infrastructure
{
    public interface IJogadorService
    {
        Jogador GetById(int id);
        Jogador GetByToken(string token);
        Jogador GetByDocumento(string documento);
        void AddJogador(Jogador jogador);
        JogadorToken GetToken(int id, string token);
        JogadorToken AddToken(Jogador jogador);
    }
}