using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

namespace Managers {
	public class ResultManager : SingletonMonoBehaviour<ResultManager> {

		private Canvas canvas;
		private GridLayoutGroup grid;

		// Use this for initialization
		void Start() {
			canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
			grid = canvas.GetComponentInChildren<GridLayoutGroup>();

			List<FoodStuff> foodStuffList = ItemBox.Instance.GetAllCollectedList<FoodStuff>();
			int i = 0;
			Observable.Timer(TimeSpan.FromSeconds(0f), TimeSpan.FromSeconds(1f))
				.Select(x => foodStuffList)
				.Take(foodStuffList.Count)
				.Subscribe(_ => {
					FoodStuff foodStuff = foodStuffList[i++];
					GameObject obj = new GameObject(foodStuffList.ToString());
					obj.transform.SetParent(grid.transform, false);
					Image image = obj.AddComponent<Image>();
					image.sprite = foodStuff.itemSprite;
				});

		}
	}
}
