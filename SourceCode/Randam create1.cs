using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Randamcreate : MonoBehaviour
{
    public GameObject[] Prefabs;
    private float time;
    private int number;
    private float spawnInterval = 3.0f; // �ŏ���300�t���[���Ɉ�񐶐�
    private float intervalChangeTime = 10.0f; // �X�s�[�h�ύX�Ԋu
    private float timeSinceLastChange = 0.0f;
    public TextMeshProUGUI textMeshProObject;
    public AudioClip sound1;
    public float displayTime = 3.0f; // �\�����ԁi�b�j

    void Start()
    {
       
        textMeshProObject.gameObject.SetActive(false); // �ŏ��ɔ�\���ɂ���
        
    }

    void Update()
    {
        time += Time.deltaTime;
        timeSinceLastChange += Time.deltaTime;
     

        if (time >= spawnInterval)
        {
            float x = Random.Range(-49.5f, 49.5f);
            Vector3 pos = new Vector3(x, 0f, 45.0f);
            number = Random.Range(0, Prefabs.Length);
            Instantiate(Prefabs[number], pos, Quaternion.identity);

            // �C���^�[�o�������Z�b�g
            time = 0f;
        }

        // �X�s�[�h�ύX�̃^�C�~���O�� spawnInterval �𒲐�
        if (timeSinceLastChange >= intervalChangeTime)
        {
            if (spawnInterval == 3.0f)
            {
                StartCoroutine(DisplayTextRoutine());
                spawnInterval = 2.0f;
            }
            else if (spawnInterval == 2.0f)
            {
                StartCoroutine(DisplayTextRoutine());
                spawnInterval = 1.5f;
            }
            else if (spawnInterval == 1.5f)
            {
                StartCoroutine(DisplayTextRoutine());
                spawnInterval = 1.0f;
            }
            else if (spawnInterval ==1.0f)
                {

            }
            // �^�C�~���O�����Z�b�g
            timeSinceLastChange = 0.0f;
        }
    }

    IEnumerator DisplayTextRoutine()
    {
        AudioSource.PlayClipAtPoint(sound1, transform.position);
        // �e�L�X�g��\��
        textMeshProObject.gameObject.SetActive(true);

        // ��莞�ԑҋ@
        yield return new WaitForSeconds(displayTime);

        // �e�L�X�g���\��
        textMeshProObject.gameObject.SetActive(false);
    }
}