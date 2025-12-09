using UnityEngine;

public class SolveLight : MonoBehaviour
{
    public Lazers lazers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lazers.solved.AddListener(OnSolved);
    }

    void OnSolved()
    {
        var renderer = GetComponent<Renderer>();
        if (lazers.done) renderer.material = lazers.lazerMaterial; else renderer.material = lazers.unlitMaterial;
    }
}
