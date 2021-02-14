using UnityEngine;

public class Build : MonoBehaviour
{

    [SerializeField] Animator charAnimator;
    [SerializeField] float aimingAnimTransitionTime;

    float weight = 0f;
    bool aiming = false;
    bool finishedAiming = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && weight < 1f)
        {
            weight += 1 / aimingAnimTransitionTime * Time.deltaTime;
            weight = Mathf.Clamp(weight, 0f, 1f);
            charAnimator.SetLayerWeight(1, weight);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            finishedAiming = true;
        }

        if (finishedAiming)
        {
            if (weight > 0f)
            {
                weight -= 1 / aimingAnimTransitionTime * Time.deltaTime;
                weight = Mathf.Clamp(weight, 0f, 1f);
                charAnimator.SetLayerWeight(1, weight);
            }

            else
            {
                finishedAiming = false;
            }
        }
    }
}
