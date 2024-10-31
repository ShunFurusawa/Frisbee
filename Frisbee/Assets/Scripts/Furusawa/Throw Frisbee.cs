using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;


public class ThrowFrisbee : MonoBehaviour
{
   public XRNode node;

   public bool tracked = false; //データ取得可能か
   public Vector3 _velocity; // 速度

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
                    tracked = s.tracked;
                    s.TryGetVelocity(out _velocity);
                
                    // s.TryGetAcceleration();   前回加速度の取得できなかったきがする
                    break;
                }
            }

            if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
            {
                //Ready状態でTriggerの入力がなくなる = 投げ?
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
        
       // m_RB.AddForce(m_velocity, ForceMode.Impulse);
       
       _RB.velocity = AdjustVelocity(_velocity);
    }

    private Vector3 AdjustVelocity(Vector3 velocity)
    {
        float maximum = velocity.z;

        if (maximum < velocity.x)
        {
            maximum = velocity.x;
        }
        else
        {
            Vector3.Scale(velocity, new Vector3(0f, 1f, 1f));
        }

        if (maximum < velocity.y)
        {
            maximum = velocity.y;
        }
        else
        {
            Vector3.Scale(velocity, new Vector3(1f, 0f, 1f));
        }

        if (Mathf.Approximately(maximum, velocity.z))
        {
            return velocity;   
        }
        else
        {
            Vector3.Scale(velocity, new Vector3(1f, 1f, 0f));
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
