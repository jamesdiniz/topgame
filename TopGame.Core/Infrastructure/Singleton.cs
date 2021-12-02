
namespace TopGame.Core.Infrastructure
{
    /// <summary>
    /// Classe genérica estática para armazenar objeto durante tempo de vida da aplicação
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T>
    {
        /// <summary>
        /// Construtor estático
        /// </summary>
        static Singleton()
        {
        }

        /// <summary>
        /// Instância estática do tipo T
        /// </summary>
        public static T Instance { get; set; }
    }
}