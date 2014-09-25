using System.Data.Entity.ModelConfiguration;
using TopGame.Core.Domain;

namespace TopGame.Data.Mapping
{
    public class JogadorMap : EntityTypeConfiguration<Jogador>
    {
        public JogadorMap()
        {
            ToTable("Jogador");
            Property(x => x.Nome).HasMaxLength(80).IsRequired();
            Property(x => x.Email).HasMaxLength(200).IsRequired();
            Property(x => x.Documento).HasMaxLength(11).IsRequired();
            HasMany(x => x.JogadorTokens);
        }
    }
}