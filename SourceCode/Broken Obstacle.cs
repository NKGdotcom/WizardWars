using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BrokenObstacle : MonoBehaviour
{
   
    ScoreCount scoreCount;
    public AudioClip sound;
    public int point = 1;
    private void Update()
    {
        scoreCount = gameObject.GetComponent<ScoreCount>();
    }
    
    
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "Fish")
        {
            AudioSource.PlayClipAtPoint(sound, transform.position);
            collision.gameObject.SetActive(false);
           
           

        }
        
    }
}