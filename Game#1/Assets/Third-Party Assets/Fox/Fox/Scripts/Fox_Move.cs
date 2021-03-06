﻿using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class Fox_Move : MonoBehaviour {

    public float speed,jumpForce,cooldownHit;
	public bool running,up,down,jumping,crouching,dead,attacking,special;
    private Rigidbody2D rb;
    private Animator anim;
	private SpriteRenderer sp;
	private float rateOfHit;
	private GameObject[] life;
	[SerializeField] private int qtdLife = 3;
    private GameManager _gameManager;
    public bool isGrappling = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
		sp=GetComponent<SpriteRenderer>();
		running=false;
		up=false;
		down=false;
		jumping=false;
		crouching=false;
		rateOfHit=Time.time;
		life=GameObject.FindGameObjectsWithTag("Life");
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>() as GameManager;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(dead==false){
		//Character doesnt choose direction in Jump					
			if(!attacking){		
                if(!isGrappling) Movement();
				Attack();
				Special();

				Jump();
				Crouch();
			}
			//Dead();
		}
        //Dead();
	}

	void Movement(){
		//Character Move
		float move = Input.GetAxisRaw("Horizontal");
		if(Input.GetButtonDown("Walk")){
			//Walk
			rb.velocity = new Vector2(move*speed*Time.deltaTime,rb.velocity.y);
			running=false;
		}else{
			//Run
			rb.velocity = new Vector2(move*speed*Time.deltaTime*3,rb.velocity.y);
			running=true;
		}

		//Turn
		if(rb.velocity.x<0){
			sp.flipX=true;
		}else if(rb.velocity.x>0){
			sp.flipX=false;
		}
		//Movement Animation
		if(rb.velocity.x!=0&&running==false){
			anim.SetBool("Walking",true);
		}else{
			anim.SetBool("Walking",false);
		}
		if(rb.velocity.x!=0&&running==true){
			anim.SetBool("Running",true);
		}else{
			anim.SetBool("Running",false);
		}
	}

	void Jump(){
		//Jump
		//if(Input.GetButtonDown("Jump") && rb.velocity.y == 0){
		//	rb.AddForce(new Vector2(0,jumpForce));

		//}
		//Jump Animation
		if(rb.velocity.y>0&&up==false){
			up=true;
			jumping=true;
			anim.SetTrigger("Up");
		}else if(rb.velocity.y<0&&down==false){
			down=true;
			jumping=true;
			anim.SetTrigger("Down");
		}else if(rb.velocity.y==0&&(up==true||down==true)){
			up=false;
			down=false;
			jumping=false;
			anim.SetTrigger("Ground");
		}
	}

	void Attack(){																//I activated the attack animation and when the 
		//Atacking																//animation finish the event calls the AttackEnd()
		//if(Input.GetButton("Fire1")){
		//	rb.velocity=new Vector2(0,0);
		//	anim.SetTrigger("Attack");
		//	attacking=true;
		//}
	}

	void AttackEnd(){
		attacking=false;
	}

	void Special(){
		if(Input.GetButton("Fire2")){
			anim.SetBool("Special",true);
            attacking = true;
		}else{
			anim.SetBool("Special",false);
            attacking = false;
		}
	}

	void Crouch(){
		//Crouch
		if(Input.GetButton("Crouch")){
			anim.SetBool("Crouching",true);
		}else{
			anim.SetBool("Crouching",false);
		}
	}

	void OnTriggerEnter2D(Collider2D other){							//Case of Bullet
		if(other.CompareTag("Enemy")){
            if (attacking)
                other.gameObject.GetComponent<EnemyController>().EnemyAttacked(this.transform.position);
            else
                Dead();
		}
	}								

	void Dead(){
        Debug.Log("I'm Dead");
        GetComponent<Renderer>().enabled = false;
        transform.position = _gameManager.GetSpawnPoint();
        GetComponent<Renderer>().enabled = true;
        attacking = false;
	}

	public void TryAgain(){														//Just to Call the level again
		SceneManager.LoadScene(0);
	}
}
