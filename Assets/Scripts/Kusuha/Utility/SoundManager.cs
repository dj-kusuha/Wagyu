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

    /// <summary>
    /// サウンド定義
    /// </summary>
    public enum Sounds {
        SEPositive,
        SENegative,
    }

    /// <summary>
    /// サウンド定義と実際のファイルの名前を紐付けるディクショナリ
    /// </summary>
    private static readonly Dictionary<Sounds, string> SoundsDictionary = new Dictionary<Sounds, string>(){
        { Sounds.SEPositive, "se_positive" },
        { Sounds.SENegative, "se_negative" },
    };

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
    /// <param name="sounds">再生するサウンド</param>
    /// <param name="volume">音量( 0f ～ 1f )</param>
    /// <param name="isLoop">ループフラグ( true : ループする )</param>
    /// <returns>再生を開始したAudioClip</returns>
    public AudioSource Play( Sounds sounds, float volume = 1f, bool isLoop = false ) {
        var clip = GetAudioClip( sounds );
        // ファイル存在チェック
        if( clip == null ) {
            Debug.LogError( "指定されたサウンド「" + sounds + "」 が見つかりません。" );
            return null;
        }

        // AudioSourceを生成またはキャッシュから取得
        var source = GetAudioSource();
        // パラメータ設定
        source.clip = clip;
        source.volume = volume;
        source.loop = isLoop;
        // 再生
        source.Play();

        return source;
    }

    /// <summary>
    /// サウンド停止
    /// </summary>
    /// <param name="clipname">停止するクリップ名</param>
    public void Stop( string clipname ) {
        // 指定のクリップ名のクリップを鳴らしているものの再生を止める
        var sources = this.cache.Where( _ => _.isPlaying && _.clip.name == clipname );
        foreach( var s in sources ) {
            s.Stop();
        }
    }

    /// <summary>
    /// 全てのサウンド停止
    /// </summary>
    public void StopAll() {
        foreach( var s in this.cache ) {
            if( s.isPlaying ) { s.Stop(); }
        }
    }

    /// <summary>
    /// キャッシュをクリアする
    /// </summary>
    /// <remarks>同時再生するサウンドが多い場合、適当なタイミングで呼んでください。</remarks>
    public void ClearCache() {
        // 再生中ではないものを破棄する
        var sources = this.cache.Where( _ => !_.isPlaying );
        foreach( var s in sources ) { Destroy( s.gameObject ); }
        // 再生中のものだけにしたHashSetを新たに生成
        this.cache = new HashSet<AudioSource>( this.cache.Where( _ => _.isPlaying ) );
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

    /// <summary>
    /// 指定された名前のAudioClipを取得する
    /// </summary>
    /// <param name="sounds">サウンド定義</param>
    /// <returns>AudioClip</returns>
    private AudioClip GetAudioClip( Sounds sounds ) {
        AudioClip ret = null;
        if( SoundsDictionary.ContainsKey( sounds ) ) {
            this.Clips.TryGetValue( SoundsDictionary[ sounds ], out ret );
        }
        return ret;
    }

    #endregion
    //-------------------------------------------------------------------------

}