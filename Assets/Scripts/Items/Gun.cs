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
    public GameObject shotsSource;
    public bool isFire = false;
    [Space]
    [Header ("Gun settings:")]
    public GunType gunType = GunType.LaserGun;
    public float timeBtwShots = 0.0f;
    public float startTimeBtwShot = 0.1f;
    public float maxShotDistace = 0.1f;

    private void Start()
    {
        thisAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isUsed && isFire)
        {
            if (timeBtwShots <= 0.0f)
            {
                timeBtwShots = startTimeBtwShot;

                switch (gunType)
                {
                    case GunType.LaserGun:
                        RaycastHit hit;
                        if (Physics.Raycast(shotsSource.transform.position, shotsSource.transform.forward, out hit, maxShotDistace))
                        {
                            shotsSource.GetComponent<LineRenderer>().SetPositions(new Vector3[2] { shotsSource.transform.position, hit.point });
                            Destroy(hit.transform.gameObject);
                        }
                        break;

                    default:
                        print("Called default gun type");
                        break;
                }
            }
            else 
            {
                timeBtwShots -= Time.deltaTime;
            }
        }
    }

    public void Fire()
    {
        isFire = !isFire;
        timeBtwShots = 0.0f;
    }

    public override void Interaction()
    {
        isUsed = !isUsed;
        thisAnimator.SetBool("Active", isUsed);
    }
}
