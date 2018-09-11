using UnityEngine;

public class EnemyColliderControll : MonoBehaviour {
	private void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag("Player")){
			other.gameObject.GetComponent<UnityChanEventControll>().GameOver();//ゲームオーバーを起動
			Destroy(this.GetComponent<EnemyAgentControll>());//これ以上動かないようにControllを破棄
		}
	}
}
