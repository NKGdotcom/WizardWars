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


    [Header("プレイヤーの移動スピード")]
    [SerializeField] float _playerMoveSpeed;
    [Header("プレイヤーの回転スピード")]
    [SerializeField] float _playerRotationSpeed;
    [Header("弾を打った時のスピード")]
    [SerializeField] float _bulletSpeed;
    [Header("弾を打った時のインターバル")]
    [SerializeField] float _bulletInterval;
    [Header("プレイヤーのHP")]
    [SerializeField] float _playerHP;
    [Header("プレイヤーのMP")]
    [SerializeField] float _playerMP;
}
