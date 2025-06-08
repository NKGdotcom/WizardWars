using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBig : MonoBehaviour
{
    [SerializeField] private float _delayDestroy;
    private void OnEnable()
    {
        StartCoroutine(Destroy(_delayDestroy));
    }
    /// <summary>
    /// �I�u�W�F�N�g�����𐔕b�Ŕj��
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>

    private IEnumerator Destroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}