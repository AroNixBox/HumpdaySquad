using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private SceneField gamePlayScene;
    [SerializeField] private SceneField titleScene;
    public void LoadScene()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(gamePlayScene.SceneName);
    }
    public void OnClickReturn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(titleScene.SceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
