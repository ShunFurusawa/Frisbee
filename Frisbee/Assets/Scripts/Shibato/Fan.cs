using System;
using UnityEngine;
using Object = System.Object;

namespace Shibato
{
    public class Fan:MonoBehaviour
    {
        [Header("風力")]
        [Range(0,1)]
        [SerializeField] private float changeUp = 0.1f;

        private float up;
        //[SerializeField] private GameObject frisbeeGameobject;
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Frisbee"))
            {
                up = changeUp;
                collision.GetComponent<FrisbeeTest>().Fan(up);
                Debug.Log("WindHit");
            }
            else
            {
                up = 0;
                collision.GetComponent<FrisbeeTest>().Fan(up);
            }
        }
    }
}