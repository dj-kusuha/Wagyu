using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルシーンの管理クラス
/// </summary>
public class TitleManager : MonoBehaviour {

    /// <summary>
    /// スタートボタン押下時の処理
    /// </summary>
    public void OnStartButton() {
        // SE再生
        SoundManager.Instance.Play( SoundManager.Sounds.SEPositive );
        // メインゲームのシーンへ移行
        Application.LoadLevel( "MainGame" );
    }

}
