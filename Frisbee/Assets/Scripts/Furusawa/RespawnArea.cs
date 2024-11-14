using UnityEngine;

namespace Furusawa
{
    public class RespawnArea : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint;
       
        [Header("松明")]
        [SerializeField] private IgniteTorch[] Torches;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (GameManager.instance.SavePoint != respawnPoint.position)
                {
                    GameManager.instance.SavePoint = new Vector3(respawnPoint.position.x,
                        respawnPoint.position.y + 1,respawnPoint.position.z);
                    SetTorches();
                }
            }
        }

        private void SetTorches()
        {
            foreach (IgniteTorch torch in Torches)
            {
                torch.Ignite();
            }
        }
    }
}