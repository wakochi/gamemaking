using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    // ボタンが押された場合、今回呼び出される関数
    public void OnClickEye()
    {
        SceneManager.LoadScene("EyeScene", LoadSceneMode.Single);
    }

    public void OnClickSong()
    {
        SceneManager.LoadScene("SongScene", LoadSceneMode.Single);
    }

}
