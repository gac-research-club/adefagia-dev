using System;
using System.Collections.Generic;
using Adefagia.ObstacleSystem;
using Adefagia.RobotSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleManager : MonoBehaviour
{
    [Header("List of Particles Global")]
    public ParticleSystem globalObstacleParticleDestroyed;

    public ParticleSystem globalObstacleHit;

    public float timeRemaining = 5;

    private float _timer;
    private bool _timerRunning;

    private void Start()
    {
        CheckParticles(new List<ParticleSystem>
        {
            globalObstacleParticleDestroyed, 
            globalObstacleHit
        });

        ObstacleController.ObstacleDestroyed += OnPlayParticleDestroyed;
        ObstacleController.ObstacleHit += OnPlayParticleHit;
        RobotController.TakeDamageHappened += OnPlayParticleHit;
    }

    private void Update()
    {
        if (_timerRunning)
        {
            _timer += Time.deltaTime;
            if (_timer > timeRemaining)
            {
                _timer -= timeRemaining;
                _timerRunning = false;
                globalObstacleParticleDestroyed.Stop();
            }
        }
    }

    private void OnPlayParticleDestroyed(Vector3 position)
    {
        globalObstacleParticleDestroyed.transform.position = position;
        globalObstacleParticleDestroyed.Play();

        _timerRunning = true;
    }
    
    private void OnPlayParticleHit(Vector3 position)
    {
        globalObstacleHit.transform.position = position;
        globalObstacleHit.Play();

        _timerRunning = true;
    }
    
    // CheckParticles check is particles available, if particles empty than
    // close app
    private void CheckParticles(List<ParticleSystem> particles)
    {
        foreach (var particle in particles)
        {
            if (particle == null)
            {
                var errText = "Particle {0} Not available, Please import first";
                Debug.LogFormat(errText, particle.name);
                Application.Quit();
            }
        }
    }
}
