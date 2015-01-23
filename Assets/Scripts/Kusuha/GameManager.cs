using UnityEngine;
using System.Collections;

/// <summary>
/// ゲームのマネージャクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager> {

    [SerializeField]
    private GameObject unityChan;

    public void OnHitEnemy() {
        Debug.Log( "OnHitEnemy" );

        Destroy( this );
        Destroy( this.unityChan );
    }

    public void OnHitGoal() {
        Debug.Log( "OnHitEnemy" );
    }
}
