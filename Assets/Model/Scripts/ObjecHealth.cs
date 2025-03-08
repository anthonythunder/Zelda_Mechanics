using UnityEngine;

public class ObjecHealth : MonoBehaviour,IExplosion
{
    private Rigidbody rb;

    public float explosionForce;
    public float explosionUpwardForce;
    public float explosionRadius;
    public Vector3 ExplosionOrigin
    {
        get
        {
            return transform.position;
        }
        set
        {
            return;
        }

    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    #region Explosion Interface
    public void Explode()
    {
        rb.AddExplosionForce(explosionForce, ExplosionOrigin, 10, explosionUpwardForce);
    }
    #endregion
}
