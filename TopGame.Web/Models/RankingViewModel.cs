
namespace TopGame.Web.Models
{
    public class RankingViewModel
    {
        public int Posicao { get; set; }
        public int JogadorId { get; set; }
        public string Jogador { get; set; }
        public double Pontuacao { get; set; }
        public double PontuacaoAdicional { get; set; }
    }
}