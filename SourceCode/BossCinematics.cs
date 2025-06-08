using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCinematics : MonoBehaviour
{
    [SerializeField] private GameObject _bossCinematics;

    [SerializeField] private float _bossWalkTime = 68 / 60f; // 秒に変換
    [SerializeField] private float _bossWalkSpeed = 0.2f;
    [SerializeField] private float _cameraUpTime = 2f;
    [SerializeField] private float _cameraUpSpeed = 0.5f;

    private float _elapsedTime; // 経過時間を管理
    private bool _stopCamera;
    private bool _stopWalk;

    // Start is called before the first frame update
    void Start()
    {
        _stopCamera = false;
        _stopWalk = false;
        _elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime; // フレームごとに経過時間を加算

        if (!_stopWalk)
        {
            if (_elapsedTime < _bossWalkTime)
            {
                // ボスが前進する処理
                transform.position += new Vector3(0, 0, _bossWalkSpeed * Time.deltaTime);
            }
            else
            {
                _stopWalk = true;
                _elapsedTime = 0f; // 経過時間をリセット
            }
        }
        else if (!_stopCamera)
        {
            if (_elapsedTime < _cameraUpTime)
            {
                // カメラが上に移動する処理
                _bossCinematics.transform.position += new Vector3(0, _cameraUpSpeed * Time.deltaTime, 0);
            }
            else
            {
                _stopCamera = true;
            }
        }
    }
}
