using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAManager : MonoBehaviour
{
    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameObject enemyDeathEffect;

    GameManager gameManager;
    SpriteRenderer sr = null;

    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    Rigidbody2D rigidbody2DPlayer;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sr = GetComponent<SpriteRenderer>();
        rigidbody2DPlayer = GetComponent<Rigidbody2D>();
        direction = DIRECTION_TYPE.LEFT;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsGround())
        {
            ChangeDirection();
        }
    }

    private void FixedUpdate()
    {
        if (sr.isVisible)
        {
            switch (direction)
            {
                case DIRECTION_TYPE.STOP: //止まっている
                    speed = 0;
                    break;
                case DIRECTION_TYPE.RIGHT: //右に動く
                    speed = 4;
                    transform.localScale = new Vector3(1, 1, 1); //キャラクターの向きを右に変更
                    break;
                case DIRECTION_TYPE.LEFT: //左に動く
                    speed = -4;
                    transform.localScale = new Vector3(-1, 1, 1); //キャラクターの向きを左に変更
                    break;
            }
        }     
        rigidbody2DPlayer.velocity = new Vector2(speed, rigidbody2DPlayer.velocity.y);
    }

    bool IsGround()
    {
        Vector3 startVec = transform.position + transform.right * 0.25f * transform.localScale.x;
        Vector3 endVec = startVec - transform.up * 0.5f;
        Debug.DrawLine(startVec, endVec);
        return Physics2D.Linecast(startVec, endVec, blockLayer);
    }

    void ChangeDirection()
    {
        if(direction == DIRECTION_TYPE.RIGHT)
        {
            direction = DIRECTION_TYPE.LEFT;
        }else if(direction == DIRECTION_TYPE.LEFT)
        {
            direction = DIRECTION_TYPE.RIGHT;
        }
    }

    public void DestroyEnemy()
    {
        gameManager.Addscore(100);
        Instantiate(enemyDeathEffect, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "BreakableBlock")
        {
            ChangeDirection();
        }
    }
}
