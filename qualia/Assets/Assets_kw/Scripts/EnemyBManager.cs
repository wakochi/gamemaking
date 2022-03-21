using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBManager : MonoBehaviour
{
    [SerializeField] GameObject enemyDeathEffect;

    GameManager gameManager;
    //SpriteRenderer sr = null;

    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    Rigidbody2D rigidbody2DEnemyB;
    float speed;
    private float originCurrentPositionDifferenceThreshold = 5.0f; //�ŏ��̈ʒu�ƌ��݂̈ʒu�̍���臒l
    private Vector3 originPosition = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //sr = GetComponent<SpriteRenderer>();
        rigidbody2DEnemyB = GetComponent<Rigidbody2D>();
        direction = DIRECTION_TYPE.LEFT;
        originPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        ChangeDirection();
    }

    private void FixedUpdate()
    {
        switch (direction)
        {
            case DIRECTION_TYPE.STOP: //�~�܂��Ă���
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT: //�E�ɓ���
                speed = 4;
                transform.localScale = new Vector3(-1, 1, 1); //�L�����N�^�[�̌������E�ɕύX
                break;
            case DIRECTION_TYPE.LEFT: //���ɓ���
                speed = -4;
                transform.localScale = new Vector3(1, 1, 1); //�L�����N�^�[�̌��������ɕύX
                break;
        }
        rigidbody2DEnemyB.velocity = new Vector2(speed, rigidbody2DEnemyB.velocity.y);
    }


    void ChangeDirection()
    {
        // enemyposition ------------臒l���傫������----------- originPosition�@���̈ʒu�֌W�̏ꍇ�A���g������]��
        if (originPosition.x - transform.position.x > originCurrentPositionDifferenceThreshold) 
        {
            direction = DIRECTION_TYPE.RIGHT;
        }
        // originPosition ------------������臒l���傫��----------- enemyposition�@���̈ʒu�֌W�̏ꍇ�A���g������]��
        else if (transform.position.x - originPosition.x > originCurrentPositionDifferenceThreshold) 
        {
            direction = DIRECTION_TYPE.LEFT;
        }
    }

    public void DestroyEnemy()
    {
        gameManager.Addscore(100);
        Instantiate(enemyDeathEffect, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
