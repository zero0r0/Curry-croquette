using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResultManager : SingletonMonoBehaviour<ResultManager> {

	private Canvas canvas;

	// Use this for initialization
	void Start () {
		canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
		List<FoodStuff> foodStuffList = ItemBox.Instance.GetAllCollectedList<FoodStuff>();
		if (foodStuffList != null) {
			foreach (var item in foodStuffList) {
				GameObject obj = new GameObject(item.item.ToString());
				obj.transform.SetParent(canvas.transform, false);
				Image image = obj.AddComponent<Image>();
				image.sprite = item.itemSprite;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
