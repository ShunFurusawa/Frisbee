using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;


public class ThrowFrisbee : MonoBehaviour
{
   public XRNode node;

   [SerializeField] bool beforeVer = false; //フリスビーの飛ばし方
   private Vector3 _velocity; // 速度

    [Header("フリスビーは加速度を〇倍した速度で飛ぶ")]
    [SerializeField] private Vector3 controlPower = default!;
  
    private List<XRNodeState> states;
    private Rigidbody _RB;
    // Start is called before the first frame update
    void Start()
    {
        states = new List<XRNodeState>();
        _RB = gameObject.GetComponent<Rigidbody>();
        FreezeRB(true);
    }

    // Update is called once per frame
    void Update()
    {
        InputTracking.GetNodeStates(states);
        CheckReadyInput();
        CheckVelocity();
    }

    private void CheckVelocity()
    {
        if (GameManager.instance.State == FrisbeeState.Ready)
        {
            foreach (XRNodeState s in states)
            {
                if (s.nodeType == node)
                {
                 //   tracked = s.tracked;
                    s.TryGetVelocity(out _velocity);
                    Debug.Log(_velocity);
                    // s.TryGetAcceleration();   前回加速度の取得できなかったきがする
                    break;
                }
            }

            if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
            {
                //Ready状態でTriggerの入力がなくなる = 投げ
                GameManager.instance.State = FrisbeeState.Fly;
                SetUpFrisbeeRB();
                Throw();
            }
        }
    }

    private bool once;
    private void CheckReadyInput()
    {
        //フリスビーを持っている状態じゃなかったら入力取らない
        if (GameManager.instance.State != FrisbeeState.Have)
            return;

        if (_RB.freezeRotation == false)
        {
            FreezeRB(true);
            gameObject.GetComponent<AfterThrow>().SetFrisbeeAtHand();
        }
       
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            GameManager.instance.State = FrisbeeState.Ready;
        }
    }

    /// <summary>
    /// RotationとPositionのロック 持っている間だけ
    /// </summary>
    /// <param name="freeze">trueでロック falseで解除</param>
    private void FreezeRB(bool freeze)
    {
        if (freeze)
        {
            _RB.constraints = RigidbodyConstraints.FreezePosition;
            _RB.freezeRotation = true;
        }
        else
        {
            _RB.constraints = RigidbodyConstraints.None;
            _RB.freezeRotation = false;
        }
    }

    private void Throw()
    {
        //Frisbee飛ばす処理
        _velocity = Vector3.Scale(_velocity,  controlPower);
      
       if (beforeVer)
       {
           _RB.velocity = _velocity;
       }
       else
       {
           _RB.velocity = AdjustVelocity(_velocity);
       }
   
    }

    /// <summary>
    /// X, Z軸について、加速度が大きい方を優先させる。Ｙはそのまま
    /// </summary>
    /// <param name="velocity">コントローラーの加速度</param>
    /// <returns>優先度の低い軸の加速度は0</returns>
    private Vector3 AdjustVelocity(Vector3 velocity)
    {
        //累乗で符号外して比較する
        float maximum = Mathf.Pow(velocity.z, 2);

        if (maximum < Mathf.Pow(velocity.x, 2))
        {
            maximum = velocity.x;
        }
        else
        {
            velocity =  Vector3.Scale(velocity, new Vector3(0f, 1f, 1f));
        }

        /*if (maximum < Mathf.Pow(velocity.y, 2))
        {
            maximum = velocity.y;
        }
        else
        {
            velocity = Vector3.Scale(velocity, new Vector3(1f, 0f, 1f));
        }*/

        if (Mathf.Approximately(maximum, Mathf.Pow(velocity.z, 2)))
        {
            return velocity;   
        }
        else
        {
            velocity =  Vector3.Scale(velocity, new Vector3(1f, 1f, 0f));
            return velocity;
        }
    }

    private void SetUpFrisbeeRB()
    {
        //重力on 親子付け解除　加速度デカくしたほうがよさそう
        _RB.useGravity = true;
        gameObject.transform.parent = null;
        FreezeRB(false);
    }
} 
