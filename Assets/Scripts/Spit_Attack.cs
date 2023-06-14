using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class Spit_Attack : StateMachineBehaviour
{
    public float speed = 2.5f;
    private GameObject[] players;
    private GameObject targetPlayer;
    private Rigidbody rb, projectileRb;
    private Vector3 spawnPos, targetPlayerPos;
    public int damage;

    Vector3 target, target2;
    private Vector3 velocity = Vector3.zero;
    public float speedForTracking;

    public GameObject projectilePrefab;
    private GameObject projectile;

    PhotonView view;

    private float timer, lastTime;
    private bool projectileCreated;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        lastTime = 0f;
        projectileCreated = false;

        view = animator.GetComponent<PhotonView>();

        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(go => go.name).ToArray();

        rb = animator.GetComponent<Rigidbody>();
        spawnPos = new Vector3(rb.position.x, rb.position.y + 7, rb.position.z - 2);

        targetPlayer = players[animator.GetInteger("randNum")];
        targetPlayerPos = targetPlayer.transform.position;

        animator.SetInteger("Damage", damage);

        
    }

   
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer >= lastTime + 0.65 * stateInfo.length && projectileCreated == false)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                projectile = PhotonNetwork.Instantiate(projectilePrefab.name, spawnPos, Quaternion.identity, 0, new object[] { view.ViewID });
                projectileRb = projectile.GetComponent<Rigidbody>();
                projectileCreated = true;
            }
        }
        timer += Time.deltaTime;
        if (projectileCreated == true)
        {
            if (projectileRb == null)
                return;
            target2 = Vector3.MoveTowards(projectileRb.position, targetPlayerPos, speedForTracking * Time.deltaTime);
            target = Vector3.MoveTowards(projectileRb.position, targetPlayerPos, speed * 30 * Time.deltaTime);
            projectileRb.position = new Vector3(target2.x, target2.y, target.z);
        }
            
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (GameObject player in players)
        {
            player.transform.Find("Target").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0);
        }
    }
}
