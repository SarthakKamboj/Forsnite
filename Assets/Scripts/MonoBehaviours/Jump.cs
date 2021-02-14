using UnityEngine;

public class Jump : MonoBehaviour
{

    [SerializeField] CharacterController cc;
    [SerializeField] Animator characterAnimator;
    [SerializeField] GameObject ninjaRoot;
    [SerializeField] Gravity gravity;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform debugTransform;

    float jumpAnimTimeCounter = 0f;
    float jumpAnimClipLength;
    float origGfxPosY = 0f;
    float origCCPosY;
    float origGroundCheckPosY;
    bool jumping = false;
    int isJumpingHash;

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

        isJumpingHash = Animator.StringToHash("isJumping");
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

            float yOffset = ninjaRoot.transform.position.y - origGfxPosY;
            yOffset = Mathf.Clamp(yOffset, 0f, Mathf.Infinity);

            Vector3 ccCenter = cc.center;
            ccCenter.y = origCCPosY + yOffset;
            cc.center = ccCenter;

            Vector3 groundCheckPos = groundCheck.transform.position;
            groundCheckPos.y = origGroundCheckPosY + yOffset;
            groundCheck.position = groundCheckPos;
        }

        if (jumpAnimTimeCounter >= jumpAnimClipLength)
        {
            jumpAnimTimeCounter = 0f;
            jumping = false;
            characterAnimator.SetBool(isJumpingHash, false);

            gravity.enabled = true;

            Vector3 ccCenter = cc.center;
            ccCenter.y = origCCPosY;
            cc.center = ccCenter;

            Vector3 groundCheckPos = groundCheck.transform.position;
            groundCheckPos.y = origGroundCheckPosY;
            groundCheck.position = groundCheckPos;

            finalPos = debugTransform.position;
        }

        if (Input.GetKeyDown(KeyCode.Space) && gravity.isGrounded())
        {
            characterAnimator.SetBool(isJumpingHash, true);
            prevPos = debugTransform.position;
            jumping = true;
            origGfxPosY = ninjaRoot.transform.position.y;
            origCCPosY = cc.center.y;
            origGroundCheckPosY = groundCheck.position.y;
            gravity.enabled = false;
        }

    }
}
