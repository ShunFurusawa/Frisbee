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
           // Debug.Log("now return");
            ReturnFrisbee();
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (GameManager.instance.State == FrisbeeState.Fly ||
            GameManager.instance.State == FrisbeeState.GetItem)
        {
            //TP可能な床か調べる
         //   if (Physics.Raycast(gameObject.transform.position, Vector3.down, out _hit, 1.0f))
          //  {}
                if (other.collider.gameObject.CompareTag("CanTP"))
                {
                    Teleport(_cameraRig, gameObject.transform);
                }
                else
                {
                    Debug.Log("can`t TP");
                    SetVelocityToZero();
                    GameManager.instance.State = FrisbeeState.Return;
                    if (GameManager.instance.State == FrisbeeState.Return)
                    {
                        Debug.Log("return state");
                    }
                    _RB.useGravity = false;
                    gameObject.layer = LayerMask.NameToLayer("Frisbee");
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {    //  Debug.Log("Trigger");
           Debug.Log("state = " + GameManager.instance.State);
        if (GameManager.instance.State == FrisbeeState.Return)
        {
            Debug.Log("call State");
            if (other.gameObject.CompareTag("GameController"))
            {
                Debug.Log("Tag");
                SetFrisbeeAtHand();
                Debug.Log("Return!");
            }
        }
    }

    private void Teleport(Transform tpObj, Transform tpPoint)
    {
        tpObj.position = tpPoint.position;

        if (tpPoint.position == tpObj.position)
        {
            GameManager.instance.State = FrisbeeState.Return;
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
    [SerializeField] private float _moveSpeed = 5f;
        
    private void ReturnFrisbee()
    {
      　//コライダー無効可->手元に飛ばす->SetFrisbee呼び出す
      // _collider.enabled = false;

       _target = _frisParent;
       _direction = (_target.position - transform.position).normalized;
       _RB.AddForce(_direction * _moveSpeed);
    }

    public void SetFrisbeeAtHand()
    {
        SetVelocityToZero();
        this.gameObject.transform.parent = _frisParent;
        //this.gameObject.transform.position = new Vector3(0, 0, 0.2f);
        
        if (gameObject.transform.parent == _frisParent)
        {
            
            if (gameObject.layer == LayerMask.NameToLayer("Frisbee"))
            {
                Debug.Log("Layer");
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            Debug.Log("call return frisbee");
           GameManager.instance.State = FrisbeeState.Have;
           SetVelocityToZero();
           this.gameObject.transform.localPosition = new Vector3(0, 0, 0.2f);
        }
        else
        {
            Debug.Log("parent fail!");
        }
        if (_collider.enabled == false)
        {
            _collider.enabled = true;
        }
    }
}
