using UnityEngine;

public class Build : MonoBehaviour
{

    [SerializeField] GameObject wallPrefab;
    [SerializeField] Camera mainCamera;
    [SerializeField] Animator charAnimator;
    [SerializeField] LayerMask buildLayerMask;
    [SerializeField] LayerMask wallLayerMask;
    [SerializeField] float aimingAnimTransitionTime;
    [SerializeField] float wallCheckRadius = 0.1f;
    [SerializeField] int buildPolygonSides = 4;

    [HideInInspector]
    public static float maxBuildDistance;
    float wallWidth;
    float weight = 0f;
    float buildPolygonAngle;
    bool aiming = false;
    GameObject wall;
    Vector3 posToPlaceWall = Vector3.one;

    void Awake()
    {
        buildPolygonAngle = 360f / buildPolygonSides;
        GameObject tempWall = Instantiate(wallPrefab);
        Bounds wallBounds = tempWall.transform.Find("GFX").GetComponent<Collider>().bounds;
        maxBuildDistance = wallBounds.extents.y;
        wallWidth = wallBounds.size.y;
        Destroy(tempWall);
    }

    void Update()
    {
        HandleAimAnim();
        HandleWallSim();
        HandleWallCreation();
    }

    void HandleWallCreation()
    {
        Debug.Log("available: " + !Physics.CheckSphere(posToPlaceWall, wallCheckRadius, wallLayerMask));
        if (aiming && Input.GetMouseButton(0) && !Physics.CheckSphere(posToPlaceWall, wallCheckRadius, wallLayerMask))
        {
            // GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // s.transform.localScale = Vector3.one * wallCheckRadius;
            // s.transform.position = posToPlaceWall;
            wall.GetComponent<WallCreation>().WallCreated();
            wall = null;
        }
    }

    void OnGizmosDraw()
    {
        // Gizmos.DrawWireSphere(posToPlaceWall, wallCheckRadius);
    }

    void HandleWallSim()
    {
        if (aiming)
        {
            if (!wall)
            {
                wall = Instantiate(wallPrefab, Vector3.zero, Quaternion.Euler(Vector3.up));
            }

            // set wall rotation
            float camYRot = mainCamera.transform.rotation.eulerAngles.y;
            float newYRot = 0f;
            for (int i = 0; i < buildPolygonSides; i++)
            {
                float min = i * buildPolygonAngle;
                float max = (i + 1) * buildPolygonAngle;
                if (camYRot >= min && camYRot <= max)
                {
                    newYRot = CustomRound(camYRot, min, max);
                    break;
                }
            }

            Vector3 rot = wall.transform.rotation.eulerAngles;
            rot.y = newYRot;

            wall.transform.rotation = Quaternion.Euler(rot);

            // set wall position

            Vector3 camForwardRelToPlayer = Quaternion.Euler(new Vector3(0f, newYRot, 0f)) * Vector3.forward;
            Vector3 tempWallTransform = transform.position + camForwardRelToPlayer * wallWidth;

            posToPlaceWall = CustomFloorVec(tempWallTransform, wallWidth);
            wall.transform.position = posToPlaceWall;

        }
        else
        {
            if (wall)
            {
                Destroy(wall);
            }
        }
    }

    void HandleAimAnim()
    {
        if (Input.GetKey(KeyCode.Tab) && weight < 1f)
        {
            weight += 1 / aimingAnimTransitionTime * Time.deltaTime;
            weight = Mathf.Clamp(weight, 0f, 1f);
            charAnimator.SetLayerWeight(1, weight);
            aiming = true;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            aiming = false;
        }

        if (!aiming)
        {
            if (weight > 0f)
            {
                weight -= 1 / aimingAnimTransitionTime * Time.deltaTime;
                weight = Mathf.Clamp(weight, 0f, 1f);
                charAnimator.SetLayerWeight(1, weight);
            }
        }
    }

    Vector3 CustomFloorVec(Vector3 vec, float dist)
    {

        vec.x = Mathf.Floor(Mathf.Round(vec.x / dist)) * dist;
        vec.y = Mathf.Floor(Mathf.Round(vec.y / dist)) * dist;
        vec.z = Mathf.Floor(Mathf.Round(vec.z / dist)) * dist;

        return vec;
    }

    float CustomRound(float value, float min, float max)
    {
        if (Mathf.Abs(value - min) > Mathf.Abs(value - max))
        {
            return max;
        }
        return min;
    }
}
