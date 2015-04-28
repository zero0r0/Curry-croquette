using UnityEngine;
using System.Collections;

/// <summary>
/// ステージ管理用クラス
/// </summary>
public class StageManager : MonoBehaviour {

    public void TransitionToEndingScene() {
        Application.LoadLevel("Ending");
    }
}
