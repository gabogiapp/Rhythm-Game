using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class PlayerController : MonoBehaviour
{
    PhotonView view;
    public float moveSpeed;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public float groundDrag;
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    float horizontalInput;
    float verticalInput;

    public Vector3 sizeChange;
    private Vector3 playerPos;
    Vector3 notePos;

    public KeyCode[] KeysToPress;
    private KeyCode keyToPress;

    int timingScore;
    private int currentMultiplier;
    private int multiplierTracker;
    private int[] multiplierThresholds;
    public CanvasGroup canvasGroup;
    public TMP_Text scoreFeedbackText;

    private Rigidbody rb;

    private bool canBePressed;
    private bool isHit;
    public int damage = 0;

    private GameObject note;

    

    PlayerManager playerManager;
    private void Awake()
    {
        
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerManager = PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerManager>();
        if (!view.IsMine)
        {
            Destroy(rb);
        }
        currentMultiplier = 1;
        multiplierThresholds = new int[] { 3, 10, 20, 30 };
    }

    private void Update()
    {
        if (view.IsMine)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            MyInput();
            SpeedControl();

            if (grounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 0;
            }
            foreach (KeyCode KeyToPress in KeysToPress)
            {
                if (Input.GetKeyDown(KeyToPress))
                {
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                }
                if (Input.GetKeyUp(KeyToPress))
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                }
            }

            if (Input.GetKeyDown(keyToPress) || Input.GetKeyDown(KeyCode.Space))
            {
                if (canBePressed)
                {
                    if (Mathf.Abs(notePos.z) <= 0.25f)
                    {
                        timingScore = 100;
                        AppearText("Perfect");
                    }
                    else if (Mathf.Abs(notePos.z) <= 0.5f)
                    {
                        timingScore = 75;
                        AppearText("Excellent");

                    }
                    else if (Mathf.Abs(notePos.z) <= 1.00f)
                    {
                        timingScore = 50;
                        AppearText("Good");

                    }
                    else
                    {
                        timingScore = 25;
                        AppearText("Meh");

                    }

                    if (currentMultiplier -1 < multiplierThresholds.Length)
                    {
                        multiplierTracker++;
                        if(multiplierThresholds[currentMultiplier -1] <= multiplierTracker)
                        {
                            currentMultiplier++;
                        }
                    }

                    timingScore *= currentMultiplier;
                    view.RPC(nameof(HitNote), view.Owner, timingScore);
                    Destroy(note);
                }
                else
                {
                    AppearText("Missed");
                    view.RPC(nameof(MissedNote), view.Owner);
                    currentMultiplier = 1;
                    multiplierTracker = 0;
                }
            }

            if (isHit)
            {
                
                view.RPC(nameof(Hit), view.Owner, damage);             
                isHit = false;
            }


        }

    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            MovePlayer();
        }
    }

    IEnumerator WaitAndDisappearText(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canvasGroup.alpha = 0;
    }

    private void AppearText(string input)
    {
        Vector3 playerPos = GetComponent<RectTransform>().position;
        scoreFeedbackText.text = input;
        scoreFeedbackText.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-20f, 20f));
        scoreFeedbackText.transform.position = new Vector3(playerPos.x + Random.Range(-3f, 3f), playerPos.y + Random.Range(-0.5f, 3f), 0);
        float randScale = Random.Range(0.5f, 1.25f);
        scoreFeedbackText.transform.localScale = new Vector3(randScale, randScale, randScale);
        canvasGroup.alpha = 1;
        StartCoroutine(WaitAndDisappearText(1f));
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        if (grounded)
        {
            rb.AddForce(horizontalInput * moveSpeed * 10f, 0f, 0f, ForceMode.Force);
        }
        if (!grounded)
        {
            if (verticalInput < 0)
            {
                rb.AddForce(horizontalInput * moveSpeed * 10f, verticalInput * moveSpeed * 5f, 0f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(horizontalInput * moveSpeed * 10f, 0f, 0f, ForceMode.Force);
            }

        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, rb.velocity.y, 0f);

        if (flatVel.magnitude > moveSpeed * 2.5)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, rb.velocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
        rb.AddForce(0f, verticalInput * jumpForce, 0f, ForceMode.Impulse);

    }


    private void ResetJump()
    {
        readyToJump = true;
    }

    
        
           
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Note")
        {
            canBePressed = true;
            keyToPress = other.gameObject.GetComponent<NoteObject>().keyToPress;
            note = other.gameObject; 
        }

        if (other.tag == "Boss")
        {
            damage = other.GetComponent<Animator>().GetInteger("Damage");
            isHit = true;  
        }
        if (other.tag == "Obstacle")
        {
            damage = GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>().GetInteger("Damage");
            isHit = true;
            Debug.Log(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Note")
        {
            AppearText("Missed");
            view.RPC(nameof(MissedNote), view.Owner);
            canBePressed = false;
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            currentMultiplier = 1;
            multiplierTracker = 0;
        }
        if (other.tag == "Obstacle")
        {
            isHit = false;
        }
        if (other.tag == "Boss")
        {
            isHit = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        notePos = other.transform.position;
    }

    [PunRPC]
    void HitNote(int score, PhotonMessageInfo info)
    {
        PlayerManager.Find(info.Sender).UpdateScore(score);
    }
    [PunRPC]
    void MissedNote(PhotonMessageInfo info)
    {
        PlayerManager.Find(info.Sender).UpdateMisses();
    }
    [PunRPC]
    void Hit(int score, PhotonMessageInfo info)
    {
        PlayerManager.Find(info.Sender).Hit(score);
    }
}
