using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// ユーザインタフェース管理用クラス
/// </summary>
public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text item;
    
    /// <summary>
    /// アイテム情報を表示する
    /// </summary>
    /// <param name="item">アイテムID</param>
    public void SetItem(ItemManager.ItemId item) {
        this.item.text = item.ToString();
    }

    public void SetHp() {

    }

}
