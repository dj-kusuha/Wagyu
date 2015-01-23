using UnityEngine;
using System.Collections;

/// <summary>
/// ゲームのマネージャクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager> {
    //-------------------------------------------------------------------------
    #region // Inspectorで設定するprivate変数

    /// <summary>
    /// プレイヤーの初期ヒットポイント
    /// </summary>
    [SerializeField, Tooltip( "プレイヤーの初期ヒットポイント" )]
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



    #endregion
    //-------------------------------------------------------------------------
    #region // public変数

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <remarks>ゲーム開始時に必ず呼ぶ</remarks>
    public void Initialize() {
        this.time = 0f;
        this.hp = this.PlayerStartHP;
        this.player = GameObject.FindWithTag( "Player" );
    }

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

    #endregion
    //-------------------------------------------------------------------------
}
