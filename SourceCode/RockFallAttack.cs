using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFallAttack : MonoBehaviour
{
    //h = h0 -v0t -1/2gt^2 (ï®óùÇÃìôâ¡ë¨ìxíºê¸â^ìÆ)
    [SerializeField] private float _firstVelocityMin;
    [SerializeField] private float _firstVelocityMax;
    private float _firstVelocity;
    [SerializeField] private float _accelationMin;
    [SerializeField] private float _accelationMax;
    private float _accelation;
    
    // Start is called before the first frame update
    void Start()
    {
        _firstVelocity = Random.Range(_firstVelocityMin, _firstVelocityMax);
        _accelation = Random.Range(_accelationMin, _accelationMax);
    }

    // Update is called once per frame
    void Update()
    {
        FallRock();
    }
    private void FallRock()
    {
        Vector3 position = transform.position;
        position.y = transform.position.y - _firstVelocity * Time.deltaTime - 0.5f * _accelation * Time.deltaTime * Time.deltaTime;
        transform.position = position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            gameObject.SetActive(false);
        }
    }
}
