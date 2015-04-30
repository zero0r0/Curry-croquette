﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// ユーザインタフェース管理用クラス
/// </summary>
public class UIManager : SingletonMonoBehaviour<UIManager> {

	// プレイヤーの体力UI
    private Image hp;

	private ItemBehaviour[] items;

	private float hpRate = 0.33f;

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

    public void IncreasePlayerHP() {
		if (hp.fillAmount > 0.5f)
        hp.fillAmount -= hpRate;
    }

}
