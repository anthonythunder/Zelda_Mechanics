using UnityEngine;
using System.Collections.Generic;

public class BombRune : MonoBehaviour
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask DamageableMask;

    public void Detonate()
    {
        Collider [] nearbyObjects =  Physics.OverlapSphere(transform.position, explosionRadius, DamageableMask);

        foreach (var obj in nearbyObjects)
        {
            if(Vector3.Distance(obj.transform.position, transform.position) <= explosionRadius)
            {
                obj.GetComponent<ObjecHealth>().Explode(transform);
            }
        }

        Destroy(gameObject);
    }

}
