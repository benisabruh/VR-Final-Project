using UnityEngine;

public class Rotater : MonoBehaviour
{
    float num;
    float num2;
    float num3;
    // Start is called before the first frame update
    void Start()
    {
        num = Random.Range(-180f, 180f);
        num2 = Random.Range(-180f, 180f);
        num3 = Random.Range(-180f, 180f);

    }

    // create a random

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(num, num2, num3) * Time.deltaTime);

    }

}
