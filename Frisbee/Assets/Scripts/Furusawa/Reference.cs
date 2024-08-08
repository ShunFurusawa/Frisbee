using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;


public class Reference : MonoBehaviour
{
    public XRNode node;

    public bool tracked = false; // データ取得可能か
    public Vector3 velocity; // 速度
    
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float power = 0.01f;

    [SerializeField] private float controlPower = 0.05f;
    [SerializeField] AudioClip RacketHitS = default!;

    // Start is called before the first frame update
    void Start()
    {
     //   RightCon = OVRInput.Controller.RTouch;

    }

    // Update is called once per frame
    void Update()
    {
        // すべてのノードのデータを取得する

        // データ用のListを用意する
        List<XRNodeState> states = new List<XRNodeState>();
        // 最新のデータを取得する（全ノード分）
        InputTracking.GetNodeStates(states);
        
        // 取得したデータを確認する
        foreach (XRNodeState s in states)
        {
            if (s.nodeType == node) //ノードが合えば
            {
                tracked = s.tracked;
                s.TryGetVelocity(out velocity);
                break;
            }
        }
        
       // Debug.Log(tracked);
        
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
           // GameManager.instance.StartButton();
            
            /*GameObject ball;
            Rigidbody ballRigidbody;
            
            //Aボタンが押されたときの処理

            ball = Instantiate(ballPrefab, SpawnPoint.position, Quaternion.identity);

            ballRigidbody = ball.GetComponent<Rigidbody>();
            
            ballRigidbody.AddForce(new Vector3(0f, 0f, power), ForceMode.Impulse);*/

        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            //B
          //  GameManager.instance.RestartButton();
        }
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            //Y
           // GameManager.instance.EndButton();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
           // GameManager.instance.PlaySE(RacketHitS);

            //velocityがそのままだと強すぎるのでcontrolPowerを掛けて制限する
            velocity = Vector3.Scale(velocity, new Vector3(controlPower, controlPower, controlPower));
            other.rigidbody.velocity = velocity;
            other.rigidbody.AddForce(velocity, ForceMode.Impulse);
        }
    }
}