using System;
using UnityEngine;

namespace Shibato
{
    public class Clear :MonoBehaviour
    {
        [SerializeField] private Wall wall; 
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Frisbee"))
            {
                wall.ResetWallPosition();
                SoundManager.instance.StopPlay("Last");
                SoundManager.instance.StopPlay("BGM");
                SoundManager.instance.Play("clear");
                Destroy(gameObject);
            }
        }
    }
}