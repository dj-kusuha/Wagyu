using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ゲームのマネージャクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager> {
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

    #endregion
    //-------------------------------------------------------------------------
    #region // Inspectorで設定するprivate変数

    /// <summary>
    /// 時間を表示するText
    /// </summary>
    [SerializeField, Tooltip( "時間を表示するTextを指定します" ), Header( "Parameter Labels" )]
    private Text timeText;

    /// <summary>
    /// プレイヤーの初期ヒットポイント
    /// </summary>
    [SerializeField, Tooltip( "プレイヤーの初期ヒットポイントを指定します" ), Header( "Settings" )]
    private int PlayerStartHP;

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

        // ヒットポイントを減らす
        this.hp -= damage;
        // ヒットポイントが0以下になったら終わり
        if( this.hp <= 0 ) {
            ChangePhase( Phase.Dead );
        }
    }

    /// <summary>
    /// ゴールに当たった時
    /// </summary>
    public void OnHitGoal() {
        Debug.Log( "OnHitGoal" );

        ChangePhase( Phase.Goal );
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // private関数

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
                    Application.LoadLevel( Application.loadedLevel );
                    this.phase = Phase.None;
                }
                break;

            // ゴールしたよ
            case Phase.Goal:
                // TODO: とりあえずキー押し直したらリスタート。この辺はちょっと変える
                if( Input.anyKeyDown ) {
                    Application.LoadLevel( Application.loadedLevel );
                    this.phase = Phase.None;
                }
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
        this.timeText.text = this.time.ToString( "f2" );
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <remarks>ゲーム開始時に必ず呼ぶ</remarks>
    private void Initialize() {
        this.time = 0f;
        this.hp = this.PlayerStartHP;
        this.player = GameObject.FindWithTag( "Player" );
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
                // 今はとりあえずユニティちゃんを削除しておく
                Destroy( this.player );
                break;
            case Phase.Goal:
                break;

            default:
                Debug.LogError( "Unknown Phase: " + this.phase );
                break;
        }

        this.phase = nextPhase;
    }


    #endregion
    //-------------------------------------------------------------------------
}