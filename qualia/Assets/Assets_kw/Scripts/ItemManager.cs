using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    // プレイヤーを追尾する時の加速度、数値が大きいほどすぐ加速する
public float m_followAccel = 0.01f;

private bool m_isFollow; // プレイヤーを追尾するモードに入った場合 true
private float m_followSpeed; // プレイヤーを追尾する速さ
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var flag_suiyose = PlayerManager.m_instance.chocolate_eat;

// プレイヤーの現在地を取得する
var playerPos = PlayerManager.m_instance.transform.localPosition;

// プレイヤーとコインの距離を計算する
var distance = Vector3.Distance( playerPos, transform.localPosition );

// プレイヤーとコインの距離が近づいた場合
   if ( distance < PlayerManager.m_instance.m_magnetDistance && flag_suiyose == true)
        {
            // プレイヤーを追尾するモードに入る
            m_isFollow = true;
        }
// プレイヤーを追尾するモードに入っている場合かつ
// プレイヤーがまだ死亡していない場合
        if ( m_isFollow && PlayerManager.m_instance.gameObject.activeSelf )
        {
            // プレイヤーの現在位置へ向かうベクトルを作成する
            var direction = playerPos - transform.localPosition;
            direction.Normalize();

    // コインをプレイヤーが存在する方向に移動する
    transform.localPosition += direction * m_followSpeed;

    // 加速しながら近づく
    m_followSpeed += m_followAccel;
    return;
        }
    }

    public void GetItem()
    {
            // 衝突したオブジェクトがプレイヤーではない場合は無視する
    //if ( !collision.name.Contains( "Player" ) ) return;
        gameManager.Addscore(100);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D( Collider2D collision )
{
    // 衝突したオブジェクトがプレイヤーではない場合は無視する
    if ( !collision.name.Contains( "Player" ) ) return;

    // コインを削除する
    Destroy( gameObject );
}

}
