using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public Camera camera_normal;
	public Camera camera_lock;


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
//	private uint status_jump = 256;//0001 0000 0000
//	private uint status_run_attack=512;//0010 0000 0000



//	private uint status_levelup = 128;
////1000 0000
//	private uint status_attack = 768;
////0011 0000 0000
//	private uint status_magic = 3072;
////1100 0000 0000
//	private uint status_damage = 12288;
////0011 0000 0000 0000
//	private uint status_avoid = 16384;
////0100 0000 0000 0000
//	private uint status_dead = 32768;
//1000 0000 0000 0000

	//PlayerStatue  0000 0000
	private uint PlayerStatus = 0;
	//can the animation be interrupted?
	//private bool interruption = true;

	//the interval between the behavior
	private float interval=0.0f;

	// Use this for initialization
	void Start ()
	{
		//AnimationisPlaying = gameObject.GetComponent<Animation>().clip;
		AnimationPlay = gameObject.GetComponent<Animation> ();
		//AnimationPlay.Play ("Normal_Run");
		PlayerRigidbody = gameObject.GetComponent<Rigidbody> ();
//		foreach (AnimationState anim in AnimationPlay) {
//			Debug.Log (anim.name);
//		}
		//play the default animation
		AnimationPlay.playAutomatically=true;


	}
	// Update is called once per frame
	void Update ()
	{
		//can the animation be interrupted?
		if(interval<=0.0f){
			statuscontroller ();
			//movecontroller ();


		}
			
		//time controll,some animation can not be interrupted
		if(interval>0){
			interval-=Time.deltaTime;
		}
	}

	void LateUpdate ()
	{
		Debug.Log ("PlayerStatus:");
		Debug.Log (PlayerStatus);
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

		default:
			break;
		}
	}

	//player action controller
	void statuscontroller ()
	{
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
			//Debug.Log(PlayerStatus);
			PlayerStatus = (uint)(PlayerStatus | status_run);//example:0001 0000
			//Debug.Log(PlayerStatus);
		}
		else{
			PlayerStatus = (uint)(PlayerStatus & (uint)~status_run);
			//Debug.Log(PlayerStatus);
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
			playAnimation ("Run_Attack");
		}
		//check is jumping?
		if(Input.GetKeyDown(KeyCode.Space)){
			Debug.Log ("Jump");
			//PlayerStatus = (uint)(PlayerStatus | status_jump);
			//Invoke ("setstatus_jump_to0",(AnimationPlay ["Jump"].length / AnimationPlay ["Jump"].speed) * 0.8f);
			//PlayerRigidbody.AddForce (new Vector3(0.0f,3.0f,0.0f));
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




//		//check dodge
//		if (Input.GetKeyDown (KeyCode.Q)) {
//			//Q avoid skill action status
//
//			//gameObject.GetComponent<Animation> ().CrossFade ("avoid", 0.3f);
//		}
//		if (Input.GetKeyUp (KeyCode.Q)) {
//		
//		}
//
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
				Debug.Log ("move to right");
				Debug.Log (direction_value_now);
				lock_in_movementcontroller (direction_right);
				playAnimation ("Battle_Run_R");
				break;
			case 2:
				Debug.Log("move to left");
				Debug.Log (direction_value_now);
				lock_in_movementcontroller (direction_left);
				playAnimation ("Battle_Run_L");

				break;
			case 4:
				Debug.Log("move to up");
				Debug.Log (direction_value_now);
				lock_in_movementcontroller (direction_up);
				playAnimation ("Battle_Walk");

				break;
			case 8:
				Debug.Log("move to down");
				Debug.Log (direction_value_now);
				lock_in_movementcontroller (direction_down);
				playAnimation ("Battle_Run_B");
				break;
			default:
				Debug.Log("not move");
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
			Debug.Log ("fangxiang1");
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
			Debug.Log ("fangxiang2");
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
			Debug.Log ("fangxiang3");
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
			Debug.Log ("fangxiang4");
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
			Debug.Log ("fangxiang5");
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
			Debug.Log ("fangxiang6");
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
			Debug.Log ("fangxiang7");
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
			Debug.Log ("fangxiang8");
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
		
}
