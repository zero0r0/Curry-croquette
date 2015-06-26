using System;

namespace Managers {
	/// <summary>
	/// アイテム管理用クラス
	/// </summary>
	public class ItemManager : SingletonMonoBehaviour<ItemManager> {

		// アイテムID
		public enum ItemId { Carrot, Potato, Onion, Mince, Curry, Croquette, Umai, Katsu, Choco }

		public Item carrot;
		public Item potato;
		public Item onion;
		public Item mince;
		public Item curry;
		public Item croquette;
		public Item umai;
		public Item katsu;
		public Item choco;

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
		new Item(ItemId.Curry),
		new Item(ItemId.Croquette),
		new Item(ItemId.Umai),
		new Item(ItemId.Katsu),
		new Item(ItemId.Choco)
	};

		new void Awake() {
			CheckInstance();
			DontDestroyOnLoad(gameObject);
		}

		/// <summary>
		/// プレイヤーから送られたアイテムを取得し、所持数を増やす
		/// </summary>
		/// <param name="item">アイテムID</param>
		public void SendItem(ItemId item) {
			SelectArrayFromItemId(item, items, (ref Item x) => x.count++);
			UIManager.Instance.SetItem(item);
		}

		public Item[] GetCollectedItems() {
			return items;
		}

		/// <summary>
		/// すべてのアイテムを取得しているかチェックする
		/// </summary>
		/// <returns>すべてのアイテムを取得しているか</returns>
		public bool CheckCollectAllItmes() {
			foreach (Item item in items) {
				if (item.count == 0)
					return false;
			}
			return true;
		}

		public bool CheckCollectNecessaryItems() {
			foreach (Item item in items) {
				if (item.itemId == ItemId.Carrot && item.count == 0)
					return false;
				else if (item.itemId == ItemId.Potato && item.count == 0)
					return false;
				else if (item.itemId == ItemId.Onion && item.count == 0)
					return false;
				else if (item.itemId == ItemId.Mince && item.count == 0)
					return false;
				//else if (item.itemId == ItemId.Curry && item.count == 0)
				//	return false;
				//else if (item.itemId == ItemId.Croquette && item.count == 0)
				//	return false;
			}
			return true;
		}

		/// <summary>
		/// 該当するitemidのオブジェクトに対して、指定された処理を行う
		/// </summary>
		/// <param name="itemid">アイテムid</param>
		/// <param name="items">アイテムリスト</param>
		/// <param name="func">処理</param>
		public void SelectArrayFromItemId(ItemId itemId, ItemBehaviour[] items, Action<ItemBehaviour> func) {
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
}
