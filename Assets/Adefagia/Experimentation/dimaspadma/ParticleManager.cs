using System;
using Adefagia.ObstacleSystem;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem GlobalParticle;

    public float timeRemaining = 5;

    private float _timer;
    private bool _timerRunning;
    
    private void Start()
    {
        ObstacleController.ObstacleDestroyed += PlayParticle;
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
                GlobalParticle.Stop();
            }
        }
    }

    private void PlayParticle(Vector3 position)
    {
        GlobalParticle.transform.position = position;
        GlobalParticle.Play();

        _timerRunning = true;
    }
}
