using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour{
	public float gameTime;

	public bool isActive;//タイマーを止めれるようにする
	private Text tex;

	private void Start (){
		tex = this.GetComponent<Text>();
		isActive = true;
	}

	private void Update (){
		if(!isActive)return;//returnするとそこから下は実行されない
		gameTime -= Time.deltaTime;
		if (gameTime <= 0){
			gameTime = 0;
		}
		tex.text = Mathf.Floor(gameTime) + "s";//小数点以下は切り捨てて表示
	}
}

