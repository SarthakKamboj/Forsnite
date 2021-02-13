using UnityEngine;

public class VerticalPlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    [SerializeField] Transform groundCheck;
    [SerializeField] Animator characterAnimator;
    [SerializeField] GameObject ninjaGFX;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] Vector3 ninjaGFXJumpPosOffset;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float jumpDist;
    [SerializeField] float jumpTime = 1.25f;

    float jumpAnimationSpeedUpMultipler;
    float yVel = 0f;
    float gravity = Physics.gravity.y;
    float jumpAnimTimeCounter = 0f;
    bool jumping = false;

    void Start()
    {
        AnimationClip[] clips = characterAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Jumping")
            {
                jumpAnimationSpeedUpMultipler = clip.length / jumpTime;
            }
        }
    }

    void Update()
    {
        yVel += gravity * Time.deltaTime;
        float deltaY = yVel * Time.deltaTime;
        cc.Move(new Vector3(0f, deltaY, 0f));

        HandleJump();

        if (isGrounded() && yVel <= 0f)
        {
            yVel = 0f;
            ResetAnimatorSpeed();
            if (characterAnimator.GetBool("Jumping"))
            {
                // ninjaGFX.transform.position -= ninjaGFXJumpPosOffset;
            }
            characterAnimator.SetBool("Jumping", false);
        }

    }

    bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayerMask);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            yVel = jumpDist / jumpTime * 2f;
            SpeedUpJumpAnim();
            characterAnimator.SetBool("Jumping", true);
            // ninjaGFX.transform.position += ninjaGFXJumpPosOffset;
        }
    }

    void SpeedUpJumpAnim()
    {
        // characterAnimator.speed = jumpAnimationSpeedUpMultipler;
    }

    void ResetAnimatorSpeed()
    {
        characterAnimator.speed = 1f;
    }

}
