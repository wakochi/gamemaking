using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int jumpForce;
    private bool isJumping = false;
    public static PlayerController m_instance;
    public float m_magnetDistance; // コインを引きつける距離
    public bool chocolate_eat = false;
    public bool chocolate_full = false;
    public bool super_mode = false;
    public bool super_jump = false;
    public bool stay = false;
    public bool first_jump = false;
    public bool second_jump = false;
    void Start()
    {
        m_instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !(rb.velocity.y < -0.5f) && super_jump == false)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !first_jump && !second_jump && super_jump == true)
        {
            Jump();
            first_jump = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && first_jump == true && !second_jump && super_jump == true)
        {
            Jump();
            second_jump = true;
        }
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
        if (Input.GetKeyDown("q") && chocolate_full == false && super_mode == false)
            {
            chocolate_eat = true;
            chocolate_full = true;
            Invoke("Eat",1.0f);
            Invoke("Full",10.0f);
            }

        if (Input.GetKeyDown("e") && super_mode == false)
        {
            super_mode = true;
            Invoke("Super",10.0f);
        }
    }
    void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Super()
    {
        moveSpeed = moveSpeed*3;
        super_jump = true;
        //jumpForce = jumpForce*3;
        Invoke("Nomal",10.0f);
    }

    void Nomal()
    {
        super_mode = false;
        super_jump = false;
        moveSpeed = moveSpeed/3;
        //jumpForce = jumpForce/3;
    }

    void Eat()
    {
        chocolate_eat = false;
    }

    void Full()
    {
        chocolate_full = false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stage"))
        {
            isJumping = false;
            first_jump = false;
            second_jump = false;
        }
    }
}


