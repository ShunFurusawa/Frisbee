using System;
using UnityEngine;
namespace Shibato
{
    public class Treasure:MonoBehaviour
    {
        //[SerializeField] private GameObject totem;
        [SerializeField] private Animator boxAnimator;
        [SerializeField] private Animator totemAnimator;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Frisbee"))
            {
                Debug.Log("があふぁあ");
                boxAnimator.Play("treasure");
                totemAnimator.Play("Totem");
            }
        }

        public void Destroy()
        {
            GameManager.instance.GetTotem();
            Destroy(gameObject);
        }
    }
}