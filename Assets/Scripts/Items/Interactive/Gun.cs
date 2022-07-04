using System.Collections;
using UnityEngine;


public abstract class Gun : Item
{
    public GameObject shotsSource;
    public Transform fpsCameraTransform;
    public GameObject explosion;
    [Space]
    [Header ("Gun settings:")]
    public Vector3 offsetPosInHand = Vector3.zero;
    public LayerMask layerReceivingDamage;
    public float fireRate = 0.1f;
    public float maxShotDistace = 0.1f;
    public float damage = 50.0f;
    public float impactForce = 30.0f;
    public int magazinSize = 10;

    protected Vector3 rayOrigin;
    protected Vector3 rayDirection;
    protected float nextShotTime = 0.0f;
    protected int shotsCount = 10;
    protected Transform usePlaceTransform;

    public override Transform UsePlaceTransform { get => usePlaceTransform; set => usePlaceTransform = value; }
    public Vector3 RayOrigin { get => rayOrigin; set => rayOrigin = value; }
    public Vector3 RayDirection { get => rayDirection; set => rayDirection = value; }

    public abstract void Shot(); 

    protected void UpdateItemTransform()
    {
        transform.position = usePlaceTransform.position + offsetPosInHand;

        if (CurrentSettings.cameraMod > 1)
        {
            Vector3 cameraVector = fpsCameraTransform.eulerAngles;
            cameraVector.x *= -1;
            transform.eulerAngles = cameraVector + new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = usePlaceTransform.eulerAngles;
        }
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
