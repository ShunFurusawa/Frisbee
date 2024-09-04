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
    
    [SerializeField] private FrisbeeState currentFrisbeeState;
    private void Awake()
    {
        State = FrisbeeState.Have;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private FrisbeeState _state;
    public FrisbeeState State
    {
        get { return _state; }
        set { _state = value; }
    }
  
}
