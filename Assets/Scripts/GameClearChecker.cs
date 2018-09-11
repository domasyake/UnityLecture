using UnityEngine;

public class GameClearChecker : MonoBehaviour{
	[SerializeField] private UnityChanEventControll unityChanEventControll;
	[SerializeField] private GameObject enemy;
	
	private Timer timer;

	private void Start (){
		timer = this.GetComponent<Timer>();
	}
	
	private void Update () {
		if (timer.gameTime == 0){
			unityChanEventControll.GameClear();//ゲームクリア関数実行
			Destroy(enemy);//エネミー破壊
			Destroy(this);//1回実行すれば用済みなので自殺
		}		
	}
}

