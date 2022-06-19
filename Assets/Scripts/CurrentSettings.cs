using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentSettings
{
    public static int cameraMod = 0;    //0 - camera is controlled by the player;
                                        //1 - camera is controlled by the mouse  
    public static void UpdateCameraMod() => cameraMod = (controlMod + 1) % 2;

    public static int controlMod = 0;   //0 - �������� ������ � ����� (w-s), �������� (a-d);
                                        //1 - �������� ������ � ����� (w-s), �������� (������);
    public static void UpdateControlMod() => controlMod = cameraMod!=0 ? (controlMod + 1) % 2 : 0;
}
