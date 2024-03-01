using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    private Vector3 _playerCapsulePosition;
    private Quaternion _playerCameraRootRotation;

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

    private void SavePlayerState(Vector3 position, Quaternion rotation)
    {
        _playerCapsulePosition = position;
        _playerCameraRootRotation = rotation;
    }

    public (Vector3 position, Quaternion rotation) LoadPlayerState()
    {
        Vector3 playerCapsulePosition = Vector3.zero;
        Quaternion playerCameraRootRotation = Quaternion.identity;

        if (_playerCapsulePosition != Vector3.zero && _playerCameraRootRotation != Quaternion.identity)
        {
            playerCapsulePosition = _playerCapsulePosition;
            playerCameraRootRotation = _playerCameraRootRotation;
        }

        return (playerCapsulePosition, playerCameraRootRotation);
    }

    
    public void ChangeScene(Vector3 position, Quaternion rotation, string sceneName)
    {
        SavePlayerState(position, rotation);
        StartCoroutine(ChangeSceneCoroutine(sceneName));
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        //Loading Transition
        
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        //Loading Transition
    }
}