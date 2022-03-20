using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck2 playerCheck2;

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn||playerCheck2.isOn){
            Destroy(this.gameObject);
        }
        
    }
}
