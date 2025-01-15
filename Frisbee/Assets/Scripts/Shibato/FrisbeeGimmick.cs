using System;
using UnityEngine;

namespace Shibato
{
    public class FrisbeeGimmick :MonoBehaviour
    {
        [SerializeField][JapaneseLabel("リセット位置")] private Transform spawnPoint;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("spines"))
            {
                gameObject.transform.position = spawnPoint.position;
            }
        }
    }
}