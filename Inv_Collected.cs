using UnityEngine;

public class Inv_Collected : MonoBehaviour
{
    //Название объекта
    public string name;
    //Картинка(спрайт), который будет отображаться в инвентаре
    public Sprite image;
    //Ссылка на скрипт ивентаря
    private Inv_Inventory inventory;

    private void Start()
    {
        //Ищем объект со скриптом инвентаря и кладём его в переменную
        inventory = FindObjectOfType<Inv_Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {                 
        //При подборе предмета вызываем метод добавления предмета в скрипте инвентаря
        //И передаем спрайт, имя и объект, который мы подобрали
        inventory.AddItem(image, name, gameObject);
    }
}
