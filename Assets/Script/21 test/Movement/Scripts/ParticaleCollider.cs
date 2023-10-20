using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticaleCollider : MonoBehaviour
{
    ParticleSystem ps;

    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }


    private void Update()
    {

    }

    private void OnParticleTrigger()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            int triggedParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
            for (int i = 0; i < triggedParticles; i++)
            {
                ParticleSystem.Particle p = particles[i];
                p.remainingLifetime = 0;
                Debug.Log("We collected 1 particale");
                particles[i] = p;
            }
            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        }
        /*int triggedParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        for(int i = 0;i < triggedParticles; i++)
        {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            Debug.Log("We collected 1 particale");
            particles[i] = p;
        }
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);*/
    }
}
