using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun : Gun
{
    [Space]
    [Header("Bullet settings:")]
    public GameObject bullet;
    public float bulletForce;
    public bool alwaysExploseBullets = true;
    public float explosionScale = 1.5f;
    public float damageRadius = 0.5f;

    private void Update()
    {
        if (isUsing)
        {
            if (CurrentSettings.cameraMod > 1)
            {
                //зажата кнопка, есть патроны, перезарядка завершена
                if (Input.GetMouseButton(0) && shotsCount > 0 && nextShotTime <= Time.time)
                {
                    shotsCount--;
                    nextShotTime = Time.time + fireRate;
                    Shot();
                }

                if (Input.GetKey(KeyCode.R))
                {
                    shotsCount = magazinSize;
                }
            }

            UpdateItemTransform();
        }
    }

    public override void Shot()
    {
        RaycastHit hit; 
        GameObject newBullet = Instantiate(bullet, shotsSource.transform.position, shotsSource.transform.rotation);
        newBullet.GetComponent<Bullet>().parentGun = this;
        newBullet.GetComponent<Bullet>().isActive = true;
        if (Physics.Raycast(RayOrigin, RayDirection, out hit, maxShotDistace, layerReceivingDamage))
        {
            newBullet.GetComponent<Rigidbody>().AddForceAtPosition(shotsSource.transform.forward * bulletForce, hit.point);
        }
        else
        {
            newBullet.GetComponent<Rigidbody>().AddForceAtPosition(shotsSource.transform.forward * bulletForce, shotsSource.transform.position + (shotsSource.transform.forward) * maxShotDistace);
        }
    }
}
