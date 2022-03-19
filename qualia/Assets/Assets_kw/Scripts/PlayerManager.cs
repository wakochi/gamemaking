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
        float x = Input.GetAxis("Horizontal"); //方向キーの取得
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
            //SpaceキーおよびコントローラーAボタン
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

        //aキーおよびコントローラーXボタン
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

        //zキーおよびコントローラーBボタン
        //if ((Input.GetKeyDown("z") || Input.GetKeyDown("joystick button 1")) && isRotating == false && gameManager.canRotateflg == true)
        if ((Input.GetButtonDown("Controller button B") && isRotating == false && gameManager.canRotateflg == true))
        {
            isRotating = true;
            RightArrowBlockEffect = true;
            UpArrowBlockEffect = true;
            animator.SetBool("isrotating", true);
            Invoke("rotateCancel", 1.5f); //1.5s後に回転キャンセル
            gameManager.StartCoroutine("RotationCoolTimeCounterZero");

        }

        //xキーおよびコントローラーYボタン
        //if ((Input.GetKeyDown("x") || Input.GetKeyDown("joystick button 3")) && VibrationEnergyUsing == true)
        if ((Input.GetButtonDown("Controller button Y") && VibrationEnergyUsing == true))
        {
            ControllerVibrationStart();
           // StartCoroutine("TemporaryWait"); //コルーチンにしたけど、結局使わず
            CinemachineOfMainCameraAndPlayerStop();
            objectShaker.Shake(mainCameraObject); 
            Invoke("CinemachineOfMainCameraAndPlayerActive", 1.5f); //1,5s後に無効化したコンポーネントおよびスクリプトを有効にする
            ReleaseVibrationEnergy(); //画面内の敵を倒す
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
            case DIRECTION_TYPE.STOP: //止まっている
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT: //右に動く
                speed = 4;
                if(Dash)
                {
                    speed = 8;
                }
                transform.localScale = new Vector3(1, 1, 1); //キャラクターの向きを右に変更
                break;
            case DIRECTION_TYPE.LEFT: //左に動く
                speed = -4;
                if (Dash)
                {
                    speed = -8;
                }
                transform.localScale = new Vector3(-1, 1, 1); //キャラクターの向きを左に変更
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
            //アイテム取得
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyAManager enemy = collision.gameObject.GetComponent<EnemyAManager>();
            if (this.transform.position.y + 0.2f > enemy.transform.position.y) //上から踏む
            {
                rigidbody2DPlayer.velocity = new Vector2(rigidbody2DPlayer.velocity.x, 0);
                jump();
                enemy.DestroyEnemy();
            }
            else // 横からぶつかる
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

        if (collision.gameObject.tag == "VibrationEnergy") //キラキラに入ると振動を使える
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
                rigidbody2DPlayer.velocity = Vector2.zero; //ジャンプ中だとy方向の速度と足し合わさって大きく飛んでしまうため、速度0で初期化する
                rigidbody2DPlayer.AddForce(Vector2.up * jumpPower * 2);
                UpArrowBlockEffect = false;
            }
        }

        if (collision.gameObject.tag == "RightMovingBlock")
        {
            if (isRotating == true && RightArrowBlockEffect == true)
            {
                rigidbody2DPlayer.velocity = Vector2.zero; //x軸方向に速度を持っていると足し合わさって大きく移動しまうため、速度0で初期化する
                rigidbody2DPlayer.AddForce(Vector2.right * sidewaysPower * 2);
                RightArrowBlockEffect = false;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision) //キラキラから出ると振動を止める
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
    // プレイヤー共通関数群Start
    // ===================================================================================================================================

    /// <summary>
    /// プレイヤーをジャンプさせる
    /// </summary>
    void jump()
    {
        //上に力を加える
        rigidbody2DPlayer.AddForce(Vector2.up * jumpPower);
        animator.SetBool("isjumping", true);

    }
    

   /// <summary>
   /// プレイヤーと地面の設置判定を行う
   /// </summary>
    bool IsGround()
    {
        //始点と終点を作成
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f + Vector3.up * 0.04f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f + Vector3.up * 0.04f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        //Debug.DrawLine(leftStartPoint, endPoint);
        //Debug.DrawLine(rightStartPoint, endPoint);
        return Physics2D.Linecast(leftStartPoint, endPoint, blockLayer)
            || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }

    /// <summary>
    /// コントローラーの振動を開始する
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
    /// コントローラーの振動を止める
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
    /// MainCameraのChinemachineBrainとこのスクリプト自身をアクティブにする
    /// </summary>
    void CinemachineOfMainCameraAndPlayerActive()
    {
        mainCaneraObjectComponent.enabled = true;
        this.enabled = true;

    }

    /// <summary>
    /// MainCameraのChinemachineBrainとこれ自身を非アクティブにする
    /// </summary>
    void CinemachineOfMainCameraAndPlayerStop()
    {
        mainCaneraObjectComponent.enabled = false; //ChinemachineBrainを無効
        animator.SetFloat("speed", Mathf.Abs(0));  
        rigidbody2DPlayer.velocity = new Vector2(0, rigidbody2DPlayer.velocity.y); //動きながらギミックを発動する場合も考慮してここでプレイヤーの動きを止める
        this.enabled = false; //このスクリプトを無効にしてプレイヤーを動かないようにする
    }

    // ===================================================================================================================================
    // プレイヤー共通関数群End
    // ===================================================================================================================================

    // ===================================================================================================================================
    // 回転ステージ関数群Start
    // ===================================================================================================================================

    /// <summary>
    /// プレイヤーの回転をキャンセルする（Invokeで呼び出して使用）
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
    // 回転ステージ関数群End
    // ===================================================================================================================================

    // ===================================================================================================================================
    // 振動ステージ関数群Start
    // ===================================================================================================================================

    /// <summary>
    /// 周囲のコライダーを集めて、敵のコライダーであればその敵を倒す
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

        if (vibrationEnergy != null) vibrationEnergy.OnCompleteEffect(); //キラキラエフェクトを消す
        VibrationEnergyUsing = false; //連打防止
    }

    /// <summary>
    /// 一時的にプレイヤーを動けなくする
    /// </summary>
        private IEnumerator TemporaryWait()
    {
        ControllerVibrationStart();
        yield return new WaitForSeconds(1.0f);
        CinemachineOfMainCameraAndPlayerStop();
        objectShaker.Shake(mainCameraObject);
        Invoke("CinemachineOfMainCameraAndPlayerActive", 1.5f); //1,5s後に無効化したコンポーネントおよびスクリプトを有効にする
        ReleaseVibrationEnergy(); //画面内の敵を倒す
        Invoke("ControllerVibrationEnd", 1.5f);
    }

    // ===================================================================================================================================
    // 振動ステージ関数群End
    // ===================================================================================================================================

}
