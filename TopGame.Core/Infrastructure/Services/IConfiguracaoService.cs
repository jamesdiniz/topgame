using TopGame.Core.Domain;

namespace TopGame.Core.Infrastructure.Services
{
    public interface IConfiguracaoService
    {
        Configuracao GetConfiguracaoById(int jogoId);
        Configuracao GetConfiguracao(int jogoId);
    }
}