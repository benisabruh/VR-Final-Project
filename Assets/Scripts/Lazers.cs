using UnityEngine;
using System.Collections.Generic;
using System;

public class Lazers : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Lazer prefab")]
    GameObject lazer;

    [SerializeField]
    [Tooltip("Lazer destination")]
    GameObject destination;

    [SerializeField]
    [Tooltip("Parent object of lazer sources")]
    GameObject sourcesParent;

    [SerializeField]
    [Tooltip("Parent object of lazer targets")]
    GameObject targetsParent;

    [NonSerialized]
    public bool done = false;

    Material lazerMaterial;
    Material unlitMaterial;

    void Start()
    {
        foreach (Transform child in sourcesParent.transform)
        {
            var lazer = Instantiate(this.lazer, Vector3.zero, Quaternion.identity);
            lazer.GetComponent<Lazer>().Init(child.position, destination);
            if (lazerMaterial == null) lazerMaterial = lazer.GetComponent<LineRenderer>().material;
        }
        unlitMaterial = targetsParent.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
    }

    void Update()
    {
        var hitObjects = new List<GameObject>();
        var hits = 0;

        for (var i = 0; i < sourcesParent.transform.childCount; i++)
        {
            var source = sourcesParent.transform.GetChild(i).transform;
            RaycastHit hit;
            var maxDistance = Vector3.Distance(source.position, destination.transform.position);
            var direction = (destination.transform.position - source.position).normalized;
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
                Debug.Log("All targets hit!");
            }
        }
        else if (done) done = false;
    }
}
