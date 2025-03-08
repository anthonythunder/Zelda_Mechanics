using UnityEngine;
using System.Collections.Generic;

public class BombRune : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionUpwardForce;

    private List<IExplosion> all_IExplosion = new List<IExplosion>();

    private void Start()
    {
        MonoBehaviour[] allScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var script in allScripts)
        {
            if (script is IExplosion)
            {
                all_IExplosion.Add(script as IExplosion);
            }
        }
        rb = GetComponent<Rigidbody>();
    }
    public void Detonate()
    {
        foreach(var explosion in all_IExplosion)
        {
            explosion.ExplosionOrigin = transform.position;
            explosion.Explode();
        }
    }

}
