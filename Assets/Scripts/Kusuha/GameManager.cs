using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ゲームのマネージャクラス
/// </summary>
public class GameManager : MonoBehaviour {
    //-------------------------------------------------------------------------
    #region // 宣言・定数

    /// <summary>
    /// フェーズ定義
    /// </summary>
    private enum Phase {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// プレイスタート
        /// </summary>
        PlayStart,

        /// <summary>
        /// ゲームプレイ
        /// </summary>
        Play,

        /// <summary>
        /// ポーズ
        /// </summary>
        Pause,

        /// <summary>
        /// 死亡
        /// </summary>
        Dead,

        /// <summary>
        /// ゴール
        /// </summary>
        Goal,

    }

    /// <summary>
    /// フェードイン・アウトの時間
    /// </summary>
    private const float FadeTime = 0.25f;

    /// <summary>
    /// サウンドの音量
    /// </summary>
    private const float SoundVolume = 1f;

    /// <summary>
    /// 机の高さ
    /// </summary>
    private const float DeskHeight = 1278.68f;

    [System.Serializable]
    private struct StartPosition {
        public Vector3 positon;
        public Vector3 rotation;        
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // Inspectorで設定するprivate変数

    /// <summary>
    /// leafのGameObject
    /// </summary>
    [SerializeField, Tooltip( "leafを指定します。" ), Header( "Game Objects" )]
    private GameObject leaf;


    /// <summary>
    /// UI RootのGameObject
    /// </summary>
    [SerializeField, Tooltip( "UI Rootを指定します。" ), Header( "UI Objects" )]
    private GameObject uiRoot;

    /// <summary>
    /// 死亡時に表示を行うCanvas
    /// </summary>
    [SerializeField, Tooltip( "死亡時に表示を行うCanvasのPrefabを指定します。" )]
    private GameObject deadCanvas;

    /// <summary>
    /// クリア時に表示を行うCanvas
    /// </summary>
    [SerializeField, Tooltip( "クリア時に表示を行うCanvasのPrefabを指定します。" )]
    private GameObject resultCanvas;

    /// <summary>
    /// ヒットポイントを表示するText
    /// </summary>
    [SerializeField, Tooltip( "ヒットポイントを表示するTextを指定します" )]
    private Text hitPointValueText;

    /// <summary>
    /// 高さを表示するText
    /// </summary>
    [SerializeField, Tooltip( "高さを表示するTextを指定します" )]
    private Text heightValueText;

    /// <summary>
    /// 距離を表示するText
    /// </summary>
    [SerializeField, Tooltip( "距離を表示するTextを指定します" )]
    private Text distanceValueText;

    /// <summary>
    /// 時間を表示するText
    /// </summary>
    [SerializeField, Tooltip( "時間を表示するTextを指定します" )]
    private Text timeValueText;

    /// <summary>
    /// プレイヤーの初期ヒットポイント
    /// </summary>
    [SerializeField, Tooltip( "プレイヤーの初期ヒットポイントを指定します" ), Header( "Settings" )]
    private int playerStartHP;

    /// <summary>
    /// プレイヤーの初期位置リスト
    /// </summary>
    [SerializeField, Tooltip( "プレイヤーの初期位置リストを指定します" )]
    private StartPosition[] playerStartPositions;

    /// <summary>
    /// BGMの指定
    /// </summary>
    [SerializeField, Tooltip( "BGMを指定します。スタート時にこの中からランダムで選ばれます。" ), Header( "Sound Settings" )]
    private SoundManager.Sounds[] bgms;

    /// <summary>
    /// ぶつかるSE
    /// </summary>
    [SerializeField, Tooltip( "ぶつかるSE" )]
    private SoundManager.Sounds[] damageSE;

    /// <summary>
    /// 死亡SE
    /// </summary>
    [SerializeField, Tooltip( "死亡SE" )]
    private SoundManager.Sounds[] deadSE;

    /// <summary>
    /// クリアSE
    /// </summary>
    [SerializeField, Tooltip( "クリアSE" )]
    private SoundManager.Sounds[] clearSE;

