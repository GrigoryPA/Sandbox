using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetMan : NetworkManager
{
    bool playerSpawned;
    NetworkConnection connection;
    bool playerConnected;


    //СЕРВЕР метод непосредственно спавна, который будет выполняется только на сервере
    public void OnCreateCharacter(NetworkConnection conn, PosMessage message)
    {
        GameObject go = Instantiate(playerPrefab, message.vector2, Quaternion.identity); //локально на сервере создаем gameObject
        NetworkServer.AddPlayerForConnection((NetworkConnectionToClient)conn, go); //присоеднияем gameObject к пулу сетевых объектов и отправляем информацию об этом остальным игрокам
    }

    //СЕРВЕР перегрузим мeтод OnStartServer(выполняется только на сервере) и добавим в него обработчик сетевого сообщения
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<PosMessage>(OnCreateCharacter); //указываем, какой struct должен прийти на сервер, чтобы выполнился спавн
    }

    //КЛИЕНТ метод, который будет активировать спавн (и выполняться локально на клиенте):
    public void ActivatePlayerSpawn()
    {
        //позиция спавна
        Vector3 pos = Input.mousePosition;
        pos.z = 10f;
        pos = Camera.main.ScreenToWorldPoint(pos);
        PosMessage m = new PosMessage() { vector2 = pos }; //создаем struct определенного типа, чтобы сервер понял к чему эти данные относятся

        connection.Send(m); //отправка сообщения на сервер с координатами спавна
        playerSpawned = true; //для этого клиента спавн был сделан
    }

    //КЛИЕНТ Вызывается на клиенте при подключении к серверу
    public override void OnClientConnect()
    {
        base.OnClientConnect(); //базовая обработка подключения
        connection = NetworkClient.connection; //запоминаем подключение
        playerConnected = true; //игрок подключился
    }

    //КЛИЕНТ Вызывается на клиенте при отклюяился от сервера
    public override void OnClientDisconnect()
    {
        playerConnected = false;
        playerSpawned = false;
        base.OnClientDisconnect();
    }

    //КЛИЕНТ
    private void Update()
    {
        //если еще не было спавна, если пользователь подключился, если нажата кнопка мышки
        if (!playerSpawned && playerConnected && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ActivatePlayerSpawn(); //генерирует точку спавна и отправляем ее серверу
        }
    }
}

public struct PosMessage : NetworkMessage //наследуемся от интерфейса NetworkMessage, чтобы система поняла какие данные упаковывать
{
    public Vector2 vector2; //нельзя использовать Property
}