using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupSimulationAnimationHandler : MonoBehaviour
{
    public GameObject simulationStartupMesh;
    public float movementSpeed = 10;
    public float pauseTime = 1.5f;
    public Vector3 startingLocalPosition;
    public Vector3 pauseLocalPosition = Vector3.zero;
    public Vector3 finishLocalPosition;

    public bool start = false;
    public bool isPlaying = false;
    public bool complete = false;

    private bool passedPausePoint = false;


    private void Start() {
        simulationStartupMesh.transform.localPosition = startingLocalPosition;
    }

    private void Update() {
        if(start && !isPlaying && !complete) {
            isPlaying = true;
            start = false;
        }

        if(isPlaying) {
            Move();
        }
    }

    void Move() {
        if(!passedPausePoint) {
            simulationStartupMesh.transform.localPosition = Vector3.MoveTowards(simulationStartupMesh.transform.localPosition, pauseLocalPosition, movementSpeed * Time.deltaTime);
        }
        if(simulationStartupMesh.transform.localPosition == pauseLocalPosition) {
            StartCoroutine(PauseAnimation());
        }

        if(passedPausePoint) {
            simulationStartupMesh.transform.localPosition = Vector3.MoveTowards(simulationStartupMesh.transform.localPosition, finishLocalPosition, movementSpeed * 2 * Time.deltaTime);
        }
        if(simulationStartupMesh.transform.localPosition == finishLocalPosition) {
            complete = true;
            isPlaying = false;
        }
    }

    IEnumerator PauseAnimation() {
        yield return new WaitForSeconds(pauseTime);
        passedPausePoint = true;
    }
}
