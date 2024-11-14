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
            }
        }
    }
}