    /// <summary>
    /// リスタートSE
    /// </summary>
    [SerializeField, Tooltip( "リスタートSE" )]
    private SoundManager.Sounds[] restartSE;

    #endregion
    //-------------------------------------------------------------------------
    #region // private変数

    /// <summary>
    /// フェーズ
    /// </summary>
    private Phase phase;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float time;

    /// <summary>
    /// プレイヤーの現在のヒットポイント
    /// </summary>
    private int hp;

    /// <summary>
    /// プレイヤー(ユニティちゃん)のGameObject
    /// </summary>
    private GameObject player;

    /// <summary>
    /// 直近のBGM
    /// </summary>
    private static SoundManager.Sounds prevBgm;

    /// <summary>
    /// 現在のBGMのAudioSource
    /// </summary>
    private static AudioSource bgmSource;

    /// <summary>
    /// ゲームオーバー時のAudioSource
    /// </summary>
    private AudioSource gameoverJingleSource;

    #endregion
    //-------------------------------------------------------------------------
    #region // publicプロパティ

    /// <summary>
    /// インスタンスプロパティ
    /// </summary>
    public static GameManager Instance { get; private set; }

    #endregion
    //-------------------------------------------------------------------------
    #region // privateプロパティ

    /// <summary>
    /// ゲームをプレー中かどうか
    /// </summary>
    /// <remarks>死んだり、ゴールしたり、ポーズしたりしたらfalse</remarks>
    private bool IsPlaying { get { return this.phase == Phase.Play; } }

    #endregion
    //-------------------------------------------------------------------------
    #region // public変数

    /// <summary>
    /// ダメージを食らう処理
    /// </summary>
    public void OnDamage( int damage ) {
        Debug.Log( "OnDamage" );

        // プレイ中のみ
        if( this.phase == Phase.Play ) {
            // ヒットポイントを減らす
            this.hp -= damage;
            // ヒットポイントが0以下になったら終わり
            if( this.hp <= 0 ) {
                // 死んだ時SE
                SoundManager.Instance.RandomSEPlay( this.deadSE );

                ChangePhase( Phase.Dead );
            } else {
                // ダメージ食らったSE
                SoundManager.Instance.RandomSEPlay( this.damageSE );
            }
        }
    }

