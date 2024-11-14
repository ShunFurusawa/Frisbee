using System.Collections;
using UnityEngine;

namespace Shibato
{
    public class FrisbeeTest : MonoBehaviour
    {
        [SerializeField] [JapaneseLabel("プレイヤーカメラ")]
        private GameObject player;

        [SerializeField] private GameManager gameManager;
        
        // [SerializeField] [JapaneseLabel("移動速度")]
        // private float speed = 5f;

        [SerializeField] [JapaneseLabel("火のオブジェクト、エフェクト")]
        private GameObject fireGameObject;

        private PlayerCamera _camera;
        private Rigidbody _rb;
        private const float CameraRemovalDelay = 1f;
        private const float DestroyDelay = 2f;
        
        public bool FireElement { get; private set; }

        private void Awake()
        {
            _rb = transform.GetComponent<Rigidbody>();
            if (player == null)
                Debug.LogError($"[{nameof(FrisbeeTest)}] Player reference is not set!", this);
            _camera = player.gameObject.GetComponent<PlayerCamera>();
            if (_camera == null)
                Debug.LogError($"[{nameof(FrisbeeTest)}] PlayerCamera component not found on player!", this);
        }

        private void Update()
        {
            //_rb.velocity = new Vector3(speed, _rb.velocity.y, _rb.velocity.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case Tags.Item:
                    ItemCollision(other);
                    break;
                case Tags.Spines:
                    SpinesCollision();
                    break;
                case Tags.Fire:
                    FireCollision();
                    break;
                case Tags.Rock:
                    SpinesCollision();
                    break;
            }
        }

        private void ItemCollision(Collider item)
        {
            var copiedObject = Instantiate(item.gameObject, transform.position + Vector3.up, Quaternion.identity,
                transform);
            copiedObject.transform.localScale = item.transform.localScale;
            Destroy(item.gameObject);
        }

        private void SpinesCollision()
        {
            //_camera.MyDestroyed();
            //Destroyanimetion();
             gameManager.Respawn();
        }
        private void FireCollision()
        {
            if (!FireElement)
            {
                var fireEffect = Instantiate(fireGameObject, transform.position, Quaternion.identity, transform);
                if (fireEffect != null) FireElement = true;
            }
        }

        private void Destroyanimetion()
        {
            if (_camera != null)
                StartCoroutine(DestroySequence());
            else
                Destroy(gameObject);
        }

        private IEnumerator DestroySequence()
        {
            yield return new WaitForSeconds(CameraRemovalDelay);
            _camera.RemoveCamera();
            yield return new WaitForSeconds(DestroyDelay - CameraRemovalDelay);
            Destroy(gameObject);
        }

        private static class Tags
        {
            public const string Item = "Item";
            public const string Spines = "spines";
            public const string Fire = "Fire";
            public const string Rock = "Rock";
        }
    }
}