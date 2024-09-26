using System;
using UnityEngine;
using Object = System.Object;

namespace Shibato
{
    public class Fan:MonoBehaviour
    {
        [Header("風力")]
        [Range(0,10)]
        [SerializeField] private float changeUp = 3;

        private float up;
        [SerializeField] private GameObject frisbeeGameobject;
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Frisbee"))
            {
                up = changeUp;
                collision.gameObject.GetComponent<FrisbeeTest>().Fan(up);
                Debug.Log("WindHit");
            }
            else
            {
                up = 0;
                collision.gameObject.GetComponent<FrisbeeTest>().Fan(up);
            }
        }
    }
}