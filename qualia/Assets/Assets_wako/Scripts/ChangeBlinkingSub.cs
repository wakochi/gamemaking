using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBlinkingSub : MonoBehaviour
{
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
            this.gameObject.SetActive (false);
            Invoke(nameof(DelayMethod), 10f);
        }
    }

    void DelayMethod()
    {
        this.gameObject.SetActive (true);
    }

}
