using System.Collections.Generic;
using TopGame.App.Bilionario.Repository;
using TopGame.Core.Domain.Pontuacao;

namespace TopGame.App.Bilionario.Service
{
    public class AppService
    {
        #region Fields

        private readonly AppRepository _appRepository;

        #endregion

        #region Ctor

        public AppService()
        {
            _appRepository = new AppRepository();
        }

        #endregion

        #region Methods

        public void IncluiPontuacao(decimal pontoRanking, decimal pontoFortuna, int jogadorRespostaId, int jogadorId)
        {
            _appRepository.IncluiPontuacao(pontoRanking, pontoFortuna, jogadorRespostaId, jogadorId);
        }

        public IEnumerable<PontuacaoAppBilionarios> GetRanking(int jogoId)
        {
            return _appRepository.GetRanking(jogoId);
        }


        public IEnumerable<PontuacaoAppBilionarios> GetRankingDetalhes(int jogoId, int jogadorId)
        {
            return _appRepository.GetRankingDetalhes(jogoId, jogadorId);
        }

        #endregion
    }
}
