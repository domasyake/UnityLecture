using UnityEngine;
using UnityEngine.AI;

public class EnemyAgentControll : MonoBehaviour {
	[SerializeField]private GameObject target;
	private NavMeshAgent agent;

	private void Start () {
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update(){
		agent.destination = target.transform.position;// ターゲットの座標を目的地に設定する。
	}
}

