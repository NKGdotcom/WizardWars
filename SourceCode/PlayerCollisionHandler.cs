using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerHealthManager playerHealthManager;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthManager = GetComponent<PlayerHealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision _col)
    {

    }
}
