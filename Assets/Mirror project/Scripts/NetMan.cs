using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetMan : NetworkManager
{
    bool playerSpawned;
    NetworkConnection connection;
    bool playerConnected;


    //������ ����� ��������������� ������, ������� ����� ����������� ������ �� �������
    public void OnCreateCharacter(NetworkConnection conn, PosMessage message)
    {
        GameObject go = Instantiate(playerPrefab, message.vector2, Quaternion.identity); //�������� �� ������� ������� gameObject
        NetworkServer.AddPlayerForConnection((NetworkConnectionToClient)conn, go); //������������ gameObject � ���� ������� �������� � ���������� ���������� �� ���� ��������� �������
    }

    //������ ���������� �e��� OnStartServer(����������� ������ �� �������) � ������� � ���� ���������� �������� ���������
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<PosMessage>(OnCreateCharacter); //���������, ����� struct ������ ������ �� ������, ����� ���������� �����
    }

    //������ �����, ������� ����� ������������ ����� (� ����������� �������� �� �������):
    public void ActivatePlayerSpawn()
    {
        //������� ������
        Vector3 pos = Input.mousePosition;
        pos.z = 10f;
        pos = Camera.main.ScreenToWorldPoint(pos);
        PosMessage m = new PosMessage() { vector2 = pos }; //������� struct ������������� ����, ����� ������ ����� � ���� ��� ������ ���������

        connection.Send(m); //�������� ��������� �� ������ � ������������ ������
        playerSpawned = true; //��� ����� ������� ����� ��� ������
    }

    //������ ���������� �� ������� ��� ����������� � �������
    public override void OnClientConnect()
    {
        base.OnClientConnect(); //������� ��������� �����������
        connection = NetworkClient.connection; //���������� �����������
        playerConnected = true; //����� �����������
    }

    //������ ���������� �� ������� ��� ���������� �� �������
    public override void OnClientDisconnect()
    {
        playerConnected = false;
        playerSpawned = false;
        base.OnClientDisconnect();
    }

    //������
    private void Update()
    {
        //���� ��� �� ���� ������, ���� ������������ �����������, ���� ������ ������ �����
        if (!playerSpawned && playerConnected && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ActivatePlayerSpawn(); //���������� ����� ������ � ���������� �� �������
        }
    }
}

public struct PosMessage : NetworkMessage //����������� �� ���������� NetworkMessage, ����� ������� ������ ����� ������ �����������
{
    public Vector2 vector2; //������ ������������ Property
}