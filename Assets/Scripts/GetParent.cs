using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GetParent : MonoBehaviour
{
    [PunRPC]
        private void SetParent()
        {
            
            this.gameObject.transform.SetParent(GameObject.Find("SpawnPoints").transform, false);
           
        }

}
