using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Winning : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       gameObject.SetActive(false); 
    }
    void Awake(){
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab == null){
            Debug.Log("Error, no grab interactable");
        }
        else if (grab != null){
            grab.selectEntered.AddListener(OnGrab);
        }
    }
    void OnGrab(SelectEnterEventArgs args){
        //SceneManager.LoadScene("next");
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
