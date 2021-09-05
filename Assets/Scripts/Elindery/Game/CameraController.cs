using System;
using UnityEngine;

namespace Elindery.Game
{
    public class CameraController : MonoBehaviour
    {
#region Singleton
        static CameraController _instance;
        public static CameraController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = GameObject.Find("Game Camera").GetComponent<CameraController>();
                    _instance._camera = _instance.GetComponent<Camera>();
                }
                return _instance;
            }
        }
#endregion

        public static event EventHandler<Transform> OnFollowedChanged;
        
        Transform _followingTarget;
        bool _following;
        Camera _camera;

        public void EnableFollowing(Transform target)
        {
            _followingTarget = target;
            _following = true;
            OnFollowedChanged?.Invoke(this, _followingTarget);
        }

        public Vector2 GetMousePosition()
        {
            Vector3 vector3 =_camera.ScreenToWorldPoint(Input.mousePosition);
            vector3.z = 0;
            return vector3;
        }

        void FixedUpdate()
        {
            if (!_following) return;
        
            Vector3 followingPosition = _followingTarget.localPosition;
            transform.localPosition = new Vector3(followingPosition.x, followingPosition.y, -10);
        }
    }
}
