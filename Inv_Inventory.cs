using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inv_Inventory : MonoBehaviour
{       
    //Все кнопки UI инвентаря
    [SerializeField] List<Button> buttons = new List<Button>();   
    //Все объекты из папки Resources
    [SerializeField] List<GameObject> resourceItems = new List<GameObject>();
    [SerializeField] GameObject buttonsPath;
    //Имена объектов, которые мы подняли
    [SerializeField] List<string> inventoryItems = new List<string>();
    //То что у нас в руке
    GameObject itemInArm;
    //Точка в которую спавнятся объекты из инвентаря
    [SerializeField] Transform itemPoint;
    [SerializeField] Transform[] itemPositions;
    //Сообщения инвентаря(Text)
    [SerializeField] TMP_Text warning;
    //Предметы игрока на сцене
    [SerializeField] List<GameObject> playerItems = new List<GameObject>();
    GameObject itemPosition;


    private void Start()
    {
        //Получаем все возможные объекты инвентаря, которые лежат в папке Resources
        GameObject[] objArr = Resources.LoadAll<GameObject>("Items");
        
        //Заполняем список возможных предметов инвентаря
        resourceItems.AddRange(objArr);
        //Перебираем все кнопки инвентаря на сцене и кладём их в список
        foreach(Transform child in buttonsPath.transform)
        {
            buttons.Add(child.GetComponent<Button>());
        }
    }
    private void Update()
    {
        //Включение/отключение курсора мыши на клавишу Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void AddItem(Sprite img, string itemName, GameObject obj)
    {        
        //Если у нас полный инвентарь, то выводим об этом сообщение и прерываем скрипт
        if (inventoryItems.Count >= buttons.Count)
        {
            warning.text = "Full Inventory!";
            Invoke("WarningUpdate", 1f);
            return;
        }
        //Если в инвентаре уже есть такой предмет, то выводим об этом сообщение и прерываем скрипт
        if (inventoryItems.Contains(itemName))
        {
            warning.text = "You already have " + itemName;
            Invoke("WarningUpdate", 1f);
            return;
        }
        //Добавляем bfимя предмета в инвентарь
        inventoryItems.Add(itemName);
        //получаем следующую свободную кнопку и её компонент Image
        var buttonImage = buttons[inventoryItems.Count - 1].GetComponent<Image>();
        //выставляем в кнопку картинку предмета, который подняли
        buttonImage.sprite = img;
        //Уничтожаем объект, который подобрали
        Destroy(obj);
    }
    //Метод, который стирает все сообщения
    void WarningUpdate()
    {
        warning.text = "";
    }
    //Этот метод вызывается по нажатию кнопки
    public void UseItem(int itemPos)
    {           
        //Если мы нажали кнопку, в которой ничего нет, то прерываем скрипт
        if (inventoryItems.Count <= itemPos) return;
        //записываем имя объекта, который присвоен этой кнопке в переменную
        string item = inventoryItems[itemPos];
        //Вызываем метод взятия объекта из инвентаря и передаем имя объекта, который хотим взять
        GetItemFromInventory(item);
    }
    //Этот метод вызывается после обработки нажатия на кнопку и выполнения метода UseItem
    public void GetItemFromInventory(string itemName)
    {
        //Ищем объект с нужным именем в списке заранее загруженных объектов
        var resourceItem = resourceItems.Find(x => x.name == itemName);
        //Если объекта с таким имененм нет в папке ресурсов, то прерываем скрипт
        if (resourceItem == null) return;

        //Ищем объект с нужным именем в списке объектов игрока на сцене
        var putFind = playerItems.Find(x => x.name == itemName);

        //Если такого объекта еще нет на сцене, то создаем его
        if (putFind == null)
        {
            //Если есть уже активированный объект, то отключаем его
            if (itemInArm != null)
            {
                itemInArm.SetActive(false);
            }
            //Проверяем на какую часть тела должен заспавниться объект
            var pos = resourceItem.GetComponent<Inv_ItemPosition>().positon;
            if (pos == Inv_ItemPosition.ItemPos.Head)
            {
                itemPoint.position = itemPositions[0].position;
                itemPosition = itemPositions[0].gameObject;
            }
            else if (pos == Inv_ItemPosition.ItemPos.Spine)
            {
                itemPoint.position = itemPositions[1].position;
                itemPosition = itemPositions[1].gameObject;
            }
            else
            {
                itemPoint.position = itemPositions[2].position;
                itemPosition = itemPositions[2].gameObject;
            }
            //Спавним объект в правильную точку, которую определили раньше
            var newItem = Instantiate(resourceItem, itemPoint);
            //Перемещаем этот объект в иерархию игрока
            newItem.transform.parent = itemPosition.transform;
            //Даем ему имя 
            newItem.name = itemName;
            //Добавляем в список предметов игрока этот объект
            playerItems.Add(newItem);
            //Теперь говорим Юнити, что переменная itemInArm = этому объекту(то есть запоминаем то, что у нас сейчас взято из инвентаря)
            itemInArm = newItem;
        }
        //Если такой объект уже есть на сцене, то..
        else
        {
            //Если это объект, который уже держится в руках, то просто меняем его состояние
            if (putFind == itemInArm)
            {
                putFind.SetActive(!putFind.activeSelf);
            }
            //Если это другой объект, то отключаем текущий объект в руках и включаем новый
            else
            {
                itemInArm.SetActive(false);
                putFind.SetActive(true);
                itemInArm = putFind;
            }
        }
    }
}
