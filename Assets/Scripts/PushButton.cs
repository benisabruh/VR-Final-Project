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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
