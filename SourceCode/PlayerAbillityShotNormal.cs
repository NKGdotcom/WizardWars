using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ݒ肵���A�r���e�B�̒ʏ�ʂ𔭎�
/// </summary>
public class PlayerAbillityShotNormal : MonoBehaviour
{
    [Header("�v���C���[�̃f�[�^")]
    [SerializeField] private PlayerData playerData;
    private PlayerMagicPointManager playerMagicPointManager;

    [SerializeField] private Transform shotPos;
    private GameObject firstAbilityPrefab;
    private GameObject secondAbilityPrefab;
    [SerializeField] private float bulletDestroyTime = 3;

    //Prefab�������łɊl�����Ă��邩
    private bool getFirstAbilityInformation = false;
    private bool getSecondAbilityInformation = false;

    private bool readySpecialShot; //�K�E�V���b�g�̏���
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
    /// ��ڂ̔\�͂𔭎�
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
    /// 2�ڂ̔\�͂𔭎�
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
    /// �\�͂�����ė͂�������
    /// </summary>
    /// <param name="_prefab"></param>
    private void NormalShot(GameObject _prefab)
    {
        if (readySpecialShot) return;
        playerMagicPointManager.ReduceMPNormalShot(); //MP���܂����邩���f

        GameObject _bullet = Instantiate(_prefab, shotPos.transform.position, Quaternion.identity);
        Rigidbody _bulletRb = _bullet.GetComponent<Rigidbody>();
        _bulletRb.AddForce(shotPos.forward * playerData.ButtletSpeed);
        Destroy(_bullet, bulletDestroyTime);
    }
}
