using Elindery.Internals;
using Elindery.WindowControllers;
using UnityEngine;

namespace Elindery.Game
{
    public class LoadController : MonoBehaviour
    {
        void Start()
        {
            OnStart();
            LogInController.Instance.Show();
        }

        public static void OnStart()
        {
            Application.targetFrameRate = 60;
            ConfigsProvider.Initialize();
        }
    }
}
