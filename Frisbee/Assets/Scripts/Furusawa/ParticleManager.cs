using System;
using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Furusawa
{
    public class ParticleManager : MonoBehaviour
    {
         [SerializeField] private ParticleSystem TPparticle;
         [SerializeField] private ParticleSystem FrisParticle;

        private float time;

        private void Update()
        {
            if (TPparticle.isPlaying)
            {
                time += Time.deltaTime;

                if (time >= 3f)
                {
                    TPparticle.Stop();
                    Debug.Log("Stop Particle");
                    time = 0;
                }
            }
        }

        public void ParticleStop()
        {
            FrisParticle.Stop();
        }

        public void ParticlePlay(bool isTpPart)
        {
            if (isTpPart)
            {
                if (TPparticle.isPlaying)
                {
                    TPparticle.Stop();
                    time = 0;
                }
                TPparticle.Play();
            }
            else
            {
                if (FrisParticle.isPlaying)
                {
                    FrisParticle.Stop();
                }
                FrisParticle.Play();
            }
        
        }
    }
}