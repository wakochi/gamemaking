using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingTile : MonoBehaviour
{
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;

    public GameObject MainOb;
    void Start()
    {
        InvokeRepeating("ChangeActive",0f,5f);
    }

    void ChangeActive()
    {
        if(MainOb.activeSelf == false)
        {
            MainOb.SetActive(true);
        }else{
            MainOb.SetActive(false);
        }
    }
    
}
