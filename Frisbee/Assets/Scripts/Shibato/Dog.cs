using UnityEngine;
using UnityEngine.AI;

public class Dog : MonoBehaviour
{
    public Transform target; // 追いかけるターゲット（例: プレイヤー）
    public float stoppingDistance = 1.5f; // ターゲットに近づきすぎない距離
    private NavMeshAgent agent; // ナビゲーションメッシュエージェント
    private bool isTargetInRange; // ターゲットが範囲内にいるかどうか

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isTargetInRange)
        {
            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            agent.SetDestination(target.position);

            if (distanceToTarget < stoppingDistance)
                agent.isStopped = true;
            else
                agent.isStopped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            isTargetInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTargetInRange = false;
            agent.isStopped = true;
        }
    }
}