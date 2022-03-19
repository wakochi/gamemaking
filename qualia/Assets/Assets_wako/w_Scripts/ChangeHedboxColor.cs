using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHedboxColor : MonoBehaviour
{
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(255, 255, 255, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn){
            GetComponent<Renderer>().material.color = Color.white;
            // Invoke(nameof(DelayMethod), 10f);
        }
    }

    // void DelayMethod()
    // {
    //     GetComponent<Renderer>().material.color = new Color(255, 255, 255, 0);
    // }

}
