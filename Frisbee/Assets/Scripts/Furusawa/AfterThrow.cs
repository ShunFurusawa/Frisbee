using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AfterThrow : MonoBehaviour
{
    private RaycastHit _hit;
    
    private  Rigidbody _RB;
    private BoxCollider _collider;
     [SerializeField] private Transform _cameraRig;
   
    [Header("フリスビーの親オブジェクト(RightControllerAnchor)")]
    [SerializeField] private Transform _frisParent;

    private void Start()
    {
        _RB = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.State == FrisbeeState.Return)
        {
            ReturnFrisbee();
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        //TP可能な床か調べる
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out _hit, 1.0f))
        {
            if (_hit.collider.gameObject.CompareTag("CanTP"))
            {
                Teleport(_cameraRig, gameObject.transform);
            }
        }

        if (GameManager.instance.State == FrisbeeState.Return)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                SetFrisbeeAtHand();
            }
        }
    }

    private void Teleport(Transform tpObj, Transform tpPoint)
    {
        tpObj.position = tpPoint.position;

        if (tpPoint.position == tpObj.position)
        {
            SetFrisbeeAtHand();
            Debug.Log("TP Success!");
        }
        else
        {
            GameManager.instance.State = FrisbeeState.Return;
            
            //念のため速度ゼロに
            SetVelocityToZero();
            Debug.Log("TP Fail");
        }
    }

    private void SetVelocityToZero()
    {
        _RB.velocity = Vector3.zero;
        _RB.angularVelocity = Vector3.zero;
    }

    private Vector3 _direction;
    private Transform _target;
    [SerializeField] private float _moveSpeed = 0.5f;
        
    private void ReturnFrisbee()
    {
      　//コライダー無効可->手元に飛ばす->SetFrisbee呼び出す
       _collider.enabled = false;

       _target = _frisParent;
       _direction = (_target.position - transform.position).normalized;
       _RB.AddForce(_direction * _moveSpeed);
    }

    private void SetFrisbeeAtHand()
    {
        if (_collider.enabled == false)
        {
            _collider.enabled = true;
        }

        SetVelocityToZero();
        this.gameObject.transform.parent = _frisParent;
        this.gameObject.transform.position = new Vector3(0, 0, 0.2f);
        GameManager.instance.State = FrisbeeState.Have;
        
        if (gameObject.transform.parent == _frisParent)
        {
            Debug.Log("Return!");
        }
    }
}
