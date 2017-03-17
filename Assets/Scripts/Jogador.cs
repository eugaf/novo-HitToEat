using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (Transform))]
[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (Animator))]
public class Jogador : MonoBehaviour {
	
	//Inputs
	public  static    Jogador   singleton;
	[HideInInspector]
    public  int       jogadorQualJogador;
	public  string 	  axisJogadorVertical, axisJogadorHorizontal, axisJogadorPulo, axisJogadorSocoBotao, axisJogadorRolarBotao;

	//Movimento
	[HideInInspector]
	public  bool	  movimentoPode = true, jogadorNoChao;
	[HideInInspector]
	public  float	  movimentoVelocidadeInicial = 0f;
	[Range(0,10)]
	public  float 	  movimentoVelocidade = 6f;
	public  float	  vertical = 0, horizontal = 0, movimentoVelocidadeSocoEstaSocando = 2f, rotacaoVelocidade = 20f;
	private	Vector3	  direcaoMovimento;
	private bool	  jogadorCorrendo;

	//Pulo
	[HideInInspector]
	public  float 	  puloForcaInicial = 6f;
	[HideInInspector]
	public	bool	  puloPulando = false;
	public 	bool	  puloPodePular = false, puloIntervalo = false;
	public  float	  puloPularIntervalo = 0.3f, puloForca = 7f;

	//Soco
	public  float	  socoForca = 500f;
	[HideInInspector]
	public  float 	  socoForcaInicio = 1f;
	public  float 	  socoCarregarLimite = 3, socoIntervalo = 1f, socoCarregado = 0f;
	public  bool 	  socoPodeSocar = false, socoEstaSocando = false;
	private bool 	  socoCoroutine;

	//Rolamento
	private float	  rolamentoVelocidade, rolamentoVelocidadeMultiplicador = 0.25f, rolamentoDuracao = 0.5f, rolamentoIntervalo = 1f;
	private Vector3	  rolamentoMovimentoDirecao;
	public  bool      rolamentoTravar;

	//Laser
	public  bool	  laserFritando = false;

	//PowerUps
	public  float     powerUpAgilidadeMultiplicador;

	//Outros
	public  bool	  jogadorProtecaoRespawn;
	public  float	  jogadorProtecaoRespawnDuracao = 3f;

	//Componentes
	private Rigidbody _rigidbody;
	public  Animator  _animator;
	public  SocoAnimacaoResetar[] socoAnimacao;
	public  JogadorLevantandoStateMachine jogadorLevantandoAnimacao;
	public  JogadorDashStateMachine jogadorDashAnimacao;

	void Awake () {
		singleton = this;

		//Pega os componentes necessarios
		_rigidbody = GetComponent<Rigidbody> ();
		_animator = GetComponent<Animator>();

		//Configura a velocidade do jogador no inicio do jogo
		movimentoVelocidadeInicial = movimentoVelocidade;

		//Define as condicoes do jogador
		//Soco
		socoPodeSocar = true;
		socoEstaSocando = false;
		socoCarregado = socoForcaInicio;

		//Movimento
		movimentoPode = true;

		//Pulo
		puloPulando = false;
		puloPodePular = true;
		puloIntervalo = false;

		//Laser
		laserFritando = false;

		//Outros
		jogadorProtecaoRespawn = false;

		//IMPORTANTE pois se o valor for 0 a animaçãode socar nao funciona
		powerUpAgilidadeMultiplicador = 1f;

		//Animacao
		//Pega os scripts das animacoes
		socoAnimacao = _animator.GetBehaviours<SocoAnimacaoResetar>();
		jogadorLevantandoAnimacao = _animator.GetBehaviour<JogadorLevantandoStateMachine>();
		jogadorDashAnimacao = _animator.GetBehaviour<JogadorDashStateMachine>();
	
		//Configura a instancia do jogador nos scripts da animacao
		foreach (SocoAnimacaoResetar animacao in socoAnimacao) {
			animacao.jogadorReferencia = this;
		}
		jogadorLevantandoAnimacao.jogadorReferencia = this;
		jogadorDashAnimacao.jogadorReferencia = this;
    }

