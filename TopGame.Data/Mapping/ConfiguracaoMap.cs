using System.Data.Entity.ModelConfiguration;
using TopGame.Core.Domain;

namespace TopGame.Data.Mapping
{
    public class ConfiguracaoMap : EntityTypeConfiguration<Configuracao>
    {
        public ConfiguracaoMap()
        {
            ToTable("Configuracao");
            HasRequired(c => c.Jogo);
        }
    }
}