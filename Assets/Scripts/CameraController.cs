using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float cameraSpeed;
    public Transform cameraCenterTransform;
    public int cameraMod = 1; //0 - camera is controlled by the player, 1 - camera is controlled by the mouse  
    public float sensitivity = 100; 

    // Start is called before the first frame update
    void Start()
    {
        transform.position = offset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            cameraMod = (cameraMod + 1) % 2;
        }

        switch (cameraMod)
        {
            case 0:
                cameraCenterTransform.position = playerTransform.localPosition;
                cameraCenterTransform.eulerAngles = new Vector3(0, playerTransform.eulerAngles.y, 0);
                break;

            case 1:
                cameraCenterTransform.position = playerTransform.localPosition;
                Vector3 MousePos = Input.mousePosition;
                float myAngle = (MousePos.x * 2 / Screen.width) * 90 - 35;
                //cameraCenterTransform.transform.RotateAround(cameraCenterTransform.position, Vector3.up, myAngle);
                myAngle = (MousePos.y * 2 / Screen.height) * 90 - 35;
                cameraCenterTransform.transform.rotation = Quaternion.AngleAxis(myAngle, cameraCenterTransform.right);
                break;

            default:
                cameraCenterTransform.position = playerTransform.localPosition;
                cameraCenterTransform.eulerAngles = new Vector3(0, playerTransform.eulerAngles.y, 0);
                break;
        }
    }
}
