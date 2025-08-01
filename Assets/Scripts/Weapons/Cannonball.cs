using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(Collision collision) {
        // TODO: We hit something
        Destroy(gameObject);

        ContactPoint hit = collision.contacts[0];
        if(hit.otherCollider.transform.TryGetComponent(out ImpactVFXController ivfx)) {
            ivfx.PlayVFX(hit.point);
        }
        //if(hit.otherCollider.transform.TryGetComponent<Destructable>(out Destructable d)) {
        //    d.Destruct(hit.point);
        //}

        //if(hit.otherCollider.transform.TryGetComponent<NonPlayerCharacter_Smart>(out NonPlayerCharacter_Smart npc)) {
        //    npc.TakeDamage(damage);

        //    Transform closestBone = null;
        //    float closestDistance = Mathf.Infinity;
        //    foreach(Transform child in npc.transform) {
        //        if(!child.name.Contains("mixamo")) {
        //            continue;
        //        }

        //        float distance = Vector3.Distance(child.position, hit.point);
        //        if(distance < closestDistance) {
        //            closestDistance = distance;
        //            closestBone = child;
        //        }
        //    }

        //    transform.parent = closestBone;
        //    GetComponent<Rigidbody>().isKinematic = true;
        //    GetComponent<Rigidbody>().useGravity = false;
        //}

        //// TODO: Update Research
        //if(hit.otherCollider.transform.TryGetComponent<IResearchableAnimal>(out IResearchableAnimal animal)) {
        //    animal.Discovered = true;
        //}

        //GetComponent<Rigidbody>().velocity = Vector3.zero;
        //GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other) {
        bool hitOthers = false;
        var hit = other.ClosestPointOnBounds(transform.position);

        ImpactVFXController otherImpactVFX;
        bool hasImpactVFX = false;
        if(other.transform.TryGetComponent(out otherImpactVFX)) {
            hasImpactVFX = true;
        }

        Collider[] hitColliders = Physics.OverlapSphere(hit, 5f);
        foreach(var hitCollider in hitColliders) {
            if(hitCollider.transform.TryGetComponent<Bomb>(out Bomb b)) {
                hitOthers = true;
                b.Explode();
            }
        }

        if(!hitOthers && hasImpactVFX) {
            otherImpactVFX.PlayVFX(hit);
        }

        Destroy(gameObject);
    }
}
