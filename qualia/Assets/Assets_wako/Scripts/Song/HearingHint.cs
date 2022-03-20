using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingHint : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;
    private bool firstFlag;
    [SerializeField] private AudioClip b1;
    [SerializeField] private AudioClip b2;
    [SerializeField] private AudioClip b3;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        firstFlag = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetHearingItem()
    {
        if (firstFlag)
        {
            GetComponent<Renderer>().material.color = new Color(255, 255, 255, 0);
            firstFlag = false;
            audioSource.PlayOneShot(b1);
            Debug.Log(b1.length);
            // オーディオクリップの再生が終了するまで
            Invoke(nameof(DelayMethod1), b1.length);
            Invoke(nameof(DelayMethod2), b1.length + b2.length);
            //Destroy(this.gameObject);
        }
    }
    void DelayMethod1()
    {
        audioSource.PlayOneShot(b2);
    }
    void DelayMethod2()
    {
        audioSource.PlayOneShot(b3);
    }


}
