using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{

    // void Start()
    // {
        
    // }

    // void Update()
    // {
        
    // }



   // 判定内にプレイヤーがいる
   [HideInInspector] public bool isOn = false;
   private string playerTag = "Player";

   #region//接触判定
    private void OnTriggerEnter2D(Collider2D collision)
   {
       if (collision.tag == playerTag)
       {
          isOn = true;
          SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
       }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
           isOn = false;
        }
    }
    #endregion





    // // 他のオブジェクトと当たった時に呼び出される関数
    // private void OnTriggerEnter2D( Collider2D other )
    // {
    //     // 名前に「Player」が含まれるオブジェクトと当たったら
    //     if ( other.name.Contains( "SongPlayerController" ) )
    //     {
    //         // プレイヤーから Player スクリプトを取得する
    //         var player = other.GetComponent<SongPlayerController>();
    //         // プレイヤーがやられた時に呼び出す関数を実行する
    //         player.Dead();
    //     }
    // }




}
