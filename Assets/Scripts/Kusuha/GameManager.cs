using UnityEngine;
using System.Collections;

/// <summary>
/// ゲームのマネージャクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager> {

    
    private GameObject _player;
    private GameObject Player {
        get {
            if( this._player == null ) {
                this._player = GameObject.FindWithTag( "Player" );
            }
            return this._player;
        }
    }

    public void OnHitEnemy() {
        Debug.Log( "OnHitEnemy" );

        Destroy( this );
        Destroy( this.Player );
    }

    public void OnHitGoal() {
        Debug.Log( "OnHitEnemy" );
    }
}
