using UnityEngine;

namespace Rope
{
    public class RopeController : MonoBehaviour
    {
        [SerializeField] GameObject fragmentPrefab;

        [SerializeField] int fragmentCount = 12;

        [SerializeField] Transform startTransform;
        [SerializeField] Transform endTransform;

        GameObject[] fragments;

        float[] xPositions;
        float[] yPositions;
        float[] zPositions;

        CatmullRomSpline splineX;
        CatmullRomSpline splineY;
        CatmullRomSpline splineZ;

        int splineFactor = 4;

        void Start()
        {
            fragments = new GameObject[fragmentCount];

            var position = startTransform.position;

            for (var i = 0; i < fragmentCount; i++)
            {
                fragments[i] = Instantiate(fragmentPrefab, position, Quaternion.identity);
                fragments[i].transform.SetParent(transform);

                var joint = fragments[i].GetComponent<SpringJoint>();
                if (i > 0)
                {
                    joint.connectedBody = fragments[i - 1].GetComponent<Rigidbody>();
                }

                position = startTransform.position + ((endTransform.position-startTransform.position) * (i / fragmentCount));
            }

            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = (fragmentCount - 1) * splineFactor + 1;

            xPositions = new float[fragmentCount];
            yPositions = new float[fragmentCount];
            zPositions = new float[fragmentCount];

            splineX = new CatmullRomSpline(xPositions);
            splineY = new CatmullRomSpline(yPositions);
            splineZ = new CatmullRomSpline(zPositions);

            fragments[0].GetComponent<Rigidbody>().isKinematic = true;
            fragments[fragments.Length - 1].GetComponent<Rigidbody>().isKinematic = true;
            Destroy(fragments[0].GetComponent<SpringJoint>());
            //Destroy(fragments[fragments.Length-1].GetComponent<SpringJoint>());
        }

        void Update()
        {   
            /*
            float distanceFactor = Mathf.InverseLerp(1, 5, Vector3.Distance(startTransform.position, endTransform.position));
            activeFragmentCount = Mathf.CeilToInt(Mathf.Lerp(3, fragmentCount, distanceFactor));
            
            for (var i = 0; i < fragmentCount; i++)
            {
                if (i <= fragmentCount - activeFragmentCount)
                {
                    //fragments[i].GetComponent<Rigidbody>().position = Vector3.zero;
                    fragments[i].GetComponent<Rigidbody>().isKinematic = true;
                }
                else
                {
                    fragments[i].GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            */
            
            fragments[0].GetComponent<Rigidbody>().MovePosition(startTransform.transform.position);
            fragments[0].GetComponent<Rigidbody>().MoveRotation(startTransform.rotation);
            fragments[fragmentCount - 1].GetComponent<Rigidbody>().MovePosition(endTransform.transform.position);
            fragments[fragmentCount - 1].GetComponent<Rigidbody>().MoveRotation(endTransform.transform.rotation);
            
        }

        void LateUpdate()
        {
            // Copy rigidbody positions to the line renderer
            var lineRenderer = GetComponent<LineRenderer>();

            // No interpolation
            //for (var i = 0; i < fragmentNum; i++)
            //{
            //    renderer.SetPosition(i, fragments[i].transform.position);
            //}

            for (var i = 0; i < fragmentCount; i++)
            {
                var position = fragments[i].transform.position;
                xPositions[i] = position.x;
                yPositions[i] = position.y;
                zPositions[i] = position.z;
            }

            for (var i = 0; i < (fragmentCount - 1) * splineFactor + 1; i++)
            {
                lineRenderer.SetPosition(i, new Vector3(
                    splineX.GetValue(i / (float) splineFactor),
                    splineY.GetValue(i / (float) splineFactor),
                    splineZ.GetValue(i / (float) splineFactor)));
            }
        }
    }
}
