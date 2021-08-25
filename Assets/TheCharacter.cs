using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCharacter : MonoBehaviour
{
    Transform _transform;
    public Transform Transform
    {
        get
        {
            if (_transform) return _transform;

            _transform = transform;
            _gunTransform = transform.Find("Gun").transform;
            _gunRenderer = _gunTransform.GetComponent<SpriteRenderer>();
            return _transform;
        }
    }
    Transform _gunTransform;
    SpriteRenderer _gunRenderer;

    public void SetPosition(Vector2 position)
    {
        transform.Translate(position * Time.fixedDeltaTime);
    }

    public void SetLookPosition(Vector3 position)
    {
        float angle = Vector2.Angle(Vector2.right, position - _gunTransform.position);//угол между вектором от объекта к мыше и осью х
        _gunTransform.eulerAngles = new Vector3(0f, 0f, _gunTransform.position.y < position.y ? angle : -angle);//немного магии на последок
        
        _gunRenderer.flipY = !(_gunTransform.eulerAngles.z < 90 || _gunTransform.eulerAngles.z > 270);
    }
}
