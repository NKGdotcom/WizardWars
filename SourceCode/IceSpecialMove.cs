using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpecialMove : MonoBehaviour //•X‚Ì˜A‘±UŒ‚‚ğ10‰ñ‚Ù‚ÇL”ÍˆÍ‚É•ú‚¿A‚ ‚Ä‚½“G‚É‘Î‚µ‚ÄƒXƒs[ƒh‚ğ’x‚­‚·‚é
{
    [SerializeField] private Boss _boss;

    [SerializeField] private GameObject[] _iceBallPrefabs;
    [SerializeField] private Transform _instantiatePosition;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _projectileSpeed;

    void OnEnable()
    {
        IceProjectilesAtOnePoint();
    }
    /// <summary>
    /// •X‚Ì‹Ê‚ğˆê“_‚ÉŒü‚©‚Á‚Ä”­Ë
    /// </summary>
    public void IceProjectilesAtOnePoint()
    {
        Vector3 onePoint = _instantiatePosition.position;
        for (int i = 0; i < _iceBallPrefabs.Length; i++)
        {
            float angle = i * Mathf.PI * 2 / _iceBallPrefabs.Length;
            Vector3 spawnPosition = _instantiatePosition.position + new Vector3(Mathf.Cos(angle)*_spawnRadius,5,Mathf.Sin(angle)*_spawnRadius);
            _iceBallPrefabs[i].SetActive(true);
            _iceBallPrefabs[i].transform.position = spawnPosition;
            Rigidbody rb = _iceBallPrefabs[i].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
            _iceBallPrefabs[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            if (rb != null )
            {
                Vector3 direction = (onePoint - spawnPosition).normalized;
                Vector3 directionDown = direction + _iceBallPrefabs[i].transform.up * (-0.5f);
                rb.velocity = directionDown * _projectileSpeed;
            }
        }
    }
}
