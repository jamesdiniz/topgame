
namespace TopGame.Core.Domain.Pontuacao
{
    public abstract class PontuacaoBase
    {
        public int PontuacaoId { get; set; }
        
        public int JogadorId { get; set; }
        
        public virtual Jogador Jogador { get; set; }
    }
}