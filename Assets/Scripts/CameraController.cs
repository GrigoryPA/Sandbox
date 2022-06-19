using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public Transform cameraCenterTransform;
    public float sensitivity = 100;
    public Vector2 minMaxAngleY = Vector2.one;
    public float maxAngleX = 180;
    public Vector3 centerCameraMod2 = new Vector3(0, 1, 0);
    [Range (0.1f, 3.0f)]
    public float mouseSensitivityX = 1.0f;
    [Range (0.1f, 2)]
    public float mouseSensitivityY = 1.0f;



    // Start is called before the first frame update
    void Start()
    {
        SetStartPositionCameraMod(CurrentSettings.cameraMod);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CurrentSettings.UpdateCameraMod();
            SetStartPositionCameraMod(CurrentSettings.cameraMod);
        }

        RotateCameraMod(CurrentSettings.cameraMod);
    }

    void SetStartPositionCameraMod(int cameraMod)
    {
        switch (cameraMod)
        {
            case 0:
                cameraCenterTransform.position = playerTransform.position;
                transform.localPosition = offset;
                transform.localEulerAngles = new Vector3(25, 0, 0);
                break;

            case 1:
                cameraCenterTransform.position = playerTransform.position + centerCameraMod2;
                transform.localPosition = new Vector3(0.0f, 0.0f, offset.z);
                transform.localRotation = Quaternion.identity;
                Cursor.lockState = CursorLockMode.None;
                break;

            default:
                cameraCenterTransform.position = playerTransform.position;
                transform.localPosition = offset;
                transform.localRotation = Quaternion.AngleAxis(25, transform.right);
                break;
        }
    }
    void RotateCameraMod(int cameraMod)
    {
        switch (cameraMod)
        {
            case 0:
                cameraCenterTransform.position = playerTransform.position;
                cameraCenterTransform.rotation = Quaternion.AngleAxis(playerTransform.eulerAngles.y, Vector3.up);
                break;

            case 1:
                cameraCenterTransform.position = playerTransform.position + centerCameraMod2;
                Vector3 MousePos = Input.mousePosition;

                float newAngleX = mouseSensitivityX * ((MousePos.x / Screen.width) * (2 * maxAngleX) - maxAngleX);

                float newAngleY = -mouseSensitivityY * (((MousePos.y) / Screen.height) * (minMaxAngleY.x + minMaxAngleY.y) - minMaxAngleY.y);
                newAngleY = (newAngleY <= minMaxAngleY.y && newAngleY >= minMaxAngleY.x)
                    ? newAngleY
                    : (newAngleY > minMaxAngleY.y)
                    ? minMaxAngleY.y
                    : minMaxAngleY.x;


                cameraCenterTransform.eulerAngles = new Vector3(newAngleY, newAngleX, 0.0f);

                break;

            default:
                cameraCenterTransform.position = playerTransform.localPosition;
                cameraCenterTransform.eulerAngles = new Vector3(0, playerTransform.eulerAngles.y, 0);
                break;
        }
    }
}
