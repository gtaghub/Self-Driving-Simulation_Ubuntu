using UnityEngine;
using System.Collections;

public class StateMachineBehaviourScript : StateMachineBehaviour {

	override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Debug.Log("OnStateEnter ");
	}
}
