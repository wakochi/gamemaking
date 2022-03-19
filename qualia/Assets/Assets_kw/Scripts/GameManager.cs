using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;
    [SerializeField] Text scoreText;
    [SerializeField] Slider rotationCoolTimeSlider;

    const int MAX_SCORE = 9999;
    int score = 0;
    public bool canRotateflg = true;

    public void Addscore(int scoreval)
    {
        score += scoreval;
        if(score > MAX_SCORE)
        {
            score = MAX_SCORE;
        }
        scoreText.text = score.ToString();

    }
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString();
        //rotationCoolTimeSlider.value = 1;
        // rotationCoolTimeSlider = GameObject.Find("RotationCoolTimeSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "UseRotationStage")
        {
            RotationCoolTimeCounterUp();
        }

    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        Invoke("RestartScene", 1.5f);
    }

    public void GameClear()
    {
        gameClearText.SetActive(true);
        Invoke("RestartScene", 1.5f);
    }

    public void RestartScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
    public void RotationCoolTimeCounterUp()
    {
        rotationCoolTimeSlider.value += 0.7f * Time.deltaTime;
        if (rotationCoolTimeSlider.value == 1) canRotateflg = true;
    }

    public IEnumerator RotationCoolTimeCounterZero()
    {
        if (SceneManager.GetActiveScene().name == "UseRotationStage")
        {
            yield return new WaitForSeconds(1.5f);
            rotationCoolTimeSlider.value = 0;
            canRotateflg = false;
        }   
    }
}
