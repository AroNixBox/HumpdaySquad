using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void LoadScene()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Nard_Leveldesign");
    }
    public void OnClickReturn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
