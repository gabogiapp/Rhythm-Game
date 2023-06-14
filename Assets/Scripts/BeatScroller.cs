using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BeatScroller : MonoBehaviour
{
    public float bpm;
    private float lastTime, deltaTime, timer;
    private float songLength;
    public int offset;
    public float speed;
    public float speedForTracking;
    public int count;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private GameObject note;
    public List<GameObject> myNotes = new List<GameObject>(); // list of Notes in everytime they spawn to launch them periodically

    private AudioSource audio;


    PlayerManager playerManager;
    GameManager GameManager;
    private PhotonView view;

    public bool hasStarted;
    // Start is called before the first frame update
    void Start()
    {
        audio = GameManager.instance.theMusic;
        lastTime = 0f;
        deltaTime = 0f;
        timer = 0f;
        count = 0;
        songLength = audio.clip.length;
        view = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerManager>();
        
    }
    void Update()
    {
        CreateNotes();
        LaunchNotes();
    }

    public void CreateNotes()
    {
        if (hasStarted)
        {
            if (audio.time < songLength - offset)
            {
                deltaTime = audio.time - lastTime;
                timer += deltaTime;


                if (timer >= ((60f / bpm)))
                {
                    /*if (count < Notes.transform.childCount)
                    {
                        GameObject myNote = Notes.transform.GetChild(count).gameObject;
                        myNotes.Add(myNote);
                        count++;
                    }*/

                    GameObject NotesAuto = Instantiate(note, new Vector3(0, 1, 60), Quaternion.identity);
                    myNotes.Add(NotesAuto);
                    timer -= (60f / bpm);

                }
            } 
          

        }

        lastTime = audio.time;
    }

    public void LaunchNotes()
    {
        if (hasStarted && playerManager.newPlayer != null)
        {
            Vector3 PlayerPos = playerManager.newPlayer.transform.position;
            Vector3 BehindPlayerPos = new Vector3(PlayerPos.x, PlayerPos.y, PlayerPos.z - 6f);
            foreach (GameObject note in myNotes)
            {
                if (note != null)
                {
                    Vector3 notePos = Vector3.MoveTowards(note.transform.position, BehindPlayerPos, speed * 100 / bpm * Time.deltaTime);

                    //Vector3 notePos = Vector3.SmoothDamp(note.transform.position, BehindPlayerPos, ref velocity, Time.deltaTime, 60/bpm * 200);

                    Vector3 notePos2 = Vector3.SmoothDamp(note.transform.position, BehindPlayerPos, ref velocity, Time.deltaTime, speedForTracking);

                    note.transform.position = new Vector3(notePos2.x, notePos2.y, notePos.z);
                }              
            }
        }     
    }

}
