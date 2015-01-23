using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// ゲームのマネージャクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager> {
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
    private bool IsPlaying { get; set; }

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
            // 今はとりあえずユニティちゃんを削除しておく
            Destroy( this.player );

            this.IsPlaying = false;
        }
    }

    /// <summary>
    /// ゴールに当たった時
    /// </summary>
    public void OnHitGoal() {
        Debug.Log( "OnHitGoal" );
    }

    #endregion
    //-------------------------------------------------------------------------
    #region // private関数

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start() {
        Initialize();
    }

    /// <summary>
    /// 定期処理
    /// </summary>
    private void Update() {
        // ゲームプレイ中
        if( this.IsPlaying ) {
            // パラメータ更新
            UpdateParameters();
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

        this.IsPlaying = true;
    }


    #endregion
    //-------------------------------------------------------------------------
}