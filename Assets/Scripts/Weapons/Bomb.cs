using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bomb : MonoBehaviour
{
    public VisualEffect explosionVFX;
    public float destroyTimer = 5f;
    public bool exploded = false;

    [Space(2), Header("Bounds")]
    public Transform clippingCube;
    public float xOffset = 4f;
    public Vector2 yRange;
    public bool grounded = false;


    private void Start() {
        explosionVFX.Stop();
    }

    private void Update() {
        KeepInBounds();
    }

    private void OnCollisionEnter(Collision collision) {
        if(!collision.collider.CompareTag("Water") && !exploded) {
            Explode();
        }
    }

    void HideAndDestroy() {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, destroyTimer);
    }

    public void Explode() {
        exploded = true;
        explosionVFX.Play();
        HideAndDestroy();
    }

    void KeepInBounds() {
        float maxXPosition = clippingCube.position.x + ((clippingCube.localScale.x / 2) - xOffset);
        float minXPosition = clippingCube.position.x - ((clippingCube.localScale.x / 2) - xOffset);

        if(transform.position.x >= maxXPosition) {
            transform.position = new Vector3(maxXPosition, transform.position.y, transform.position.z);

        } else if(transform.position.x <= minXPosition) {
            transform.position = new Vector3(minXPosition, transform.position.y, transform.position.z);
        }

        if(!grounded) {
            if(transform.position.y <= yRange.x) {
                transform.position = new Vector3(transform.position.x, yRange.x, transform.position.z);

            } else if(transform.position.y >= yRange.y) {
                transform.position = new Vector3(transform.position.x, yRange.y, transform.position.z);
            }
        }
    }
}
