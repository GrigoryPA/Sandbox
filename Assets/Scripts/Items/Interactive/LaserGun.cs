using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Gun
{
    [Space]
    [Header("Laser settings:")]
    public LineRenderer lineLaser;
    public float shotDuration = 1;
    [Range (0.0f, 1.0f)]
    public float damageRate = 1.0f;
    private float nextDamageTime = 0.0f;
    private bool isShotting = false;

    private void Update()
    {
        if (isUsing)
        {
            if (CurrentSettings.cameraMod > 1)
            {
                //зажата кнопка, есть патроны, (выстрел и перезарядка) завершены
                if (Input.GetMouseButton(0) && shotsCount > 0 && nextShotTime <= Time.time)
                {
                    shotsCount--;
                    nextShotTime = Time.time + fireRate + shotDuration;
                    Shot();
                }

                if (isShotting)
                {
                    UpdateLaser(nextDamageTime <= Time.time ? true : false);
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
        UpdateLaser(true);
        StartCoroutine(LaserShot(lineLaser));
    }

    private void UpdateLaser(bool isTakeDamage)
    {
        RaycastHit hit;
        lineLaser.SetPosition(0, shotsSource.transform.position);
        if (Physics.Raycast(RayOrigin, RayDirection, out hit, maxShotDistace, layerReceivingDamage))
        {
            lineLaser.SetPosition(1, hit.point);

            if (isTakeDamage)
            {
                Body body = hit.transform.gameObject.GetComponent<Body>();
                if (body != null)
                {
                    nextDamageTime = Time.time + damageRate;
                    body.TakeDamage(damage);
                }
            }

            GameObject explosionObj = Instantiate(explosion, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(explosionObj, explosionObj.GetComponent<ParticleSystem>().main.duration);
        }
        else
        {
            lineLaser.SetPosition(1, shotsSource.transform.position + shotsSource.transform.forward * maxShotDistace);
        }
    }

    private IEnumerator LaserShot(LineRenderer lineLaser)
    {
        isShotting = true;
        lineLaser.enabled = true;

        yield return new WaitForSeconds(shotDuration);

        lineLaser.enabled = false;
        isShotting = false;
    }
}
