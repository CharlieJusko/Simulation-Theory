using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateAnimation : MonoBehaviour
{
    //public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 0.5f;

    public float degreesPerSecond = 15.0f;

    private Vector3 startingPosition;
    private Vector3 tempPosition;


    void Start() {
        // Store the starting position & rotation of the object
        startingPosition = transform.position;
    }

    private void Update() {
        Levitate();
        Spin();
    }

    // Float up/down with a Sin()
    void Levitate() {
        tempPosition = startingPosition;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPosition;
    }

    void Spin() {
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
    }
}
