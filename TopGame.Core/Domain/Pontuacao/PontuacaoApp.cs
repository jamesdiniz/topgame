
namespace TopGame.Core.Domain.Pontuacao
{
    public class PontuacaoApp : PontuacaoBase
    {
        public decimal Quantidade { get; set; }
        public int PerguntaId { get; set; }
        public virtual Pergunta Pergunta { get; set; }
    }
}