using UnityEngine;

namespace Elindery.Game
{
    public class ServerControlledUnit : MonoBehaviour
    {
        public string ClientID { get; set; }
        public Transform Transform => _transform;
        protected Rigidbody2D Rigidbody2D { get; private set; }

        Transform _transform;
        protected Transform Holder;
    
        protected float WalkTurnOffTime;
        float _serverMoveDelayTime = 100; //0.1 сек. для плавной отрисовки движения и поворота с момента получения команды с сервера

        public Health Health { get; protected set; }

        //Server
        protected Vector2 OldPosition;
        protected Vector2 NewPosition;
        protected float OldRotation;
        protected float NewRotation;

        public virtual void Initialize()
        {
            _transform = transform;
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Holder = _transform.Find("Holder");
        
            OldPosition = NewPosition = Rigidbody2D.position;
            OldRotation = NewRotation = Rigidbody2D.rotation;

            _serverMoveDelayTime = 100;
        }
    
        public virtual void InitializePositionFromServer(Vector3 position)
        {
            Transform.position = position;
            OldPosition = NewPosition = _transform.position;//Специально сделан трансформ, т.к. при использовании Rigidbody они бьются о стены.
        }

        public virtual void InitializeLookDirectionFromServer(float angle)
        {
            Rigidbody2D.MoveRotation(angle);
            OldRotation = NewRotation = angle;
        }
    
        public virtual void SetServerPosition(Vector3 position)
        {
            OldPosition = Rigidbody2D.position;
            NewPosition = position;
            _serverMoveDelayTime = 0f;
            WalkTurnOffTime = 0.1f;
        }

        public void SetServerLookAngle(float angle)
        {
            OldRotation = Rigidbody2D.rotation;
            NewRotation = angle;
        }

        protected virtual void FixedUpdate()
        {
            PlayMoveAndRotationFromServer();

            if (WalkTurnOffTime > 0) WalkTurnOffTime -= Time.deltaTime;
        }

        void PlayMoveAndRotationFromServer()
        {
            if (_serverMoveDelayTime < 0 || _serverMoveDelayTime > 1) return;
        
            Rigidbody2D.MovePosition(Vector3.Lerp(OldPosition, NewPosition, _serverMoveDelayTime));
            float angle = Mathf.LerpAngle(OldRotation , NewRotation, _serverMoveDelayTime);
            Rigidbody2D.MoveRotation(angle);
            _serverMoveDelayTime += Time.deltaTime * 10;
        }
    }
}
