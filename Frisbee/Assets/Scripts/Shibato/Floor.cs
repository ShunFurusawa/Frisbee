using System;
using Unity.VisualScripting;
using UnityEngine;
namespace Shibato
{
    public class Floor : MonoBehaviour
    {
        [SerializeField, JapaneseLabel("出現させる床")]
        private GameObject floor;

        [SerializeField, JapaneseLabel("出現させる壁")]
        private GameObject wall;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Frisbee"))
            {
                floor.SetActive(true);
                wall.SetActive(false);
            }
        }

        void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    var rb = other.gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Debug.Log("Destroying Rigidbody...");
                        rb.isKinematic = true; // Optional: Stop physics before destroying.
                        Destroy(rb);
                    }
                    else
                    {
                        Debug.Log("Rigidbody not found.");
                    }
                }
            }
        }
    }