using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// ユーザインタフェース管理用クラス
/// </summary>
public class UIManager : MonoBehaviour {

    [SerializeField]
    private Image carrot;
    [SerializeField]
    private Image potato;
    [SerializeField]
    private Image onion;

    [SerializeField]
    private Image hp;
    
    delegate void image(Image x);

    /// <summary>
    /// 取得アイテム情報を表示する
    /// </summary>
    /// <param name="item">アイテムID</param>
    public void SetItem(ItemManager.ItemId item) {
        image setColor = (Image x) => x.color = new Color(1, 1, 1, 1);
        switch (item) {
            case ItemManager.ItemId.Carrot:
                setColor(carrot);
                break;
            case ItemManager.ItemId.Potato:
                setColor(potato);
                break;
            case ItemManager.ItemId.Onion:
                setColor(onion);
                break;
        }
    }

    public void IncreasePlayerHP() {
        hp.enabled = false;
    }

}
