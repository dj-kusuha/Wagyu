using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルシーンの管理クラス
/// </summary>
public class TitleManager : MonoBehaviour {

    /// <summary>
    /// BGM
    /// </summary>
    [SerializeField, Tooltip( "BGM" )]
    private SoundManager.Sounds bgm;

    /// <summary>
    /// リスタートSE
    /// </summary>
    [SerializeField, Tooltip( "スタートSE" )]
    private SoundManager.Sounds[] startSE;


    private AudioSource bgmSource;

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start() {
        this.bgmSource = SoundManager.Instance.Play( this.bgm, isLoop: true );
    }

    /// <summary>
    /// 破棄時の処理
    /// </summary>
    private void OnDestroy() {
        this.bgmSource.Stop();
    }

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
