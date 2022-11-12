using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrometeoHelper : MonoBehaviour
{
    [Header("Steering Wheel")]
    public GameObject steeringWheel;
    public Vector3 steeringWheelRotationFactor;

    [Header("Impact")]
    [SerializeField] private float minImpact = 1f;
    [SerializeField] private float maxImpact = 10f;

    private Rigidbody rigidbody;
    private PrometeoCarController carController;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        carController = GetComponent<PrometeoCarController>();
    }

    public void RotateSteeringWheel(float steer)
    {
        //Debug.Log($"SteerFactor: {steer}");
        steeringWheel.transform.localRotation = Quaternion.Euler(steeringWheelRotationFactor * -steer);
    }

    public void ActivateHandbrake()
    {
        carController.Handbrake();
    }
    public void ActivateInput(bool activate)
    {
        carController.inputEnabled = activate;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impact = collision.impulse.magnitude;

        float impactScale = Mathf.InverseLerp(minImpact, maxImpact, impact);

        if (impactScale > 0)
        {
            //Debug.Log($"Impact: {impact} - Scale : {impactScale}");
            GameManager.Instance.cameraSelector.ShakeCamera(impactScale);
        }
    }
}
