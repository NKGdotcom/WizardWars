using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�������^�[�Q�b�g�ɒǔ�
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + cameraOffset;
    }
}
