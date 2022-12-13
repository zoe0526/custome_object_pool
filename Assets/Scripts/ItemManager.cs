using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;
    public static ItemManager Instance
    { get { return instance; } }


    [SerializeField]
    public Transform item_pool_pos;

    Dictionary<string, Queue<Item>> _item_dic; //아이템 이름, 생성된 오브젝트 배열 순
    private Item _item;
    public void Awake()
    {
        _item_dic = new Dictionary<string, Queue<Item>>();
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

    }
    public void make_item(string item_name)
    {
        _item = PoolAllocater.Instance.get_pool_obj(item_name + "_Pool").GetComponent<Item>();
        _item.transform.SetParent(item_pool_pos);
        _item.gameObject.SetActive(true);
        if (!_item_dic.ContainsKey(_item.get_item_name()))
            _item_dic.Add(_item.get_item_name(), new Queue<Item>());

        _item_dic[_item.get_item_name()].Enqueue(_item);

    }
    public void return_all_item_to_pool()
    {
        if (_item_dic.Count <= 0)
            return;
        foreach (var obj in _item_dic)
            foreach (Item item in obj.Value)
                item.Return();
        _item_dic.Clear();
    }
    public void return_each_item_to_pool(string item_name)
    {
        if (_item_dic == null || _item_dic.Count <= 0 || !_item_dic.ContainsKey(item_name) || _item_dic[item_name].Count <= 0)
            return;

        _item_dic[item_name].Peek().Return();
        _item_dic[item_name].Dequeue();
        if (_item_dic[item_name].Count == 0)
            _item_dic.Remove(item_name);
    }

    private void OnDestroy()
    {
        _item_dic.Clear();
        _item = null;
    }

}
