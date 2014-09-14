using TopGame.Core.Data;
using TopGame.Core.Domain.Pontuacao;
using TopGame.Core.Infrastructure;

namespace TopGame.Service
{
    public class PontuacaoService : IPontuacaoService
    {
        #region Fields

        private readonly IRepository<PontuacaoApp> _pontuacaoRepository;

        #endregion

        #region Ctor

        public PontuacaoService(IRepository<PontuacaoApp> pontuacaoRepository)
        {
            _pontuacaoRepository = pontuacaoRepository;
        }

        #endregion

        #region Methods

        public void Add(PontuacaoApp pontuacao)
        {
            _pontuacaoRepository.Add(pontuacao);
        }

        #endregion
    }
}