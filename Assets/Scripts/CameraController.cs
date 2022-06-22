using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraCenterTransform;
    public Vector3 cameraOffsetMod1;
    public Transform cameraPositionMod2;
    public Transform cameraPositionMod3;
    [Space (15)]
    public float sensitivity = 100;
    public Vector2 minMaxAngleY = Vector2.one;
    public float maxAngleX = 180;
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

    private void SetStartPositionCameraMod(int cameraMod)
    {
        switch (cameraMod)
        {
            case 0:
                cameraCenterTransform.position = playerTransform.position;
                transform.localPosition = cameraOffsetMod1;
                transform.localEulerAngles = new Vector3(25, 0, 0);
                break;

            case 1:
                cameraCenterTransform.position = cameraPositionMod2.position;
                transform.localPosition = new Vector3(0.0f, 0.0f, cameraOffsetMod1.z);
                transform.localRotation = Quaternion.identity;
                Cursor.lockState = CursorLockMode.None;
                break;

            case 2:
                cameraCenterTransform.position = cameraPositionMod3.position;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                Cursor.lockState = CursorLockMode.None;
                break;

            case 3:
                cameraCenterTransform.position = cameraPositionMod3.position;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                Cursor.lockState = CursorLockMode.None;
                break;

            default:
                print("Default camera SetStartPositionCameraMod()");
                cameraCenterTransform.position = playerTransform.position;
                transform.localPosition = cameraOffsetMod1;
                transform.localEulerAngles = new Vector3(25, 0, 0);
                break;
        }
    }
    private void RotateCameraMod(int cameraMod)
    {
        switch (cameraMod)
        {
            case 0:
                cameraCenterTransform.position = playerTransform.position;
                cameraCenterTransform.rotation = Quaternion.AngleAxis(playerTransform.eulerAngles.y, Vector3.up);
                break;

            case 1:
                cameraCenterTransform.position = cameraPositionMod2.position;
                cameraCenterTransform.eulerAngles = MouseControllerEulerAngles(maxAngleX, minMaxAngleY.x, minMaxAngleY.y);

                break;

            case 2:
                cameraCenterTransform.position = cameraPositionMod3.position;
                cameraCenterTransform.rotation = Quaternion.AngleAxis(playerTransform.eulerAngles.y, Vector3.up);
                break;

            case 3:
                cameraCenterTransform.position = cameraPositionMod3.position;
                cameraCenterTransform.eulerAngles = MouseControllerEulerAngles(maxAngleX, -90, 60);
                break;

            default:
                cameraCenterTransform.position = playerTransform.position;
                cameraCenterTransform.rotation = Quaternion.AngleAxis(playerTransform.eulerAngles.y, Vector3.up);
                break;
        }
    }

    private Vector3 MouseControllerEulerAngles(float maxAngleX, float minAngleY, float maxAngleY)
    {
        Vector3 MousePos = Input.mousePosition;

        float newAngleX = mouseSensitivityX * ((MousePos.x / Screen.width) * (2 * maxAngleX) - maxAngleX);

        float newAngleY = -mouseSensitivityY * (((MousePos.y) / Screen.height) * (maxAngleY - minAngleY) - maxAngleY);
        newAngleY = (newAngleY <= maxAngleY && newAngleY >= minAngleY)
            ? newAngleY
            : (newAngleY > maxAngleY)
            ? maxAngleY
            : minAngleY;


        return new Vector3(newAngleY, newAngleX, 0.0f);
    }
}
