using UnityEngine;

public class WallCreation : MonoBehaviour
{

    [SerializeField] Material simMat;
    [SerializeField] Material builtMat;
    [SerializeField] Renderer renderer;

    // TODO: raycast from camera and simulate wall creation
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            renderer.material = simMat;
        }
        else
        {
            renderer.material = builtMat;
        }
    }
}
