
using System.Collections.Generic;

namespace TopGame.Web.Models
{
    public class RankingDetalheViewModel
    {
        public int JogoId { get; set; }
        public int JogadorId { get; set; }
        public string Jogador { get; set; }
        public int Posicao { get; set; }
        public IEnumerable<RankingPerguntaViewModel> Perguntas { get; set; }
    }

    public class RankingPerguntaViewModel
    {
        public int PerguntaId { get; set; }
        public int PerguntaRespostaJogadorId { get; set; }
        public int PerguntaRespostaId { get; set; }
        public string PerguntaDescricao { get; set; }
        public decimal RespostaCorretaRanking { get; set; }
        public decimal RespostaCorretaFortuna { get; set; }
        public decimal RespostaInformadaRanking { get; set; }
        public decimal RespostaInformadaFortuna { get; set; }
        public decimal PontoRanking { get; set; }
        public decimal PontoFortuna { get; set; }
        public int? Ordem { get; set; }
    }
}