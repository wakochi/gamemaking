using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaosManager : MonoBehaviour
{
    [SerializeField] GameObject enemyDeathEffect;

    GameManager gameManager;
    //SpriteRenderer sr = null;

    float alpha = 0.1f;
    float gamma = 0.1f;
    float omega = 8.0f;

    Rigidbody2D rigidbody2DEnemyChaos;
    private float originCurrentPositionDifferenceThreshold = 2.0f; //最初の位置と現在の位置の差の閾値
    private Vector3 originPosition = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //sr = GetComponent<SpriteRenderer>();
        rigidbody2DEnemyChaos = GetComponent<Rigidbody2D>();
        originPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
        //rigidbody2DEnemyChaos.velocity = new Vector2(speed_x, speed_y);
        //Vector3 pos = transform.position;
        //transform.position = new Vector3(transform.position.x - alpha * Mathf.Pow((transform.position.x - originPosition.x), 3) - gamma * omega * Mathf.Cos(omega * transform.position.x),
        //                                 transform.position.y - alpha * Mathf.Pow((transform.position.y - originPosition.y), 3) - gamma * omega * Mathf.Cos(omega * transform.position.y), 
        //                                 pos.z);
        ChaosUpdate();
    }


    private void ChaosUpdate()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(transform.position.x - alpha * Mathf.Pow((transform.position.x - originPosition.x), 3) - gamma * omega * Mathf.Cos(omega * transform.position.x),
                                         transform.position.y - alpha * Mathf.Pow((transform.position.y - originPosition.y), 3) - gamma * omega * Mathf.Cos(omega * transform.position.y),
                                         pos.z);

        // enemyposition ------------閾値より大きい距離----------- originPosition　左の位置関係の場合、閾値まで位置を戻す
        if (originPosition.x - transform.position.x > originCurrentPositionDifferenceThreshold)
        {
            Vector3 pos_local = transform.position;
            transform.position = new Vector3(originPosition.x - originCurrentPositionDifferenceThreshold, pos_local.y, pos_local.z);
        }
        // originPosition ------------距離が閾値より大きい----------- enemyposition　左の位置関係の場合、閾値まで位置を戻す
        else if (transform.position.x - originPosition.x > originCurrentPositionDifferenceThreshold)
        {
            Vector3 pos_local = transform.position;
            transform.position = new Vector3(originPosition.x + originCurrentPositionDifferenceThreshold, pos_local.y, pos_local.z);
        }
        // enemyposition ------------閾値より大きい距離----------- originPosition　左の位置関係の場合、閾値まで位置を戻す
        if (originPosition.y - transform.position.y > originCurrentPositionDifferenceThreshold)
        {
            Vector3 pos_local = transform.position;
            transform.position = new Vector3(pos_local.x, originPosition.y - originCurrentPositionDifferenceThreshold, pos_local.z);
        }
        // originPosition ------------距離が閾値より大きい----------- enemyposition　左の位置関係の場合、閾値まで位置を戻す
        else if (transform.position.y - originPosition.y > originCurrentPositionDifferenceThreshold)
        {
            Vector3 pos_local = transform.position;
            transform.position = new Vector3(pos_local.x, originPosition.y + originCurrentPositionDifferenceThreshold, pos_local.z);
        }
    }

    public void DestroyEnemy()
    {
        gameManager.Addscore(100);
        Instantiate(enemyDeathEffect, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
