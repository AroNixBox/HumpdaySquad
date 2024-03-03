using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class ChangeScene : MonoBehaviour
{
    public void OnClickReturn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    public void Quit() 
    { 
        Application.Quit();
    }
}
