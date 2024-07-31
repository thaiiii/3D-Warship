using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 targetPosition; // Vị trí mong muốn
    public float moveSpeed = 2.0f; // Tốc độ di chuyển

    void Start()
    {
        StartCoroutine(MoveToPosition(targetPosition));
    }

    IEnumerator MoveToPosition(Vector3 newPosition)
    {
        Vector3 currentPos = cameraTransform.position;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;

            cameraTransform.position = Vector3.Lerp(currentPos, newPosition, t);

            yield return null;
        }
    }
}
