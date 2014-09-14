using System.Collections.Generic;

namespace TopGame.Core.Domain
{
    public class Pergunta
    {
        /// <summary>
        /// Sequência de identificação
        /// </summary>
        public int PerguntaId { get; set; }

        /// <summary>
        /// Texto descritivo que aparecerá para o jogador
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Texto com a dica para ajudar o jogador
        /// </summary>
        public string Dica { get; set; }

        /// <summary>
        /// Tipo de resposta que o jogador irá responder
        /// </summary>
        public TipoResposta TipoResposta { get; set; }

        /// <summary>
        /// Informa se as respostas serão ordenadas aleatoriamente
        /// </summary>
        public bool TipoRespostaAleatoria { get; set; }

        /// <summary>
        /// Status que informa se está ativa para ser respondida
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Sequência de identificação do jogo
        /// </summary>
        public int JogoId { get; set; }

        /// <summary>
        /// Objeto do relacionamento com o jogo
        /// </summary>
        public virtual Jogo Jogo { get; set; }

        /// <summary>
        /// Lista com as opções de resposta
        /// </summary>
        public virtual ICollection<PerguntaResposta> PerguntaRespostas { get; set; }
    }
}