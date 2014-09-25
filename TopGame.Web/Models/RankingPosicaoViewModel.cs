

namespace TopGame.Web.Models
{
    public class RankingPosicaoViewModel
    {
        public int JogoId { get; set; }
        public int JogadorId { get; set; }
        public string Jogador { get; set; }
        public int Posicao { get; set; }
        public double PontoRanking { get; set; }
        public double PontoFortuna { get; set; }
    }
}