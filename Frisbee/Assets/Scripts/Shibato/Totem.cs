using System;
using UnityEngine;

namespace Shibato
{
    public class Totem :MonoBehaviour
    {
        private GameObject stoneHit;
        

        private void Awake()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CrackedRock"))
            {
                
            }
        }
    }
}