using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundChange : MonoBehaviour
{
    [Header("プレイヤーの判定")] public PlayerManager playerManagerCheck;
    // Update is called once per frame

    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(0, 0, 0, 1); 
 
    }


    void Update()
    {
        if (playerManagerCheck.isVisualOn)
        {
            GetComponent<Renderer>().material.color = Color.white;
            Invoke(nameof(DelayMethod), 10f);
        }
    }

    void DelayMethod()
    {
        GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
    }



}
