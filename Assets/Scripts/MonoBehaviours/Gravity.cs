using UnityEngine;

public class Gravity : MonoBehaviour
{

    [SerializeField] CharacterController cc;
    public Transform groundCheck;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundCheckRadius;

    float yVel = 0f;
    float gravity = Physics.gravity.y;

    void Update()
    {
        yVel += gravity * Time.deltaTime;
        float deltaY = yVel * Time.deltaTime;
        cc.Move(new Vector3(0f, deltaY, 0f));

        if (isGrounded() && yVel <= 0f)
        {
            yVel = -2f;
        }
    }


    public bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayerMask);
    }


}
