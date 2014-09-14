
namespace TopGame.Core.Domain
{
    public class PerguntaRespostaJogador
    {
        /// <summary>
        /// Sequência de identificação
        /// </summary>
        public int PerguntaRespostaJogadorId { get; set; }

        /// <summary>
        /// Resposta informada pelo jogador
        /// </summary>
        public string Resposta { get; set; }

        /// <summary>
        /// Status da resposta da pergunta
        /// </summary>
        /// <remarks>'R' Respondida, 'P' Pendente</remarks>
        public string Status { get; set; }

        /// <summary>
        /// Sequência de identificação da pergunta
        /// </summary>
        public int PerguntaId { get; set; }

        /// <summary>
        /// Sequência de identificação da resposta
        /// </summary>
        public int? PerguntaRespostaId { get; set; }

        /// <summary>
        /// Sequência de identificação do jogador
        /// </summary>
        public int JogadorId { get; set; }

        /// <summary>
        /// Sequência de identificação do token do jogador
        /// </summary>
        public int JogadorTokenId { get; set; }
        public virtual Pergunta Pergunta { get; set; }
        public virtual PerguntaResposta PerguntaResposta { get; set; }
        public virtual Jogador Jogador { get; set; }
        public virtual JogadorToken JogadorToken { get; set; }
    }
}