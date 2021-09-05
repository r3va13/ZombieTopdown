using UnityEngine;

namespace Elindery.Game
{
    public class FXProvider : MonoBehaviour
    {
#region Singleton
        static FXProvider _instance;
        public static FXProvider Instance
        {
            get
            {
                if (!_instance) _instance = GameObject.Find("Game").GetComponent<FXProvider>();
                return _instance;
            }
        }
#endregion
        
        public Material BulletTracerMaterial;
    }
}
