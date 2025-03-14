using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class BombRune : MonoBehaviour
{
    public GameObject BlastEffect;
    public CinemachineImpulseSource cinemachineImpulse;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask DamageableMask;

    public void Detonate()
    {
        GameObject blasteffect = Instantiate(BlastEffect, transform.position, Quaternion.identity);
        Collider [] nearbyObjects =  Physics.OverlapSphere(transform.position, explosionRadius, DamageableMask);

        foreach (var obj in nearbyObjects)
        {
            if(Vector3.Distance(obj.transform.position, transform.position) <= explosionRadius)
            {
                obj.GetComponent<ObjecHealth>().Explode(transform);
            }
        }

        cinemachineImpulse.GenerateImpulse();

        GetComponent<MeshRenderer>().enabled = false;
        Destroy(blasteffect,0.33f);
        Destroy(gameObject, 0.33f);
    }

}
