
namespace TopGame.Core.Domain
{
    public class Premiacao
    {
        /// <summary>
        /// Sequência de identificação
        /// </summary>
        public int PremiacaoId { get; set; }

        /// <summary>
        /// Posição da premiação
        /// </summary>
        /// <remarks>Caso seja um brinde, valor deverá ser 999</remarks>
        public int Posicao { get; set; }

        /// <summary>
        /// Descrição contendo o nome do item a ser premiado
        /// </summary>
        public string Descricao { get; set; }
        
        /// <summary>
        /// Informa se premiação é um brinde
        /// </summary>
        public bool Brinde { get; set; }

        /// <summary>
        /// Sequência de identificação do jogo
        /// </summary>
        public int JogoId { get; set; }

        /// <summary>
        /// Objeto do relacionamento com o jogo
        /// </summary>
        public virtual Jogo Jogo { get; set; }
    }
}