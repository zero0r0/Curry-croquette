using UnityEngine;
using System.Collections;

/// <summary>
/// アイテム動作用クラス
/// </summary>
public class ItemBehaviour : MonoBehaviour {

    [SerializeField]
    private ItemManager.ItemId itemId;
    public ItemManager.ItemId ItemId {
        get { return this.itemId; }
    }
}
