using UnityEngine;
using System.Collections;

public class RockFall : MonoBehaviour
{
    [JapaneseLabel("岩のオブジェクト")][SerializeField] private GameObject rockPrefab;
    
    [JapaneseLabel("落石のクールタイム")][SerializeField] private  float spawnInterval = 2.0f;

    [JapaneseLabel("岩の生存時間")] [SerializeField]
    private float rockDestroyTime = 5.0f;

    private Coroutine spawnRoutine;
    private void OnValidate()
    {
        if (rockPrefab == null)
            Debug.LogError($"RockPrefab is required on {gameObject.name}");
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

    // 落石の生成を停止するメソッド
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
            // スクリプトが付いているオブジェクトの位置に落石を生成
            GameObject rock = Instantiate(rockPrefab, transform.position, transform.rotation);
            
            Destroy(rock, rockDestroyTime);
            
            // 次の生成まで待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // クールタイム
    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
    }
}