using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameObject playerDeathEffect;
    [SerializeField] ObjectShaker objectShaker;

    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    Rigidbody2D rigidbody2DPlayer;
    GameObject mainCameraObject;
    CinemachineBrain mainCaneraObjectComponent;
    VibrationEnergyManager vibrationEnergy;
    VibrationBlockManager vibrationBlock;

    float speed;

    Animator animator;
    bool isRotating = false;
    bool RightArrowBlockEffect = false;
    bool UpArrowBlockEffect = false;
    bool Dash = false;
    bool VibrationEnergyUsing = false;
    bool canVibrationBlockUsing = false;

    float jumpPower = 500;
    float sidewaysPower = 5000;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2DPlayer = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCameraObject = Camera.main.gameObject;
        mainCaneraObjectComponent= mainCameraObject.GetComponent<CinemachineBrain>();       
    }

    // Update is called once per frame
    private void Update()
    {
        float x = Input.GetAxis("Horizontal"); //�����L�[�̎擾
        animator.SetFloat("speed", Mathf.Abs(x));

        if (x == 0)
        {
            direction = DIRECTION_TYPE.STOP;
        }
        else if (x > 0)
        {
            direction = DIRECTION_TYPE.RIGHT;
        }
        else if (x < 0)
        {
            direction = DIRECTION_TYPE.LEFT;
        }

        if (IsGround())
        {
            //Space�L�[����уR���g���[���[A�{�^��
            //if (Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 0"))
            if(Input.GetButtonDown("Controller button A"))
            {
                jump();
            }
            else
            {
                animator.SetBool("isjumping", false);
            }
        }

        //a�L�[����уR���g���[���[X�{�^��
        //if (Input.GetKey("a") || Input.GetKey("joystick button 2"))
        if (Input.GetButton("Controller button X") )
        {
            if (IsGround())
            {
                Dash = true;
            }
        }
        else
        {
            Dash = false;
        }

        //z�L�[����уR���g���[���[B�{�^��
        //if ((Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 1")) && isRotating == false && gameManager.canRotateflg == true)
        if ((Input.GetButtonDown("Controller button B") && isRotating == false && gameManager.canRotateflg == true))
        {
            isRotating = true;
            RightArrowBlockEffect = true;
            UpArrowBlockEffect = true;
            animator.SetBool("isrotating", true);
            Invoke("rotateCancel", 1.5f); //1.5s��ɉ�]�L�����Z��
            gameManager.StartCoroutine("RotationCoolTimeCounterZero");

        }

        //x�L�[����уR���g���[���[Y�{�^��
        //if ((Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 3")) && VibrationEnergyUsing == true)
        if ((Input.GetButtonDown("Controller button Y") && VibrationEnergyUsing == true))
        {
            ControllerVibrationStart();
           // StartCoroutine("TemporaryWait"); //�R���[�`���ɂ������ǁA���ǎg�킸
            CinemachineOfMainCameraAndPlayerStop();
            objectShaker.Shake(mainCameraObject); 
            Invoke("CinemachineOfMainCameraAndPlayerActive", 1.5f); //1,5s��ɖ����������R���|�[�l���g����уX�N���v�g��L���ɂ���
            ReleaseVibrationEnergy(); //��ʓ��̓G��|��
            Invoke("ControllerVibrationEnd", 2.0f); 
        }

        if ((Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 3")) && canVibrationBlockUsing == true)
        {
            vibrationBlock.RisingBlock();
            canVibrationBlockUsing = false;
            ControllerVibrationEnd();
        }
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
                if(Dash)
                {
                    speed = 8;
                }
                transform.localScale = new Vector3(1, 1, 1); //�L�����N�^�[�̌������E�ɕύX
                break;
            case DIRECTION_TYPE.LEFT: //���ɓ���
                speed = -4;
                if (Dash)
                {
                    speed = -8;
                }
                transform.localScale = new Vector3(-1, 1, 1); //�L�����N�^�[�̌��������ɕύX
                break;
        }
        rigidbody2DPlayer.velocity = new Vector2(speed, rigidbody2DPlayer.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            Destroy(this.gameObject);
            Instantiate(playerDeathEffect, this.transform.position, this.transform.rotation);
            gameManager.GameOver();
        }

        if (collision.gameObject.tag == "Item")
        {
            //�A�C�e���擾
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyAManager enemy = collision.gameObject.GetComponent<EnemyAManager>();
            if (this.transform.position.y + 0.2f > enemy.transform.position.y) //�ォ�瓥��
            {
                rigidbody2DPlayer.velocity = new Vector2(rigidbody2DPlayer.velocity.x, 0);
                jump();
                enemy.DestroyEnemy();
            }
            else // ������Ԃ���
            {
                if (isRotating == true)
                {
                    enemy.DestroyEnemy();
                }
                else
                {
                    Destroy(this.gameObject);
                    Instantiate(playerDeathEffect, this.transform.position, this.transform.rotation);
                    gameManager.GameOver();
                }
            }
        }
        if (collision.gameObject.tag == "Finish")
        {
            gameManager.GameClear();
        }

        if (collision.gameObject.tag == "VibrationEnergy") //�L���L���ɓ���ƐU�����g����
        {
            vibrationEnergy = collision.gameObject.GetComponent<VibrationEnergyManager>();
            //ControllerVibrationStart();
            VibrationEnergyUsing = true;
        }
  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BreakableBlock")
        {
            BreakableBlockManager breakableBlock = collision.gameObject.GetComponent<BreakableBlockManager>();

            if (isRotating == true)
            {
                breakableBlock.DestroyBreakableBlock();
            }
        }

        if (collision.gameObject.tag == "VibrationBlock")
        {
            vibrationBlock = collision.gameObject.GetComponent<VibrationBlockManager>();
            ControllerVibrationStart();
            canVibrationBlockUsing = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "UpMovingBlock")
        {
            if (isRotating == true && UpArrowBlockEffect == true)
            {
                rigidbody2DPlayer.velocity = Vector2.zero; //�W�����v������y�����̑��x�Ƒ������킳���đ傫�����ł��܂����߁A���x0�ŏ���������
                rigidbody2DPlayer.AddForce(Vector2.up * jumpPower * 2);
                UpArrowBlockEffect = false;
            }
        }

        if (collision.gameObject.tag == "RightMovingBlock")
        {
            if (isRotating == true && RightArrowBlockEffect == true)
            {
                rigidbody2DPlayer.velocity = Vector2.zero; //x�������ɑ��x�������Ă���Ƒ������킳���đ傫���ړ����܂����߁A���x0�ŏ���������
                rigidbody2DPlayer.AddForce(Vector2.right * sidewaysPower * 2);
                RightArrowBlockEffect = false;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision) //�L���L������o��ƐU�����~�߂�
    {
        if (collision.gameObject.tag == "VibrationEnergy")
        {
            VibrationEnergyUsing = false;
            //ControllerVibrationEnd();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "VibrationBlock")
        {
            vibrationBlock = collision.gameObject.GetComponent<VibrationBlockManager>();
            ControllerVibrationEnd();
            canVibrationBlockUsing = false;
        }
    }


    // ===================================================================================================================================
    // �v���C���[���ʊ֐��QStart
    // ===================================================================================================================================

    /// <summary>
    /// �v���C���[���W�����v������
    /// </summary>
    void jump()
    {
        //��ɗ͂�������
        rigidbody2DPlayer.AddForce(Vector2.up * jumpPower);
        animator.SetBool("isjumping", true);

    }
    

   /// <summary>
   /// �v���C���[�ƒn�ʂ̐ݒu������s��
   /// </summary>
    bool IsGround()
    {
        //�n�_�ƏI�_���쐬
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f + Vector3.up * 0.04f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f + Vector3.up * 0.04f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        //Debug.DrawLine(leftStartPoint, endPoint);
        //Debug.DrawLine(rightStartPoint, endPoint);
        return Physics2D.Linecast(leftStartPoint, endPoint, blockLayer)
            || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }

    /// <summary>
    /// �R���g���[���[�̐U�����J�n����
    /// </summary>

    void ControllerVibrationStart()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.5f, 1.0f);
        }
    }

    /// <summary>
    /// �R���g���[���[�̐U�����~�߂�
    /// </summary>
    void ControllerVibrationEnd()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

    /// <summary>
    /// MainCamera��ChinemachineBrain�Ƃ��̃X�N���v�g���g���A�N�e�B�u�ɂ���
    /// </summary>
    void CinemachineOfMainCameraAndPlayerActive()
    {
        mainCaneraObjectComponent.enabled = true;
        this.enabled = true;

    }

    /// <summary>
    /// MainCamera��ChinemachineBrain�Ƃ��ꎩ�g���A�N�e�B�u�ɂ���
    /// </summary>
    void CinemachineOfMainCameraAndPlayerStop()
    {
        mainCaneraObjectComponent.enabled = false; //ChinemachineBrain�𖳌�
        animator.SetFloat("speed", Mathf.Abs(0));  
        rigidbody2DPlayer.velocity = new Vector2(0, rigidbody2DPlayer.velocity.y); //�����Ȃ���M�~�b�N�𔭓�����ꍇ���l�����Ă����Ńv���C���[�̓������~�߂�
        this.enabled = false; //���̃X�N���v�g�𖳌��ɂ��ăv���C���[�𓮂��Ȃ��悤�ɂ���
    }

    // ===================================================================================================================================
    // �v���C���[���ʊ֐��QEnd
    // ===================================================================================================================================

    // ===================================================================================================================================
    // ��]�X�e�[�W�֐��QStart
    // ===================================================================================================================================

    /// <summary>
    /// �v���C���[�̉�]���L�����Z������iInvoke�ŌĂяo���Ďg�p�j
    /// </summary>
    private void rotateCancel()
    {
        if (isRotating)
        {
            isRotating = false;
            animator.SetBool("isrotating", false);
            //gameManager.RotationCoolTimeCounterZero();       
        }
    }

    // ===================================================================================================================================
    // ��]�X�e�[�W�֐��QEnd
    // ===================================================================================================================================

    // ===================================================================================================================================
    // �U���X�e�[�W�֐��QStart
    // ===================================================================================================================================

    /// <summary>
    /// ���͂̃R���C�_�[���W�߂āA�G�̃R���C�_�[�ł���΂��̓G��|��
    /// </summary>
    private void ReleaseVibrationEnergy()
    {
        Collider2D[] enemyCols = Physics2D.OverlapBoxAll(transform.position, new Vector2(18.0f, 10.0f), 0.0f);
        if (enemyCols != null)
        {
            for (int i = 0; i < enemyCols.Length; i++)
            {
                EnemyAManager enemy = enemyCols[i].GetComponent<EnemyAManager>();
                if (enemy != null)
                {

                    enemy.DestroyEnemy();
                }
            }
        }

        if (vibrationEnergy != null) vibrationEnergy.OnCompleteEffect(); //�L���L���G�t�F�N�g������
        VibrationEnergyUsing = false; //�A�Ŗh�~
    }

    /// <summary>
    /// �ꎞ�I�Ƀv���C���[�𓮂��Ȃ�����
    /// </summary>
        private IEnumerator TemporaryWait()
    {
        ControllerVibrationStart();
        yield return new WaitForSeconds(1.0f);
        CinemachineOfMainCameraAndPlayerStop();
        objectShaker.Shake(mainCameraObject);
        Invoke("CinemachineOfMainCameraAndPlayerActive", 1.5f); //1,5s��ɖ����������R���|�[�l���g����уX�N���v�g��L���ɂ���
        ReleaseVibrationEnergy(); //��ʓ��̓G��|��
        Invoke("ControllerVibrationEnd", 1.5f);
    }

    // ===================================================================================================================================
    // �U���X�e�[�W�֐��QEnd
    // ===================================================================================================================================

}