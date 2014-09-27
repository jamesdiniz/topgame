using TopGame.Core.Domain.Pontuacao;

namespace TopGame.Core.Infrastructure.Services
{
    public interface IPontuacaoService
    {
        void Add(PontuacaoApp pontuacao);
    }
}