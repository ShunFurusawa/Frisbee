using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FrisbeeState
{
    Have,
    Ready,
    Fly,
    GetItem
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [SerializeField] private FrisbeeState currentFrisbeeState;
    private void Awake()
    {
        SetCurrentState(FrisbeeState.Have);

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
    public void SetCurrentState(FrisbeeState state)
    {
        currentFrisbeeState = state;
    }
    public FrisbeeState ReturnCurrentState()
    {
        return currentFrisbeeState;
    }
}
