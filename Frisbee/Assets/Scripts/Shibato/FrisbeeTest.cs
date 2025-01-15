using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Shibato
{
    public class FrisbeeTest : MonoBehaviour
    {
        [SerializeField] [JapaneseLabel("プレイヤーカメラ")]
        private GameObject player;
        
        // [SerializeField] [JapaneseLabel("移動速度")]
        // private float speed = 5f;

        [SerializeField] [JapaneseLabel("火のオブジェクト、エフェクト")]
        private GameObject fireGameObject;
        [SerializeField] [JapaneseLabel("岩が壊れるエフェクト")]
        private GameObject stoneHitEffectPrefab;

        [SerializeField] private GameObject text;
        private PlayerCamera _camera;
        private Rigidbody _rb;
        private const float CameraRemovalDelay = 1f;
        private const float DestroyDelay = 2f;
        private bool totem = false;
        
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
            GameObject otherObject= other.gameObject;
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
                    RockCollision(otherObject);
                    break;
                case Tags.Escape:
                    EscapeCollision(otherObject);
                    break;
            }
        }
        private void ItemCollision(Collider item)
        {
            var copiedObject = Instantiate(item.gameObject, transform.position + Vector3.up, Quaternion.identity,
                transform);
            //copiedObject.transform.localScale = item.transform.localScale;
            Destroy(item.gameObject);
        }

        private void SpinesCollision()
        {
            //_camera.MyDestroyed();
            //Destroyanimetion();
            //GameManager.instance.Respawn();
        }

        private void RockCollision(GameObject crackedRock)
        {
            if (totem)
            {
                crackedRock.SetActive(false);
                // エフェクトを生成し、1秒後に削除
                if (stoneHitEffectPrefab != null)
                {
                    var stoneHit = Instantiate(stoneHitEffectPrefab, crackedRock.transform.position, Quaternion.identity);
                    SoundManager.instance.Play("break");
                    StartCoroutine(DestroyEffectAfterDelay(stoneHit, 1f));
                }
            }

        }
        private void FireCollision()
        {
            if (!FireElement)
            {
                var fireEffect = Instantiate(fireGameObject, transform.position, Quaternion.identity, transform);
                if (fireEffect != null) FireElement = true;
            }
        }

        private void EscapeCollision(GameObject Rock)
        {
            Rock.SetActive(false); 
            GameManager.instance.EscapeGame();
            // エフェクトを生成し、1秒後に削除
            if (stoneHitEffectPrefab != null)
            {
                var stoneHit = Instantiate(stoneHitEffectPrefab, Rock.transform.position, Quaternion.identity);
                SoundManager.instance.Play("break");
                StartCoroutine(DestroyEffectAfterDelay(stoneHit, 1f));
                
            }
            
        }

        public void GetTotem()
        {
            totem = true;
            text.SetActive(true);
            StartCoroutine(Text());
        }

        private void Destroyanimetion()
        {
            if (_camera != null)
                StartCoroutine(DestroySequence());
            else
                Destroy(gameObject);
        }
        private IEnumerator Text()
        {
            yield return new WaitForSeconds(5);
            text.SetActive(false);
        }
        private IEnumerator DestroySequence()
        {
            yield return new WaitForSeconds(CameraRemovalDelay);
            _camera.RemoveCamera();
            yield return new WaitForSeconds(DestroyDelay - CameraRemovalDelay);
            Destroy(gameObject);
        }
        private IEnumerator DestroyEffectAfterDelay(GameObject effect, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (effect != null)
            {
                Destroy(effect);
            }
        }

        private static class Tags
        {
            public const string Item = "Item";
            public const string Spines = "spines";
            public const string Fire = "Fire";
            public const string Rock = "CrackedRock";
            public const string Escape = "escape";
        }
    }
}