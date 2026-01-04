using UnityEngine;

public class RotateScale2D : MonoBehaviour
{
    void Update()
    {
        // Uniform scaling (same X and Y)
        transform.localScale = new Vector3(2f, 2f, 1f);

        // Rotation in 2D (Z axis)
        transform.Rotate(0f, 0f, 30f * Time.deltaTime);
    }
}
