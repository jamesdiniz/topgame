using System.Collections.Generic;
using TopGame.App.Bilionario.Repository;
using TopGame.Core.Domain.Pontuacao;

namespace TopGame.App.Bilionario.Service
{
    public class AppService
    {
        #region Fields

        private readonly IAppRepository _appRepository;

        #endregion

        #region Ctor

        public AppService(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        #endregion

        #region Methods

        public void AddPontuacao(decimal pontoRanking, decimal pontoFortuna, int jogadorRespostaId, int jogadorId)
        {
            _appRepository.AddPontuacao(pontoRanking, pontoFortuna, jogadorRespostaId, jogadorId);
        }

        public IEnumerable<PontuacaoAppBilionarios> GetRanking(int jogoId, bool? completo)
        {
            return _appRepository.GetRanking(jogoId, completo);
        }

        public IEnumerable<PontuacaoAppBilionarios> GetRankingDetalhes(int jogoId, int jogadorId)
        {
            return _appRepository.GetRankingDetalhes(jogoId, jogadorId);
        }

        #endregion
    }
}
