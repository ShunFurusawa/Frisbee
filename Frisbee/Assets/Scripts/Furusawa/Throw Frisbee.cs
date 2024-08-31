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
    public Vector3 m_velocity; // 速度

    [SerializeField] private Vector3 controlPower = default!;
  
    private List<XRNodeState> states;
    private Rigidbody m_RB;
    // Start is called before the first frame update
    void Start()
    {
        states = new List<XRNodeState>();
        m_RB = gameObject.GetComponent<Rigidbody>();
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
        if (GameManager.instance.GetCurrentState() == FrisbeeState.Ready)
        {
            foreach (XRNodeState s in states)
            {
                if (s.nodeType == node)
                {
                    tracked = s.tracked;
                    s.TryGetVelocity(out m_velocity);
                
                    // s.TryGetAcceleration();   前回加速度の取得できなかったきがする
                    break;
                }
            }

            if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
            {
                //Ready状態でTriggerの入力がなくなる = 投げ?
                GameManager.instance.SetCurrentState(FrisbeeState.Fly);
                SetUpFrisbeeRB();
                Throw();
            }
        }
    }

    private void CheckReadyInput()
    {
        //フリスビーを持っている状態じゃなかったら入力取らない
        if (GameManager.instance.GetCurrentState() != FrisbeeState.Have)
            return;
        
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            GameManager.instance.SetCurrentState(FrisbeeState.Ready);
        }
    }

    private void Throw()
    {
        //Frisbee飛ばす処理
        Vector3 direction;
      //  direction = (transform.position - m_direction.position).normalized;
     //   m_velocity = Vector3.Scale(m_velocity, direction);
        m_velocity = Vector3.Scale(m_velocity,  controlPower);
        
       // m_RB.AddForce(m_velocity, ForceMode.Impulse);
       m_RB.velocity = m_velocity;
    }

    private void SetUpFrisbeeRB()
    
    {
        //重力on 親子付け解除　加速度デカくしたほうがよさそう
        m_RB.useGravity = true;
        gameObject.transform.parent = null;
    }
} 
