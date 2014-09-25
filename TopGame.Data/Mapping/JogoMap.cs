using System.Data.Entity.ModelConfiguration;
using TopGame.Core.Domain;

namespace TopGame.Data.Mapping
{
    public class JogoMap : EntityTypeConfiguration<Jogo>
    {
        public JogoMap()
        {
            ToTable("Jogo");
            Property(x => x.Titulo).HasMaxLength(100).IsRequired();
            Property(x => x.TituloUrl).HasMaxLength(200).IsRequired();
            Property(x => x.Descricao).HasMaxLength(11).IsOptional();
            Property(x => x.Token).HasMaxLength(40).IsRequired();
            HasMany(x => x.Perguntas);
            HasMany(x => x.Premiacoes);
        }
    }
}