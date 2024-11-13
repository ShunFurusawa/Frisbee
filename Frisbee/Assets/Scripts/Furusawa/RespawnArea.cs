using System;
using UnityEngine;

namespace Furusawa
{
    public class RespawnArea : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (GameManager.instance.SavePoint != respawnPoint.position)
                  GameManager.instance.SavePoint = respawnPoint.position;
            }
        }
    }
}