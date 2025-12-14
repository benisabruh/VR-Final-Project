using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MovingToStart : MonoBehaviour
{
    Vector3 direction = new Vector3(-0.327f, 4f, -1.53f);

    private Transform target;
    public GameObject ring;
    public GameObject key;

    public bool forceShowOnStart = true;

    // XR references
    private XRGrabInteractable grab;
    private XRInteractionManager interactionManager;
    
    //New code here🔥🔥🔥🔥🔥
    public GameObject handle;
    

    [System.Obsolete]
    void Start()
    {
        //New code here🔥🔥🔥🔥🔥
        if(handle.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>() == null)
        {
            Debug.LogWarning("Handle does not have XR interaction.");
        }
        if (key == null)
        {
            Debug.LogWarning("MovingToStart: 'key' is not assigned in the Inspector.");
        }
        else
        {
            key.SetActive(false);
        }

        if (ring == null)
        {
            Debug.LogWarning("MovingToStart: 'ring' is not assigned in the Inspector.");
            return;
        }

        grab = ring.GetComponent<XRGrabInteractable>();
        interactionManager = FindObjectOfType<XRInteractionManager>();

        if (grab == null)
            Debug.LogWarning("MovingToStart: XRGrabInteractable not found on ring.");
        if (interactionManager == null)
            Debug.LogWarning("MovingToStart: XRInteractionManager not found in scene.");

        Debug.Log("MovingToStart.Start() running");
        LogAndForceShow(ring, "ring");

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (GetComponent<Collider>() != null)
        {
            Debug.Log("This GameObject has a Collider");
        }
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("wire"))
        {
            // 🔥 FORCE DROP IF PLAYER IS HOLDING IT
            if (grab != null && grab.isSelected && interactionManager != null)
            {
                var interactor = grab.firstInteractorSelecting;

                interactionManager.SelectExit(
                    (IXRSelectInteractor)interactor,
                    (IXRSelectInteractable)grab
                );
            }
            //New code here🔥🔥🔥🔥🔥
            handle.gameObject.SetActive(false);
            ring.transform.position = new Vector3(-0.323f, 0.027f, 0.964f);
            ring.transform.eulerAngles = new Vector3(90f, 0f, 0f);
            handle.gameObject.SetActive(true);
        }
        else if (col.CompareTag("Winner"))
        {
            Winning.win(key);
        }
    }



    private void LogAndForceShow(GameObject obj, string name)
    {
        if (obj == null)
        {
            Debug.LogWarningFormat("MovingToStart: '{0}' is null.", name);
            return;
        }

        string parentPath = obj.transform.parent == null ? "(root)" : obj.transform.parent.name;
        Debug.LogFormat(
            "{0}: activeSelf={1}, activeInHierarchy={2}, pos={3}, scale={4}, parent={5}",
            name, obj.activeSelf, obj.activeInHierarchy,
            obj.transform.position, obj.transform.localScale, parentPath
        );

        var meshR = obj.GetComponent<MeshRenderer>();
        var skinned = obj.GetComponent<SkinnedMeshRenderer>();

        if (forceShowOnStart)
        {
            if (!obj.activeSelf)
                obj.SetActive(true);

            if (meshR != null && !meshR.enabled)
                meshR.enabled = true;

            if (skinned != null && !skinned.enabled)
                skinned.enabled = true;
        }
    }
}
