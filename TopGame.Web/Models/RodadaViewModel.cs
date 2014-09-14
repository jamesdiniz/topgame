using System.Collections.Generic;
using TopGame.Core.Domain;

namespace TopGame.Web.Models
{
    public class RodadaViewModel
    {
        public int PerguntaId { get; set; }
        public string Pergunta { get; set; }
        public string Foto { get; set; }
        public ConfiguracaoViewModel Configuracao { get; set; }
        public TipoResposta TipoResposta { get; set; }
        public List<RespostaViewModel> Respostas { get; set; }
    }
}