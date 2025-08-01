using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShipWeaponController : MonoBehaviour
{
    [Header("Damage and Range")]
    [SerializeField]
    protected float damage = 5f;
    [SerializeField]
    protected float range = 10f;
    [SerializeField]
    protected float spread = 2f;

    [Space(5)]

    [Header("Utility")]
    public bool canShoot = false;
    [SerializeField]
    protected float shotTimer = 1f;
    [SerializeField]
    protected float currentTimer = 1f;
    [SerializeField]
    protected float shotRegenFactor = 2f;
    [SerializeField]
    protected Transform bulletSpawnPos;

    [Space(5)]

    [Header("VFX")]
    [SerializeField]
    protected VisualEffect muzzleFlash;
    [SerializeField]
    protected TrailRenderer bulletTrail;
    [SerializeField]
    protected float fadeTime = 1f;
    [SerializeField]
    protected GameObject impactEffect;

    private StarterAssets.Inputs inputs;


    private void Awake() {
        if(muzzleFlash != null) {
            muzzleFlash.gameObject.SetActive(true);
            muzzleFlash.Stop();
        }
    }

    private void Update() {
        SharedUpdate();
    }

    protected void SharedUpdate() {
        //UpdateWeaponTransform();
        TimerUpdate();

        if(canShoot && GetComponent<StarterAssets.Inputs>().shoot1) {
            Shoot();
        }

        if(GetComponent<StarterAssets.Inputs>().shoot1) {
            GetComponent<StarterAssets.Inputs>().shoot1 = false;
        }
    }

    protected virtual void Shoot() { }

    #region Utility

    void TimerUpdate() {
        if(currentTimer < shotTimer) {
            canShoot = false;
            currentTimer += Time.deltaTime * shotRegenFactor;
        }

        if(currentTimer >= shotTimer) {
            currentTimer = shotTimer;
            canShoot = true;
        }
    }

    protected Vector3 GetShootDirection() {
        Vector3 targetPos;

        //This  is copy and pasted from a nother project in case we want to use the twinstick control
        //if(GetComponent<TwinStickCombatController>().aimAssistOn && GetComponent<TwinStickCombatController>().aimAssistLockedOn) {
        //    targetPos = GetComponent<TwinStickCombatController>().GetAimAssistTarget();

        //} else {
        //    targetPos = bulletSpawnPos.position + bulletSpawnPos.forward * range;
        //}

        targetPos = bulletSpawnPos.position + bulletSpawnPos.forward * range;

        targetPos = new Vector3(
            targetPos.x + Random.Range(-spread, spread),
            targetPos.y + Random.Range(-spread, spread),
            targetPos.z + Random.Range(-spread, spread)
        );

        Vector3 direction = targetPos - bulletSpawnPos.position;
        return direction.normalized;
    }

    #endregion

    #region Visual Effects
    public void PlayMuzzleFlash() {
        if(!muzzleFlash.gameObject.activeInHierarchy) {
            muzzleFlash.gameObject.SetActive(true);
        }
        muzzleFlash.Play();
    }

    public void HideMuzzleFlash() {
        muzzleFlash.Stop();
        muzzleFlash.gameObject.SetActive(false);
    }

    protected void CreateBulletTrailLaser(Vector3 endPos, bool hitSomething) {
        muzzleFlash.Play();
        TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPos.position, Quaternion.identity);
        StartCoroutine(FadeTrail(trail, endPos, hitSomething));
    }

    IEnumerator FadeTrail(TrailRenderer trail, Vector3 endPos, bool hitSomething) {
        float timer = 0;
        Vector3 startPos = trail.transform.position;
        while(timer < fadeTime) {
            trail.transform.position = Vector3.Lerp(startPos, endPos, timer);
            timer += Time.deltaTime / trail.time;
            yield return null;
        }

        if(hitSomething) {
            var impact = Instantiate(impactEffect, endPos, Quaternion.identity);
            StartCoroutine(DestroyImpact(impact, 0.75f));
        }

        Destroy(trail.gameObject, trail.time);
    }

    IEnumerator DestroyImpact(GameObject impact, float destroyTime) {
        float timer = 0;
        while(timer < destroyTime) {
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(impact);
    }
    #endregion

}
