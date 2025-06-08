using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName ="ScriptableObjects/Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float PlayerMoveSpeed { get => _playerMoveSpeed; set => _playerMoveSpeed = value; }
    public float PlayerRotationSpeed { get => _playerRotationSpeed; set => _playerRotationSpeed = value; }
    public float ButtletSpeed { get => _bulletSpeed; set => _bulletSpeed = value;}
    public float BulletInterval { get => _bulletInterval; set => _bulletInterval = value; }
    public float PlayerHP { get => _playerHP; set => _playerHP = value; }
    public float PlayerMP { get => _playerMP; set => _playerMP = value; }


    [Header("�v���C���[�̈ړ��X�s�[�h")]
    [SerializeField] float _playerMoveSpeed;
    [Header("�v���C���[�̉�]�X�s�[�h")]
    [SerializeField] float _playerRotationSpeed;
    [Header("�e��ł������̃X�s�[�h")]
    [SerializeField] float _bulletSpeed;
    [Header("�e��ł������̃C���^�[�o��")]
    [SerializeField] float _bulletInterval;
    [Header("�v���C���[��HP")]
    [SerializeField] float _playerHP;
    [Header("�v���C���[��MP")]
    [SerializeField] float _playerMP;
}
