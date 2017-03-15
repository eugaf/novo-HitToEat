using UnityEngine;
using System.Collections;

public class SocoAnimacaoResetar : StateMachineBehaviour {

	public Jogador jogadorReferencia;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//	jogadorReferencia.socoEstaSocando = true;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		for (float i = 1; i > 0; i -= 0.1f * Time.deltaTime) {
//			jogadorReferencia._animator.SetLayerWeight(1,i);
//		}
		jogadorReferencia._animator.SetLayerWeight(1,0);
		jogadorReferencia.socoEstaSocando = false;
		jogadorReferencia.socoPodeSocar = true;
		jogadorReferencia.socoCarregado = jogadorReferencia.socoForcaInicio;		
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
