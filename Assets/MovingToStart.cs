using UnityEngine;
using System.Collections;

public class MovingToStart : MonoBehaviour
{

    Vector3 direction = new Vector3(-0.327f, 4f, -1.53f);


    //     public float speed = 1f;


    private Transform target;
    public GameObject ring;
    public GameObject key;
    // If true, attempt to force the assigned objects visible and enable renderers at Start
    public bool forceShowOnStart = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if (key == null)
        {
            Debug.LogWarning("MovingToStart: 'key' is not assigned in the Inspector.");
        }
        else
        {
            // Keep the key hidden until the player wins
            key.SetActive(false);
        }

        if (ring == null)
        {
            Debug.LogWarning("MovingToStart: 'ring' is not assigned in the Inspector.");
        }

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
        key.SetActive(true);
        if (col.tag == "wire")
        {
            
            StartCoroutine(MoveToStartCoroutine(ring));
        }
        else if (col.tag == "Winner")
        {
            Winning.win(key);
        }

    }
    IEnumerator MoveToStartCoroutine(GameObject obj)
    {
        Vector3 targetPosition = new Vector3(-0.327f, -0.106f, -1.53f);
        if (obj == null)
        {
            Debug.LogWarning("MoveToStartCoroutine called with null object");
            yield break;
        }

        var objCollider = obj.GetComponent<Collider>();
        if (objCollider != null)
            objCollider.enabled = false;
        else
            Debug.Log("MoveToStartCoroutine: object has no Collider to disable");

        // Ensure object is active before moving
        if (!obj.activeInHierarchy)
        {
            Debug.Log("MoveToStartCoroutine: object is inactive in hierarchy — enabling it for movement.");
            obj.SetActive(true);
        }

        obj.transform.eulerAngles = new Vector3(90f, 0f, 0f);

        while (obj.transform.position != targetPosition /*|| this.transform.rotation != targetRotation*/)
        {
            float step = 2 * Time.deltaTime;
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, step);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            yield return null;
        }
        Debug.Log("Movement finished");
        if (objCollider != null)
            objCollider.enabled = true;
        // GetComponent<Rigidbody>().enabled = true;
    }

    // Helper: log useful info and optionally make the object visible
    private void LogAndForceShow(GameObject obj, string name)
    {
        if (obj == null)
        {
            Debug.LogWarningFormat("MovingToStart: '{0}' is null.", name);
            return;
        }

        string parentPath = obj.transform.parent == null ? "(root)" : obj.transform.parent.name;
        Debug.LogFormat("{0}: assigned, activeSelf={1}, activeInHierarchy={2}, pos={3}, scale={4}, parent={5}",
            name, obj.activeSelf, obj.activeInHierarchy, obj.transform.position, obj.transform.localScale, parentPath);

        var meshR = obj.GetComponent<MeshRenderer>();
        var skinned = obj.GetComponent<SkinnedMeshRenderer>();
        if (meshR == null && skinned == null)
        {
            Debug.LogFormat("{0}: no MeshRenderer/SkinnedMeshRenderer found.", name);
        }
        else
        {
            if (meshR != null)
                Debug.LogFormat("{0}: MeshRenderer.enabled={1}", name, meshR.enabled);
            if (skinned != null)
                Debug.LogFormat("{0}: SkinnedMeshRenderer.enabled={1}", name, skinned.enabled);
        }

        if (forceShowOnStart)
        {
            if (!obj.activeSelf)
                obj.SetActive(true);

            if (meshR != null && !meshR.enabled)
                meshR.enabled = true;
            if (skinned != null && !skinned.enabled)
                skinned.enabled = true;

            Debug.LogFormat("{0}: forceShowOnStart applied — activeSelf={1}", name, obj.activeSelf);
        }
}

}
