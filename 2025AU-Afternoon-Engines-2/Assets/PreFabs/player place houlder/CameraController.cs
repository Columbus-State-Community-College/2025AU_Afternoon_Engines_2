using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float xSensitivity = 4;
    public float ySensitivity = 4;
    public bool invertX = false;
    public bool invertY = true;
    private int xInversion = 1;
    private int yInversion = 1;

    [Header("Y Angle Settings")]
    public float yMinAngle = 0;
    public float yMaxAngle = 89;

    [Header("Player Settings")]
    public float followDistance = 5;
    public Transform player;

    private float xRotation;
    private float yRotation;

    void LateUpdate()
    {
        if (invertX)
        {
            xInversion = -1;
        }

        if (invertY)
        {
            yInversion = -1;
        }

        xRotation += xInversion * Input.GetAxisRaw("Mouse X") * xSensitivity;
        yRotation += yInversion * Input.GetAxisRaw("Mouse Y") * ySensitivity;

        yRotation = Mathf.Clamp(yRotation, yMinAngle, yMaxAngle);

        Vector3 direction = new Vector3(0, 0, -followDistance);
        Quaternion rotation = Quaternion.Euler(yRotation, xRotation, 0);

        transform.position = player.position + rotation * direction;

        transform.LookAt(player.position);

        player.transform.rotation = Quaternion.Euler(player.transform.eulerAngles.x, transform.rotation.eulerAngles.y, player.transform.eulerAngles.z);
    }
}
