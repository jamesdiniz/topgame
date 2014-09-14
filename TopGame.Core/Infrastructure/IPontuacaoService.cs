using TopGame.Core.Domain.Pontuacao;

namespace TopGame.Core.Infrastructure
{
    public interface IPontuacaoService
    {
        void Add(PontuacaoApp pontuacao);
    }
}