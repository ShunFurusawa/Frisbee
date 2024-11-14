using System;
using UnityEngine;
using UnityEngine.Serialization;

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
<<<<<<< HEAD
                {
                    GameManager.instance.SavePoint = respawnPoint.position;
                    SetTorches();
                }
            }
        }

        private void SetTorches()
        {
            foreach (IgniteTorch torch in Torches)
            {
                torch.Ignite();
=======
                  GameManager.instance.SavePoint = new Vector3(respawnPoint.position.x,
                      respawnPoint.position.y+1,respawnPoint.position.z) ;
>>>>>>> 8995921e921d14f8fed65306d0b204f0d9d83466
            }
        }
    }
}