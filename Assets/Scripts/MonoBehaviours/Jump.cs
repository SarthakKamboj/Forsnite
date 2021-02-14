using UnityEngine;

public class Jump : MonoBehaviour
{

    [SerializeField] CharacterController cc;
    [SerializeField] Animator characterAnimator;
    [SerializeField] GameObject ninjaGFX;
    [SerializeField] Gravity gravity;
    [SerializeField] Transform debugTransform;

    float jumpAnimTimeCounter = 0f;
    float jumpAnimClipLength;
    float origGfxPosY = 0f;
    float origCCPosY;
    bool jumping = false;

    Vector3 prevPos;
    Vector3 finalPos;

    void Start()
    {
        AnimationClip[] clips = characterAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Jumping")
            {
                jumpAnimClipLength = clip.length;
            }
        }
    }

    void Update()
    {
        HandleJump();
    }

    void HandleJump()
    {

        if (jumping)
        {
            jumpAnimTimeCounter += Time.deltaTime;

            float yOffset = ninjaGFX.transform.position.y - origGfxPosY;
            yOffset = Mathf.Clamp(yOffset, 0f, Mathf.Infinity);

            Vector3 ccCenter = cc.center;
            ccCenter.y = origCCPosY + yOffset;
            cc.center = ccCenter;
        }

        if (jumpAnimTimeCounter >= jumpAnimClipLength)
        {
            jumpAnimTimeCounter = 0f;
            jumping = false;
            characterAnimator.SetBool("Jumping", false);

            gravity.enabled = true;

            Vector3 ccCenter = cc.center;
            ccCenter.y = origCCPosY;
            cc.center = ccCenter;

            finalPos = debugTransform.position;
        }

        if (Input.GetKeyDown(KeyCode.Space) && gravity.isGrounded())
        {
            characterAnimator.SetBool("Jumping", true);
            prevPos = debugTransform.position;
            jumping = true;
            origGfxPosY = ninjaGFX.transform.position.y;
            origCCPosY = cc.center.y;
            gravity.enabled = false;
        }

    }
}
