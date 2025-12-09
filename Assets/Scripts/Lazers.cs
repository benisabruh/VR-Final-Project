using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class Lazers : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Lazer prefab")]
    GameObject lazer;

    [SerializeField]
    [Tooltip("Lazer destination")]
    GameObject[] destinations;

    [SerializeField]
    [Tooltip("Parent object of lazer sources")]
    GameObject[] sourcesParents;

    [SerializeField]
    [Tooltip("Parent object of lazer targets")]
    GameObject targetsParent;

    [NonSerialized]
    public bool done = false;

    [NonSerialized]
    public UnityEvent solved = new UnityEvent();

    [NonSerialized]
    public Material lazerMaterial;
    [NonSerialized]
    public Material unlitMaterial;

    void OnValidate()
    {

        if (destinations.Length != sourcesParents.Length)
        {
            Debug.LogError("Destinations and sources must match. Sources will be truncated.");
            Array.Resize(ref sourcesParents, destinations.Length);
        }
    }

    void Start()
    {
        for (var i = 0; i < destinations.Length; i++)
            foreach (Transform child in sourcesParents[i].transform)
            {
                var lazer = Instantiate(this.lazer, Vector3.zero, Quaternion.identity);
                lazer.GetComponent<Lazer>().Init(child.position, destinations[i]);
                if (lazerMaterial == null) lazerMaterial = lazer.GetComponent<LineRenderer>().material;
            }
        unlitMaterial = targetsParent.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
    }

    void Update()
    {
        var hitObjects = new List<GameObject>();
        var hits = 0;

        for (var i = 0; i < destinations.Length; i++)
            foreach (Transform source in sourcesParents[i].transform)
            {
                RaycastHit hit;
                var maxDistance = Vector3.Distance(source.position, destinations[i].transform.position);
                var direction = (destinations[i].transform.position - source.position).normalized;
                if (Physics.Raycast(source.position, direction, out hit, maxDistance) && hit.collider.tag == "Target")
                {
                    hit.collider.gameObject.GetComponent<Renderer>().material = lazerMaterial;
                    hitObjects.Add(hit.collider.gameObject);
                    hits++;
                }
            }

        var missed = new List<GameObject>();
        foreach (Transform target in targetsParent.transform)
        {
            if (!hitObjects.Contains(target.gameObject)) missed.Add(target.gameObject);
        }
        foreach (GameObject missedObject in missed)
        {
            missedObject.GetComponent<Renderer>().material = unlitMaterial;
        }

        if (hits == targetsParent.transform.childCount)
        {
            if (!done)
            {
                done = true;
                solved.Invoke();
            }
        }
        else if (done) { done = false; solved.Invoke(); }
    }
}
