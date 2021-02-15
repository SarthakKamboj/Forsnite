using UnityEngine;

public class CreateGrid : MonoBehaviour
{

    [SerializeField] Transform buildSpotsContainer;
    [SerializeField] Collider groundCollider;
    [SerializeField] Material debugMat;
    [SerializeField] float buildSpotSphereRadius;

    float width, height;

    void Start()
    {
        width = groundCollider.bounds.size.x;
        height = groundCollider.bounds.size.z;

        int count = 0;
        for (int x = 0; x < width / Build.maxBuildDistance; x++)
        {
            for (int y = 0; y < height / Build.maxBuildDistance; y++)
            {
                GameObject buildSpot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                buildSpot.transform.localScale = Vector3.one * buildSpotSphereRadius;
                buildSpot.GetComponent<Collider>().isTrigger = true;
                buildSpot.tag = "BuildSpot";
                buildSpot.name = "BuildSpot" + count;
                buildSpot.GetComponent<Renderer>().material = debugMat;
                count += 1;
                buildSpot.transform.SetParent(buildSpotsContainer, true);
            }
        }
    }

}
