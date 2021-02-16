using UnityEngine;

public class WallCreation : MonoBehaviour
{

    [SerializeField] Material simMat;
    [SerializeField] Material builtMat;
    [SerializeField] Renderer renderer;
    [SerializeField] Collider wallCollider;

    // TODO: raycast from camera and simulate wall creation

    void Start()
    {
        renderer.material = simMat;
    }

    public void WallCreated()
    {
        gameObject.layer = LayerMask.NameToLayer("Wall");
        wallCollider.isTrigger = false;
        renderer.material = builtMat;
    }
}
