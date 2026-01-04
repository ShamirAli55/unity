using UnityEngine;

public class SimpleRotateScale : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public float scaleFactor = 2.0f; 

    void Start()
    {
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime);
    }
}