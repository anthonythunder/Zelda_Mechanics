using UnityEngine;

public class Magnesisbullet : MonoBehaviour
{
    public Magnesis magnesis;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Magnesis"))
        {
            magnesis._controlObject = true;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

