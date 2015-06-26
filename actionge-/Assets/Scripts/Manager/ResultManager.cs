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
		public AudioClip croquette;

		public Sprite curryCroquette;
		public Sprite croquetteImage;

		// Use this for initialization
		void Start() {
			audioSource = GetComponent<AudioSource>();

			List<FoodStuff> foodStuffList = ItemBox.Instance.GetAllCollectedList<FoodStuff>();
			Destroy(ItemBox.Instance.gameObject);
			Destroy(ItemBox.Instance);

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
				}, () => {
					GameObject obj = new GameObject("Curry");
					obj.transform.SetParent(canvas.transform, false);
					obj.transform.localScale = new Vector3(3, 3, 3);
					Image image = obj.AddComponent<Image>();
					if (foodStuffList.Count >= 4) {
						image.sprite = curryCroquette;
						audioSource.PlayOneShot(croquette);
					} else {
						image.sprite = croquetteImage;
					}
					Observable.Timer(TimeSpan.FromSeconds(0f), TimeSpan.FromSeconds(1f))
						.Take(1)
						.Subscribe(_ => {
							var text = FindObjectOfType<Text>();
							text.text = "ゲームクリア！！！！";
						});
					this.UpdateAsObservable()
						.Where(x => Input.anyKeyDown)
						.First()
						.Subscribe(_ => Application.LoadLevel("Title"));
				}).AddTo(gameObject);

			foodStuffList.ToObservable().Select(x => x.item).Subscribe(x => Debug.Log(x));
		}
	}
}
