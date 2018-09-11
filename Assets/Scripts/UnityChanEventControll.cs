using UnityEngine;

public class UnityChanEventControll : MonoBehaviour {

	public void GameClear(){
		Destroy(this.GetComponent<UnityChanMoveControll>());
		var play_able_simple_controller = this.GetComponent<PlayAbleSimpleController>();
		play_able_simple_controller.TransAnimation("smile@sd_hmd",1);//笑って
		play_able_simple_controller.TransAnimation("Salute");//跳ねる
	}

	public void GameOver(){
		Destroy(this.GetComponent<UnityChanMoveControll>());//移動を止めるためにスクリプトを破棄
		var play_able_simple_controller = this.GetComponent<PlayAbleSimpleController>();
		play_able_simple_controller.TransAnimation("GoDown");//ダウンアニメを再生
	}
	
	public void OnCallChangeFace(){}
}