    /// <summary>
    /// ゴールに当たった時
    /// </summary>
    public void OnHitGoal() {
        Debug.Log( "OnHitGoal" );

        // プレイ中のみ
        if( this.phase == Phase.Play ) {
            ChangePhase( Phase.Goal );
        }
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // private関数

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake() {
        Instance = this;
    }

    /// <summary>
    /// 破棄時の処理
    /// </summary>
    private void OnDestroy() {
        if( Instance == this ) {
            Instance = null;
        }
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start() {
        ChangePhase( Phase.PlayStart );
    }

    /// <summary>
    /// 定期処理
    /// </summary>
    private void Update() {
        DebugUpdate();

        switch( this.phase ) {
            // なし
            case Phase.None:
                break;

            // プレイスタート
            case Phase.PlayStart:
                ChangePhase( Phase.Play );
                break;

            // プレイ中
            case Phase.Play:
                // パラメータ更新
                UpdateParameters();
                break;

            // ポーズ中
            case Phase.Pause:
                break;

            // 死んだよ
            case Phase.Dead:
                // キー押し直したらリスタート
                if( Input.anyKeyDown ) {
                    ReStart();
                }
                break;

            // ゴールしたよ
            case Phase.Goal:
                // ボタン待ちなので何もしないよ
                break;

            default:
                Debug.LogError( "Unknown Phase: " + this.phase );
                break;
        }
    }

    /// <summary>
    /// パラメータ更新処理
    /// </summary>
    private void UpdateParameters() {
        // 時間経過させる
        this.time += Time.deltaTime;
        // 各種Text更新
        var pos = this.player.transform.position;
        this.hitPointValueText.text = this.hp.ToString();
        this.heightValueText.text = ( pos.y - DeskHeight ).ToString( "f2" );
        this.distanceValueText.text = ( this.time * 7 ).ToString( "f2" );
        this.timeValueText.text = this.time.ToString( "f2" );
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <remarks>ゲーム開始時に必ず呼ぶ</remarks>
    private void Initialize() {
        // 各種変数の初期化
        this.time = 0f;
        this.hp = this.playerStartHP;
        this.player = GameObject.FindWithTag( "Player" );
        // プレイヤーの位置設定
        if( this.playerStartPositions.Length > 0 ) {
            var index = Random.Range( 0, this.playerStartPositions.Length );
            var p = this.playerStartPositions[ index ];
            this.player.transform.localPosition = p.positon;
            this.player.transform.localRotation = Quaternion.Euler( p.rotation );
        }
        // BGM再生
        PlayBgm();
        // サウンドマネージャをクリアする
        SoundManager.Instance.Clear();
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    private void PlayBgm() {
        // そもそも指定されてなければ何も再生しない
        if( this.bgms.Length == 0 ) { return; }

        // 指定されたBGM群の中からランダムで一つ選ぶ
        int index = Random.Range( 0, this.bgms.Length );
        var selected = this.bgms[ index ];
        // 最初から再生なのか、それとも一時停止から再生なのか
        bool isRestart = Random.Range( 0, 2 ) == 0;

        isRestart = true;

        // 直近のBGMと同じか調べる
        if( prevBgm == selected && isRestart ) {
            // 同じなのでそのままフェードイン再生
            bgmSource.volume = 0f;
            bgmSource.Play();
            PlayFadeIn( bgmSource, FadeTime, SoundVolume );
        } else {
            // BGMを一旦停止
            if( bgmSource != null ) {
                bgmSource.Stop();
            }
            // 改めて再生
            bgmSource = SoundManager.Instance.Play( selected, SoundVolume, true );
            // 直近のBGMを更新
            prevBgm = selected;
        }
    }

    /// <summary>
    /// リスタート処理
    /// </summary>
    private void ReStart() {
        // 死亡ジングル停止
        if( this.gameoverJingleSource != null ) {
            this.gameoverJingleSource.Stop();
        }
        // リスタートSE
        SoundManager.Instance.RandomSEPlay( this.restartSE );
        // BGMを一旦ポーズ
        bgmSource.Pause();
        // シーンロード
        Application.LoadLevel( Application.loadedLevel );
        // フェーズをなしにする
        this.phase = Phase.None;
    }

    /// <summary>
    /// フェーズ切り替え処理
    /// </summary>
    /// <param name="nextPhase">次のフェーズ</param>
    private void ChangePhase( Phase nextPhase ) {
        // 切り替えの前処理
        switch( nextPhase ) {
            case Phase.None:
                break;
            case Phase.PlayStart:
                Initialize();
                break;
            case Phase.Play:
                break;
            case Phase.Pause:
                break;
            case Phase.Dead:
                // 死亡ングル
                this.gameoverJingleSource = SoundManager.Instance.Play( SoundManager.Sounds.JingleGameOver, isLoop: true );
                // パラメータ更新
                UpdateParameters();
                // ユニティちゃんしぬ
                this.player.transform.FindChild("Armature").gameObject.SetActive( false );
                // 死亡画面出す
                this.deadCanvas.SetActive( true );
                // BGMフェードアウト
                StartCoroutine( FadeOutCoroutine( bgmSource, FadeTime, true ) );
                break;
            case Phase.Goal:
                // クリアジングル
                SoundManager.Instance.Play( SoundManager.Sounds.JingleClear );
                // ユニティちゃんを消してリーフを追加
                this.player.SetActive( false );
                this.leaf.SetActive( true );
                // リザルト画面出す
                StartCoroutine( ClearWaitSetActive( 5f ) );
                // BGMフェードアウト
                StartCoroutine( FadeOutCoroutine( bgmSource, FadeTime, true ) );
                break;

            default:
                Debug.LogError( "Unknown Phase: " + this.phase );
                break;
        }

        this.phase = nextPhase;
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // サウンド処理

    /// <summary>
    /// サウンドのフェードイン再生
    /// </summary>
    /// <param name="source">フェードインするAudioSource</param>
    /// <param name="fadeTime">フェードインする時間</param>
    /// <param name="volume">音量( 0f ～ 1f )</param>
    /// <param name="isLoop">ループフラグ( true : ループする )</param>
    /// <returns></returns>
    public AudioSource PlayFadeIn( AudioSource source, float fadeTime, float volume = 1f, bool isLoop = false ) {
        StartCoroutine( FadeInCoroutine( source, fadeTime, volume ) );
        return source;
    }

    /// <summary>
    /// サウンドのフェードアウト停止
    /// </summary>
    /// <param name="source">フェードアウト停止するAudioSource</param>
    /// <param name="fadeTime">フェードアウトする時間</param>
    public void StopFadeOut( AudioSource source, float fadeTime ) {
        StartCoroutine( FadeOutCoroutine( source, fadeTime, false ) );
    }

    /// <summary>
    /// サウンドのフェードアウト一時停止
    /// </summary>
    /// <param name="sound">フェードアウト一時停止するサウンド</param>
    /// <param name="fadeTime">フェードアウトする時間</param>
    public void PauseFadeOut( AudioSource source, float fadeTime ) {
        StartCoroutine( FadeOutCoroutine( source, fadeTime, true ) );
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // コルーチン
    /// <summary>
    /// フェードイン再生するコルーチン
    /// </summary>
    /// <param name="source">フェードイン再生するAudioSource</param>
    /// <param name="fadeTime">フェード時間</param>
    /// <param name="volume">最終的な音量</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator FadeInCoroutine( AudioSource source, float fadeTime, float volume = 1f ) {
        for( float timer = 0f; timer < fadeTime; timer += Time.deltaTime ) {
            source.volume = volume * ( timer / fadeTime );
            yield return null;
        }
        source.volume = volume;
    }

    /// <summary>
    /// フェードアウト停止するコルーチン
    /// </summary>
    /// <param name="source">フェードアウト停止するAudioSource</param>
    /// <param name="fadeTime">フェード時間</param>
    /// <param name="isPause">true の時、StopではなくPauseにする</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator FadeOutCoroutine( AudioSource source, float fadeTime, bool isPause ) {
        var prevVolume = source.volume;
        for( float timer = 0f; timer < fadeTime; timer += Time.deltaTime ) {
            source.volume = prevVolume * ( 1 - timer / fadeTime );
            yield return null;
        }

        if( isPause ) {
            source.Pause();
        } else {
            source.Stop();
        }

        source.volume = prevVolume;
    }

    /// <summary>
    /// 指定時間待って、GameObject.SetActiveを呼ぶ
    /// </summary>
    /// <param name="waitTime">待機時間</param>
    /// <param name="gameObject">GameObject</param>
    /// <param name="active">activeにするかどうか</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator ClearWaitSetActive( float waitTime ) {
        yield return new WaitForSeconds( waitTime );
        // リザルト画面描画
        this.resultCanvas.SetActive( true );
        // ユニティちゃんSE
        SoundManager.Instance.RandomSEPlay( this.clearSE );
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // ボタンコールバック

    /// <summary>
    /// もういっかい　ボタン押下時
    /// </summary>
    public void OnReStart() {
        Debug.Log( "OnReStart: " + this.phase );
        // Goalフェーズの時だけ
        if( this.phase == Phase.Goal ) {
            ReStart();
        }
    }

    /// <summary>
    /// タイトルへ　ボタン押下時
    /// </summary>
    public void OnGoToTitle() {
        // Goalフェーズの時だけ
        if( this.phase == Phase.Goal ) {
            // フェーズを切って
            this.phase = Phase.None;
            // BGM切って
            bgmSource.Stop();
            // タイトルシーンへ
            Application.LoadLevel( 0 );
        }
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // デバッグ用

    /// <summary>
    /// デバッグ用定期処理
    /// </summary>
    [System.Diagnostics.Conditional( "_DEBUG" )]
    private void DebugUpdate() {
        // リスタート処理
        if( Input.GetKey( KeyCode.Space ) ) {
            ReStart();
        }
    }

    #endregion
    //-------------------------------------------------------------------------
}