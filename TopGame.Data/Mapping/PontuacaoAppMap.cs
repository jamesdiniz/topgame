using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TopGame.Core.Domain.Pontuacao;

namespace TopGame.Data.Mapping
{
    public class PontuacaoAppMap : EntityTypeConfiguration<PontuacaoApp>
    {
        public PontuacaoAppMap()
        {
            Map(x =>
            {
                x.MapInheritedProperties();
                x.ToTable("PontuacaoApp");
            });
            HasKey(x => x.PontuacaoId);
            Property(c => c.PontuacaoId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Quantidade).HasPrecision(9, 2).IsRequired();
            HasRequired(x => x.Pergunta);
            HasRequired(x => x.Jogador);
        }
    }
}