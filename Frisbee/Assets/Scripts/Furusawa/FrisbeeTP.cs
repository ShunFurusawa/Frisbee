using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeTP : MonoBehaviour
{
    private RaycastHit m_hit;

    [SerializeField] private Transform m_cameraRig; 
  

    private void OnCollisionEnter(Collision other)
    {
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out m_hit, 1.0f))
        {
            if (m_hit.collider.gameObject.CompareTag("CanTP"))
            {
                Teleport(m_cameraRig, gameObject.transform);
            }
        }
    }

    private void Teleport(Transform tpObj, Transform tpPoint)
    {
        tpObj.position = tpPoint.position;

        if (tpPoint.position == tpObj.position)
        {
            Debug.Log("TP Success!");
        }
        else
        {
            Debug.Log("TP Fail");
        }
    }
}
