using UnityEngine;

public class Magnesisbullet : MonoBehaviour
{
    private float _lifeTime = 1.5f;
    public Magnesis magnesis;

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Destroy(gameObject);
            magnesis._shootMagnesisBullet = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Magnesis"))
        {
            magnesis._controlObject = true;
            magnesis._shootMagnesisBullet = false;

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
            magnesis._shootMagnesisBullet = false;
        }
    }
}

