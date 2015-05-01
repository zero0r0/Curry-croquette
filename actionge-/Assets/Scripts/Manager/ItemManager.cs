using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// アイテム管理用クラス
/// </summary>
public class ItemManager : SingletonMonoBehaviour<ItemManager> {

	// アイテムID
	public enum ItemId { Carrot, Potato, Onion, Mince, Curry, Croquette }

	// アイテム構造体
	public struct Item {
		public ItemId itemId;
		public int count;
		public Item(ItemId itemId) {
			this.itemId = itemId;
			this.count = 0;
		}
	}

	private Item[] items = {
		new Item(ItemId.Carrot),
		new Item(ItemId.Potato),
		new Item(ItemId.Onion),
		new Item(ItemId.Mince),
	};

	new void Awake() {
		CheckInstance();
		DontDestroyOnLoad(this.gameObject);
	}

    /// <summary>
    /// プレイヤーから送られたアイテムを取得し、所持数を増やす
    /// </summary>
    /// <param name="item">アイテムID</param>
    public void SendItem(ItemId item) {
		SelectArrayFromItemId(item, items, (ref Item x) => x.count = x.count + 1);
        UIManager.Instance.SetItem(item);
    }

	public Item[] GetCollectedItems() {
		return items;
	}

	public bool CheckCollectAllItmes() {
		foreach (Item item in items) {
			if (item.count == 0)
				return false;
		}
		return true;
	}

    public delegate void SelectFromItemBehaviour(ItemBehaviour item);
	/// <summary>
	/// 該当するitemidのオブジェクトに対して、指定された処理を行う
	/// </summary>
	/// <param name="itemid">アイテムid</param>
	/// <param name="items">アイテムリスト</param>
	/// <param name="func">処理</param>
	public void SelectArrayFromItemId(ItemId itemId, ItemBehaviour[] items, SelectFromItemBehaviour func) {
        foreach (ItemBehaviour item in items) {
            if (item.ItemId == itemId) {
				func(item);
			}
        }
    }

	public delegate void SelectFromItem(ref Item item);
	public void SelectArrayFromItemId(ItemId itemId, Item[] items, SelectFromItem func) {
		for (int i = 0; i < items.Length; i++) {
			if (items[i].itemId == itemId) {
				func(ref items[i]);
			}
		}
    }

}
