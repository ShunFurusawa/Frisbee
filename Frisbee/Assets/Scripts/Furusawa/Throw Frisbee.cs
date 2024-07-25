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

    }

    private void CheckInputState()
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
    }
}
