using System;
using System.Collections.Generic;

namespace TopGame.Core.Domain
{
    public class Jogador
    {
        /// <summary>
        /// Sequência de identificação
        /// </summary>
        public int JogadorId { get; set; }

        /// <summary>
        /// Nome do jogador
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Email do jogador
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Documento de identificação, podendo ser RG ou CPF
        /// </summary>
        public string Documento { get; set; }

        /// <summary>
        /// Data de cadastro
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Lista de tokens criados para este jogador
        /// </summary>
        public virtual ICollection<JogadorToken> JogadorTokens { get; set; }
    }
}