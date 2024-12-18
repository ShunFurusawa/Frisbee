using UnityEngine;
using UnityEngine.AI;

public class Dog : MonoBehaviour
{
    [SerializeField] [JapaneseLabel("追いかけるターゲット")] private Transform target;
    [SerializeField] [JapaneseLabel("ターゲットに近づきすぎない距離")] private float stoppingDistance = 1.5f; 
    private NavMeshAgent agent; // ナビゲーションメッシュエージェント
    private bool isTargetInRange; // ターゲットが範囲内にいるかどうか

    private void Awake()
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