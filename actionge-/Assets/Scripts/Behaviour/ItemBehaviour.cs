using UnityEngine;
using Managers;

/// <summary>
/// アイテム動作用クラス
/// </summary>
public class ItemBehaviour : MonoBehaviour {

    [SerializeField]
    private ItemManager.ItemId itemId;
    public ItemManager.ItemId ItemId {
        get { return itemId; }
    }

	[SerializeField]
	private Item item;
	public Item Item {
		get { return item; }
	}
}
