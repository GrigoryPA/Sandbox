using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    LaserGun,
    SimpleGunBulletGeneration,
    SimpleGunBulletTeleportation
}

public class Gun : Item
{
    private Animator thisAnimator;
    private bool isFire = false;
    private bool isShotting = false;
    private Vector3 rayOrigin;
    private Vector3 rayDirection;
    public GameObject shotsSource;
    public Camera fpsCamera;
    [Space]
    [Header ("Gun settings:")]
    public GunType gunType = GunType.LaserGun;
    public LayerMask layerReceivingDamage;
    public float nextShotTime = 0.0f;
    public float fireRate = 0.1f;
    public float shotDurationForSeconds = 1;
    public float maxShotDistace = 0.1f;

    private void Start()
    {
        thisAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isUsing && isFire)
        {
            if (nextShotTime <= Time.time || isShotting)
            {
                nextShotTime = isShotting ? Time.time + fireRate : nextShotTime;

                switch (gunType)
                {
                    case GunType.LaserGun:
                        RaycastHit hit;
                        LineRenderer lineLaser = shotsSource.GetComponent<LineRenderer>();
                        lineLaser.SetPosition(0, shotsSource.transform.position);
                        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxShotDistace, layerReceivingDamage))
                        {
                            lineLaser.SetPosition(1, hit.point);

                            Color past = hit.transform.gameObject.GetComponent<MeshRenderer>().material.color;
                            hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = new Color(past.r + 0.1f, past.g, past.b);
                        }
                        else
                        {
                            lineLaser.SetPosition(1, shotsSource.transform.position + shotsSource.transform.forward * maxShotDistace);
                        }

                        if (!isShotting)
                        { 
                            StartCoroutine(LaserShot(lineLaser)); 
                        }
                        break;

                    default:
                        print("Called default gun type");
                        break;
                }
            }
        }
         
        if (isUsing)
        {
            if (CurrentSettings.cameraMod > 1)
            {
                Vector3 cameraVector = fpsCamera.transform.eulerAngles;
                cameraVector.x *= -1;
                transform.eulerAngles = cameraVector + new Vector3(0, 180, 0) ;
            }
            else
            {
                transform.eulerAngles = Vector3.forward * 90;
            }
        }
    }

    IEnumerator LaserShot(LineRenderer lineLaser)
    {
        isShotting = true;
        lineLaser.enabled = true;
        
        yield return new WaitForSeconds(shotDurationForSeconds);

        isShotting = false;
        lineLaser.enabled = false;
    }

    public void Fire(Vector3 originForRaycast, Vector3 directionForRaycast)
    {
        rayOrigin = originForRaycast;
        rayDirection = directionForRaycast;
        isFire = !isFire;
        nextShotTime = Time.time;
    }

    public override bool Interaction()
    {
        isUsing = !isUsing;
        this.GetComponent<Collider>().isTrigger = isUsing;
        if (isUsing)
        {
            transform.position = Vector3.zero;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }

        return true;
    }
}
