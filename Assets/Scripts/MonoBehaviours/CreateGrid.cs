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

        float startX = transform.position.x - groundCollider.bounds.extents.x;
        float startZ = transform.position.z - groundCollider.bounds.extents.z;

        int count = 0;
        for (int x = 0; x <= width / Build.wallSeparation; x++)
        {
            for (int z = 0; z <= height / Build.wallSeparation; z++)
            {
                GameObject buildSpot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                buildSpot.transform.localScale = Vector3.one * buildSpotSphereRadius;
                Vector3 pos = buildSpot.transform.position;

                pos.x = startX + x * Build.wallSeparation;
                pos.y = groundCollider.transform.position.y;
                pos.z = startZ + z * Build.wallSeparation;

                buildSpot.transform.position = pos;
                buildSpot.GetComponent<Collider>().isTrigger = true;
                buildSpot.tag = "BuildSpot";
                buildSpot.name = "BuildSpot" + count;
                count += 1;
                buildSpot.GetComponent<Renderer>().material = debugMat;
                buildSpot.transform.SetParent(buildSpotsContainer, true);

            }
        }
    }

}
