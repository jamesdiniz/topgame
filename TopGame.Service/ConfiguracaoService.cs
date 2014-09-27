using System.Linq;
using TopGame.Core.Data;
using TopGame.Core.Domain;
using TopGame.Core.Infrastructure.Services;

namespace TopGame.Service
{
    public class ConfiguracaoService : IConfiguracaoService
    {
        #region Fields

        private readonly IRepository<Configuracao> _configuracaoRepository;

        #endregion

        #region Ctor

        public ConfiguracaoService(IRepository<Configuracao> configuracaoRepository)
        {
            _configuracaoRepository = configuracaoRepository;
        }

        #endregion

        #region Methods

        public Configuracao GetConfiguracaoById(int id)
        {
            return _configuracaoRepository.GetById(id);
        }

        public Configuracao GetConfiguracao(int jogoId)
        {
            return _configuracaoRepository.Table.FirstOrDefault(x => x.JogoId == jogoId);
        }

        #endregion
    }
}