using System;
using System.Collections.Generic;

namespace TopGame.Core.Domain
{
    public class Jogo
    {
        /// <summary>
        /// Sequência de identificação
        /// </summary>
        public int JogoId { get; set; }

        /// <summary>
        /// Nome do jogo
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Url amigável que servirá de rastreio no analytics
        /// </summary>
        public string TituloUrl { get; set; }

        /// <summary>
        /// Texto de apresentação
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Atributo para saber se jogo está ativo
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Data que jogo estará disponível
        /// </summary>
        public DateTime DataPublicacao { get; set; }

        /// <summary>
        /// Data limite permitida
        /// </summary>
        public DateTime? DataExpiracao { get; set; }

        /// <summary>
        /// Código criptografado para identificação
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Lista de perguntas para esse jogo
        /// </summary>
        public virtual ICollection<Pergunta> Perguntas { get; set; }

        /// <summary>
        /// Lista de premiações para esse jogo
        /// </summary>
        public virtual ICollection<Premiacao> Premiacoes { get; set; }
    }
}
