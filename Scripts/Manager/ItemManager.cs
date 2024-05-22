using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] itemList;

    public void ItemDrop(Vector2 _vector)
    {
        if (Random.Range(0, 200) == 0)              // 200분의 1 확률로 은신 아이템 드랍  
            Instantiate(itemList[1], new Vector2(_vector.x, _vector.y), Quaternion.identity);
        else if (Random.Range(0, 100) == 0)          // 100분의 1 확률로 포션 드랍
            Instantiate(itemList[0], new Vector2(_vector.x, _vector.y), Quaternion.identity);
    }
}
