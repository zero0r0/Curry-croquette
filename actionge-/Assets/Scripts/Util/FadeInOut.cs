using UnityEngine;
using System.Collections;

public class FadeInOut : SingletonMonoBehaviour<FadeInOut> {

	private Texture2D texture;
	private Color now;

	new void Awake() {
		CheckInstance();
		texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture.SetPixel(0, 0, Color.white);
		texture.Apply();
	}

	public void FadeIn(float fadeTime, Color color) {
		now = color;
		now.a = 0;
		StartCoroutine(TranslateFade(fadeTime));
	}

	public void FadeOut(float fadeTime, Color color) {
		now = color;
		now.a = 1;
		StartCoroutine(TranslateFade(fadeTime));
	}

	void OnGUI() {
		GUI.color = now;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
	}

	IEnumerator TranslateFade(float fadeTime) {
		float nowTime = 0f;
		float fadeRate = 1 / fadeTime / 30;
		if (now.a == 1)
			fadeRate *= -1;

		while (Mathf.Abs(nowTime) < fadeTime) {
			now.a += fadeRate;
			nowTime += fadeRate;
			yield return new WaitForSeconds(Mathf.Abs(fadeRate));
		}

	}
}
