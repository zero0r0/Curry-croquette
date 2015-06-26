using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

/// <summary>
/// フェードイン・アウトユーティリティ
/// </summary>
[RequireComponent(typeof(Canvas))]
public class FadeInOutUtil : SingletonMonoBehaviour<FadeInOutUtil> {

	// フェードイン・アウトさせるImage
	public Image FadeImage { private get; set; }

	/// <summary>
	/// フェードイン・アウトに使用するスプライト画像
	/// </summary>
	public Sprite sprite;

	private const float fixedUpdateRate = 0.02f;

	// シングルトンベースクラスのAwakeメソッドを呼ぶ
	new void Awake() {
		base.Awake();
		InitFadeImage();
	}
	
	// フェードイン・アウトに使用する画像の初期化を行う
	private void InitFadeImage() {
		// 画面すべてを覆うサイズのフェード用画像を生成する
		var fadeImageRectTransform = (new GameObject("FadeIOImage")).AddComponent<RectTransform>();
		fadeImageRectTransform.anchoredPosition = new Vector2(0.5f, 0.5f);
		fadeImageRectTransform.anchorMin = new Vector2(0, 0);
		fadeImageRectTransform.anchorMax = new Vector2(1, 1);
		
		// 生成した画像をCanvasの子にする
		fadeImageRectTransform.transform.SetParent(GetComponent<Canvas>().transform, false);
		FadeImage = fadeImageRectTransform.gameObject.AddComponent<Image>();
		FadeImage.sprite = sprite;

		// 画像を透明にする
		FadeImage.color = new Color(0, 0, 0, 0);
	}

	/// <summary>
	/// 指定の色でフェードインする
	/// </summary>
	/// <param name="fadeInTime">フェードインが終了するまでの時間</param>
	/// <param name="fadeColor">フェードインの色</param>
	public void FadeIn(float fadeInTime, Color fadeColor, Action completeAction = null) {
		FadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
		this.FixedUpdateAsObservable().Take((int) (fadeInTime * 1 / fixedUpdateRate)).Subscribe(x =>
			FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, FadeImage.color.a + fixedUpdateRate / fadeInTime),
			() => { if (completeAction != null) completeAction(); }
		);
	}

	/// <summary>
	/// 指定の時間と色でフェードアウトする
	/// </summary>
	/// <param name="fadeOutTime">フェードアウトが終了するまでの時間</param>
	/// <param name="fadeColor">フェードアウトの色</param>
	public void FadeOut(float fadeOutTime, Color fadeColor, Action completeAction = null) {
		FadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1);
		this.FixedUpdateAsObservable().Take((int) (fadeOutTime * 1 / fixedUpdateRate)).Subscribe(x =>
			FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, FadeImage.color.a - fixedUpdateRate / fadeOutTime),
			() => { if (completeAction != null) completeAction(); }
		);
	}

}
