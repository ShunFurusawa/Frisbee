using System;
using UnityEngine;

namespace Furusawa
{
    public class IgniteTorch : MonoBehaviour
    {
        private Light _lightComp;
        
        [Header("松明は〇fの明るさで点灯する")]
        [SerializeField] private float brightness = default!;
        private void Start()
        {
            _lightComp = GetComponent<Light>();
            _lightComp.intensity = 0f;
        }

        public void Ignite()
        {
            if (_lightComp == null)
            {
                _lightComp = GetComponent<Light>();
                Debug.LogAssertion("light component is null!");
            }

            _lightComp.intensity = brightness;
        }
    }
}