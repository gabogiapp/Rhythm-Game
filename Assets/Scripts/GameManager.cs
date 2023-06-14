using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPun
{

    public AudioSource theMusic;
    public bool startPlaying;
    public BeatScroller theBS;
    private float _timer = 0f;
    private float songLength;

    [SerializeField] CanvasGroup canvasGroup;

    public static GameManager instance;

    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        view = GetComponent<PhotonView>();
        songLength = GetComponent<AudioSource>().clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (_timer >= 0.5f)
            {
                startPlaying = true;
                theBS.hasStarted = true;

                theMusic.Play();
            }
            _timer += Time.deltaTime;
        }

        if (theMusic.time + 1.5 >= theMusic.clip.length)
        {
            canvasGroup.alpha = 1;
        }
    }
}
