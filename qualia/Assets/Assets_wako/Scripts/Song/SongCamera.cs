using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class SongCamera : MonoBehaviour
{
    GameObject playerObj;
    SongPlayerController player;
    Transform playerTransform;
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<SongPlayerController>();
        playerTransform = playerObj.transform;
    }
    void LateUpdate()
    {
        MoveCamera();
    }
    void MoveCamera()
    {
        //横方向だけ追従
        transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
    }
}