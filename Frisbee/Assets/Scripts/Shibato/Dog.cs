using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Dog : MonoBehaviour
{
    public Transform target;  // 追いかけるターゲット（例: プレイヤー）
    public float stoppingDistance = 1.5f;  // ターゲットに近づきすぎない距離
    private NavMeshAgent agent;  // ナビゲーションメッシュエージェント
    private bool isTargetInRange = false;  // ターゲットが範囲内にいるかどうか

    void Start()
    {
        // NavMeshAgentコンポーネントを取得
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isTargetInRange)
        {
            // ターゲットまでの距離を計算
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // ターゲットを追跡
            agent.SetDestination(target.position);

            // 停止距離に達したら停止
            if (distanceToTarget < stoppingDistance)
            {
                agent.isStopped = true;  // 追跡を停止
            }
            else
            {
                agent.isStopped = false;  // 追跡を続行
            }
        }
    }

    // トリガー範囲に入った時の処理
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            isTargetInRange = true;  // ターゲットが範囲内に入ったことを記録
        }
    }

    // トリガー範囲から出た時の処理
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTargetInRange = false;  // ターゲットが範囲外に出たことを記録
            agent.isStopped = true;  // 追跡を停止
        }
    }
}
