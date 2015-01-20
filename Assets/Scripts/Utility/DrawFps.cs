using UnityEngine;

/// <summary>
/// FPSを描画するデバッグ用クラス
/// </summary>
public class DrawFps : MonoBehaviour {
    /// <summary>
    /// 更新頻度を指定します(秒)
    /// </summary>
    [SerializeField, Tooltip( "更新頻度を指定します(秒)" )]
    private float updateTime = 1f;

    /// <summary>
    /// 描画位置及び描画範囲を指定します
    /// </summary>
    [SerializeField, Tooltip("描画位置及び描画範囲を指定します")]
    private Rect rect;

    /// <summary>
    /// 描画時のスタイルを指定します
    /// </summary>
    [SerializeField, Tooltip("描画時のスタイルを指定します")]
    private GUIStyle guiStyle;

    /// <summary>
    /// タイマー用変数
    /// </summary>
    private float timer;

    /// <summary>
    /// フレームカウンタ
    /// </summary>
    private int count;

    /// <summary>
    /// 計測したFPS
    /// </summary>
    private float fps;

    /// <summary>
    /// 定期処理
    /// </summary>
    void Update() {
        this.timer += Time.deltaTime;
        ++this.count;

        // 更新時間が経過したら更新する
        if( this.timer > this.updateTime ) {
            this.fps = this.count / this.timer;
            this.timer = 0f;
            this.count = 0;
        }
    }

    /// <summary>
    /// GUI処理
    /// </summary>
    void OnGUI() {
        GUI.Label( this.rect, "FPS: " + this.fps.ToString( "f2" ), this.guiStyle );
    }

}