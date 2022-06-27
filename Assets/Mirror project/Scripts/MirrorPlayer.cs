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
    public GameObject PointPrefab; //сюда вешаем ранее созданные префаб Point
    public LineRenderer LineRenderer; //сюда кидаем наш же компонент
    public GameObject bulletPrefab;
    private int pointsCount;

    //Переменные синхронизации
    [SyncVar(hook = nameof(SyncHealth))] //задаем метод, который будет выполняться при синхронизации переменной
    private int syncHealth;
    private SyncList<Vector3> syncVector3Vars = new SyncList<Vector3>(); //В случае SyncList не нужно ставить SyncVar и задавать метод, это делается иначе


    void Update()
    {
        if (hasAuthority) //проверяем, есть ли у нас права изменять этот объект
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float tSpeed = speed * Time.deltaTime;
            transform.Translate(new Vector2(h * tSpeed, v * tSpeed)); //делаем простейшее движение

            if (Input.GetKeyDown(KeyCode.H)) //отнимаем у себя жизнь по нажатию клавиши H
            {
                if (isServer) //если мы являемся сервером, то переходим к непосредственному изменению переменной
                {
                    ChangeHealthValue(health - 1);
                }
                else
                {
                    CmdChangeHealth(health - 1); //в противном случае делаем на сервер запрос об изменении переменной
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

    //Перегрузка метода старта клиента
    public override void OnStartClient()
    {
        base.OnStartClient();

        syncVector3Vars.Callback += SyncVector3Vars; //вместо hook, для SyncList используем подписку на Callback

        vector3Vars = new List<Vector3>(syncVector3Vars.Count); 
        //так как Callback действует только на изменение массива,  
        //а у нас на момент подключения уже могут быть какие-то данные в массиве,
        //нам нужно эти данные внести в локальный массив
        for (int i = 0; i < syncVector3Vars.Count; i++) 
        {
            vector3Vars.Add(syncVector3Vars[i]);
        }
    }


    //==========================================================================================================
    [Server]
    public void SpawnBullet(uint owner, Vector3 target)
    {
        GameObject bulletGo = Instantiate(bulletPrefab, transform.position, Quaternion.identity); //Создаем локальный объект пули на сервере
        NetworkServer.Spawn(bulletGo); //отправляем информацию о сетевом объекте всем игрокам.
        bulletGo.GetComponent<MirrorBullet>().Init(owner, target); //инициализируем поведение пули
    }

    [Command]
    public void CmdSpawnBullet(uint owner, Vector3 target)
    {
        SpawnBullet(owner, target);
    }


    //==========================================================================================================
    //КЛИЕНТ метод не выполнится, если старое значение равно новому
    //обязательно делаем два значения - старое и новое.
    private void SyncHealth(int oldValue, int newValue)
    {
        health = newValue;
    }

    //СЕРВЕР метод, который будет менять переменную _SyncHealth
    [Server] //обозначаем, что этот метод будет вызываться и выполняться только на сервере
    public void ChangeHealthValue(int newValue)
    {
        syncHealth = newValue;

        if (syncHealth <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    //СЕРВЕР 
    [Command] //обозначаем, что этот метод должен будет выполняться на сервере по запросу клиента
    public void CmdChangeHealth(int newValue) //!!! обязательно ставим Cmd в начале названия метода
    {
        ChangeHealthValue(newValue); //переходим к непосредственному изменению переменной
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
