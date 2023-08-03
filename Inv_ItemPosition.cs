using UnityEngine;

public class Inv_ItemPosition : MonoBehaviour
{
    //Выбираем позицию тела, на которой должен появится объект инвентаря
    public enum ItemPos
    {
        Head,
        Spine,
        RightArm
    }
    public ItemPos positon;
}
