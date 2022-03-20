using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    // ボタンが押された場合、今回呼び出される関数
    public void OnClickEye()
    {
        SceneManager.LoadScene("UseVisualStage", LoadSceneMode.Single);
    }

    public void OnClickSong()
    {
        SceneManager.LoadScene("UseHearingStage", LoadSceneMode.Single);
    }

    public void OnClickRotate()
    {
        SceneManager.LoadScene("UseRotationStage", LoadSceneMode.Single);
    }

    public void OnClickVibration()
    {
        SceneManager.LoadScene("UseVibrationStage", LoadSceneMode.Single);
    }

    public void OnClickGameEnd()
    {
        Application.Quit();
    }

}
