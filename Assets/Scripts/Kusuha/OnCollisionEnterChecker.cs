using UnityEngine;
using System.Collections;

/// <summary>
/// コリジョン判定クラス
/// </summary>
public class OnCollisionEnterChecker : MonoBehaviour {
    
    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider (Since v1.0)
    private void OnCollisionEnter( Collision collision ) {
        Debug.Log( "OnCollisionEnter: " + collision.gameObject.name );

        switch( collision.gameObject.tag ) {
            // 敵と当たった
            case "Enemy":
                GameManager.Instance.OnDamage( 1 );
                break;
            // ゴールと当たった
            case "Goal":
                GameManager.Instance.OnHitGoal();
                break;
            default:
                break;
        }
    }

}
