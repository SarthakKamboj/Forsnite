using System;
using UnityEngine;

public class Run : MonoBehaviour
{
    [SerializeField] Transform mainCamera;
    [SerializeField] CharacterController cc;
    [SerializeField] float speed = 100f;
    [SerializeField] float smoothTime = 0.01f;

    float curVelocity = 0f;
    float horizontal, vertical;
    float camYRot;

    void Start()
    {
        camYRot = mainCamera.rotation.eulerAngles.y;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        HandleMove();
    }

    void HandleMove()
    {
        Vector3 inputVec = new Vector3(horizontal, 0f, vertical).normalized;
        if (inputVec.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputVec.x, inputVec.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref curVelocity, smoothTime);

            Vector3 moveDir = Quaternion.Euler(new Vector3(0f, targetAngle, 0f)) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            cc.Move(moveDir.normalized * inputVec.magnitude * speed * Time.deltaTime);
        }
    }
}
