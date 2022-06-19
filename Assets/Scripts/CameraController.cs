using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float cameraSpeed;
    public Transform cameraCenterTransform;
    public int cameraMod = 0; //0 - camera is controlled by the player, 1 - camera is controlled by the mouse  
    public float sensitivity = 100;
    public Vector2 minMaxAngleY = Vector2.one;
    public float maxAngleX = 180;
    public Vector3 centerCamraMod2 = new Vector3(0, 1, 0);

    private Quaternion rotationX, rotationY;


    // Start is called before the first frame update
    void Start()
    {
        SetStartPositionCameraMod(cameraMod);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            cameraMod = (cameraMod + 1) % 2;
            SetStartPositionCameraMod(cameraMod);
        }

        switch (cameraMod)
        {
            case 0:
                cameraCenterTransform.position = playerTransform.position;
                cameraCenterTransform.rotation = Quaternion.AngleAxis(playerTransform.eulerAngles.y, Vector3.up);
                break;

            case 1:
                cameraCenterTransform.position = playerTransform.position + centerCamraMod2;
                Vector3 MousePos = Input.mousePosition;

                float myAngle = (MousePos.x / Screen.width) * (2 * maxAngleX) - maxAngleX;
                rotationX = Quaternion.AngleAxis(myAngle, Vector3.up);

                
                myAngle = (MousePos.y / Screen.height) * (minMaxAngleY.x + minMaxAngleY.y) + minMaxAngleY.x;
                rotationY = (myAngle <= minMaxAngleY.y && myAngle >= minMaxAngleY.x)
                    ? Quaternion.AngleAxis(myAngle, cameraCenterTransform.right) 
                    : (myAngle > minMaxAngleY.y) 
                    ? Quaternion.AngleAxis(minMaxAngleY.y, cameraCenterTransform.right)
                    : Quaternion.AngleAxis(minMaxAngleY.x, cameraCenterTransform.right);

                cameraCenterTransform.rotation = (rotationY.normalized * rotationX.normalized).normalized;

                break;

            default:
                cameraCenterTransform.position = playerTransform.localPosition;
                cameraCenterTransform.eulerAngles = new Vector3(0, playerTransform.eulerAngles.y, 0);
                break;
        }
    }

    void SetStartPositionCameraMod(int cameraMod)
    {
        switch (cameraMod)
        {
            case 0:
                cameraCenterTransform.position = playerTransform.position;
                transform.localPosition = offset;
                transform.localRotation = Quaternion.AngleAxis(25, transform.right);
                break;

            case 1:
                cameraCenterTransform.position = playerTransform.position + centerCamraMod2;
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
}
