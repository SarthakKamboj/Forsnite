using UnityEngine;

public class VerticalPlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController cc;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float jumpDist;

    float yVel = 0f;
    float gravity = Physics.gravity.y;

    void Update()
    {
        yVel += gravity * Time.deltaTime;
        cc.Move(new Vector3(0f, yVel * Time.deltaTime, 0f));

        HandleJump();

        if (isGrounded() && yVel <= 0f)
        {
            yVel = 0f;
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
            yVel = Mathf.Sqrt(2 * -gravity * jumpDist);
        }
    }

}
