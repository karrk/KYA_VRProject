using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandConnector : MonoBehaviour
{
    private void Start()
    {
        HandMotionController controller =
            GetComponentInParent<HandMotionController>();

        controller.SetAnimator(GetComponent<Animator>());
    }
}
