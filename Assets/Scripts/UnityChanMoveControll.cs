using System.Collections;
using UnityEngine;

public class UnityChanMoveControll : MonoBehaviour{
	[SerializeField] private float speed;//移動速度
	[SerializeField] private float rotationDouble;//回転の倍率

	private Rigidbody rigidbody;

	private PlayAbleSimpleController playAbleSimpleController;

	private void Start (){
		rigidbody = this.GetComponent<Rigidbody>();
		playAbleSimpleController = this.GetComponent<PlayAbleSimpleController>();
	}
	
	void FixedUpdate (){
		float horizontal = Input.GetAxis("Horizontal");
		Rotate(horizontal);//水平入力を渡して回転
		Run(Input.GetAxis("Vertical"),horizontal);//垂直入力を渡して前進後退
		if (Input.GetKeyDown(KeyCode.Q)){
			playAbleSimpleController.TransAnimation("smile@sd_hmd",1);//笑顔!!!!!!
		}
	}

	private void Run(float vertical,float horizontal){
		if(vertical==0){//垂直入力が0ならZ方向の加速度を0に
			rigidbody.velocity=new Vector3(rigidbody.velocity.x,rigidbody.velocity.y,0);
			//
			playAbleSimpleController.TransAnimation("Standing@loop");//停止時は立ちアニメ
			//
		}else{
			if (horizontal != 0){
				vertical *= 0.5f;//水平入力がある＝回転中の時は減速
			}
			rigidbody.AddRelativeForce(new Vector3(0, 0, vertical * speed),ForceMode.Acceleration);
			//AddRelativeForce(加速量,加速オプション)
			playAbleSimpleController.TransAnimation("Running@loop");//走るアニメ
		}
	}
			
	private void Rotate(float inp){
		transform.Rotate(new Vector3(0, 1, 0), inp*rotationDouble);
		//Rotate(回転の軸にするベクトル,回転量)
	}

	private void Jump(){
		
	}
	
	public void OnCallChangeFace(){}

}

