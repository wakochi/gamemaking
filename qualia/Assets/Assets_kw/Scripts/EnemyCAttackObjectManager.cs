using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCAttackObjectManager : MonoBehaviour
{
    [Header("スピード")] public float speed = -3.0f;
    [Header("最大移動距離")] public float maxDistance = 5.0f;
    private Rigidbody2D rb;
    private Vector3 defaultPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.Log("設定が足りません");
            Destroy(this.gameObject);
        }
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float d = Vector3.Distance(transform.position, defaultPos);

        //最大移動距離を超えている
        if (d > maxDistance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
    
}
