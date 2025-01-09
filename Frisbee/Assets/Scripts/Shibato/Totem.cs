using System;
using UnityEngine;

namespace Shibato
{
    public class Totem :MonoBehaviour
    {
        private GameObject player;

        private void Awake()
        {
            player = GameObject.Find("Player");
        }

        private void Update()
        {
            if (player != null)
            {
                transform.position = player.transform.position;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("spines"))
            {
                GameManager.instance.GetTotem();
                Destroy(gameObject);
            }
        }
    }
}