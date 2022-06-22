using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentSettings
{
    public static int cameraMod = 0;    //0 - camera is controlled by the player (3rd person);
                                        //1 - camera is controlled by the mouse (3rd person);
                                        //2 - camera is controlled by the player (1rd person);
                                        //3 - camera is controlled by the mous (1rd person);
    public static void UpdateCameraMod() => cameraMod = (cameraMod + 1) % 4;

    public static int controlMod = 0;   //0 - движение вперед и назад (w-s), вращение (a-d);
                                        //1 - движение вперед и назад (w-s), вращение (камера);
    public static void UpdateControlMod() => controlMod = (cameraMod != 0) ? (controlMod + 1) % 2 : 0;
}
