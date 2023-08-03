using UnityEngine;

public class Inv_Collected : MonoBehaviour
{
    //Имя собираемого объекта
    public string name;
    //Картинка собираемого объекта
    public Sprite image;
    //Ссылка на скрипт инвентраря
    private Inv_Inventory inventory;

    private void Start()
    {
        //ищем объект со скриптом инвенторя и кладём его в переменную
        inventory = FindObjectOfType<Inv_Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {                 
        //Вызываем метод AddItem, который находится в скрипте инвентаря и передаем ему имя, картинку и сам GameObject, который мы взяли
        inventory.AddItem(image, name, gameObject);
    }

}
