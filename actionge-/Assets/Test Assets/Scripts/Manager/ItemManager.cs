using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// アイテム管理用クラス
/// </summary>
public class ItemManager : MonoBehaviour {

    // アイテムID
    public enum ItemId { Carrot, Potato, Onion, Curry, Croquette }

    // アイテム構造体
    struct Item {
        public ItemId itemId;
        public int count;
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
    /// プレイヤーから送られたアイテムを取得し、格納する
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
    public static void SelectArrayFromItemId(ItemId itemId, List<object> list, SelectFromItemId func) {
        foreach (object item in list) {
            
        }
    }
}
