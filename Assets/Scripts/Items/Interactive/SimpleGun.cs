using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGun : Gun
{
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
        if (Physics.Raycast(RayOrigin, RayDirection, out hit, maxShotDistace, layerReceivingDamage))
        {
            Body body = hit.transform.gameObject.GetComponent<Body>();
            if (body != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
                body.TakeDamage(damage);
            }
            GameObject explosionObj = Instantiate(explosion, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(explosionObj, explosionObj.GetComponent<ParticleSystem>().main.duration);

        }
    }
}
