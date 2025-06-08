using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSpecialMove : MonoBehaviour //雷を上から数秒間降らせる(プレイヤーの周りを数秒間雷を降らせる)
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
            // ランダムなオフセットを生成（X軸とZ軸のランダム範囲）
            float randomX = Random.Range(-spawnRadius, spawnRadius);
            float randomZ = Random.Range(-spawnRadius, spawnRadius);

            // 下方向を基準にランダムな方向を計算
            Vector3 randomOffset = new Vector3(0, -1, 0).normalized; // 必ず下方向に向かせる
            _thunderBallList[i].gameObject.transform.position = _spawnPoint.position + new Vector3(randomX, 0, randomZ);

            _thunderBallList[i].SetActive(true);

            // Rigidbodyを取得して初速を設定
            Rigidbody rb = _thunderBallList[i].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
            if (rb != null)
            {
                rb.velocity = randomOffset * _bulletSpeed; // 初速を設定
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
    private void SpawnBullet()
    {
        
    }
}