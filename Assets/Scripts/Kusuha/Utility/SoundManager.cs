using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// サウンド管理クラス
/// </summary>
[RequireComponent( typeof( AudioListener ) )]
public class SoundManager : SingletonMonoBehaviour<SoundManager> {
    //-------------------------------------------------------------------------
    #region // 宣言・定数

    /// <summary>
    /// サウンド定義
    /// </summary>
    public enum Sounds {
        None,
        SEStart1,
        SEStart2,

        SEDamage1,
        SEDamage2,
        SEDamage3,

        SEDead1,
        SEDead2,

        SEClear1,
        SEClear2,

        SERestart1,
        SERestart2,

        BGMStage1,
    }

    /// <summary>
    /// サウンド定義と実際のファイルの名前を紐付けるディクショナリ
    /// </summary>
    private static readonly Dictionary<Sounds, string> SoundsDictionary = new Dictionary<Sounds, string>(){
        { Sounds.SEStart1, "univ1016" },
        { Sounds.SEStart2, "univ1072" },

        { Sounds.SEDamage1, "univ1091" },
        { Sounds.SEDamage2, "univ1092" },
        { Sounds.SEDamage3, "univ1094" },

        { Sounds.SEDead1, "univ1093" },
        { Sounds.SEDead2, "univ1230" },

        { Sounds.SEClear1, "univ1193" },
        { Sounds.SEClear2, "univ1018" },

        { Sounds.SERestart1, "univ1026" },
        { Sounds.SERestart2, "univ1027" },

        { Sounds.BGMStage1, "Stage1" },
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
    /// <param name="sound">再生するサウンド</param>
    /// <param name="volume">音量( 0f ～ 1f )</param>
    /// <param name="isLoop">ループフラグ( true : ループする )</param>
    /// <returns>再生を開始したAudioClip</returns>
    public AudioSource Play( Sounds sound, float volume = 1f, bool isLoop = false ) {
        var clip = GetAudioClip( sound );
        // ファイル存在チェック
        if( clip == null ) {
            Debug.LogError( "指定されたサウンド「" + sound + "」 が見つかりません。" );
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
    /// 指定されたサウンドの中からランダムでSEを再生する
    /// </summary>
    /// <param name="sounds">指定のサウンド群</param>
    /// <returns>再生されたサウンドのAudioSource</returns>
    public AudioSource RandomSEPlay( Sounds[] sounds ) {
        int index = Random.Range( 0, sounds.Length );
        return Play( sounds[ index ] );
    }


    internal void Clear() {
        var objs = GameObject.FindGameObjectsWithTag( "Audio" );
        var sources = objs.Select( _ => _.GetComponent<AudioSource>() ).Where( _ => !_.isPlaying );
        foreach( var s in sources ) {
            Destroy( s.gameObject );
        }
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

        // 新規に生成する
        var obj = new GameObject( "AudioSource" );
        obj.transform.parent = this.transform;
        ret = obj.AddComponent<AudioSource>();
        obj.tag = "Audio";

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