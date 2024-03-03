using System;
using System.Collections;
using Nix_Scripts.SceneSwitch;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCBehavior : MonoBehaviour, ITalkable
{
    [Header("References")]
    [SerializeField] private AudioClip[] talkClips;
    [SerializeField] private TextMeshProUGUI chatBubbleTMP;
    [SerializeField] private GameObject chatParentImage;
    
    [Header("Values")]
    [SerializeField, TextArea(3, 10)] private string[] chat;
    private int _currentChatIndex;
    private NPCHeadLookAt _npcHeadLookAt;
    
    private Coroutine _typingCoroutine;
    private bool _isTyping;

    private AudioSource _audioSource;
    private Animator _animator;
    private static readonly int TalkAnim_1 = Animator.StringToHash("Talk_1");
    private static readonly int TalkAnim_2 = Animator.StringToHash("Talk_2");
    private static readonly int TalkAnim_3 = Animator.StringToHash("Talk_3");



    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform playerHeadTransform)
    {
        //ANimator Talk
        _animator.SetTrigger(GetRandomAnim());
        chatParentImage.SetActive(true);
        _npcHeadLookAt.LookAtPosition(playerHeadTransform);
        Talk();
    }

    private void Talk()
    {
        _audioSource.PlayOneShot(talkClips[_currentChatIndex] != null
            ? talkClips[_currentChatIndex]
            : talkClips[Random.Range(0, talkClips.Length)]);
        
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
    private int GetRandomAnim()
    {
        int randomIndex = Random.Range(0, 3);
        switch (randomIndex)
        {
            case 0:
                return TalkAnim_1;
            case 1:
                return TalkAnim_2;
            case 2:
                return TalkAnim_3;
            default:
                return TalkAnim_1;
        }
    }
    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.Instance.RestartGame();
    }
}
