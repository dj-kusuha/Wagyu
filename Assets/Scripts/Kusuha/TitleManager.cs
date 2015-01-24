using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルシーンの管理クラス
/// </summary>
public class TitleManager : MonoBehaviour {

    /// <summary>
    /// リスタートSE
    /// </summary>
    [SerializeField, Tooltip( "スタートSE" )]
    private SoundManager.Sounds[] startSE;

    /// <summary>
    /// スタートボタン押下時の処理
    /// </summary>
    public void OnStartButton() {
        // SE再生
        SoundManager.Instance.RandomSEPlay( this.startSE );
        // メインゲームのシーンへ移行
        Application.LoadLevel( "MainGame" );
    }

}
