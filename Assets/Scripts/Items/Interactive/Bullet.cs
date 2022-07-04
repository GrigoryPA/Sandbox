using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletGun parentGun;
    public bool isActive;

    private void Update()
    {
        if (isActive)
        {
            //��� ������ �� ������� �� ��������� �����
            if (parentGun.maxShotDistace <= Vector3.Distance(transform.position, parentGun.shotsSource.transform.position))
            {
                if (parentGun.alwaysExploseBullets)
                {
                    //������� ����� � ���������� ��� ����� ������������ ��������
                    GameObject explosionObj = Instantiate(parentGun.explosion, transform.position, Quaternion.LookRotation(transform.forward));
                    explosionObj.transform.localScale = Vector3.one * parentGun.explosionScale;
                    Destroy(explosionObj, explosionObj.GetComponent<ParticleSystem>().main.duration);
                }
                Destroy(gameObject);
            }
            else
            {
                //��������� ��� ����� ����
                CapsuleCollider capsule = gameObject.GetComponent<CapsuleCollider>();
                Vector3 p1 = transform.position + transform.forward * (capsule.height * 0.5f - capsule.radius) + transform.forward * 0.005f;
                Vector3 p2 = transform.position - transform.forward * (capsule.height * 0.5f - capsule.radius) - transform.forward * 0.005f;

                if (Physics.CheckCapsule(p1, p2, capsule.radius + 0.001f, parentGun.layerReceivingDamage))
                { 
                    //���� ���� ���� ����� � ������� ������
                    Collider[] colliders = Physics.OverlapSphere(transform.position, parentGun.damageRadius, parentGun.layerReceivingDamage);
                    foreach (Collider colider in colliders)
                    {
                        Body body = colider.gameObject.gameObject.GetComponent<Body>();
                        if (body != null)
                        {
                            //�������� ������ � ����������� �� ������ � ��������
                            //��� ����� ����� ��� ������� ������������ � ����� ������� ������ (�� ��������� ���������� ���� � ���� ������, �� ������� - ����������� �������)
                            colider.gameObject.GetComponent<Rigidbody>().AddForce((colider.transform.position-transform.position) * parentGun.impactForce);
                            body.TakeDamage(parentGun.damage);
                        }
                    }

                    //������� ����� � ���������� ��� ����� ������������ ��������
                    GameObject explosionObj = Instantiate(parentGun.explosion, transform.position, Quaternion.identity);
                    explosionObj.transform.localScale = Vector3.one * parentGun.explosionScale;
                    Destroy(explosionObj, explosionObj.GetComponent<ParticleSystem>().main.duration);
                    Destroy(gameObject);
                }
            }
        }
    }
}
