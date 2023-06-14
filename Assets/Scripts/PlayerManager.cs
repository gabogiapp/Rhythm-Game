using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class PlayerManager : MonoBehaviourPun
{
    PhotonView view;
    public GameObject[] playerPrefabs;
    public GameObject newPlayer;
    public GameObject NoteHolder;
    GameObject beatScroller;
    

    int currentScore = 0;
    int misses = 0;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        newPlayer = null;
    }

    void Start()
    {
        if (view.IsMine)
        {
            CreatePlayer();
            CreateBeatScroller();
        }
    }


    void CreatePlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null)
        {
            GameObject playerToSpawn = playerPrefabs[0];
            newPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, new Vector3(0, 2, 0), Quaternion.identity, 0, new object[] { view.ViewID });

        }
        else
        {
            GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            newPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, new Vector3(0, 2, 0), Quaternion.identity, 0, new object[] { view.ViewID });

        }
    }

    void CreateBeatScroller()
    {
        beatScroller = PhotonNetwork.Instantiate(NoteHolder.name, new Vector3(0, 2, 70), Quaternion.identity, 0, new object[] { view.ViewID });
    }

    public void UpdateScore(int score)
    {
        view.RPC(nameof(RPC_UpdateScore), view.Owner, score);
    }

    public void UpdateMisses()
    {
        view.RPC(nameof(RPC_UpdateMisses), view.Owner);
    }
    public void Hit(int score)
    {
        view.RPC(nameof(RPC_Hit), view.Owner, score);
    }

    [PunRPC]
    void RPC_UpdateScore(int score)
    {
        currentScore += score;
        Hashtable hash = new Hashtable();
        hash.Add("Score", currentScore);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    [PunRPC]
    void RPC_UpdateMisses()
    {
        misses++;
        Hashtable hash = new Hashtable();
        hash.Add("Misses", misses);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    [PunRPC]
    void RPC_Hit(int score)
    {
        currentScore -= score;
        Hashtable hash = new Hashtable();
        hash.Add("Score", currentScore);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.view.Owner == player);
    }
}