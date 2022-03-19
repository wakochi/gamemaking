using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBlinkingSub : MonoBehaviour
{
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck2 playerCheck2;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(255, 255, 255, 1); 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn||GameObject.Find("Coin2")== null){
            this.gameObject.SetActive (false);
            Invoke(nameof(DelayMethod), 10f);
        }
    }

    void DelayMethod()
    {
        this.gameObject.SetActive (true);
    }

}
