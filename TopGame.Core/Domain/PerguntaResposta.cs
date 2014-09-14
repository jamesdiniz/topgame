
namespace TopGame.Core.Domain
{
    public class PerguntaResposta
    {
        /// <summary>
        /// Sequência de identificação
        /// </summary>
        public int PerguntaRespostaId { get; set; }
        
        /// <summary>
        /// Texto que irá aparecer para o jogador como opção de resposta
        /// </summary>
        /// <remarks>Quando tipo da resposta for do tipo 'digitação', campo poderá ser vazio</remarks>
        public string Descricao { get; set; }
        
        /// <summary>
        /// Campo que informa se a resposta é a alternativa correta
        /// </summary>
        /// <remarks>Quando nulo, significa que o tipo da resposta não é do tipo escolha</remarks>
        public bool? Correta { get; set; }

        /// <summary>
        /// Quantidade de pontos que jogador irá adquirir caso resposta esteja correta
        /// </summary>
        public decimal QuantidadePonto { get; set; }

        /// <summary>
        /// Ordem de visualização da resposta
        /// </summary>
        public int? Ordem { get; set; }

        /// <summary>
        /// Sequência de identificação da pergunta
        /// </summary>
        public int PerguntaId { get; set; }

        /// <summary>
        /// Objeto do relacionamento com a pergunta
        /// </summary>
        public virtual Pergunta Pergunta { get; set; }
    }
}
