using UnityEngine;

namespace Shibato.feature
{
    public class Fire : MonoBehaviour
    {
        [SerializeField] private GameObject firePrefab;

        [SerializeField] [Header("火が付いた時に何か起こすオブジェクト")]
        private GameObject gimmickGameObject;

        private GameObject currentFireInstance;

        private void OnDestroy()
        {
            if (currentFireInstance != null) Destroy(currentFireInstance);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            var frisbeeTest = other.GetComponent<FrisbeeTest>();
            if (frisbeeTest == null || !frisbeeTest.FireElement) return;

            if (currentFireInstance == null)
                currentFireInstance = Instantiate(firePrefab, transform.position + Vector3.up, Quaternion.identity,
                    transform);

            if (gimmickGameObject != null) Destroy(gimmickGameObject);
        }
    }
}