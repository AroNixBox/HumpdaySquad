using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    [SerializeField] private SceneField preLevel;
    [SerializeField] private SceneField postLevel;
    private Vector3 _playerCapsulePosition;
    private Quaternion _playerCapsuleRotation;
    private Quaternion _playerCameraRootRotation;

    private int CurrentSceneIndex => UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SavePlayerState(Vector3 position, Quaternion playerCameraRootRotation, Quaternion playerCapsuleRotation)
    {
        _playerCapsulePosition = position;
        _playerCameraRootRotation = playerCameraRootRotation;
        _playerCapsuleRotation = playerCapsuleRotation;
    }

    public (Vector3 playerCapsulePosition, Quaternion playerCameraRootRotation, Quaternion playerCapsuleRotation) LoadPlayerState()
    {
        Vector3 playerCapsulePosition = Vector3.zero;
        Quaternion playerCameraRootRotation = Quaternion.identity;
        Quaternion playerCapsuleRotation = Quaternion.identity;

        if (_playerCapsulePosition != Vector3.zero && _playerCameraRootRotation != Quaternion.identity && _playerCapsuleRotation != Quaternion.identity)
        {
            playerCapsulePosition = _playerCapsulePosition;
            playerCameraRootRotation = _playerCameraRootRotation;
            playerCapsuleRotation = _playerCapsuleRotation;
        }

        return (playerCapsulePosition, playerCameraRootRotation, playerCapsuleRotation);
    }

    
    public void ChangeScene(Vector3 playerCapsulePosition, Quaternion playerCameraRootRotation, Quaternion playerCapsuleRotation)
    {
        SavePlayerState(playerCapsulePosition, playerCameraRootRotation, playerCapsuleRotation);
        
        switch (CurrentSceneIndex)
        {
            case 0:
                StartCoroutine(ChangeSceneCoroutine(postLevel.SceneName));
                break;
            case 1:
                StartCoroutine(ChangeSceneCoroutine(preLevel.SceneName));
                break;
        }
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        //Loading Transition
        
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        //Loading Transition
    }
}