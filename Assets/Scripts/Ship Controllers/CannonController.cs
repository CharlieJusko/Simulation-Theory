using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : ShipWeaponController
{
    public GameObject cannonballPrefab;
    public float cannonballSpeed = 5f;
    //public GameObject currentCannonball;
    public bool shot = false;


    protected override void Shoot() {
        if(muzzleFlash != null) {
            muzzleFlash.gameObject.SetActive(true);
            muzzleFlash.Play();
        }

        currentTimer = 0f;
        shot = true;
        Vector3 shootDirection = GetShootDirection();
        var cannonball = Instantiate(cannonballPrefab, bulletSpawnPos.position, bulletSpawnPos.rotation);
        //cannonball.GetComponent<Rigidbody>().isKinematic = true;
        cannonball.GetComponent<Rigidbody>().AddForce(shootDirection * cannonballSpeed, ForceMode.Impulse);
        cannonball.GetComponent<Cannonball>().damage = damage;

        //currentArrow.transform.parent = null;
        //currentArrow.GetComponent<Rigidbody>().isKinematic = false;
        //currentArrow.GetComponent<Rigidbody>().AddForce(shootDirection * arrowSpeed, ForceMode.Impulse);
        //currentArrow.GetComponent<Arrow>().shot = true;
        //if(currentArrow.GetComponentInChildren<ChargeArrow>().fullyCharged) {
        //    currentArrow.GetComponentInChildren<ChargeArrow>().PlayShotVFX();
        //}
        //Destroy(currentArrow, destroyTime);

        //// TODO: Update to shoot then draw
        //if(GetComponent<ThirdPersonController>().twinStickMode) {
        //    GetComponent<Animator>().SetTrigger("DrawArrow");
        //}
    }
}
