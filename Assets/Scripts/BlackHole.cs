using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public Transform transform;
    Vector3 attractPoint;
    public float attractRadius = 10f;
    public float initialForce = -100f;
    public float maxDistance = 20f;
    Collider[] colliders;
    MeshRenderer meshRenderer;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.black;
    }

    void Update()
    {
        attractPoint = transform.position;
        Attract();
    }
    
    void Attract()
    {
        colliders = Physics.OverlapSphere(attractPoint, attractRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                float distance = Vector3.Distance(attractPoint, collider.transform.position);
                float force = initialForce * (1 - (distance / maxDistance));

                Vector3 direction = (collider.transform.position - attractPoint).normalized;

                rigidbody.AddForce(direction * force);
            }
        }
    }

}
