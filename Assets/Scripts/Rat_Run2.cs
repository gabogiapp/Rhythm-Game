using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class Rat_Run2 : StateMachineBehaviour
{
    public float speed = 2.5f;
    private GameObject[] players;
    private GameObject targetPlayer;
    private Rigidbody rb;
    private Vector3 originalPos, targetPlayerPos;
    public int damage;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(go => go.name).ToArray();
        rb = animator.GetComponent<Rigidbody>();
        
        originalPos = rb.position;

        targetPlayer = players[animator.GetInteger("randNum")];
        targetPlayerPos = targetPlayer.transform.position;

        animator.SetInteger("Damage", damage);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 target = new Vector3(targetPlayerPos.x, rb.position.y, targetPlayerPos.z - 20f);
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * 25 * Time.deltaTime);
        rb.position = newPos;
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.position = originalPos;
        foreach (GameObject player in players)
        {
            player.transform.Find("Target").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0);
        }
    }

}
