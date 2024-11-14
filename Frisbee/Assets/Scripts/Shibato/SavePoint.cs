using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private List<GameObject> savePoint;

    private Transform _saveTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (savePoint.Contains(other.gameObject))
        {
            _saveTransform = this.transform;
            
        }
    }

    public void Save()
    {
        this.transform.position = _saveTransform.position;
    } 
}
