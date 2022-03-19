using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationBlockManager : MonoBehaviour
{

    Rigidbody2D rigidbody2DVibrationBlock;

    float UpPower = 2000;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2DVibrationBlock = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RisingBlock()
    {
        rigidbody2DVibrationBlock.AddForce(Vector2.up * UpPower);
    }
}
