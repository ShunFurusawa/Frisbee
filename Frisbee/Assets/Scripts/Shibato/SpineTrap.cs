using UnityEngine;
using System.Collections;

namespace Shibato
{
    public class SpineTrap:MonoBehaviour
    {
        [SerializeField] private float upPositionY = 2f;  // とげが上がる位置
        [SerializeField] private float downPositionY = 0f;  // とげが下がる位置
        [SerializeField] private float moveDuration = 0.5f;  // とげが動く時間
        [SerializeField] private float interval = 5f;  // 上下する間隔
        [SerializeField] private GameManager gameManager;

        private bool isUp = false;  // 現在の状態
        private Vector3 upPosition;
        private Vector3 downPosition;

        private void Start()
        {
            upPosition = new Vector3(transform.position.x, upPositionY, transform.position.z);
            downPosition = new Vector3(transform.position.x, downPositionY, transform.position.z);
            StartCoroutine(ControlSpikes());
        }

        private IEnumerator ControlSpikes()
        {
            while (true)
            {
                Vector3 targetPosition = isUp ? downPosition : upPosition;
                yield return MoveSpikes(targetPosition);
                isUp = !isUp;
                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator MoveSpikes(Vector3 targetPosition)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;  // 最終位置を正確に設定
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("spines"))
            {
                gameManager.Respawn();
            }
        }
    }
}