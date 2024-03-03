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
    
    private Coroutine _typingCoroutine;
    private bool _isTyping;

    private void Awake()
    {
        _npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform playerHeadTransform)
    {
        Debug.Log("Interacting with NPC");
        
        //ANimator Talk
        
        chatBubbleTMP.gameObject.SetActive(true);
        _npcHeadLookAt.LookAtPosition(playerHeadTransform);
        Talk();
    }

    private void Talk()
    {
        if (_currentChatIndex == chat.Length - 1)
        {
            //last chat
            StartCoroutine(TypeSentence("You got " +
                                        Clipboard.Instance.GetComponentInChildren<UIClipboard>().DocumentPoints() +
                                        " out of 6 points, thanks for playing!"));

            StartCoroutine(RestartGame());
            return;
        }
        _typingCoroutine = StartCoroutine(TypeSentence(chat[_currentChatIndex]));
        _currentChatIndex++;
    }
    private IEnumerator TypeSentence(string sentence)
    {
        if (_isTyping)
        {
            StopCoroutine(_typingCoroutine);
        }
        _isTyping = true;
        
        chatBubbleTMP.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            chatBubbleTMP.text += letter;
            yield return new WaitForSeconds(.05f);
        }
        
        _isTyping = false;
    }
    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.Instance.RestartGame();
    }
}
