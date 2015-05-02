using UnityEngine;
using System.Collections;

public class FadeInOutUtil : SingletonMonoBehaviour<FadeInOutUtil> {

	Texture2D texture;
	Color now;

	new void Awake() {
		texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture.SetPixel(1, 1, Color.white);
		texture.Apply();
	}

	void OnGUI() {
		GUI.color = now;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
	}

	public void FadeIn(float fadeTime, Color color) {
		now = color;
		now.a = 0;
		StartCoroutine(StartFade(fadeTime));
	}

	public void FadeOut(float fadeTime, Color color) {
		now = color;
		now.a = 1;
		StartCoroutine(StartFade(fadeTime));
	}

	IEnumerator StartFade(float fadeTime) {
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
