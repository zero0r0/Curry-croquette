using UnityEngine;
using UnityEngine.UI;

namespace Managers {

	/// <summary>
	/// ユーザインタフェース管理用クラス
	/// </summary>
	public class UIManager : SingletonMonoBehaviour<UIManager> {

		// プレイヤーの体力UI
		private Image hp;

		// アイテムUI群
		private ItemBehaviour[] items;

		// HPUIを減らす割合
		private float hpRate = 1 / 3f;

		new void Awake() {
			CheckInstance();
			Canvas canvas = FindObjectOfType<Canvas>();
			items = canvas.transform.GetComponentsInChildren<ItemBehaviour>();
			hp = canvas.transform.GetComponentInChildren<HPBehaviour>().gameObject.GetComponent<Image>();
		}

		/// <summary>
		/// 取得アイテム情報を表示する
		/// </summary>
		/// <param name="item">アイテムID</param>
		public void SetItem(ItemManager.ItemId item) {
			ItemManager.Instance.SelectArrayFromItemId(item, items,
				(x) => x.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1));
		}

		public void SetItem(Item item) {
			ItemBox.Instance.AddItem(item);
		}

		/// <summary>
		/// プレイヤーのHPのUIを更新する
		/// </summary>
		public void DecreasePlayerHP() {
			hp.fillAmount -= hpRate;
		}

		public void PlayerRespawn() {
			hp.fillAmount = 1;
		}
	}
}
