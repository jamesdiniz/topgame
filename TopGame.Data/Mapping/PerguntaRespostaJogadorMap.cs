using System.Data.Entity.ModelConfiguration;
using TopGame.Core.Domain;

namespace TopGame.Data.Mapping
{
    public class PerguntaRespostaJogadorMap : EntityTypeConfiguration<PerguntaRespostaJogador>
    {
        public PerguntaRespostaJogadorMap()
        {
            ToTable("PerguntaRespostaJogador");
            Property(x => x.Resposta).HasMaxLength(50).IsOptional();
            Property(x => x.Status).HasMaxLength(1).IsRequired();
            HasRequired(x => x.Pergunta);
            HasOptional(x => x.PerguntaResposta);
            HasRequired(x => x.Jogador);
            HasRequired(x => x.JogadorToken);
        }
    }
}