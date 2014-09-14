
namespace TopGame.Core.Domain
{
    public class Configuracao
    {
        public int ConfiguracaoId { get; set; }

        /// <summary>
        /// Quantidade de perguntas a serem respondidas
        /// </summary>
        public int QuantidadePergunta { get; set; }
        
        /// <summary>
        /// Quantidade de pessoas visíveis no resultado
        /// </summary>
        public int QuantidadeResultado { get; set; }

        /// <summary>
        /// Sequência de identificação do jogo
        /// </summary>
        public int JogoId { get; set; }

        public virtual Jogo Jogo { get; set; }
    }
}