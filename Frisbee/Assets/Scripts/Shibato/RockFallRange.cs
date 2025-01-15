using UnityEngine;
using System.Collections;

namespace Shibato
{
    public class RockFallRange :MonoBehaviour
    {
        [JapaneseLabel("岩のオブジェクト")] [SerializeField]
        private GameObject rockPrefab;

        [JapaneseLabel("落石のクールタイム")] [SerializeField]
        private float spawnInterval = 2.0f;

        [JapaneseLabel("岩の生存時間")] [SerializeField]
        private float rockDestroyTime = 5.0f;

        [JapaneseLabel("生成範囲の点A")] [SerializeField]
        private Transform spawnPointA;

        [JapaneseLabel("生成範囲の点B")] [SerializeField]
        private Transform spawnPointB;

        private Coroutine spawnRoutine;

        private void OnValidate()
        {
            if (rockPrefab == null)
                Debug.LogError($"RockPrefab is required on {gameObject.name}");
            if (spawnPointA == null || spawnPointB == null)
                Debug.LogWarning($"Spawn points A and B should be set on {gameObject.name}.");
        }

        private void OnDestroy()
        {
            StopSpawning();
        }

        private void Awake()
        {
            StartSpawning();
        }

        private void StartSpawning()
        {
            StopSpawning();
            spawnRoutine = StartCoroutine(SpawnRocks());
        }

        private void StopSpawning()
        {
            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }
        }

        private IEnumerator SpawnRocks()
        {
            while (true)
            {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(spawnPointA.position.x, spawnPointB.position.x),
                    Random.Range(spawnPointA.position.y, spawnPointB.position.y),
                    Random.Range(spawnPointA.position.z, spawnPointB.position.z)
                );
                GameObject rock = Instantiate(rockPrefab, spawnPosition, transform.rotation);

                Destroy(rock, rockDestroyTime);

                yield return new WaitForSeconds(spawnInterval);
            }
        }

        public void SetSpawnInterval(float interval)
        {
            spawnInterval = interval;
        }
    }
}