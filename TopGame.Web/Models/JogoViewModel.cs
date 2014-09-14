using System.Collections.Generic;
using TopGame.Core.Domain;

namespace TopGame.Web.Models
{
    public class JogoViewModel
    {
        public string Token { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Logo { get; set; }
        public bool TemPremios { get; set; }
        public IEnumerable<Premiacao> Premios { get; set; }
        public JogadorViewModel Jogador { get; set; }
    }
}