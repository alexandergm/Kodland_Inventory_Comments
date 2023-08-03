using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inv_Inventory : MonoBehaviour
{       
    //��� ������ UI ���������
    [SerializeField] List<Button> buttons = new List<Button>();   
    //��� ������� �� ����� Resources
    [SerializeField] List<GameObject> resourceItems = new List<GameObject>();
    [SerializeField] GameObject buttonsPath;
    //����� ��������, ������� �� �������
    [SerializeField] List<string> inventoryItems = new List<string>();
    //�� ��� � ��� � ����
    GameObject itemInArm;
    //����� � ������� ��������� ������� �� ���������
    [SerializeField] Transform itemPoint;
    [SerializeField] Transform[] itemPositions;
    //��������� ���������(Text)
    [SerializeField] TMP_Text warning;
    //�������� ������ �� �����
    [SerializeField] List<GameObject> playerItems = new List<GameObject>();
    GameObject itemPosition;


    private void Start()
    {
        //�������� ��� ��������� ������� ���������, ������� ����� � ����� Resources
        GameObject[] objArr = Resources.LoadAll<GameObject>("Items");
        
        //��������� ������ ��������� ��������� ���������
        resourceItems.AddRange(objArr);
        //���������� ��� ������ ��������� �� ����� � ����� �� � ������
        foreach(Transform child in buttonsPath.transform)
        {
            buttons.Add(child.GetComponent<Button>());
        }
    }
    private void Update()
    {
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
        //���� � ��� ������ ���������, �� ������� �� ���� ��������� � ��������� ������
        if (inventoryItems.Count >= buttons.Count)
        {
            warning.text = "Full Inventory!";
            Invoke("WarningUpdate", 1f);
            return;
        }
        //���� � ��������� ��� ���� ����� �������, �� ������� �� ���� ��������� � ��������� ������
        if (inventoryItems.Contains(itemName))
        {
            warning.text = "You already have " + itemName;
            Invoke("WarningUpdate", 1f);
            return;
        }
        //��������� ���� �������� � ���������
        inventoryItems.Add(itemName);
        //�������� ��������� ��������� ������ � � ��������� Image
        var buttonImage = buttons[inventoryItems.Count - 1].GetComponent<Image>();
        //���������� � ������ �������� ��������, ������� �������
        buttonImage.sprite = img;
        //���������� ������, ������� ���������
        Destroy(obj);
    }
    //�����, ������� ������� ��� ���������
    void WarningUpdate()
    {
        warning.text = "";
    }
    //���� ����� ���������� �� ������� ������
    public void UseItem(int itemPos)
    {           
        //���� �� ������ ������, � ������� ������ ���, �� ��������� ������
        if (inventoryItems.Count <= itemPos) return;
        //���������� ��� �������, ������� �������� ���� ������ � ����������
        string item = inventoryItems[itemPos];
        //�������� ����� ������ ������� �� ��������� � �������� ��� �������, ������� ����� �����
        GetItemFromInventory(item);
    }
    public void GetItemFromInventory(string itemName)
    {
        //���� ������ � ������ ������ � ������ ������� ����������� ��������
        var resourceItem = resourceItems.Find(x => x.name == itemName);
        if (resourceItem == null) return;

        //���� ������ � ������ ������ � ������ �������� ������ �� �����
        var putFind = playerItems.Find(x => x.name == itemName);

        //���� ������ ������� ��� ��� �� �����, �� ������� ���
        if (putFind == null)
        {
            //���� ���� ��� �������������� ������, �� ��������� ���
            if (itemInArm != null)
            {
                itemInArm.SetActive(false);
            }
            //��������� �� ����� ����� ���� ������ ������������ ������
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
            //������� ��+�����
            var newItem = Instantiate(resourceItem, itemPoint);
            //���������� ���� ������ � �������� ������
            newItem.transform.parent = itemPosition.transform;
            //���� ��� ��� 
            newItem.name = itemName;
            //��������� � ������ ��������� ������ ���� ������
            playerItems.Add(newItem);
            //������ ������� �����, ��� ���������� itemInArm = ����� �������(�� ���� ���������� ��, ��� � ��� ������ ����� �� ���������)
            itemInArm = newItem;
        }
        //���� ����� ������ ��� ���� �� �����, ��..
        else
        {
            //���� ��� ������, ������� ��� �������� � �����, �� ������ ������ ��� ���������
            if (putFind == itemInArm)
            {
                putFind.SetActive(!putFind.activeSelf);
            }
            //���� ��� ������ ������, �� ��������� ������� ������ � ����� � �������� �����
            else
            {
                itemInArm.SetActive(false);
                putFind.SetActive(true);
                itemInArm = putFind;
            }
        }
    }
    //����� ������ �������
    //public void GetItemFromInventory(string itemName)
    //{
    //    //���������� ��� ������� � ����� Resources
    //    foreach (var resourceItem in resourceItems)
    //    {
    //        //���� ��� ������� ������� � ���, ������� �� ����� �����,��
    //        if (resourceItem.name == itemName)
    //        {
    //            //������� ���������� � ������ � �� ������ ��� �������� ��������� � ���, ��� ��� �����. (���� ������ ������� ���, �� ���������� ������ �������� null)
    //            GameObject putFind = playerItems.Find(x => x.name == itemName);
    //            //���� �� ����� ������ ��� �� ��������� �� ���������, ��
    //            if (putFind == null)
    //            {
    //                if(itemInArm != null)
    //                {
    //                    itemInArm.SetActive(false);
    //                }
    //                //������� ���� ������ �� ����� Resources �� ����� � ����� itemPoint(� ������ ������ ��� ����� �� ������ ����)
    //                var pos = resourceItem.GetComponent<Inv_ItemPosition>().positon;
    //                if (pos == Inv_ItemPosition.ItemPos.Head)
    //                {
    //                    itemPoint.position = itemPositions[0].position;
    //                    asd = itemPositions[0].gameObject;
    //                }
    //                else if (pos == Inv_ItemPosition.ItemPos.Spine)
    //                {
    //                    itemPoint.position = itemPositions[1].position;
    //                    asd = itemPositions[1].gameObject;
    //                }
    //                else
    //                {
    //                    itemPoint.position = itemPositions[2].position;
    //                    asd = itemPositions[2].gameObject;
    //                }
    //                var newItem = Instantiate(resourceItem, itemPoint);
    //                //���������� ���� ������ � �������� ������
    //                newItem.transform.parent = asd.transform;
    //                //���� ��� ��� 
    //                newItem.name = itemName;
    //                //��������� � ������ ��������� ������ ���� ������
    //                playerItems.Add(newItem);
    //                //������ ������� �����, ��� ���������� itemInArm = ����� �������(�� ���� ���������� ��, ��� � ��� ������ ����� �� ���������)
    //                itemInArm = newItem;
    //            }
    //            //����� ���� �������, ������� �� ����� ������� �� �������� ��� ���� �������, ��..
    //            else if (putFind.name == itemInArm.name)
    //            {
    //                //�������� ��� ��� ���������
    //                putFind.SetActive(!putFind.activeSelf);
    //            }
    //            //� ��������� �������, ���� �������, ������� �� ����� ������� ��� ��� ��������� �� ����� � ���� �� �� ��������� � ���, ��� � ��� ����� ������, ��..
    //            else
    //            {
    //                //��������� �������, ������� � ��� ������ � �����
    //                itemInArm.SetActive(false);
    //                //�������� �������, ������� ����� �����
    //                putFind.SetActive(true);
    //                //������ ������� �����, ��� ���������� itemInArm = ����� �������(�� ���� ���������� ��, ��� � ��� ������ ����� �� ���������)
    //                itemInArm = putFind;
    //            }
    //        }
    //    }
    //}
}
