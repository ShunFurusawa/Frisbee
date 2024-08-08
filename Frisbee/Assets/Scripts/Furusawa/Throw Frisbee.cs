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
    
    [SerializeField] private float controlPower = 0.05f;

    private List<XRNodeState> states;
    // Start is called before the first frame update
    void Start()
    {
        states = new List<XRNodeState>();
    }

    // Update is called once per frame
    void Update()
    {
        InputTracking.GetNodeStates(states);
        CheckInput();
        
        CheckVelocity();
        
    }

    private void CheckVelocity()
    {
        if (GameManager.instance.ReturnCurrentState() == FrisbeeState.Ready)
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

            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) == false)
            {
                //Ready状態でTriggerの入力がなくなる = 投げ?
                Throw();
                GameManager.instance.SetCurrentState(FrisbeeState.Fly);
            }
        }
    }

    private void CheckInput()
    {
        //フリスビーを持っている状態じゃなかったら入力取らない
        if (GameManager.instance.ReturnCurrentState() != FrisbeeState.Have)
            return;
        
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            GameManager.instance.SetCurrentState(FrisbeeState.Ready);
        }
    }

    private void Throw()
    {
        //Frisbee飛ばす処理
    }
}
