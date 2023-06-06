using System;
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
}
