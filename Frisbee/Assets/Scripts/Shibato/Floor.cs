using System;
using UnityEngine;
namespace Shibato
{
    public class Floor : MonoBehaviour
    {
        [SerializeField, JapaneseLabel("出現させる床")]
        private GameObject floor;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Frisbee"))
            {
               floor.SetActive(true); 
            }
        }
    }
}