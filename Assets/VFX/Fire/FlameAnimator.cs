using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FlameAnimator : MonoBehaviour
{
    public int frames = 250;
    private int index = 0;


    private void Start() {
        index = 0;
    }

    private void FixedUpdate() {
        Animate();
    }

    void Animate() {
        if(index < frames) {
            GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(index, 100);
            if(index > 0) {
                GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(index - 1, 0);
            }

            ++index;

        } else {
            index = 0;
            GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(index, 100);
            GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(frames - 1, 0);
        }
    }
}
