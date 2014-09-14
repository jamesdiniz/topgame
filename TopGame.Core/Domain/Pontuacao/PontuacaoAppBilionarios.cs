
namespace TopGame.Core.Domain.Pontuacao
{
    public class PontuacaoAppBilionarios : PontuacaoBase
    {
        public double PontoRanking { get; set; }
        public double PontoFortuna { get; set; }
        public int PerguntaRespostaJogadorId { get; set; }
        public virtual PerguntaRespostaJogador PerguntaRespostaJogador { get; set; }
    }
}