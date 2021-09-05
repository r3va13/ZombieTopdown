using UnityEngine.SceneManagement;

namespace Elindery.Internals
{
    public static class SceneManager
    {
        public static bool Initialized { get; private set; }
        public static void LoadGameScene()
        {
            Initialized = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }
}
