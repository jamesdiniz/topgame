using System.Data.Entity.ModelConfiguration;
using TopGame.Core.Domain;

namespace TopGame.Data.Mapping
{
    public class PerguntaMap : EntityTypeConfiguration<Pergunta>
    {
        public PerguntaMap()
        {
            ToTable("Pergunta");
            Property(x => x.Descricao).HasMaxLength(500).IsRequired();
            Property(x => x.Dica).HasMaxLength(255).IsOptional();
            HasRequired(x => x.Jogo);
            HasMany(x => x.PerguntaRespostas);
        }
    }
}