	void FixedUpdate() {
		//variavel para guardar as informacoes do que foi acertado
		RaycastHit hit = new RaycastHit();

		if (!movimentoPode) {
			if (Physics.BoxCast(new Vector3(transform.localPosition.x, transform.position.y + 0.1f, transform.localPosition.z), Vector3.zero, new Vector3(-0.25f,-5,-0.25f), out hit,Quaternion.identity,1 , 1 >> 0)){
				if (hit.transform.gameObject.isStatic || hit.transform.tag == "Movivel") {
					jogadorNoChao = true;
					_animator.ResetTrigger("NoAr");
				}
			}
		//	return;
		}

		//Movimentacao e soco do jogador
		if (Input.GetAxisRaw (axisJogadorSocoBotao) != 0 && socoPodeSocar && !socoCoroutine) {
			//reduzir a velocidade enquanto carrega o soco
			vertical = Input.GetAxis(axisJogadorVertical) * movimentoVelocidadeSocoEstaSocando;
			horizontal = Input.GetAxis(axisJogadorHorizontal) * movimentoVelocidadeSocoEstaSocando;
			//Aumentar a forca do soco conforme o jogador segura o botao de soco
			if (socoCarregado < socoCarregarLimite) {
				socoCarregado += socoForcaInicio * Time.deltaTime;
				if (_animator.GetLayerWeight(1) == 0) {
					StartCoroutine(Socar());
				}
			}
		} else {
			vertical = Input.GetAxis(axisJogadorVertical) * movimentoVelocidade;
			horizontal = Input.GetAxis(axisJogadorHorizontal) * movimentoVelocidade;
		}

		//Verificar o rolamento
		if (Input.GetButtonDown(axisJogadorRolarBotao) && _animator.GetCurrentAnimatorStateInfo(0).IsName("Correndo") && jogadorCorrendo && _animator.GetBool("Correndo") && jogadorNoChao && !rolamentoTravar) {
			_animator.SetBool("Dash",true);
		}
		//Verificar se o jogador esta correndo
		if (horizontal > movimentoVelocidade / 2 || horizontal < movimentoVelocidade / 2 * -1 || vertical > movimentoVelocidade / 2 || vertical < movimentoVelocidade / 2 * -1) {
			jogadorCorrendo = true;
		} else {
			jogadorCorrendo = false;
		}

		//Calculo por tempo em segundos da movimentacao
		vertical *= Time.deltaTime;
		horizontal *= Time.deltaTime;

		//Guardar a direcaoMovimento  em um vector3
		direcaoMovimento = new Vector3 (horizontal, 0.0f, vertical);

		//se estiver correndo, alterar para a animacao de correr
		if (direcaoMovimento != Vector3.zero) {
			_animator.SetTrigger("Correndo");
		} else {
			_animator.ResetTrigger("Correndo");
		}

		//Rotacao do personagem na direcaoMovimento que foi apertada
		if (direcaoMovimento != Vector3.zero && !rolamentoTravar) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direcaoMovimento), Time.deltaTime * rotacaoVelocidade);
		}

		//Se estiver no chao
		if (Physics.BoxCast(new Vector3(transform.localPosition.x, transform.position.y + 0.1f, transform.localPosition.z), Vector3.zero, new Vector3(-0.25f,-.1f,-0.25f), out hit,Quaternion.identity,1 , 1 >> 0)){
			if (hit.transform.gameObject.isStatic  || hit.transform.tag == "Movivel") {
				jogadorNoChao = true;
				_animator.ResetTrigger("NoAr");
			}

			//Se posso pular
			if (Input.GetAxisRaw(axisJogadorPulo) > 0 && jogadorNoChao && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") && !_animator.GetBool("Pular") && !puloIntervalo && puloPodePular) {
				//Adiciona a forca para cima com o modo impulso, dando mais realismo ao pulo
				jogadorNoChao = false;
				_rigidbody.AddRelativeForce (Vector3.up * puloForca, ForceMode.Impulse);
				_animator.SetBool("Pular",true);
				StartCoroutine(PularIntervalo());
			}
		} else { //Se estou no ar
			_animator.SetTrigger("NoAr");
			_animator.SetBool("Pular",false);
			jogadorNoChao = false;
		}
		//Detectar se ha um player em cima
