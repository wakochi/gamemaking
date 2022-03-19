using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundChange : MonoBehaviour
{
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck2 playerCheck2;
    // Update is called once per frame

    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0); 
 
    }


    void Update()
    {
        if (playerCheck.isOn||playerCheck2.isOn){
            GetComponent<Renderer>().material.color = Color.white;
            Invoke(nameof(DelayMethod), 10f);
        }
    }

    void DelayMethod()
    {
        GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
    }



}
