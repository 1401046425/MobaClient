using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    private ParticleSystem ps;

    // Use this for initialization
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}