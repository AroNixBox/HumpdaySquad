using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NPCBehavior : MonoBehaviour, ITalkable
{
    [SerializeField] private TextMeshProUGUI chatBubbleTMP;
    
    private int _currentChatIndex;
    [SerializeField, TextArea(3, 10)] private string[] chat;
    private NPCHeadLookAt _npcHeadLookAt;

    private void Awake()
    {
        _npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform playerHeadTransform)
    {
        Debug.Log("Interacting with NPC");
        
        //ANimator Talk
        
        chatBubbleTMP.gameObject.SetActive(true);
        _npcHeadLookAt.LookAtPosition(playerHeadTransform.position);
        Talk();
    }

    private void Talk()
    {
        if (_currentChatIndex == chat.Length - 1)
        {
            //last chat
            chatBubbleTMP.text = "You got " + Clipboard.Instance.GetComponentInChildren<UIClipboard>().DocumentPoints() + " out of 6 points, thanks for playing!";
            StartCoroutine(RestartGame());
            return;
        }
        chatBubbleTMP.text = chat[_currentChatIndex];
        _currentChatIndex++;
    }
    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.Instance.RestartGame();
    }
}
