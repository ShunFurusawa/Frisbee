using System;
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

    [SerializeField] private AfterThrow afterThrow;
   // [SerializeField] private FrisbeeState currentFrisbeeState;
    private void Awake()
    {
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
        _beforeState = _currentState;
        _currentState = FrisbeeState.Have;
        SoundManager.instance.Play("BGM");
        SoundManager.instance.Play("Fire");
    }

    private void Update()
    {
        DebugInput();
    }

    private void FixedUpdate()
    {
        DebugStateChange();
    }
    
    private Vector3 _direction;
    public Vector3 Directon
    {
        get { return _direction; }
        set { _direction = value; }
    }

    private FrisbeeState _currentState;
    private FrisbeeState _beforeState;
    private void DebugStateChange()
    {
        if (_currentState != _beforeState)
        {
            Debug.Log("state change " + _beforeState + "->" + _currentState);
        }
        _beforeState = _currentState;
    }
    
    public FrisbeeState State
    {
        get { return _currentState; }
        set { _currentState = value; }
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
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // State = FrisbeeState.Have;
    }
    private Vector3 _savePoint;
    public Vector3 SavePoint
    {
        get { return _savePoint; }
        
        set { _savePoint = value; }
    }

    public void Respawn()
    {
        afterThrow.RespawnProcess();
        State = FrisbeeState.Have;
    }
}
