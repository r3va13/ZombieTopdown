using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class TheCharacter : MonoBehaviour
{
    //public SkinConfig SkinConfig;
    
    Transform _transform;
    public Transform Transform => _transform;
    
    //Transform _gunHolder;
    //Transform _gunUpHolder;
    Transform _holder;

    /*SpriteRenderer _gunRenderer;
    SpriteRenderer _gunUpRenderer;
    SpriteRenderer _head;
    SpriteRenderer _body;
    SpriteRenderer _feet;*/

    Animator _gunAnimator;
    Animator _feetAnimator;

    enum LookSide
    {
        Side,
        Down,
        Up
    }

    public void Initialize()
    {
        _transform = transform;
        _holder = transform.Find("Holder");
        /*_gunHolder = _holder.Find("GunHolder");
        _gunUpHolder = _holder.Find("GunUpHolder");
        _gunRenderer = _gunHolder.Find("Gun").GetComponent<SpriteRenderer>();
        _gunUpRenderer = _gunUpHolder.Find("GunUp").GetComponent<SpriteRenderer>();
        _head = _holder.Find("Head").GetComponent<SpriteRenderer>();
        _body = _holder.Find("Body").GetComponent<SpriteRenderer>();
        _feet = _holder.Find("Feet").GetComponent<SpriteRenderer>();*/
        _gunAnimator = _holder.Find("Gun").GetComponent<Animator>();
        _feetAnimator = _holder.Find("Feet").GetComponent<Animator>();
    }

    public void SetPosition(Vector2 position)
    {
        transform.Translate(position * Time.fixedDeltaTime);
        _feetAnimator.SetTrigger("Run");
    }

    public void SetLookPosition(Vector3 position)
    {
        /*float gunAngle = Vector2.Angle(Vector2.right, position - _gunTransform.position);//угол между вектором от объекта к мыше и осью х
        //float gunUpAngle = Vector2.Angle(Vector2.right, position - _gunUpTransform.position);//угол между вектором от объекта к мыше и осью х
        _gunTransform.eulerAngles = new Vector3(0f, 0f, _gunTransform.position.y < position.y ? gunAngle : -gunAngle);//немного магии на последок
        _gunUpTransform.eulerAngles = new Vector3(0f, 0f, _gunUpTransform.position.y < position.y ? gunAngle : -gunAngle);//немного магии на последок*/

        Vector3 aimDirection = (position - _holder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _holder.eulerAngles = new Vector3(0, 0, angle);

        //bool isLeft = angle <= -90 || angle >= 90;
        
        //transform.localScale = _gunHolder.localScale = new Vector3((angle <= -90 || angle >= 90) ? -1 : 1, 1, 1);

        //_head.flipX = _body.flipX = _feet.flipX = isLeft;
        //_gunRenderer.flipY = _gunUpRenderer.flipY = (angle <= -90 || angle >= 90);
        
        //Debug.Log(angle);

       /* LookSide lookSide = LookSide.Up;

        if (angle < -40 && angle > -140) lookSide = LookSide.Down;
        if (angle < 40 || angle > 140) lookSide = LookSide.Side;

        switch (lookSide)
        {
            case LookSide.Side:
                _head.sprite = SkinConfig.HeadSide;
                _gunRenderer.transform.localPosition = new Vector3(_gunRenderer.transform.localPosition.x, _gunRenderer.transform.localPosition.y, -0.004f);
                break;
            case LookSide.Down:
                _head.sprite = SkinConfig.HeadDown;
                _gunRenderer.transform.localPosition = new Vector3(_gunRenderer.transform.localPosition.x, _gunRenderer.transform.localPosition.y, -0.004f);
                break;
            case LookSide.Up:
                _head.sprite = SkinConfig.HeadBack;
                _gunRenderer.transform.localPosition = new Vector3(_gunRenderer.transform.localPosition.x, _gunRenderer.transform.localPosition.y, 0f);
                break;
            default: throw new ArgumentOutOfRangeException();
        }*/
    }

    public void Shoot()
    {
        _gunAnimator.SetTrigger("Shoot");
        UtilsClass.ShakeCamera(0.05f, 0.2f);
    }
}
