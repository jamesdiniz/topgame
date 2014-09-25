using System.Data.Entity.ModelConfiguration;
using TopGame.Core.Domain;

namespace TopGame.Data.Mapping
{
    public class JogadorTokenMap : EntityTypeConfiguration<JogadorToken>
    {
        public JogadorTokenMap()
        {
            ToTable("JogadorToken");
            Property(x => x.Codigo).HasMaxLength(40).IsRequired();
            HasRequired(x => x.Jogador);
        }
    }
}