using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestVibration : MonoBehaviour
{
    bool flg = true;

    private void Start()
    {
       
    }
    // Start is called before the first frame update
    /*private IEnumerator Start()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.5f, 1.0f);
            yield return new WaitForSeconds(3.0f);
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }*/

    private IEnumerator Periodically() //0.1•b‚²‚Æ‚É¬‚İ‚ÉƒRƒ“ƒgƒ[ƒ‰‚ğU“®
    {
        Gamepad gamepad = Gamepad.current;
        int cnt = 0;
       
        while (flg)
        {
            if (gamepad != null)
            {
                Debug.Log("While");
                gamepad.SetMotorSpeeds(0.5f, 1.0f);
                yield return new WaitForSeconds(0.1f);
                gamepad.SetMotorSpeeds(0.0f, 0.0f);
                yield return new WaitForSeconds(0.1f);
                cnt++;
                if (cnt > 20) flg = false;
            }
        }
        yield return 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(1 / Time.deltaTime);
        if (Input.GetKeyDown("joystick button 0"))
        {
            StartCoroutine("Periodically");
        }
        if (Input.GetKeyDown("joystick button 1"))
        {
            StopCoroutine("Periodically");
        }

        Gamepad gamepad = Gamepad.current;
        float x = 0;
        x = Mathf.Abs(Mathf.Sin(Time.time)); //Œo‰ß‚ğæ“¾‚µ‚Äsin‚ÅU“®
        gamepad.SetMotorSpeeds(x, x);

    }
}
