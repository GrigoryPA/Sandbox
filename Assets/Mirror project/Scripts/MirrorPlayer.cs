using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MirrorPlayer : NetworkBehaviour
{
    public float speed = 5f;
    public int health = 4;
    public GameObject[] healthGos;
    public List<Vector3> vector3Vars;
    public GameObject PointPrefab; //���� ������ ����� ��������� ������ Point
    public LineRenderer LineRenderer; //���� ������ ��� �� ���������
    public GameObject bulletPrefab;
    private int pointsCount;

    //���������� �������������
    [SyncVar(hook = nameof(SyncHealth))] //������ �����, ������� ����� ����������� ��� ������������� ����������
    private int syncHealth;
    private SyncList<Vector3> syncVector3Vars = new SyncList<Vector3>(); //� ������ SyncList �� ����� ������� SyncVar � �������� �����, ��� �������� �����


    void Update()
    {
        if (hasAuthority) //���������, ���� �� � ��� ����� �������� ���� ������
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float tSpeed = speed * Time.deltaTime;
            transform.Translate(new Vector2(h * tSpeed, v * tSpeed)); //������ ���������� ��������

            if (Input.GetKeyDown(KeyCode.H)) //�������� � ���� ����� �� ������� ������� H
            {
                if (isServer) //���� �� �������� ��������, �� ��������� � ����������������� ��������� ����������
                {
                    ChangeHealthValue(health - 1);
                }
                else
                {
                    CmdChangeHealth(health - 1); //� ��������� ������ ������ �� ������ ������ �� ��������� ����������
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (isServer)
                {
                    ChangeVector3Vars(transform.position);
                }
                else
                {
                    CmdChangeVector3Vars(transform.position);
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 pos = Input.mousePosition;
                pos.z = 10f;
                pos = Camera.main.ScreenToWorldPoint(pos);

                if (isServer)
                {
                    SpawnBullet(netId, pos);
                }
                else
                {
                    CmdSpawnBullet(netId, pos);
                }
            }
        }

        for (int i = 0; i < healthGos.Length; i++)
        {
            healthGos[i].SetActive(!(health - 1 < i));
        }

        for (int i = pointsCount; i < vector3Vars.Count; i++)
        {
            Instantiate(PointPrefab, vector3Vars[i], Quaternion.identity);
            pointsCount++;

            LineRenderer.positionCount = vector3Vars.Count;
            LineRenderer.SetPositions(vector3Vars.ToArray());
        }
    }

    //���������� ������ ������ �������
    public override void OnStartClient()
    {
        base.OnStartClient();

        syncVector3Vars.Callback += SyncVector3Vars; //������ hook, ��� SyncList ���������� �������� �� Callback

        vector3Vars = new List<Vector3>(syncVector3Vars.Count); 
        //��� ��� Callback ��������� ������ �� ��������� �������,  
        //� � ��� �� ������ ����������� ��� ����� ���� �����-�� ������ � �������,
        //��� ����� ��� ������ ������ � ��������� ������
        for (int i = 0; i < syncVector3Vars.Count; i++) 
        {
            vector3Vars.Add(syncVector3Vars[i]);
        }
    }


    //==========================================================================================================
    [Server]
    public void SpawnBullet(uint owner, Vector3 target)
    {
        GameObject bulletGo = Instantiate(bulletPrefab, transform.position, Quaternion.identity); //������� ��������� ������ ���� �� �������
        NetworkServer.Spawn(bulletGo); //���������� ���������� � ������� ������� ���� �������.
        bulletGo.GetComponent<MirrorBullet>().Init(owner, target); //�������������� ��������� ����
    }

    [Command]
    public void CmdSpawnBullet(uint owner, Vector3 target)
    {
        SpawnBullet(owner, target);
    }


    //==========================================================================================================
    //������ ����� �� ����������, ���� ������ �������� ����� ������
    //����������� ������ ��� �������� - ������ � �����.
    private void SyncHealth(int oldValue, int newValue)
    {
        health = newValue;
    }

    //������ �����, ������� ����� ������ ���������� _SyncHealth
    [Server] //����������, ��� ���� ����� ����� ���������� � ����������� ������ �� �������
    public void ChangeHealthValue(int newValue)
    {
        syncHealth = newValue;

        if (syncHealth <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    //������ 
    [Command] //����������, ��� ���� ����� ������ ����� ����������� �� ������� �� ������� �������
    public void CmdChangeHealth(int newValue) //!!! ����������� ������ Cmd � ������ �������� ������
    {
        ChangeHealthValue(newValue); //��������� � ����������������� ��������� ����������
    }



    //==========================================================================================================
    [Server]
    void ChangeVector3Vars(Vector3 newValue)
    {
        syncVector3Vars.Add(newValue);
    }

    [Command]
    public void CmdChangeVector3Vars(Vector3 newValue)
    {
        ChangeVector3Vars(newValue);
    }

    void SyncVector3Vars(SyncList<Vector3>.Operation op, int index, Vector3 oldItem, Vector3 newItem)
    {
        switch (op)
        {
            case SyncList<Vector3>.Operation.OP_ADD:
                {
                    vector3Vars.Add(newItem);
                    break;
                }
            case SyncList<Vector3>.Operation.OP_CLEAR:
                {

                    break;
                }
            case SyncList<Vector3>.Operation.OP_INSERT:
                {

                    break;
                }
            case SyncList<Vector3>.Operation.OP_REMOVEAT:
                {

                    break;
                }
            case SyncList<Vector3>.Operation.OP_SET:
                {

                    break;
                }
        }
    }
}
