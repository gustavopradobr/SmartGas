using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private LineRenderer gpsLineRenderer;
    [SerializeField] private LineRenderer worldLineRenderer;

    [Header("Properties")]
    [SerializeField] private float updatePathInterval = 0.1f;
    [SerializeField] private float gpsLineRendererHeight = 0.1f;
    [SerializeField] private float worldLineRendererHeight = 0.1f;
    [SerializeField] private float worldLineMoveSpeed = 2.5f;

    private NavMeshPath path;
    private Vector3[] pathCornerResults { get; } = new Vector3[10];
    [HideInInspector] public float pathIntervalElapsed = 0.0f;
    private string navigationAreaName = "Navigation";
    private int navigationAreaIndex = -1;
    private Material worldLineMaterial;


    void Start()
    {
        pathIntervalElapsed = 0;
        path = new NavMeshPath();
        navigationAreaIndex = NavMesh.GetAreaFromName(navigationAreaName);
        worldLineRenderer.material = Instantiate<Material>(worldLineRenderer.material);
        worldLineMaterial = worldLineRenderer.material;

        if(worldLineMoveSpeed != 0)
            StartCoroutine(LineRendererMoveCoroutine());
    }

    void Update()
    {
        pathIntervalElapsed += Time.deltaTime;
        if (pathIntervalElapsed < updatePathInterval)
            return;
        pathIntervalElapsed = 0;

        CalculateNavigation();
    }

    public void EnableWorldRoute(bool enable)
    {
        worldLineRenderer.enabled = enable;
    }

    private void RenderPath()
    {
        //Use this instead of "tmpPath.corners" to avoid Garbage and limit array to 7 to use only first 7 corners
        int pathLenght = path.GetCornersNonAlloc(pathCornerResults);

        gpsLineRenderer.positionCount = pathLenght;
        worldLineRenderer.positionCount = pathLenght;

        if (pathLenght > 0)
        {
            Vector3[] pathCorner = pathCornerResults;
            for (int i = 0; i < pathCorner.Length; i++)
                pathCorner[i].y = gpsLineRendererHeight != 0 ? gpsLineRendererHeight : pathCorner[i].y;
            gpsLineRenderer.SetPositions(pathCorner);

            pathCorner = pathCornerResults;
            for (int i = 0; i < pathCorner.Length; i++)
                pathCorner[i].y = worldLineRendererHeight != 0 ? worldLineRendererHeight : pathCorner[i].y;
            worldLineRenderer.SetPositions(pathCorner);
        }
    }


    private void CalculateNavigation()
    {
        path = new NavMeshPath();

        //Set Destination
        NavMesh.CalculatePath(playerTransform.position, targetTransform.position, navigationAreaIndex, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            RenderPath();
        }
        else
        {
            NavMeshHit playerSample;
            NavMeshHit targetSample;
            NavMesh.SamplePosition(playerTransform.position, out playerSample, 10.0f, navigationAreaIndex);
            NavMesh.SamplePosition(targetTransform.position, out targetSample, 10.0f, navigationAreaIndex);
            NavMesh.CalculatePath(playerSample.position, targetSample.position, navigationAreaIndex, path);

            if (path.status == NavMeshPathStatus.PathComplete) //nearestPath
            {
                RenderPath();
            }
            else
            {
                Debug.Log("<color=red>Path denied</color>");
            }
        }
    }

    private IEnumerator LineRendererMoveCoroutine()
    {
        float offset = 0;
        while (true)
        {
            worldLineMaterial.mainTextureOffset = new Vector2(offset, 0);
            offset -= Time.deltaTime * worldLineMoveSpeed;
            if (offset < 100)
                offset += 100;
            else if (offset > 100)
                offset -= 100;
            yield return null;
        }
    }

}
