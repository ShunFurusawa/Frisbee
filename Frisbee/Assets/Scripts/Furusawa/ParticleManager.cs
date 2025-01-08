using System;
using UnityEngine;

namespace Furusawa
{
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        private float time;

        private void Update()
        {
            if (particle.isPlaying)
            {
                time += Time.deltaTime;

                if (time >= 3f)
                {
                    particle.Stop();
                }
            }
        }

        public void ParticlePlay()
        {
            if (particle.isPlaying)
            {
                particle.Stop();
            }
            particle.Play();
        }
    }
}