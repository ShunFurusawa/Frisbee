using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Update()
    {
        DebugInput();
    }

    private void FixedUpdate()
    {
        DebugStateChange();
    }

    private FrisbeeState currentState;
    private FrisbeeState m_beforeState;
    private void DebugStateChange()
    {
        if (currentState != m_beforeState)
        {
            Debug.Log("state change " + m_beforeState + "->" + currentState);
        }
        
        m_beforeState = currentState;
    }
    
    public FrisbeeState State
    {
        get { return currentState; }
        set { currentState = value; }
    }
    
    private void DebugInput()
    {
        // Yボタンでリスタート
        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
