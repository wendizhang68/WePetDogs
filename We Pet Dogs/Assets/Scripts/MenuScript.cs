using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public Color origColor;

	public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    public void ChangeTextColor(Text text)
    {
        if (text.color == Color.white)
        {
            origColor = text.color;
            text.color = Color.black;
        }
        else
        {
            origColor = text.color;
            text.color = Color.white;
        }
    }

    public void ExitTextColor(Text text)
    {
        text.color = origColor;
    }
}