//		if (Physics.BoxCast(new Vector3(transform.localPosition.x, transform.position.y + 0.1f, transform.localPosition.z), Vector3.zero, new Vector3(0.25f,5,0.25f), out hit,Quaternion.identity,2f)){
//			//Se o jogador esta em cima de um jogador, pular e fazer o outro se abaixar
//			//Debug.Log(hit.collider);
//			if (hit.transform.tag == "Player" && !_animator.GetBool("LevandoPisada") && hit.transform.gameObject != this.gameObject && hit.collider is CapsuleCollider) {
//				StartCoroutine(LevouPisada());
//				_animator.SetBool("LevandoPisada", true);
//				hit.rigidbody.AddRelativeForce (Vector3.up * puloForca, ForceMode.Impulse);
//				hit.rigidbody.AddRelativeForce (Vector3.forward * puloForca, ForceMode.Impulse);
//			}
//		}

		//Verificar se o jogador esta dando o dash
		if (rolamentoTravar) {
			direcaoMovimento = rolamentoMovimentoDirecao * rolamentoVelocidade;
		} else {
			rolamentoMovimentoDirecao = direcaoMovimento;
		}

		//Correcao do movimento em diagonal
		if (direcaoMovimento.x != 0f && direcaoMovimento.z != 0f) {
			direcaoMovimento = direcaoMovimento / 1.5f;
		}

        //Movimentar o personagem na direcaoMovimento apertada
		transform.Translate(direcaoMovimento, Space.World);
	}

	void LateUpdate() {
		//Configurar a velocidade de rolamento
		rolamentoVelocidade = movimentoVelocidade * rolamentoVelocidadeMultiplicador;

		#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * 1 /* Variavel Check*/),Color.red);
		#endif
	}

	void JogadorCongelar() {
		_rigidbody.isKinematic = true;
		movimentoPode = false;
	}

	void JogadorDescongelar() {
		_rigidbody.isKinematic = false;
		movimentoPode = true;
	}

	void JogadorRespawn(Vector3 spawnPosicao) {
		singleton.transform.position = spawnPosicao;
	}

	//Triggers
	void OnTriggerStay (Collider col) {
		if (col.gameObject.isStatic == false) {
			if (!col.isTrigger) {
				//Verificar se acertou um jogador ou um objeto movivel e esta socando
				if (col.tag == "Player" && !col.GetComponent<Jogador>().jogadorProtecaoRespawn || col.tag == "Movivel") {

//					if(col.transform.name == "SocoDetector"){
//						if(col.transform.parent.gameObject.GetComponent<Jogador>().socoEstaSocando == true){
//							DestroyObject();
//						}
//					}
						
					if (socoEstaSocando == true){
						if (col.GetComponent<ParapeitoQuebrarSoco>() != null) {
							col.GetComponent<ParapeitoQuebrarSoco>().ParapeitoDestruir();
						}

						col.GetComponent<Rigidbody>().AddForce (transform.forward * socoForca * socoCarregado);
						if (col.tag == "Player") {
							if (!col.GetComponent<Jogador>()._animator.GetBool("LevarSoco")) {
								col.GetComponent<Jogador>()._animator.SetBool("LevarSoco",true);
							}
						}
						socoEstaSocando = false;
					}
				}
			}
		}
	}

	//Colisores
	void OnCollisionEnter (Collision other) {
		if (other.transform.tag == "Movivel" && rolamentoTravar && other.transform.GetComponent<ParapeitoQuebrarSoco>() != null) {
			other.transform.GetComponent<ParapeitoQuebrarSoco>().ParapeitoDestruir();
		}
	}

	//Coroutines
	/// <summary>
	/// Faz o personagem socar
	/// </summary>
	private IEnumerator Socar() {
		if (_animator.GetLayerWeight(0) == 0 && !socoCoroutine) {
			StopCoroutine(Socar ());
			socoCoroutine = true;
			_animator.SetTrigger("CarregandoSoco");
			float increase = 0;
			socoPodeSocar = false;

			do {
				increase += Time.deltaTime * powerUpAgilidadeMultiplicador;
				_animator.SetLayerWeight(1, increase);
				socoCarregado += socoForcaInicio * Time.deltaTime;
				yield return new WaitForSeconds(0.05f * Time.deltaTime);
			} while (Input.GetAxisRaw (axisJogadorSocoBotao) == 1 && socoCarregado < socoCarregarLimite);

			float socoCarregarLimiteMedia = socoCarregarLimite / 2f;
			//Socar mesmo se eu dar so um clique

			do {
				yield return new WaitForSeconds(1f * Time.deltaTime);
			} while (Input.GetAxisRaw(axisJogadorSocoBotao) == 1 && !rolamentoTravar);

			socoEstaSocando = true;

			if (socoCarregado < socoCarregarLimiteMedia) {
				_animator.SetLayerWeight(1, 1);
				_animator.SetBool("SocoFraco",true);
				_animator.ResetTrigger("CarregandoSoco");
				yield return new WaitForSeconds(socoIntervalo/2);
				socoCoroutine = false;
				yield return null;
			} else {
				_animator.SetBool("SocoForte",true);
			}

			_animator.ResetTrigger("CarregandoSoco");
			yield return new WaitForSeconds(socoIntervalo);
			socoCoroutine = false;
		} else {
			yield return null;
		}
	}
	/// <summary>
	/// Intervalo de pulo para o jogador nao ficar flutuando
	/// </summary>
	/// <returns>The intervalo.</returns>
	private IEnumerator PularIntervalo() {
		if (!puloIntervalo) {
//			StopCoroutine(PularIntervalo());
			yield return new WaitForSeconds (1);
			do {
				puloIntervalo = true;
				puloPodePular = false;
				yield return new WaitForSeconds(1 * Time.deltaTime);
			} while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && Input.GetAxisRaw(axisJogadorPulo) != 0);

			yield return new WaitForSeconds(puloPularIntervalo);
			puloIntervalo = false;
			puloPodePular = true;
		}
	}

	/// <summary>
	/// Reduz a velocidade do jogador ao levar uma pisada na cabeca
	/// </summary>
	/// <returns>The pisada.</returns>
	private IEnumerator LevouPisada() {
		movimentoVelocidade = movimentoVelocidade / 2;
		yield return new WaitForSeconds (2);
		movimentoVelocidade = movimentoVelocidadeInicial;
		yield return null;
	}
	/// <summary>
	/// Executar a animacao de torrar e apos isto destruir o jogador
	/// </summary>
	/// <returns>The torrar.</returns>
	public IEnumerator LaserTorrar(float tempoLaser) {
		if (!laserFritando) {
			laserFritando = true;
			JogadorCongelar();
			_animator.SetTrigger("FritandoNoLaser");
			yield return new WaitForSeconds(tempoLaser);
			//		Destroy(this.gameObject);
			JogadorRespawn(new Vector3(Random.Range(-3, 0), Random.Range(0, 7), Random.Range(0, 4)));
            _animator.ResetTrigger("FritandoNoLaser");
			laserFritando = false;
			JogadorDescongelar();
            JogadoresVidas.singleton.VidaDiminuir(jogadorQualJogador);
			jogadorProtecaoRespawn = true;
			yield return new WaitForSeconds(jogadorProtecaoRespawnDuracao);
			jogadorProtecaoRespawn = false;
        }
		yield return null;
	}

	//Key na animacao de dash
	public void AnimacaoDashDestravarMovimento(){
		rolamentoTravar = false;
		movimentoPode = false;
		puloPodePular = true;
	}
}