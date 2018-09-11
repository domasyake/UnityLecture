using UnityEngine;

public class CameraControll : MonoBehaviour{
	[SerializeField]private GameObject defoultCamera;//デフォルトカメラ
	[SerializeField] private GameObject backCamera;//バックビュー用カメラ
	
	private void Update () {
		//マウスの左クリック入力があるときはdefoultを、ない時はbackをActiveにする
		if (Input.GetMouseButton(0)){
			defoultCamera.SetActive(false);
			backCamera.SetActive(true);
		}else{
			defoultCamera.SetActive(true);
			backCamera.SetActive(false);
		}	
	}
}
