using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MirrorBullet : NetworkBehaviour
{
    uint owner; //кто сделал выстрел
    bool inited; //инициализированна
    Vector3 target; //куда должна лететь пуля

    //Инициализация пули сервером
    [Server]
    public void Init(uint owner, Vector3 target)
    {
        this.owner = owner; 
        this.target = target; 
        inited = true;
    }

    void Update()
    {
        //Если пуля инициализирована и это сервер
        if (inited && isServer)
        {
            //двигаем пулю
            transform.Translate((target - transform.position).normalized * 0.04f);

            //проходимся по всем обхектам столкновения пули
            foreach (var item in Physics.OverlapSphere(transform.position, 0.5f))
            {
                MirrorPlayer player = item.GetComponent<MirrorPlayer>();
                if (player)
                {
                    if (player.netId != owner)
                    {
                        player.ChangeHealthValue(player.health - 1); //отнимаем одну жизнь по аналогии с примером SyncVar
                        NetworkServer.Destroy(gameObject); //уничтожаем пулю
                    }
                }
            }

            //если пуля улетела слишком далеко
            if (Vector3.Distance(transform.position, target) < 0.1f) 
            {
                NetworkServer.Destroy(gameObject); //значит ее можно уничтожить
            }
        }
    }
}
