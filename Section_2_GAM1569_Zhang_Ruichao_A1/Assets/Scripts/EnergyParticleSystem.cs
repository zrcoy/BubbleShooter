using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyParticleSystem : MonoBehaviour
{
    private float timeLeft;
    public void Awake()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        timeLeft = particle.startLifetime;
    }
    public void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
