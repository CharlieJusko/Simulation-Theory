using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeMove : MonoBehaviour
{
    public bool move = true;
    public float movementSpeed = 3f;


    private void Update() {
        if(move) {
            Move();
        }
    }

    void Move() {
        transform.position = transform.position + Vector3.back * movementSpeed * Time.deltaTime;
    }
}
