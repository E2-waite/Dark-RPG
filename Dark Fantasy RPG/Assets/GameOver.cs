using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOver : MonoSingleton<GameOver>
{
    public int restartTime = 20;
    public Text gameOverText, timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = restartTime.ToString();
    }

    public void Display()
    {
        Debug.Log("DISPLAY");
        gameOverText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        StartCoroutine(Timer());
    }


    IEnumerator Timer()
    {
        float timeLeft = restartTime;
        yield return new WaitForSeconds(1);
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = ((int)timeLeft).ToString();
            yield return null;
        }
        SceneManager.LoadScene(0);
    }

}
