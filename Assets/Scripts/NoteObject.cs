using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NoteObject : MonoBehaviour
{
    public Renderer Object;
    public Material[] materials;
    public KeyCode[] KeysToPress;
    public KeyCode keyToPress;
   

    void Awake()
    {
        SetAppearance(Random.Range(0, 4));
    }



    private void SetAppearance(int randNum)
    {
       
        Object.material = materials[randNum];
        keyToPress = KeysToPress[randNum];
    }
    /*[PunRPC]
    private void ChangeOwnerToMC(int viewID)
    {
        if ((view.Owner != PhotonNetwork.MasterClient) && (view != null))
        {
            view.RequestOwnership();
        }
    } */

   
}
