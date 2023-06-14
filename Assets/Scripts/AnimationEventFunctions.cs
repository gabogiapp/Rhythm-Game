using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
public class AnimationEventFunctions : MonoBehaviour
{
    PhotonView view;
    private GameObject[] players;
    private GameObject targetPlayer;
    List<int> previousNums = new List<int>{};
    private int randNum, attack;
    public int numOfAttacks;

    void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    [PunRPC]
    void RPC_ChooseTarget(int num)
    {
        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(go => go.name).ToArray();

        randNum = num;
        GetComponent<Animator>().SetInteger("randNum", randNum);

        GameObject targetPlayer = players[randNum];
        targetPlayer.transform.Find("Target").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, .5f);
    }

    public void ChooseTarget()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            randNum = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
            previousNums.Add(randNum);
            view.RPC(nameof(RPC_ChooseTarget), RpcTarget.All, randNum);
        }  
    }
    public void RemoveTarget()
    {
        if (players == null)
        {
            return;
        }
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                player.transform.Find("Target").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0);
            }
        } 
    }

    public void WhichAttack()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            attack = Random.Range(0, numOfAttacks);
            view.RPC(nameof(RPC_WhichAttack), RpcTarget.All, attack);
        }
    }
    [PunRPC]
    void RPC_WhichAttack(int num)
    {
        attack = num;
        GetComponent<Animator>().SetInteger("attack", attack);
    }
}
