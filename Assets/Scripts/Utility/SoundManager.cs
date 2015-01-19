using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// サウンド管理クラス
/// </summary>
public class SoundManager : SingletonMonoBehaviour<SoundManager> {
    //-------------------------------------------------------------------------
    #region // 宣言・定数

    #endregion
    //-------------------------------------------------------------------------
    #region // Inspectorで設定するprivate変数

    /// <summary>
    /// サウンドファイルが置かれるResources内パス
    /// </summary>
    [SerializeField]
    private string soundsPath = "Sounds";

    #endregion
    //-------------------------------------------------------------------------
    #region // private変数

    /// <summary>
    /// キャッシュ用変数
    /// </summary>
    private HashSet<AudioSource> cache = new HashSet<AudioSource>();

    /// <summary>
    /// Resourcesに含まれるサウンドファイルリスト
    /// </summary>
    private Dictionary<string, AudioClip> _clips;

    #endregion
    //-------------------------------------------------------------------------
    #region // privateプロパティ

    /// <summary>
    /// Resourcesに含まれるサウンドファイルリスト
    /// </summary>
    private Dictionary<string, AudioClip> Clips {
        get {
            if( this._clips == null ) {
                this._clips = Resources.LoadAll<AudioClip>( this.soundsPath ).ToDictionary( _ => _.name, _ => _ );
            }
            return this._clips;
        }
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // public関数

    /// <summary>
    /// サウンド再生
    /// </summary>
    /// <param name="clipname"></param>
    /// <param name="volume"></param>
    /// <param name="isLoop"></param>
    /// <returns></returns>
    public AudioSource Play( string clipname, float volume, bool isLoop ) {
        // ファイル存在チェック
        if( !this.Clips.ContainsKey( clipname ) ) {
            Debug.LogError( "指定されたサウンドファイル 「" + clipname + "」 が見つかりません。" );
            return null;
        }

        // AudioSourceを生成またはキャッシュから取得
        var source = GetAudioSource();
        // パラメータ設定
        source.clip = this.Clips[ clipname ];
        source.volume = volume;
        source.loop = isLoop;
        // 再生
        source.Play();

        return source;
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // private関数

    /// <summary>
    /// AudioSourceを生成、またはキャッシュから取得
    /// </summary>
    /// <returns>AudioSource</returns>
    private AudioSource GetAudioSource() {
        AudioSource ret;

        // キャッシュ内を検索し、使えるものがあればそれを返す
        ret = this.cache.FirstOrDefault( _ => !_.isPlaying );
        if( ret != null ) { return ret; }

        // 見つからないので新規に生成する
        var obj = new GameObject( "AudioSource" );
        obj.transform.parent = this.transform;
        ret = obj.AddComponent<AudioSource>();

        // キャッシュに積む
        this.cache.Add( ret );

        return ret;
    }

    #endregion
    //-------------------------------------------------------------------------

}
