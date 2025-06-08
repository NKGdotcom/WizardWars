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
    private float spawnInterval = 3.0f; // 最初は300フレームに一回生成
    private float intervalChangeTime = 10.0f; // スピード変更間隔
    private float timeSinceLastChange = 0.0f;
    public TextMeshProUGUI textMeshProObject;
    public AudioClip sound1;
    public float displayTime = 3.0f; // 表示時間（秒）

    void Start()
    {
       
        textMeshProObject.gameObject.SetActive(false); // 最初に非表示にする
        
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

            // インターバルをリセット
            time = 0f;
        }

        // スピード変更のタイミングで spawnInterval を調整
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
            // タイミングをリセット
            timeSinceLastChange = 0.0f;
        }
    }

    IEnumerator DisplayTextRoutine()
    {
        AudioSource.PlayClipAtPoint(sound1, transform.position);
        // テキストを表示
        textMeshProObject.gameObject.SetActive(true);

        // 一定時間待機
        yield return new WaitForSeconds(displayTime);

        // テキストを非表示
        textMeshProObject.gameObject.SetActive(false);
    }
}