using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class BoomController : MonoBehaviour
{
    public Transform transform;
    Vector3 explosionPoint;
    public float explosionRadius = 10f;
    public float initialForce = 1000f;
    public float maxDistance = 20f;
    Collider[] colliders;
    private List<GameObject> clones;
    private GameObject[] gameObjects;
    MeshRenderer meshRenderer;

    void Start()
    {
        gameObjects = GameObject.FindGameObjectsWithTag ( "Bomb" );
        explosionPoint = transform.position;
        clones = new List<GameObject>(gameObjects);
        clones.Remove(GameObject.Find("TNT"));
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.gray;
    }

    void Update()
    {
        StartCoroutine(Explode());
        StartCoroutine(ChangeColor());
    }

    private void Boom()
    {
        colliders = Physics.OverlapSphere(explosionPoint, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                float distance = Vector3.Distance(explosionPoint, collider.transform.position);
                float force = initialForce * (1 - (distance / maxDistance));

                Vector3 direction = (collider.transform.position - explosionPoint).normalized;

                rigidbody.AddForce(direction * force);
            }
        }
        if(clones.Contains(gameObject))
            Destroy(gameObject);
        
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(2f);
        Boom();
    }

    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(0.5f);
        meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        meshRenderer.material.color = Color.gray;
    }
}
