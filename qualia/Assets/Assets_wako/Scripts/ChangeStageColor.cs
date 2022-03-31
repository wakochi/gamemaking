using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStageColor : MonoBehaviour
{
    //[Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    //[Header("プレイヤーの判定")] public PlayerTriggerCheck2 playerCheck2;
    [Header("プレイヤーの判定")] public PlayerManager playerManagerCheck;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(255, 255, 255, 1);  
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManagerCheck.isVisualOn)
        {
            GetComponent<Renderer>().material.color = Color.white;
            // Invoke(nameof(DelayMethod), 10f);
        }else{
            GetComponent<Renderer>().material.color = new Color(255, 255, 255, 1); 
        }
    }

    // void DelayMethod()
    // {
    //     GetComponent<Renderer>().material.color = new Color(255, 255, 255, 1); 
    // }

}
