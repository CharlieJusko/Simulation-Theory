using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSail : MonoBehaviour
{
    private Cloth sail;
    private Vector3 startingWind;

    private void Start() {
        sail = GetComponentInChildren<Cloth>();
        startingWind = sail.externalAcceleration;
    }

    public void ChangeWindDirection(float xFactor) {
        Vector3 currentWind = startingWind;
        currentWind.x += startingWind.x * xFactor;
        sail.externalAcceleration = currentWind;
    }
}
