using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.GridSystem;
using Adefagia.PlayerAction;
using Adefagia.RobotSystem;
using UnityEngine;

public class VFXdzawil : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    
    
    private void OnEnable()
    {
        // RobotAttack.VFXEvent += PlayVFX;
    }

    private void OnDisable()
    {
        // RobotAttack.VFXEvent -= PlayVFX;
    }

    void PlayVFX(RobotController robotController, GridController gridController)
    {
        // Play VFX

        var direction = gridController.gameObject.transform.position - robotController.gameObject.transform.position;

        transform.position = robotController.gameObject.transform.position;
        Debug.Log(direction);
    }
    
    public enum Weapon
    {
        Shotgun,
        Sniper,
        RockerLauncer
    }
}
