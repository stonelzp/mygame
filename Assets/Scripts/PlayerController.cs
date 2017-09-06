using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
//	public Camera camera_normal;
//	public Camera camera_lock;
	//partical system controll
	public GameObject Particle01;
	public GameObject Particle02;
	public GameObject Particle03;
    //Player Objecj: W_Bone
    public GameObject ObjectWeaponCollider;


	private Animation AnimationPlay;
	private Rigidbody PlayerRigidbody;


	//direction
	// 0:still,1:velocity_right,2:velocity_left,4:velocity_up,8:velocity_down
	private uint direction_value_now = 0;//0000
	private uint direction_right = 1;//→  0001
	private uint direction_left = 2;//←  0010
	private uint direction_up = 4;//↑  0100
	private uint direction_down = 8;//↓  1000
	private uint direction_mask = 15;//1111
	private uint direction_right_up=5;//0101
	private uint direction_right_down=9;//1001
	private uint direction_left_up=6;//0110
	private uint direction_left_down=10;//1010

	private Vector3 velocity_zero = new Vector3 (0.0f, 0.0f, 0.0f);
	private Vector3 velocity_right = new Vector3 (1.0f, 0.0f, 0.0f);
	private Vector3 velocity_left = new Vector3 (-1.0f, 0.0f, 0.0f);
	private Vector3 velocity_down = new Vector3 (0.0f, 0.0f, -1.0f);
	private Vector3 velocity_up = new Vector3 (0.0f, 0.0f, 1.0f);
	private Vector3 velocity_right_up = new Vector3 (0.707f, 0.0f, 0.707f);
	private Vector3 velocity_right_down = new Vector3 (0.707f, 0.0f, -0.707f);
	private Vector3 velocity_left_up = new Vector3 (-0.707f, 0.0f, 0.707f);
	private Vector3 velocity_left_down = new Vector3 (-0.707f, 0.0f, -0.707f);

	private uint status_zero = 0;//0000 0000 0000 0000 0000 0000 0000 0000
	private uint status_run = 16;//0001 0000
	private uint status_battle = 32;//0010 0000
	private uint status_lock = 64;//0100 0000
	private uint status_lock_battle=96;//0110 0000
	private uint status_run_attack=128;//1000 0000
	private uint status_dodge=256;//0001 0000 0000
	private uint status_attack_skill02=512;//0010 0000 0000
	private uint status_jump=1024;//0100 0000 0000
    private uint status_attack_skill01 =2048;//1000 0000 0000
	//private uint status_damage03=2048;//1000 0000 0000




	//PlayerStatue  0000 0000
	private uint PlayerStatus = 0;

	//the interval between the behavior
	private float interval=0.0f;

	// Use this for initialization
	void Start ()
	{
		//AnimationisPlaying = gameObject.GetComponent<Animation>().clip;
		AnimationPlay = gameObject.GetComponent<Animation> ();
		//AnimationPlay.Play ("Normal_Run");
		PlayerRigidbody = gameObject.GetComponent<Rigidbody> ();
		//play the default animation
		AnimationPlay.playAutomatically=true;
		interval = 0.0f;
		//initialize direction_value_now value
		direction_value_now = direction_left;

	}
	// Update is called once per frame
	void Update ()
	{
		//can the animation be interrupted?
		if(interval<=0.0f){
			statuscontroller ();
		}
		//skill run attack movement controller
		if((PlayerStatus&status_run_attack)==status_run_attack && interval>0.0f){
			run_attack_movement ();
		}
		//skill dodge movement controller
		if((PlayerStatus&status_dodge)==status_dodge && interval>0.0f){
			dodge_movement ();
		}
		//attack skill02  movement controller
		if((PlayerStatus&status_attack_skill02)==status_attack_skill02 && interval>0.0f){
			attack02_movement ();
			Attack02ColliderController();
			if (interval <= 0.1f) {
				PlayerStatus = (uint)(PlayerStatus & (uint)~status_attack_skill02);
			}
		}
		//jump movement controller
		if((PlayerStatus&status_jump)==status_jump && interval>0.0f){
			jump_movement ();
		}
        //damage movement controller
        //		if((PlayerStatus&status_damage03)==status_damage03 && interval>0.0f){
        //			damage_movement ();
        //		}

        //attack skill01 collider controll when interval>0.1f set collider to enable
        if ((PlayerStatus & status_attack_skill01) == status_attack_skill01 && interval > 0.1f) {
            Attack01ColliderController();
			if (interval <= 0.15f) {
				PlayerStatus = (uint)(PlayerStatus & (uint)~status_attack_skill01);//状态参数的恢复我觉得最好在update函数里面跟随着interval一起只有一次的统一恢复的比较好
			}
        }	
		//attack skill02 collider controll when interval>0.1f set collider to enable
		/*if((PlayerStatus & status_attack_skill02) == status_attack_skill02 && interval > 0.1f){
			Attack02ColliderController();
		}*/

			
		//time controll,some animation can not be interrupted
		if(interval>0){
			interval-=Time.deltaTime;
		}
	}
	void LateUpdate ()
	{
        Debug.Log(PlayerStatus);
        //camera
    }

	void playAnimation(string AnimationName){
		switch (AnimationName) {
		case "Run_Attack":
			AnimationPlay ["Run_Attack"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Run_Attack", 0.3f);
			AnimationPlay ["Battle_Idle"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFadeQueued("Battle_Idle",0.3f);
			interval = (AnimationPlay ["Run_Attack"].length / AnimationPlay ["Run_Attack"].speed) * 0.95f;
			break;
		case "Normal_Walk":
			AnimationPlay ["Normal_Walk"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFade ("Normal_Walk", 0.3f);
			break;
		case "Normal_Idle":
			AnimationPlay ["Normal_Idle"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFade ("Normal_Idle", 0.3f);
			break;
		case "Normal_Run":
			AnimationPlay ["Normal_Run"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFade ("Normal_Run", 0.3f);
			break;
		case "Battle_Idle":
			AnimationPlay ["Battle_Idle"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFade ("Battle_Idle", 0.3f);
			break;
		case "Battle_Run_R":
			AnimationPlay ["Battle_Run_R"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFade ("Battle_Run_R", 0.3f);
			break;
		case "Battle_Run_L":
			AnimationPlay ["Battle_Run_L"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFade ("Battle_Run_L", 0.3f);
			break;
		case "Battle_Run_B":
			AnimationPlay ["Battle_Run_B"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFade ("Battle_Run_B", 0.3f);
			break;
		case "Battle_Run":
			AnimationPlay ["Battle_Run"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFade ("Battle_Run", 0.3f);
			break;
		case "Battle_Walk":
			AnimationPlay ["Battle_Walk"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFade ("Battle_Walk", 0.3f);
			break;
		case "Battle_In":
			AnimationPlay ["Battle_In"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Battle_In", 0.3f);
			AnimationPlay ["Battle_Idle"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			//the animation can not be interrupted
			interval = (AnimationPlay ["Battle_In"].length / AnimationPlay ["Battle_In"].speed) * 0.85f;
			break;
		case "Battle_Out":
			AnimationPlay ["Battle_Out"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Battle_Out", 0.3f);
			AnimationPlay ["Normal_Idle"].wrapMode = WrapMode.Loop;
			AnimationPlay.CrossFadeQueued ("Normal_Idle",0.3f);
			//the animation can not be interrupted
			interval = (AnimationPlay ["Battle_Out"].length / AnimationPlay ["Battle_Out"].speed) * 0.85f;
			break;
		case "Jump":
			AnimationPlay ["Jump"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Jump", 0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval=(AnimationPlay ["Jump"].length / AnimationPlay ["Jump"].speed) * 0.95f;
			break;
		case "Dodge":
			AnimationPlay ["Dodge"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Dodge", 0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval=(AnimationPlay ["Dodge"].length / AnimationPlay ["Dodge"].speed) * 0.98f;
			break;
		case "Damage01":
			AnimationPlay ["Damage01"].wrapMode = WrapMode.Once;
			//AnimationPlay ["Damage01"].speed = f;
			AnimationPlay.CrossFade ("Damage01", 0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval=(AnimationPlay ["Damage01"].length / AnimationPlay ["Damage01"].speed) * 1.0f;
			break;
		case "Damage02":
			AnimationPlay ["Damage02"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Damage02", 0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval=(AnimationPlay ["Damage02"].length / AnimationPlay ["Damage02"].speed) * 1.0f;
			break;
		case "Damage03":
			AnimationPlay ["Damage03"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Damage03", 0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval=(AnimationPlay ["Damage03"].length / AnimationPlay ["Damage03"].speed) * 1.0f;
			break;
		case "Dead":
			AnimationPlay ["Dead"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Dead", 0.3f);
			//AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval=20.0f;
			break;
		case "Level_Up":
			AnimationPlay ["Level_Up"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Level_Up", 0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval=(AnimationPlay ["Level_Up"].length / AnimationPlay ["Level_Up"].speed) * 0.98f;
			break;
		case "Skill01":
			AnimationPlay ["Skill01"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Skill01", 0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval = (AnimationPlay ["Skill01"].length / AnimationPlay ["Skill01"].speed) * 0.98f;
			Debug.Log ("skill01 interval"+interval);//1.306s
			break;
		case "Skill02":
			AnimationPlay ["Skill02"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Skill02", 0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval=(AnimationPlay ["Skill02"].length / AnimationPlay ["Skill02"].speed) * 0.98f;
			Debug.Log ("skill02 interval"+interval);//1.568s
			break;
		case "Skill03":
			AnimationPlay ["Skill03"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Skill03", 0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.3f);
			interval = (AnimationPlay ["Skill03"].length / AnimationPlay ["Skill03"].speed) * 0.98f;
			Invoke ("particle03Play",0.5f);
			break;
		case "MagicSkill01":
			AnimationPlay ["MagicAttack_B"].wrapMode = WrapMode.Once;
			AnimationPlay ["MagicAttack_M"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("MagicAttack_B", 0.0f);
			AnimationPlay.CrossFadeQueued ("MagicAttack_M", 0.1f);
			AnimationPlay.CrossFadeQueued ("MagicAttack_E", 0.1f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle", 0.0f);
			interval = (AnimationPlay ["MagicAttack_B"].length / AnimationPlay ["MagicAttack_B"].speed + AnimationPlay ["MagicAttack_M"].length / AnimationPlay ["MagicAttack_M"].speed + AnimationPlay ["MagicAttack_E"].length / AnimationPlay ["MagicAttack_E"].speed);
			//magic particle
			//particleEffectManagement(1);
            skillManagement("MagicSkill01");



//			if (!Particle01.activeSelf) {
//				Particle01.SetActive (true);
//			} else {
//				Particle01.SetActive (false);
//				Particle01.SetActive (true);
//			}
//			Invoke ("particle02Play",1.5f);
			break;
		case "Normal_Talk":
			AnimationPlay ["Normal_Talk"].wrapMode = WrapMode.Loop;
			AnimationPlay ["Normal_Talk"].speed = 0.3f;
			AnimationPlay.CrossFade ("Normal_Talk",0.4f);

			break;
		default:
			break;
		}
	}

	//player action controller
	void statuscontroller (){
		//check is lock status
		if (Input.GetKeyDown (KeyCode.L)) {
			if ((PlayerStatus & status_lock) == status_lock) {
				PlayerStatus = (uint)(PlayerStatus & (uint)~status_lock);
			} else {
				PlayerStatus = (uint)(PlayerStatus | status_lock);
			}
		}
		//check is running?
		if (Input.GetKey (KeyCode.LeftShift)) {
			PlayerStatus = (uint)(PlayerStatus | status_run);//example:0001 0000
		}
		else{
			PlayerStatus = (uint)(PlayerStatus & (uint)~status_run);
		}

		//check is battle status
		if(Input.GetKeyDown(KeyCode.B)){
			if ((PlayerStatus & status_battle) == status_battle) {
				PlayerStatus = (uint)(PlayerStatus & (uint)~status_battle);
				playAnimation ("Battle_Out");
			} else {
				PlayerStatus = (uint)(PlayerStatus | status_battle);
				playAnimation ("Battle_In");
			}

		}
		//
		if(Input.GetKeyDown(KeyCode.I)){
			PlayerStatus = (uint)(PlayerStatus | status_battle);
			PlayerStatus = (uint)(PlayerStatus | status_run_attack);
			playAnimation ("Run_Attack");
		}
		//check is jumping?
		if(Input.GetKeyDown(KeyCode.Space)){
			if((PlayerStatus&status_battle)==status_battle){
				PlayerStatus = (uint)(PlayerStatus | status_jump);
				playAnimation ("Jump");
			}
		}
		//check dodge
		if(Input.GetKeyDown(KeyCode.Q)){
			if((PlayerStatus&status_battle)==status_battle){
				PlayerStatus = (uint)(PlayerStatus | status_dodge);
				playAnimation ("Dodge");
			}
		}
		//check is damaged?
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			if((PlayerStatus&status_battle)==status_battle){
				playAnimation ("Damage01");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			if((PlayerStatus&status_battle)==status_battle){
				playAnimation ("Damage02");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			if((PlayerStatus&status_battle)==status_battle){
				//PlayerStatus = (uint)(PlayerStatus | status_damage03);
				playAnimation ("Damage03");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha0)){
			if((PlayerStatus&status_battle)==status_battle){
				playAnimation ("Dead");
			}
		}

		if(Input.GetKeyDown(KeyCode.U)){
			if((PlayerStatus&status_battle)==status_battle){
				playAnimation ("Level_Up");
			}
		}
		//skill Attack
		if(Input.GetKeyDown(KeyCode.F)){
			if((PlayerStatus&status_battle)==status_battle){
				playAnimation ("Skill03");
			}
		}
		if(Input.GetKeyDown(KeyCode.J)){//Attack01
			if((PlayerStatus&status_battle)==status_battle){
                PlayerStatus = (uint)(PlayerStatus | status_attack_skill01);
                playAnimation ("Skill01");
			}
		}
		if(Input.GetKeyDown(KeyCode.K)){//Attack02
			if((PlayerStatus&status_battle)==status_battle){
				PlayerStatus = (uint)(PlayerStatus | status_attack_skill02);
				playAnimation ("Skill02");
			}
		}
		if(Input.GetKeyDown(KeyCode.M)){
			if((PlayerStatus&status_battle)==status_battle){
				playAnimation ("MagicSkill01");
			}
		}

		//check is moving?
		if (Input.GetKey (KeyCode.D)) {
			PlayerStatus = (uint)(PlayerStatus | direction_right);
		}
		else{
			PlayerStatus = (uint)(PlayerStatus & (uint)~direction_right);
		}

		if (Input.GetKey (KeyCode.A)) {
			PlayerStatus = (uint)(PlayerStatus | direction_left);
		}
		else{
			PlayerStatus = (uint)(PlayerStatus & (uint)~direction_left);
		}

		if (Input.GetKey (KeyCode.W)) {
			PlayerStatus = (uint)(PlayerStatus | direction_up);
		}
		else {
			PlayerStatus = (uint)(PlayerStatus & (uint)~direction_up);
		}

		if (Input.GetKey (KeyCode.S)) {
			PlayerStatus = (uint)(PlayerStatus | direction_down);
		}
		else {
			PlayerStatus = (uint)(PlayerStatus & (uint)~direction_down);
		}



		if(interval<=0){
			movecontroller();
		}	
		//this frame status complete then show animation

	}
		
	//player move controller
	void movecontroller ()
	{	
		uint movement_status = 0;
		movement_status = (uint)(PlayerStatus & direction_mask);
		//check is lock status ?
		if ((PlayerStatus & status_lock_battle) == status_lock_battle) {
			switch (movement_status) {
			case 0:
				playAnimation ("Battle_Idle");
				break;
			case 1:
				//move to right
				lock_in_movementcontroller (direction_right);
				playAnimation ("Battle_Run_R");
				break;
			case 2:
				lock_in_movementcontroller (direction_left);
				playAnimation ("Battle_Run_L");

				break;
			case 4:
				lock_in_movementcontroller (direction_up);
				playAnimation ("Battle_Walk");

				break;
			case 8:
				lock_in_movementcontroller (direction_down);
				playAnimation ("Battle_Run_B");
				break;
			default:
				break;
			}
		} else {
			switch (movement_status) {
			case 0:
				break;
			case 1:
			//move to right
				if ((PlayerStatus & status_run) > 0) {
					PlayerRigidbody.velocity = velocity_right * 2;//(1,0)
				} else {
					PlayerRigidbody.velocity = velocity_right;//(1,0)
				}
			//rotation change
				transform.rotation = Quaternion.Euler (0.0f, 90.0f, 0.0f);
				direction_value_now = direction_right;
				break;
			case 2:
				if ((PlayerStatus & status_run) > 0) {
					PlayerRigidbody.velocity = velocity_left * 2;//(-1,0)
				} else {
					PlayerRigidbody.velocity = velocity_left;//(-1,0)
				}
			//rotation change
				transform.rotation = Quaternion.Euler (0.0f, -90.0f, 0.0f);
				direction_value_now = direction_left;
				break;
			case 4:
				if ((PlayerStatus & status_run) > 0) {
					PlayerRigidbody.velocity = velocity_up * 2;//(0,1)
				} else {
					PlayerRigidbody.velocity = velocity_up;//(0,1)
				}
			//rotation change
				transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
				direction_value_now = direction_up;
				break;
			case 5:
				if ((PlayerStatus & status_run) > 0) {
					PlayerRigidbody.velocity = velocity_right_up * 2;//(1,1)
				} else {
					PlayerRigidbody.velocity = velocity_right_up;//(1,1)
				}

			//rotation change
				transform.rotation = Quaternion.Euler (0.0f, 45.0f, 0.0f);
				direction_value_now = direction_right_up;
				break;
			case 6:
				if ((PlayerStatus & status_run) > 0) {
					PlayerRigidbody.velocity = velocity_left_up * 2;//(-1,1)
				} else {
					PlayerRigidbody.velocity = velocity_left_up;//(-1,1)
				}
			//rotation change
				transform.rotation = Quaternion.Euler (0.0f, -45.0f, 0.0f);
				direction_value_now = direction_left_up;
				break;
			case 8:
				if ((PlayerStatus & status_run) > 0) {
					PlayerRigidbody.velocity = velocity_down * 2;//(0,-1)
				} else {
					PlayerRigidbody.velocity = velocity_down;//(0,-1)
				}
			//rotation change
				transform.rotation = Quaternion.Euler (0.0f, -180.0f, 0.0f);
				direction_value_now = direction_down;
				break;
			case 9:
				if ((PlayerStatus & status_run) > 0) {
					PlayerRigidbody.velocity = velocity_right_down * 2;//(1,-1)
				} else {
					PlayerRigidbody.velocity = velocity_right_down;//(1,-1)
				}
			//rotation change
				transform.rotation = Quaternion.Euler (0.0f, 135.0f, 0.0f);
				direction_value_now = direction_right_down;
				break;
			case 10:
				if ((PlayerStatus & status_run) > 0) {
					PlayerRigidbody.velocity = velocity_left_down * 2;//(-1,-1)
				} else {
					PlayerRigidbody.velocity = velocity_left_down;//(-1,-1)
				}
				//rotation change
				transform.rotation = Quaternion.Euler (0.0f, -135.0f, 0.0f);
				direction_value_now = direction_left_down;
				break;
			default:
				break;

			}
			if (PlayerRigidbody.velocity == velocity_zero) {
				if ((PlayerStatus & status_battle) == status_battle) {
					playAnimation ("Battle_Idle");
				} else {
					playAnimation ("Normal_Idle");
				}
			} else {
				if ((PlayerStatus & status_battle) == status_battle) {
					if ((PlayerStatus & status_run) == status_run) {
						playAnimation ("Battle_Run");
					} else {
						playAnimation ("Battle_Walk");
					}

				} else {
					if ((PlayerStatus & status_run) == status_run) {
						playAnimation ("Normal_Run");
					} else {
						playAnimation ("Normal_Walk");
					}
				}
			}
				
		}
	}

	//lock status movecontroller
		void lock_in_movementcontroller(uint direction){
			//uint rotation_y=(uint)(transform.rotation.y);
			switch(direction_value_now){
			case 4://direction up
				if (direction == direction_right) {
					PlayerRigidbody.velocity = velocity_right;
				}
				if (direction == direction_left) {
					PlayerRigidbody.velocity = velocity_left;
				}
				if (direction == direction_up) {
					PlayerRigidbody.velocity = velocity_up;
				}
				if (direction == direction_down) {
					PlayerRigidbody.velocity = velocity_down;
				}
				break;
			case 5://direction right-up
				if (direction == direction_right) {
					PlayerRigidbody.velocity = velocity_right_down;
				}
				if (direction == direction_left) {
					PlayerRigidbody.velocity = velocity_left_up;
				}
				if (direction == direction_up) {
					PlayerRigidbody.velocity = velocity_right_up;
				}
				if (direction == direction_down) {
					PlayerRigidbody.velocity = velocity_left_down;
				}
				break;
			case 1://direction right
				if (direction == direction_right) {
					PlayerRigidbody.velocity = velocity_down;
				}
				if (direction == direction_left) {
					PlayerRigidbody.velocity = velocity_up;
				}
				if (direction == direction_up) {
					PlayerRigidbody.velocity = velocity_right;
				}
				if (direction == direction_down) {
					PlayerRigidbody.velocity = velocity_left;
				}
				break;
			case 9://direction right_down
				if (direction == direction_right) {
					PlayerRigidbody.velocity = velocity_left_down;
				}
				if (direction == direction_left) {
					PlayerRigidbody.velocity = velocity_right_up;
				}
				if (direction == direction_up) {
					PlayerRigidbody.velocity = velocity_right_down;
				}
				if (direction == direction_down) {
					PlayerRigidbody.velocity = velocity_left_up;
				}
				break;
			case 8://direction down
				if (direction == direction_right) {
					PlayerRigidbody.velocity = velocity_left;
				}
				if (direction == direction_left) {
					PlayerRigidbody.velocity = velocity_right;
				}
				if (direction == direction_up) {
					PlayerRigidbody.velocity = velocity_down;
				}
				if (direction == direction_down) {
					PlayerRigidbody.velocity = velocity_up;
				}
				break;
			case 10://direction left_down
				if (direction == direction_right) {
					PlayerRigidbody.velocity = velocity_left_up;
				}
				if (direction == direction_left) {
					PlayerRigidbody.velocity = velocity_right_down;
				}
				if (direction == direction_up) {
					PlayerRigidbody.velocity = velocity_left_down;
				}
				if (direction == direction_down) {
					PlayerRigidbody.velocity = velocity_right_up;
				}
				break;
			case 2://direction left
				if (direction == direction_right) {
					PlayerRigidbody.velocity = velocity_up;
				}
				if (direction == direction_left) {
					PlayerRigidbody.velocity = velocity_down;
				}
				if (direction == direction_up) {
					PlayerRigidbody.velocity = velocity_left;
				}
				if (direction == direction_down) {
					PlayerRigidbody.velocity = velocity_right;
				}
				break;
			case 6://direction left_up
				if (direction == direction_right) {
					PlayerRigidbody.velocity = velocity_right_up;
				}
				if (direction == direction_left) {
					PlayerRigidbody.velocity = velocity_left_down;
				}
				if (direction == direction_up) {
					PlayerRigidbody.velocity = velocity_left_up;
				}
				if (direction == direction_down) {
					PlayerRigidbody.velocity = velocity_right_down;
				}
				break;
			default:
				break;

			}
		}

	//run attack movement controller
	void run_attack_movement(){
		//warn:it should not exit number 0.75
		//
		if (interval <= 0.75f) {
			PlayerStatus = (uint)(PlayerStatus & (uint)~status_run_attack);
		}
		switch (direction_value_now) {
		case 1:
			PlayerRigidbody.velocity = new Vector3 (2.5f,0.0f,0.0f);
			break;
		case 2:
			PlayerRigidbody.velocity = new Vector3 (-2.5f,0.0f,0.0f);
			break;
		case 4:
			PlayerRigidbody.velocity = new Vector3 (0.0f,0.0f,2.5f);
			break;
		case 5:
			PlayerRigidbody.velocity = new Vector3 (1.58f,0.0f,1.58f);
			break;
		case 6:
			PlayerRigidbody.velocity = new Vector3 (-1.58f,0.0f,1.58f);
			break;
		case 8:
			PlayerRigidbody.velocity = new Vector3 (0.0f,0.0f,-2.5f);
			break;
		case 9:
			PlayerRigidbody.velocity = new Vector3 (1.58f,0.0f,-1.58f);
			break;
		case 10:
			PlayerRigidbody.velocity = new Vector3 (-1.58f,0.0f,-1.58f);
			break;

		default:
			break;
		}
	}
	//dodge movement controller
	void dodge_movement(){
		//warn:it should not exit number 0.75
		//

		if(interval<=0.2f){
			PlayerStatus = (uint)(PlayerStatus & (uint)~status_dodge);
		}
		switch(direction_value_now){
		case 1:
			PlayerRigidbody.velocity = new Vector3 (-4.0f,0.0f,0.0f);
			break;
		case 2:
			PlayerRigidbody.velocity = new Vector3 (4.0f,0.0f,0.0f);
			break;
		case 4:
			PlayerRigidbody.velocity = new Vector3 (0.0f,0.0f,-4.0f);
			break;
		case 5:
			PlayerRigidbody.velocity = new Vector3 (-2.828f,0.0f,-2.828f);
			break;
		case 6:
			PlayerRigidbody.velocity = new Vector3 (2.828f,0.0f,-2.828f);
			break;
		case 8:
			PlayerRigidbody.velocity = new Vector3 (0.0f,0.0f,4.0f);
			break;
		case 9:
			PlayerRigidbody.velocity = new Vector3 (-2.828f,0.0f,2.828f);
			break;
		case 10:
			PlayerRigidbody.velocity = new Vector3 (2.828f,0.0f,2.828f);
			break;

		default:
			break;
		}
		
	}
	//attack02 movement controller
	void attack02_movement(){
		if (interval >= 0.8f) {
			float movement = 1.0f;
			switch (direction_value_now) {
			case 1:
				PlayerRigidbody.velocity = new Vector3 (movement, 0.0f, 0.0f);
				break;
			case 2:
				PlayerRigidbody.velocity = new Vector3 (-movement, 0.0f, 0.0f);
				break;
			case 4:
				PlayerRigidbody.velocity = new Vector3 (0.0f, 0.0f, movement);
				break;
			case 5:
				PlayerRigidbody.velocity = new Vector3 (movement * 0.707f, 0.0f, movement * 0.707f);
				break;
			case 6:
				PlayerRigidbody.velocity = new Vector3 (-movement * 0.707f, 0.0f, movement * 0.707f);
				break;
			case 8:
				PlayerRigidbody.velocity = new Vector3 (0.0f, 0.0f, -movement);
				break;
			case 9:
				PlayerRigidbody.velocity = new Vector3 (movement * 0.707f, 0.0f, -movement * 0.707f);
				break;
			case 10:
				PlayerRigidbody.velocity = new Vector3 (-movement * 0.707f, 0.0f, -movement * 0.707f);
				break;

			default:
				break;
			}
		} else {
			PlayerRigidbody.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		}
	}
	//jump movement controller
	void jump_movement(){
		if (interval <= 1.0f) {
			PlayerStatus=(uint)(PlayerStatus & (uint)~status_jump);
		}
		switch (direction_value_now) {
		case 1:
			PlayerRigidbody.velocity = new Vector3 (2.0f, 0.0f, 0.0f);
			break;
		case 2:
			PlayerRigidbody.velocity = new Vector3 (-2.0f,0.0f,0.0f);
			break;
		case 4:
			PlayerRigidbody.velocity = new Vector3 (0.0f,0.0f,2.0f);
			break;
		case 5:
			PlayerRigidbody.velocity = new Vector3 (2.828f,0.0f,2.828f);
			break;
		case 6:
			PlayerRigidbody.velocity = new Vector3 (-2.828f,0.0f,2.828f);
			break;
		case 8:
			PlayerRigidbody.velocity = new Vector3 (0.0f,0.0f,-2.0f);
			break;
		case 9:
			PlayerRigidbody.velocity = new Vector3 (2.828f,0.0f,-2.828f);
			break;
		case 10:
			PlayerRigidbody.velocity = new Vector3 (-2.828f,0.0f,-2.828f);
			break;
		default:
			break;
		}
		if ((PlayerStatus & status_run) == status_run) {
			//if is running
			PlayerRigidbody.velocity=PlayerRigidbody.velocity*1.3f;
		}
	}
//	//damage movement controller
//	void damage_movement(){
//		if (interval <= 0.2f) {
//			PlayerStatus=(uint)(PlayerStatus & (uint)~status_damage03);
//			switch (direction_value_now) {
//			case 1:
//				gameObject.transform.position=new Vector3(gameObject.transform.position.x-0.5f,gameObject.transform.position.y,gameObject.transform.position.z);
//				break;
//			default:
//				break;
//			}
//		}
	//}

	//当攻击的时候激活武器的碰撞体相应的时间然后取消碰撞
    private void Attack01ColliderController()
    {
		if (interval <= 0.52f) {
			//PlayerStatus = (uint)(PlayerStatus & (uint)~status_attack_skill01);
			if (ObjectWeaponCollider.GetComponent<CapsuleCollider> ().enabled) {
				ObjectWeaponCollider.GetComponent<CapsuleCollider> ().enabled = false;
			}
		} else if(interval<0.86f){//这个地方放数字是不对的需要之后找一个参数代替
			if (!ObjectWeaponCollider.GetComponent<CapsuleCollider> ().enabled) {
				ObjectWeaponCollider.GetComponent<CapsuleCollider> ().enabled = true;
			}
		}
    }
	private void Attack02ColliderController(){
		if (interval <= 0.8f) {
			//PlayerStatus = (uint)(PlayerStatus & (uint)~status_attack_skill02);
			if (ObjectWeaponCollider.GetComponent<CapsuleCollider> ().enabled) {
				ObjectWeaponCollider.GetComponent<CapsuleCollider> ().enabled = false;
			}
		} else if(interval<1.05f){//这个地方放数字是不对的需要之后找一个参数代替
			if (!ObjectWeaponCollider.GetComponent<CapsuleCollider> ().enabled) {
				ObjectWeaponCollider.GetComponent<CapsuleCollider> ().enabled = true;
			}
		}
	}
    //技能管理器，释放技能的时候调用
    private void skillManagement(string SkillName)
    {
        switch (SkillName)
        {
            case "MagicSkill01":
                particleEffectManagement(1);
                StartCoroutine(delayToPlayParticle02());
                break;
            default:
                break;
        }
    }


    //粒子特效管理器，当需要播放某个粒子特效的时候调用
	private void particleEffectManagement(int AttackParticleNum){
		switch(AttackParticleNum){
		case 1:
			//Attack01 skill particle effect
			float OffsetX01 = -0.732f;
			float OffsetY01 = 1.132f;
			float OffsetZ01 = -0.36f;
                //Vector3 V = new Vector3(OffsetX01, OffsetY01, OffsetZ01);
                Vector3 A_P01 = getTargetParticlePosotion(OffsetX01, OffsetY01, OffsetZ01);
                GameObject AttackParticle01 = Instantiate (Particle01, A_P01, Quaternion.identity);
			StartCoroutine (GameobjectDestory (AttackParticle01,2.0f));
			break;
            case 2:
                //Attack01 skill particle effect
                float OffsetX02 = -2.331f;
                float OffsetY02 = 1.173f;
                float OffsetZ02 = -0.089f;
                Vector3 A_P02 = getTargetParticlePosotion(OffsetX02, OffsetY02, OffsetZ02);
                //Vector3 V2 = new Vector3(OffsetX02, OffsetY02, OffsetZ02);
                //Attack02 skill particle effect
                GameObject AttackParticle02 = Instantiate(Particle02,A_P02,Quaternion.identity);
                //StartCoroutine(doAttack02ParticleEffect());
                break;
		default:
			break;

		}
	}
    //
    private Vector3 getTargetParticlePosotion(float OffsetX,float OffsetY,float OffsetZ)
    {
        Vector3 V = new Vector3(OffsetX, OffsetY, OffsetZ);
        Vector3 A_P = new Vector3(0.0f, 0.0f, 0.0f);
        if (direction_value_now == direction_left)
        {
            //A_P01 = new Vector3 (gameObject.transform.position.x + V.x, gameObject.transform.position.y + V.y, gameObject.transform.position.z + V.z);
        }
        else if (direction_value_now == direction_right)
        {
            V = Quaternion.AngleAxis(180, Vector3.up) * V;
        }
        else if (direction_value_now == direction_up)
        {
            V = Quaternion.AngleAxis(90, Vector3.up) * V;
        }
        else if (direction_value_now == direction_down)
        {
            V = Quaternion.AngleAxis(270, Vector3.up) * V;
        }
        else if (direction_value_now == direction_left_up)
        {
            V = Quaternion.AngleAxis(45, Vector3.up) * V;
        }
        else if (direction_value_now == direction_right_up)
        {
            V = Quaternion.AngleAxis(135, Vector3.up) * V;
        }
        else if (direction_value_now == direction_right_down)
        {
            V = Quaternion.AngleAxis(225, Vector3.up) * V;
        }
        else if (direction_value_now == direction_left_down)
        {
            V = Quaternion.AngleAxis(315, Vector3.up) * V;
        }
        A_P = new Vector3(gameObject.transform.position.x + V.x, gameObject.transform.position.y + V.y, gameObject.transform.position.z + V.z);
        return A_P;
    }


    private IEnumerator delayToPlayParticle02()
    {
        yield return new WaitForSeconds(1.5f);
        particleEffectManagement(2);
    }

    //after delaytime destory the gameobject 
	private IEnumerator GameobjectDestory(GameObject objectname, float delaytime){
		yield return new WaitForSeconds (delaytime);
		Destroy (objectname);

	}



	void particle02Play(){
		if (!Particle02.activeSelf) {
			Particle02.SetActive (true);
		} else {
			Particle02.SetActive (false);
			Particle02.SetActive (true);
		}
	}
	void particle03Play(){
		if (!Particle03.activeSelf) {
			Particle03.SetActive (true);
		} else {
			Particle03.SetActive (false);
			Particle03.SetActive (true);
		}
	}

	//when DialogueTrigger is trigged,play talk animation
	public void DialogueAnimationTalkPlay(){
		playAnimation ("Normal_Talk");
	}
	public void DialogueAnimationTalkPlayEnd(){
		playAnimation ("Normal_Idle");
	}
}
