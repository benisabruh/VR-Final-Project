using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
   public float deadTime = 1.0f;
//Bool used to lock down button during its set dead time 
private bool _deadTimeActive = false;
//public Unity Events we can use in the editor and tie other functions to. 
public UnityEvent onPressed, onReleased;

private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Button" && !_deadTimeActive)
        {
            onPressed?.Invoke();
        }
    }

private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Button" && !_deadTimeActive)
        {
            onReleased?.Invoke();
            StartCoroutine(WaitForDeadTime());
        }
    }

    IEnumerator WaitForDeadTime()
    {
        _deadTimeActive = true;
        yield return new WaitForSeconds(deadTime);
         _deadTimeActive = false;
    }
}
