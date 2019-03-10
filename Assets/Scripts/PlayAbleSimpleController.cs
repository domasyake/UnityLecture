using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

		
//<summary>
//　AnimationControllerのレイヤーごとにコンバータを保持することで各レイヤーごとのアニメーションブレンドを実現します
//　AnimatorWindowのLayersの上から順にIndex番号が振られます
//　DefoltのAnimationClipだけは各レイヤーに登録しておいてください
//</summary>
public class PlayAbleSimpleController : MonoBehaviour{
	//LayerMaskと適用するLayerのIndexの組み合わせ
	[Serializable]private struct MaskBox{
		[SerializeField] public AvatarMask layerMask;
		[SerializeField] public uint numberToMask;
	}
		
	[SerializeField] private float transtime;//次のアニメーションまでの遷移時間
	[SerializeField] private MaskBox[] maskBoxs;

	[SerializeField] private List<AnimationClip> animationClips;//自身で保持するAnimationClip
		
	private List<Converter> converters = new List<Converter>();
		
	private AnimationLayerMixerPlayable layerMixer;//PlayerAbleで複数レイヤーを扱うために必要
	private PlayableGraph graph;//PlayAbleとの接続


	private void Awake(){
		graph = PlayableGraph.Create ();	
		var anim = GetComponent<Animator>();
		
		var defo=new List<AnimationClip>();
		//全レイヤーの初期アニメを取得し、デフォルトアニメとしてコンバータにセット
		foreach (var i in Enumerable.Range(0,anim.layerCount)){
			var temp = anim.GetCurrentAnimatorClipInfo(i)[0].clip;
			converters.Add(new Converter(graph,temp));
			defo.Add(temp);
			Debug.Log("Layer num "+i+",defoult anim "+temp.name);
		}

		//LayerMask情報をコンバータにセット
		layerMixer = AnimationLayerMixerPlayable.Create(graph, anim.layerCount);
		foreach (var item in maskBoxs){
			layerMixer.SetLayerMaskFromAvatarMask(item.numberToMask,item.layerMask );	
		}
			
		var output = AnimationPlayableOutput.Create (graph, "output", anim);//自身のAnimatorを出力先に決定
		output.SetSourcePlayable(layerMixer);
		graph.Play ();
	}

	//Destory時に呼ばれるメソッド
	private void OnDestroy(){
		converters = new List<Converter>();
		graph.Destroy();//PlayableGraphは不要になった段階でリリースしておかないと最悪クラッシュする
	}
	

	/*
	//<public Methods>
	*/
	public void TransAnimation(AnimationClip animation_clip,int layernum=0){//指定レイヤーのアニメを遷移する
		var conv = InitConverterToLayer(animation_clip,layernum);
		StartCoroutine(Trans(conv));		
	}

	public void TransAnimation(string name,int layernum=0){//メソッドのオーバーロード
		TransAnimation(ConvertClipByString(name),layernum);
	}

	public void StopAnimation(AnimationClip animation_clip){
		foreach (var item in converters){
			if (item.GetCurrentMotion == animation_clip){
				item.RollBack();
				StartCoroutine(Trans(item));
			}
		}
	}

	public void PauseAnimation(){
		foreach (var item in converters){
			item.Pause();
		}
	}
	/*
	//</public Methods>
	*/

	
	/*
	//<private Methods>
	*/
	private Converter InitConverterToLayer(AnimationClip animation_clip,int layernum){
		var conv = converters[layernum];
		graph.Disconnect(layerMixer,layernum);
		conv.ReConnect(animation_clip);
		layerMixer.ConnectInput(layernum,conv.mixer,0,1);
		return conv;
	}

	private AnimationClip ConvertClipByString(string str){//保持しているAnimationClipから名前で検索する
		return animationClips.Find(n => n.name == str);
	}
	/*
	//</private Methods>
	*/


	private IEnumerator Trans(Converter conv){//渡されたコンバータのアニメの遷移を行う
		if (!conv.PlayAble) yield break;
		var wait_time = Time.timeSinceLevelLoad + transtime;
		yield return new WaitWhile(() =>{
			var diff = wait_time - Time.timeSinceLevelLoad;
			if (diff <= 0){
				conv.mixer.SetInputWeight(1, 0);
				conv.mixer.SetInputWeight(0, 1);
				return false;
			}else{
				var rate = Mathf.Clamp01(diff / transtime);
				conv.mixer.SetInputWeight(1, rate);
				conv.mixer.SetInputWeight(0, 1 - rate);
				return true;
			}
		});
	}
	
	
	
	
	/// <summary>
	/// this class have current and before AnimationClipPlayAble.
	/// Combine them to realize anim transitions.
	/// </summary>
	private class Converter{
		private readonly AnimationClip defoultAnim;

		public AnimationClip GetCurrentMotion{ get; private set; }

		public bool PlayAble{ get; private set; }
			
		public readonly AnimationMixerPlayable mixer;
		private AnimationClipPlayable prePlayable; 
		private AnimationClipPlayable currentPlayable;
		private PlayableGraph graph;
	

		public Converter(PlayableGraph graph,AnimationClip anim_clip){
			this.prePlayable = AnimationClipPlayable.Create(graph, null);
			this.mixer = AnimationMixerPlayable.Create(graph, 2);
			this.graph = graph;
			defoultAnim = anim_clip;
			currentPlayable = AnimationClipPlayable.Create(graph, anim_clip);
			GetCurrentMotion = defoultAnim;
		}
			
		
		/*
		//<public Methods>
		*/
		public void ReConnect(AnimationClip animation_clip){
			ConnectProcess(animation_clip);
		}
		public void RollBack(){
			ConnectProcess(defoultAnim);
		}

		public void Pause(){
			currentPlayable.Pause();
		}
			
		/*
		//</public Methods>
		*/

			
		private void ConnectProcess(AnimationClip animation_clip){
			if (animation_clip == GetCurrentMotion){
				PlayAble = false;
				return;
			}
			PlayAble = true;
			graph.Disconnect(mixer, 0);
			graph.Disconnect(mixer, 1);
			if (prePlayable.IsValid()){
				prePlayable.Destroy();
			}
			prePlayable = currentPlayable;
			GetCurrentMotion = animation_clip;
			currentPlayable = AnimationClipPlayable.Create(graph, animation_clip);
			mixer.ConnectInput(1, prePlayable, 0);
			mixer.ConnectInput(0, currentPlayable, 0);
		}
	}
}