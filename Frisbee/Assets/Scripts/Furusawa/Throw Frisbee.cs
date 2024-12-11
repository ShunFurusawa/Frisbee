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

    void Start()
    {
        states = new List<XRNodeState>();
        _RB = gameObject.GetComponent<Rigidbody>();
        FreezeRB(true);
    }
   
    void Update()
    {
        InputTracking.GetNodeStates(states);
        CheckReadyInput();
        CheckVelocity();
    }

    public Vector3 Velocity
    {
        get { return _velocity; }

        set { _velocity = value; }
    }

    private void CheckVelocity()
    {
        if (GameManager.instance.State == FrisbeeState.Ready)
        {
            foreach (XRNodeState s in states)
            {
                if (s.nodeType == node)
                {
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
          //  SoundManager.instance.Play("Ready");
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

    [Header("フリスビーは上方向に〇fの力が加わる")]
    [SerializeField] private float upScale;
    /// <summary>
    /// Frisbee飛ばす一連の処理
    /// </summary>
    private void Throw()
    {
      
        _velocity = Vector3.Scale(_velocity,  controlPower);

        Vector3 direction = GameManager.instance.Directon;
           
        _velocity = AdjustVelocity(_velocity);
           
        //振る速度応じて上げたい
        // _RB.velocity = Vector3.Scale(direction, _velocity);
        _RB.velocity =　Vector3.Scale(direction, _velocity);
        
        _RB.AddForce(Vector3.up * upScale, ForceMode.Impulse);
        SoundManager.instance.Play("Throw");
    }

    /// <summary>
    /// 振った速度に応じてフリスビーの加速度を変化させるためにコントローラーの加速度3軸について、大きい方を優先させ、velocityを新たに計算
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
        
        if (maximum < Mathf.Pow(velocity.y, 2))
        {
            maximum = velocity.y;
        }
        
        if (Mathf.Approximately(maximum, Mathf.Pow(velocity.z, 2)))
        {
            //平方根で元に戻す
            maximum = Mathf.Sqrt(maximum);
        }
      
        velocity = new Vector3(maximum, maximum, maximum);
            
        return velocity;   
    }

    private void SetUpFrisbeeRB()
    {
        //重力on 親子付け解除　加速度デカくしたほうがよさそう
        _RB.useGravity = true;
        gameObject.transform.parent = null;
        FreezeRB(false);
    }
} 
