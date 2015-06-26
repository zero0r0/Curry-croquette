using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

namespace Managers {
	/// <summary>
	/// リザルトシーン管理クラス
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class ResultManager : SingletonMonoBehaviour<ResultManager> {

		private Canvas canvas;
		private GridLayoutGroup grid;
		public GameObject effect;

		private readonly float itemDisplayInterval = 1f;

		private const int itemListWidth = 4;

		private AudioSource audioSource;
		public AudioClip effectSe;

		// Use this for initialization
		void Start() {
			audioSource = GetComponent<AudioSource>();

			List<FoodStuff> foodStuffList = ItemBox.Instance.GetAllCollectedList<FoodStuff>();

			canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
			grid = canvas.GetComponentInChildren<GridLayoutGroup>();
			List<Image> imageList = new List<Image>();
			int i = 0;
			Observable.Timer(TimeSpan.FromSeconds(itemDisplayInterval), TimeSpan.FromSeconds(itemDisplayInterval))
				.Select(x => foodStuffList)
				.Take(foodStuffList.Count)
				.Subscribe(_ => {
					FoodStuff foodStuff = foodStuffList[i++];
					GameObject obj = new GameObject(foodStuff.item.ToString());
					obj.transform.SetParent(grid.transform, false);
					Instantiate(effect).transform.SetParent(obj.transform, false);
					Image image = obj.AddComponent<Image>();
					image.sprite = foodStuff.itemSprite;
					imageList.Add(image);
					audioSource.PlayOneShot(effectSe);
				});

			foodStuffList.ToObservable().Select(x => x.item).Subscribe(x => Debug.Log(x));
		}
	}
}
