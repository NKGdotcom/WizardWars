using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 設定したアビリティの通常玉を発射
/// </summary>
public class PlayerAbillityShotNormal : MonoBehaviour
{
    [Header("プレイヤーのデータ")]
    [SerializeField] private PlayerData playerData;
    private PlayerMagicPointManager playerMagicPointManager;

    [SerializeField] private Transform shotPos;
    private GameObject firstAbilityPrefab;
    private GameObject secondAbilityPrefab;
    [SerializeField] private float bulletDestroyTime = 3;

    //Prefab情報をすでに獲得しているか
    private bool getFirstAbilityInformation = false;
    private bool getSecondAbilityInformation = false;

    private bool readySpecialShot; //必殺ショットの準備
    // Start is called before the first frame update
    void Start()
    {
        playerMagicPointManager = GetComponent<PlayerMagicPointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) readySpecialShot = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) readySpecialShot = false;
    }
    /// <summary>
    /// 一つ目の能力を発射
    /// </summary>
    /// <param name="_shotPrefabs"></param>
    public void ShotFirstAbility(GameObject _shotPrefab)
    {
        if (!getFirstAbilityInformation)
        {
            firstAbilityPrefab = _shotPrefab;
        }
        NormalShot(_shotPrefab);
    }
    /// <summary>
    /// 2つ目の能力を発射
    /// </summary>
    /// <param name="_shotPrefab"></param>
    public void ShotSecondAbility(GameObject _shotPrefab)
    {
        if (!getSecondAbilityInformation)
        {
            secondAbilityPrefab = _shotPrefab;
        }
        NormalShot(_shotPrefab);
    }
    /// <summary>
    /// 能力を放って力を加える
    /// </summary>
    /// <param name="_prefab"></param>
    private void NormalShot(GameObject _prefab)
    {
        if (readySpecialShot) return;
        playerMagicPointManager.ReduceMPNormalShot(); //MPがまだあるか判断

        GameObject _bullet = Instantiate(_prefab, shotPos.transform.position, Quaternion.identity);
        Rigidbody _bulletRb = _bullet.GetComponent<Rigidbody>();
        _bulletRb.AddForce(shotPos.forward * playerData.ButtletSpeed);
        Destroy(_bullet, bulletDestroyTime);
    }
}
