using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using UnityEngine.SceneManagement;

public class Winning : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         // Previously this disabled the GameObject on Start which hid the staff.
         // Commented out so the object remains active and visible.
         // gameObject.SetActive(false);
    }
    void Awake(){
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab == null){
            Debug.Log("Error, no grab interactable");
        }
    }
    void OnEnable(){
        if (grab != null){
            grab.selectEntered.AddListener(OnGrab);
        }
    }
    void OnDisable(){
        if (grab != null){
            grab.selectEntered.RemoveListener(OnGrab);
        }
    }
    void OnGrab(SelectEnterEventArgs args){
        SceneManager.LoadScene("VR Room");
        grab.selectEntered.RemoveListener(OnGrab);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(1f,1f,1f));
    }
    public static void win(GameObject obj){
        obj.gameObject.SetActive(true);
    }
}
