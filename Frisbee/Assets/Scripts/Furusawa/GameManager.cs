using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FrisbeeState
{
    Have,
    Ready,
    Fly,
    Return,
    GetItem
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
   // [SerializeField] private FrisbeeState currentFrisbeeState;
    private void Awake()
    {
        currentState = FrisbeeState.Have;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        m_beforeState = currentState;
    }
    
    private void FixedUpdate()
    {
        DebugStateChange();
    }

    private FrisbeeState m_beforeState;
    private void DebugStateChange()
    {
        if (currentState != m_beforeState)
        {
            Debug.Log("state change " + m_beforeState + "->" + currentState);
        }
        
        m_beforeState = currentState;
    }

    private FrisbeeState currentState;
    public FrisbeeState State
    {
        get { return currentState; }
        set { currentState = value; }
    }
  
}
