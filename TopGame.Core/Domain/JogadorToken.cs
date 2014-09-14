using System;

namespace TopGame.Core.Domain
{
    public class JogadorToken
    {
        /// <summary>
        /// Sequência de identificação
        /// </summary>
        public int JogadorTokenId { get; set; }

        /// <summary>
        /// Código criptografado no formato SHA1
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Sequência de identificação do jogador
        /// </summary>
        public int JogadorId { get; set; }

        /// <summary>
        /// Objeto do relacionamento com o jogador
        /// </summary>
        public virtual Jogador Jogador { get; set; }
    }
}