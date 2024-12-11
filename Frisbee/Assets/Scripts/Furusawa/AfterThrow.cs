using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AfterThrow : MonoBehaviour
{
    private RaycastHit _hit;
    
    private  Rigidbody _rb;
    private BoxCollider _collider;
     [SerializeField] private Transform _cameraRig;
   
    [Header("フリスビーの親オブジェクト(RightControllerAnchor)")]
    [SerializeField] private Transform _frisParent;
    
    [Header("フリスビーは〇fのスピードから加速して手元に戻ってくる")]
    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    private float time;
    private void Update()
    {
        // Bボタンでフリスビー戻す
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            if (GameManager.instance.State == FrisbeeState.Fly)
            {
                ReturnFrisbee();
            }
        }
        
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            RespawnProcess();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.State == FrisbeeState.Return)
        {
           // Debug.Log("now return");
            time += Time.fixedDeltaTime;
            AddReturnForce();
        }
        else
        {
            time = 0f;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (GameManager.instance.State == FrisbeeState.Fly ||
            GameManager.instance.State == FrisbeeState.GetItem)
        {
            //TP可能な床ならTP
            if (other.gameObject.CompareTag("CanTP"))
            {
                Teleport(_cameraRig, gameObject.transform);
            }
            else
            {
                Debug.Log("can`t TP");
                    
                // 反射可能オブジェクトなら手元に戻さない *注意:タグが認識されない可能性あり
                if (other.gameObject.CompareTag("Reflective"))
                    return;

                ReturnFrisbee();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (GameManager.instance.State == FrisbeeState.Return)
        {
            if (other.gameObject.CompareTag("GameController"))
            {
                SetFrisbeeAtHand();
                SoundManager.instance.Play("Catch");
            }
        }
    }

    private void Teleport(Transform tpObj, Transform tpPoint)
    {
        //tpObj.position = tpPoint.position;
        tpPoint.position= new Vector3(tpPoint.position.x, tpPoint.position.y+1, tpPoint.position.z);
        tpObj.position = tpPoint.position;

        if (tpPoint.position == tpObj.position)
        {
            GameManager.instance.State = FrisbeeState.Return;
            SetFrisbeeAtHand();
            Debug.Log("TP Success!");
            SoundManager.instance.Play("TP");
        }
        else
        {
            GameManager.instance.State = FrisbeeState.Return;
            SoundManager.instance.Play("Fail");
            //念のため速度ゼロに
            SetVelocityToZero();
            Debug.Log("TP Fail");
        }
    }

    private void SetVelocityToZero()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private Vector3 _direction;
    private Transform _target;
    private void AddReturnForce()
    {
      　//コライダー無効可->手元に飛ばす->SetFrisbee呼び出す
       // _collider.enabled = false;

       _target = _frisParent;
       _direction = (_target.position - transform.position).normalized;
       _direction *= moveSpeed; 
       _rb.velocity = _direction * time;
    }

    /// <summary>
    /// これ呼べば手元に戻ってくる
    /// </summary>
    public void ReturnFrisbee()
    {
        SetVelocityToZero();
        GameManager.instance.State = FrisbeeState.Return;
                    
        /*if (GameManager.instance.State == FrisbeeState.Return)
        {
            Debug.Log("return state");
        }*/
        _rb.useGravity = false;
        gameObject.layer = LayerMask.NameToLayer("Frisbee");
        SoundManager.instance.Play("Return");
    }

    public void SetFrisbeeAtHand()
    {
        SetVelocityToZero();
        this.gameObject.transform.parent = _frisParent;
        
        if (gameObject.transform.parent == _frisParent)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Frisbee"))
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
            } 
            
            //Debug.Log("call return frisbee");
            GameManager.instance.State = FrisbeeState.Have;
            SetVelocityToZero();
            this.gameObject.transform.localPosition = new Vector3(0, 0, 0.2f);
            this.gameObject.transform.localRotation = Quaternion.identity;
            SoundManager.instance.StopPlay("Return");
        }
        else
        {
            Debug.LogAssertion("parent fail!");
        }
        if (_collider.enabled == false)
        {
            _collider.enabled = true;
        }
    }

    public void RespawnProcess()
    {
        _cameraRig.position = GameManager.instance.SavePoint;
        SetFrisbeeAtHand();
        SoundManager.instance.Play("Respawn");
    }
}
