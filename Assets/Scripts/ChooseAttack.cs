using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseAttack : StateMachineBehaviour
{
    GameManager GameManager;
    private float timer, lastTime, songLength;
    private int offset = 5;
    private AudioSource audio;
    public float timeBtwAttacks = 10f;
    private int whichAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audio = GameManager.instance.theMusic;
        timer = 0f;
        lastTime = 0f;
        songLength = audio.clip.length;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer >= lastTime + timeBtwAttacks && timer + offset <= songLength)
        {
            lastTime = Time.deltaTime;
            whichAttack = animator.GetInteger("attack");
            if (whichAttack == 0)
            {
                animator.SetBool("RunAttack", true);
            }
            else if (whichAttack == 1)
            {
                animator.SetBool("SpitAttack", true);
            }
        }
        timer += Time.deltaTime;
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("RunAttack", false);
        animator.SetBool("SpitAttack", false);
    }
}
