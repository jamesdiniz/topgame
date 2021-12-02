using System.Runtime.CompilerServices;

namespace TopGame.Core.Infrastructure
{
    public class EngineContext
    {
        #region Properties

        public static IAppEngine Current
        {
            get
            {
                if (Singleton<IAppEngine>.Instance == null)
                {
                    Initialize();
                }
                return Singleton<IAppEngine>.Instance;
            }
        }

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IAppEngine Initialize()
        {
            var instance = Singleton<IAppEngine>.Instance;
            if (instance == null)
            {
                instance = CreateEngineInstance();
                instance.Initialize();

                Singleton<IAppEngine>.Instance = instance;
            }
            return instance;
        }

        #endregion

        #region Utilities

        protected static IAppEngine CreateEngineInstance()
        {
            return new AppEngine();
        }

        #endregion
    }
}