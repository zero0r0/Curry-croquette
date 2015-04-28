using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// アイテム管理用クラス
/// </summary>
public class ItemManager : MonoBehaviour {

    // アイテムID
    public enum ItemId { Carrot, Potato, Onion, Curry, Croquette }

    // アイテム構造体
    public struct Item {
        public ItemId itemId;
        public int count;
        public Item(ItemId itemId) {
            this.itemId = itemId;
            this.count = 0;
        }
    }

    [System.Serializable]
    public class Items {
        public Item carrot = new Item(ItemId.Carrot);
        public Item potato = new Item(ItemId.Potato);
        public Item onion = new Item(ItemId.Onion);
        public Item curry = new Item(ItemId.Curry);
        public Item croquette = new Item(ItemId.Croquette);
    }

    // 各アイテム構造体
    private Item carrot;
    private Item curry;
    private Item croquette;

    // UI管理クラス
    [SerializeField]
    private UIManager uiManager;

    void Awake() {
        if (uiManager == null)
            uiManager = GameObject.FindWithTag("UI Manager").GetComponent<UIManager>();
    }

    /// <summary>
    /// プレイヤーから送られたアイテムを取得し、所持数を増やす
    /// </summary>
    /// <param name="item">アイテムID</param>
    public void SendItem(ItemId item) {
        switch (item) {
            case ItemId.Curry:
                curry.count++;
                break;
        }

        this.SendItemInfoToUI(item);
    }
    
    /// <summary>
    /// アイテムの情報をUIManagerに送り、UI表示を更新する
    /// </summary>
    /// <param name="item">アイテムID</param>
    private void SendItemInfoToUI(ItemId item) {
        uiManager.SetItem(item);
    }

    public delegate void SelectFromItemId(object obj);
    public static void SelectArrayFromItemId(ItemId itemId, List<Item> list, SelectFromItemId func) {
        foreach (object item in list) {
            
        }
    }
}
