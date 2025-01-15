using System;
using UnityEngine;
namespace Shibato
{
    public class Treasure:MonoBehaviour
    {
        //[SerializeField] private GameObject totem;
        [SerializeField] private Animator boxAnimator;
        [SerializeField] private Animator totemAnimator;
        [SerializeField] private FrisbeeTest frisbeeTest;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Frisbee"))
            {
                boxAnimator.Play("treasure");
                totemAnimator.Play("Totem");
                SoundManager.instance.Play("kirakira");
            }
        }

        public void Destroy()
        {
            GameManager.instance.GetTotem();
            frisbeeTest.GetTotem();
            Destroy(gameObject);
        }
    }
}