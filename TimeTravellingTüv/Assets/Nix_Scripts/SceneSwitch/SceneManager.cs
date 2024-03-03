using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    [SerializeField] private GameObject preLevel;
    [SerializeField] private GameObject postLevel;
    private int _currentLevel = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ChangeLevel()
    {
        switch (_currentLevel)
        {
            case 0:
                _currentLevel = 1;
                postLevel.SetActive(true);
                preLevel.SetActive(false);
                break;
            
            case 1:
                _currentLevel = 0;
                postLevel.SetActive(false);
                preLevel.SetActive(true);
                break;
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}