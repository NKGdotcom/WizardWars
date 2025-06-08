using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSpecialMove : MonoBehaviour //�����ォ�琔�b�ԍ~�点��(�v���C���[�̎���𐔕b�ԗ����~�点��)
{
    [SerializeField] private GameObject[] _thunderBallList;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _spawnInterval = 0.1f;
    [SerializeField] private float downwardAngle = -10f;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(SpawnLightningBullet());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator SpawnLightningBullet()
    {
        for (int i = 0; i < _thunderBallList.Length; i++)
        {
            // �����_���ȃI�t�Z�b�g�𐶐��iX����Z���̃����_���͈́j
            float randomX = Random.Range(-spawnRadius, spawnRadius);
            float randomZ = Random.Range(-spawnRadius, spawnRadius);

            // ����������Ƀ����_���ȕ������v�Z
            Vector3 randomOffset = new Vector3(0, -1, 0).normalized; // �K���������Ɍ�������
            _thunderBallList[i].gameObject.transform.position = _spawnPoint.position + new Vector3(randomX, 0, randomZ);

            _thunderBallList[i].SetActive(true);

            // Rigidbody���擾���ď�����ݒ�
            Rigidbody rb = _thunderBallList[i].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
            if (rb != null)
            {
                rb.velocity = randomOffset * _bulletSpeed; // ������ݒ�
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
    private void SpawnBullet()
    {
        
    }
}