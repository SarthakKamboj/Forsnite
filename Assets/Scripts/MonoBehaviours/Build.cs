using UnityEngine;
using Cinemachine;
using static Cinemachine.CinemachineFreeLook;

public class Build : MonoBehaviour
{

    [SerializeField] GameObject wallPrefab;
    [SerializeField] Camera mainCamera;
    [SerializeField] CinemachineFreeLook cm;
    [SerializeField] Animator charAnimator;
    [SerializeField] float aimingAnimTransitionTime;
    [SerializeField] float wallCheckRadius = 0.1f;
    [SerializeField] int buildPolygonSides = 4;

    [HideInInspector]
    public static float wallSeparation;
    float wallWidth;
    float weight = 0f;
    float buildPolygonAngle;
    float count = 0;
    bool aiming = false;
    bool movedCamCloser = false;
    GameObject wall;
    Vector3 posToPlaceWall = Vector3.one;
    Renderer wallRenderer;
    Orbit[] orbits;

    void Awake()
    {
        buildPolygonAngle = 360f / buildPolygonSides;
        GameObject tempWall = Instantiate(wallPrefab);
        Bounds wallBounds = tempWall.transform.Find("GFX").GetComponent<Collider>().bounds;
        wallSeparation = wallBounds.extents.y;
        wallWidth = wallBounds.size.y;
        Destroy(tempWall);

        orbits = cm.m_Orbits;
        // for (int i = 0; i < orbits.Length; i++)
        // {
        //     Orbit orbit = orbits[i];
        //     Debug.Log(orbit.m_Height);
        //     Debug.Log(orbit.m_Radius);
        // }
    }

    void Update()
    {
        HandleAimAnim();
        HandleWallSim();
        HandleWallCreation();
    }

    void HandleWallCreation()
    {

        if (aiming)
        {
            Vector3 wallCheckPos = wall.transform.Find("GFX").position;
            Collider[] cols = Physics.OverlapSphere(wallCheckPos, wallCheckRadius);
            bool available = cols.Length == 1;
            if (available)
            {
                wallRenderer.enabled = true;
                if (Input.GetMouseButton(0))
                {
                    wall.GetComponent<WallCreation>().WallCreated();
                    wall = null;
                    wallRenderer = null;
                    count += 1;
                }
            }
            else
            {
                wallRenderer.enabled = false;
            }
        }
    }

    void HandleWallSim()
    {
        if (aiming)
        {
            if (!wall)
            {
                wall = Instantiate(wallPrefab, Vector3.zero, Quaternion.Euler(Vector3.up));
                wallRenderer = wall.transform.Find("GFX").GetComponent<Renderer>();

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
            Vector3 camForwardRelToPlayer = (Quaternion.Euler(new Vector3(0f, newYRot, 0f)) * Vector3.forward).normalized;
            Vector3 tempWallTransform = transform.position + camForwardRelToPlayer * wallSeparation;
            posToPlaceWall = CustomFloorVec(tempWallTransform);
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
            if (!movedCamCloser)
            {
                Orbit[] orbits = cm.m_Orbits;
                for (int i = 0; i < orbits.Length; i++)
                {
                    orbits[i].m_Height = orbits[i].m_Height / 2f;
                    orbits[i].m_Radius = orbits[i].m_Radius / 2f;
                }
                movedCamCloser = true;

                Debug.Log("moved camera closer");
            }

        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            aiming = false;
            if (movedCamCloser)
            {
                Orbit[] orbits = cm.m_Orbits;
                for (int i = 0; i < orbits.Length; i++)
                {
                    orbits[i].m_Height = orbits[i].m_Height * 2f;
                    orbits[i].m_Radius = orbits[i].m_Radius * 2f;
                }
                movedCamCloser = false;
                Debug.Log("moved camera further");
            }
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

    Vector3 CustomFloorVec(Vector3 vec)
    {
        Vector3 prevVec = vec;

        vec.x = Mathf.Floor(Mathf.Round(vec.x / wallSeparation)) * wallSeparation;
        vec.y = Mathf.Floor(Mathf.Round(vec.y / wallWidth)) * wallWidth;
        vec.z = Mathf.Floor(Mathf.Round(vec.z / wallSeparation)) * wallSeparation;

        Vector3 newVec = vec;

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("prevVec: " + prevVec + " newVec: " + newVec);
        }

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
