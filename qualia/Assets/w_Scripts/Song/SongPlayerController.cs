using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class SongPlayerController : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int jumpForce;
    private bool isJumping = false;
    private bool SongItemflag = false;
    private List<string> PushedList = new List<string>();
    private List<string> GoalPushedList = new List<string>();

    AudioSource audioSource;
    [SerializeField] private AudioClip jumpsound;
    [SerializeField] private AudioClip dashsound;
    [SerializeField] private AudioClip eatsound;
    [SerializeField] private AudioClip Power;

	void Update () {
        audioSource = GetComponent<AudioSource>();
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !(rb.velocity.y < -0.5f))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.B)){
            OneDash();
        }

        if (Input.GetKeyDown(KeyCode.X)){
            Eat();
        }

        if (Input.GetKeyDown(KeyCode.Y)){
            Item();
        }

        GoalPushedList.Add("Dash");
        GoalPushedList.Add("Jump");
        GoalPushedList.Add("Eat");

        if(PushedList.Count == 3){
            if(GoalPushedList[0] == PushedList[0])
            {
                if(GoalPushedList[1] == PushedList[1])
                {
                    if(GoalPushedList[2] == PushedList[2]){
                        audioSource.PlayOneShot(Power);
                        PushedList.Clear();
                    }else{
                        PushedList.Clear();
                    }
                }else{
                    PushedList.Clear();
                }
            }else{
                PushedList.Clear();
            }
        }
	}

    void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        if(SongItemflag == true){
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(jumpsound);
            PushedList.Add("Jump");
            Invoke(nameof(Flagoff), 1f);
        }
    }
    void Flagoff(){
        PushedList.Clear();
    }

    void OneDash()
    {
        if(SongItemflag == true){
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(dashsound);
            PushedList.Add("Dash");
            Invoke(nameof(Flagoff), 1f);
        }
    }


    void Eat()
    {
        if(SongItemflag == true){
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(eatsound);
            PushedList.Add("Eat");
            Invoke(nameof(Flagoff), 1f);
        }
    }

    void Item()
    {
        SongItemflag = true;
        Invoke(nameof(SongItemflagoff), 10f);
    }
    void SongItemflagoff()
    {
        SongItemflag = false;
    }





    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stage"))
        {
            isJumping = false;
        }
    }

}