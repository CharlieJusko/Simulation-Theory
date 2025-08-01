using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ImpactVFXController : MonoBehaviour
{
    public GameObject vfxPrefab;
    public bool applyForceToNearbyRigidbodies = false;
    public float radius = 7f;
    public float maxForce = 7f;
    public LayerMask rigidbodiesLayers;
    public bool move = false;
    public float movementSpeed = 10f;


    public void PlayVFX(Vector3 position) {
        var vfx = Instantiate(vfxPrefab, position, Quaternion.identity);
        if(vfx.TryGetComponent(out FlatKit.Buoyancy b)) {
            b.water = transform;
            b.overrideWaterMaterial = GetComponent<MeshRenderer>().material;
        }
        if(vfx.TryGetComponent(out LandscapeMove lm)) {
            lm.move = move;
            lm.movementSpeed = movementSpeed;
        }

        Destroy(vfx, 5f);
        if(applyForceToNearbyRigidbodies) {
            ApplyForceToNearbyRigidbodies(position);
        }
    }

    public void ApplyForceToNearbyRigidbodies(Vector3 position) {
        Collider[] nearbyColliders = Physics.OverlapSphere(position, radius, rigidbodiesLayers);
        foreach(var nearbyCollider in nearbyColliders) {
            if(nearbyCollider.transform.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                Vector3 direction = nearbyCollider.transform.position - position;
                float distance = Vector3.Distance(nearbyCollider.transform.position, position);
                float calculatedForce = maxForce * (distance / radius);
                rb.AddForceAtPosition(direction * calculatedForce, position);
            }
        }
    }
}
