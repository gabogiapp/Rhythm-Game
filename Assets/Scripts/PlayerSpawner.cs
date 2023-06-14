using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject newPlayer;
    private PhotonView PV;


    private void Start()
    {
        /* if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null)
         {
             GameObject playerToSpawn = playerPrefabs[0];
             newPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, new Vector3(0, 2, 0), Quaternion.identity, 0) as GameObject;

         }
         else 
         {
             GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
             newPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, new Vector3(0, 2, 0), Quaternion.identity, 0);

         }

     }*/
    }
}


