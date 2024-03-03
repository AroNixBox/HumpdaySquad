using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCHeadLookAt : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] private Transform headLookAtTransform;
    private bool _isLookingAtPosition;
    
    private void Update()
    {
        float targetweight = _isLookingAtPosition ? 1.0f : 0.0f;
        float lerpSpeed = 2f;
        rig.weight = Mathf.Lerp(rig.weight, targetweight, Time.deltaTime * lerpSpeed);
    }
    
    public void LookAtPosition(Vector3 lookAtPosition)
    {
        _isLookingAtPosition = true;
        headLookAtTransform.position = lookAtPosition;
        
        
        //TODO: Do this when conversation ends
        StartCoroutine(ResetLookAtPosition());
    }

    private IEnumerator ResetLookAtPosition()
    {
        yield return new WaitForSeconds(2f);
        _isLookingAtPosition = false;
    }
}
