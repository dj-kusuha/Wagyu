using UnityEngine;

/// <summary>
/// MonoBehaviourのシングルトンクラス
/// </summary>
/// <typeparam name="T">シングルトンにしたいMonoBehaviourクラス</typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T> {

    /// <summary>
    /// インスタンス変数
    /// </summary>
    private static T _instance;

    /// <summary>
    /// インスタンスを取得
    /// </summary>
    public static T Instance {
        get {
            if( _instance == null ) {
                // シーン上に無いか検索
                _instance = FindObjectOfType<T>();
                if( _instance == null ) {
                    // 無い場合は生成する
                    var obj = new GameObject( typeof( T ).ToString() );
                    _instance = obj.AddComponent<T>();

                    DontDestroyOnLoad( obj );
                    Debug.Log( "Create " + typeof( T ).ToString() );
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    virtual protected void Awake() {
        CheckInstance();
    }

    /// <summary>
    /// インスタンスの存在チェック
    /// </summary>
    /// <returns>存在していない場合 true</returns>
    protected bool CheckInstance() {
        if( _instance == null ) {
            _instance = (T)this;
            DontDestroyOnLoad( this );
            return true;
        } else if( Instance == this ) {
            return true;
        }

        Destroy( this );
        return false;
    }
}