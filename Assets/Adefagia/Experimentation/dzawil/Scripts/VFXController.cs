using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adefagia.GridSystem;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public static VFXController Instance { get; set; }

    [SerializeField] private ParticleSystem buffLoopedPrefab;
    [SerializeField] private ParticleSystem deBuffLoopedPrefab;
    [SerializeField] private ParticleSystem healLoopedPrefab;
    [SerializeField] private ParticleSystem arrowFiring;
    [SerializeField] private ParticleSystem arrowImpact;
    [SerializeField] private ParticleSystem fireBallFiring;
    [SerializeField] private ParticleSystem fireBallImpact;
    [SerializeField] private ParticleSystem gunShotFiring;
    [SerializeField] private ParticleSystem gunShotImpact;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float scaleFactor;

    // Impact spawn offset
    [SerializeField] private float arrowOffset;
    [SerializeField] private float gunShotOffset;
    [SerializeField] private float fireBallOffset;
    private List<ParticleSystem> loopParticleList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        loopParticleList = new List<ParticleSystem>();
    }

    public void PlayFiringVFX(ParticleSystem particleSystem, Transform spawnPoint, Vector3 gridPosition)
    {
        ParticleSystem vfxInstance = Instantiate(particleSystem, spawnPoint.position, Quaternion.identity);
        vfxInstance.transform.parent = spawnPoint;

        gridPosition.y = vfxInstance.transform.position.y;
        vfxInstance.transform.LookAt(gridPosition);
        vfxInstance.transform.Rotate(0, -90, 0);
        
        var mainModule = vfxInstance.main;
        Destroy(vfxInstance.gameObject, mainModule.duration + 2f);
    }

    public void PlayArrowFiringVFX(Transform spawnPoint, Vector3 gridPosition)
    {
        PlayFiringVFX(arrowFiring, spawnPoint, gridPosition);
    }

    public void PlayGunShotFiringVFX(Transform spawnPoint, Vector3 gridPosition)
    {
        PlayFiringVFX(gunShotFiring, spawnPoint, gridPosition);
    }

    public void PlayFireBallFiringVFX(Transform spawnPoint, Vector3 gridPosition)
    {
        PlayFiringVFX(fireBallFiring, spawnPoint, gridPosition);
    }

    private void PlayImpactVFX(Transform spawnPoint, ParticleSystem particleSystem, float offset)
    {
        Vector3 spawnPointOffset = new Vector3(spawnPoint.position.x - offset, spawnPoint.position.y + 1f, spawnPoint.position.z);
        ParticleSystem vfxInstance = Instantiate(particleSystem, spawnPointOffset, Quaternion.identity);
        vfxInstance.transform.parent = spawnPoint.transform;

        var mainModule = vfxInstance.main;

        vfxInstance.gameObject.SetActive(true);
        Destroy(vfxInstance.gameObject, mainModule.duration + 2f);
    }

    public void PlayArrowImpactVFX(Transform spawnPoint)
    {
        PlayImpactVFX(spawnPoint, arrowImpact, arrowOffset);
    }

    public void PlayGunShotImpactVFX(Transform spawnPoint)
    {
        PlayImpactVFX(spawnPoint, gunShotImpact, gunShotOffset);
    }

    public void PlayFireBallImpactVFX(Transform spawnPoint)
    {
        PlayImpactVFX(spawnPoint, fireBallImpact, fireBallOffset);
    }

    public void SpawnProjectile(Transform spawnPoint, Vector3 gridPostition, float duration)
    {
        GameObject projectileInstance = Instantiate(projectile, spawnPoint.position, Quaternion.identity);
        projectileInstance.transform.parent = spawnPoint;

        gridPostition.y = projectileInstance.transform.position.y;
        projectileInstance.transform.LookAt(gridPostition);

        projectileInstance.transform.DOMove(gridPostition, duration).SetEase(Ease.Linear);
        
        Destroy(projectileInstance.gameObject, duration + 0.1f);
    }

    // Play Buff Particle System
    public void PlayBuffLoopVFX(Transform spawnPoint)
    {
        AddVFXList(buffLoopedPrefab, spawnPoint);
    }

    // Play DeBuff Particle System
    public void PlayDebuffLoopVFX(Transform spawnPoint)
    {
        AddVFXList(deBuffLoopedPrefab, spawnPoint);
    }

    // Play Heal Particle System
    public void PlayHealLoopVFX(Transform spawnPoint)
    {
        AddVFXList(healLoopedPrefab, spawnPoint);
    }

    // Destroy and remove all particle on list 
    public void StopBuffLoopVFX()
    {
        foreach (ParticleSystem particleSystem in loopParticleList.ToList())
        {
            Destroy(particleSystem.gameObject);
            loopParticleList.Remove(particleSystem);
        }
    }

    // Add VFX instance to the list
    private void AddVFXList(ParticleSystem vFXPrefab, Transform spawnPoint)
    {
        ParticleSystem vfxInstance = Instantiate(vFXPrefab, spawnPoint.position, Quaternion.identity);
        vfxInstance.transform.parent = spawnPoint;
        loopParticleList.Add(vfxInstance);

        ResizeVFX();
    }

    // Resize VFX Particle GameObject
    private void ResizeVFX()
    {
        foreach (ParticleSystem item in loopParticleList)
        {
            foreach (Transform particleGameObject in item.transform)
            {
                particleGameObject.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);
            }
        }
    }
}
