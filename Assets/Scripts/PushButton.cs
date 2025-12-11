using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class PushButton : MonoBehaviour

{
    public float deadTime = 1.0f;
private bool _deadTimeActive = false;
public UnityEvent onPressed, onReleased;

private void OnTriggerEnter(Collider other)
    {
         if(other.tag == "Button" && !_deadTimeActive)
        {
            onPressed?.Invoke();
        }
        
    }

    // Update is called once per frame
    

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
