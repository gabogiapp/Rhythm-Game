using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class selfDestruct : MonoBehaviour
{
    private float timer, lastTime;
    void Awake()
    {
        timer = 0f;
        lastTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timer >= lastTime + 0.75f)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
            timer += Time.deltaTime;
        }
        
    }
}
