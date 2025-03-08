using UnityEngine;

public class ObjecHealth : MonoBehaviour
{
    private Rigidbody rb;

    public float explosionForce = 100;
    public float explosionUpwardForce = 1;
    private float explosionRadius = 7;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    #region Explosion Interface
    public void Explode(Transform bomb)
    {
        rb.AddExplosionForce(explosionForce, bomb.position, explosionRadius, explosionUpwardForce);
    }
    #endregion
}
