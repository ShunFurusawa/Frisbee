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
        private GameObject spawnWall;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Frisbee"))
            {
                    floor.SetActive(true);
                    spawnWall.SetActive(false);
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
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                    else
                    {
                        Debug.Log("Rigidbody not found.");
                    }
                }
            }
        }
    }