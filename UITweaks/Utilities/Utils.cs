using IPA.Loader;
using UnityEngine.SceneManagement;

namespace UITweaks.Utilities
{
    /// <summary>
    /// A collection of useful utilities for debugging purposes. Mainly used in the C# REPL shipped with the RUE
    /// </summary>
    public class Utils
    {
        public static void OutputAllActiveScenesToLog(IPA.Logging.Logger logger)
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++) logger.Info(SceneManager.GetSceneAt(i).name);
        }
    }
}
