using System;
using UnityEngine;
namespace Shibato
{
    public class Treasure:MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                
                Destroy(gameObject);
            }
        }
    }
}