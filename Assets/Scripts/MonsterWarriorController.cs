using UnityEngine;
using System.Collections;
using UnityEngine.AI;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class MonsterWarriorController : MonoBehaviour {
	//when we to initial a monster,we should add Player to this AttackTarget
    public Transform AttackTarget;
    //set the patrol position/direction to this monster
	public float PatrolDirection_x;
	public float PatrolDirection_z;


    private Animator MonsterAnimator;
    private bool isPatrolling;
	//near the player to attack;
	public bool NearTarget;
    //Moster status
    private bool MonsterIsDead;
    private float DeadToDisappearTime;
	//the monster next to Patrol position
    private Vector3 PatrolPosition;
	private Vector3 StartPosition;
	private Vector3 EndPosition;
	private bool PatrolBack = false;

    //the distance of Monster Attack Area
    private float AttackAreaDistance = 0.0f;
	private float AttackCoolDown = 0.0f;

	//the monster origin position
	private Vector3 MonsterOriginPosition;
	private float monsterFollowingDistance = 15.0f;
	private float monsterPatrollMaxDistance = 20.0f;
	private bool animationAttackIsPlaying = false;

	// Use this for initialization
	void Start () {
        MonsterAnimator = gameObject.GetComponent<Animator>();
		StartPosition = gameObject.transform.position;
        //set the monster patrol position
		PatrolPosition=new Vector3 (transform.position.x + PatrolDirection_x,transform.position.y,transform.position.z + PatrolDirection_z);
		EndPosition = PatrolPosition;
		gameObject.GetComponent<NavMeshAgent> ().destination = PatrolPosition;
		isPatrolling = true;
		NearTarget = false;
        MonsterIsDead = false;
        DeadToDisappearTime = 3.0f;

		MonsterOriginPosition = gameObject.transform.position;

	}

    // Update is called once per frame
    void Update() {
        //GetMonsterInput(); 
		if(AttackCoolDown>0.0f){
			AttackCoolDown -= Time.deltaTime;
		}

//		if (isPatrolling) {
//			MonsterPatrol ();
//		} 
//
//		if(NearTarget){
//			//if NearTarget is true,monster should attack the enemy
//		}
//
		MonsterAIManagement();


        if (MonsterIsDead)
        {
            if (DeadToDisappearTime > 0.0f)
            {
                DeadToDisappearTime -= Time.deltaTime;
            }
            else {
                MonsterToDelete();
            }
        }

    }

    private void GetMonsterInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MonsterAnimator.SetBool("Run", true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MonsterAnimator.SetBool("Run", true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MonsterAnimator.SetBool("Run", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MonsterAnimator.SetBool("Run", true);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            MonsterAnimator.SetBool("Run", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            MonsterAnimator.SetBool("Run", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            MonsterAnimator.SetBool("Run", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            MonsterAnimator.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            MonsterAnimator.SetTrigger("Dead");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            MonsterAnimator.SetTrigger("Attack01");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            MonsterAnimator.SetTrigger("Attack02");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            MonsterAnimator.SetTrigger("Damage");
        }
    }



    //Monsters AI :contains
    //patroling routine
    //Attack
	//怪物巡逻的制作：使用怪物的自动寻路系统转弯的时候总觉得违和，考虑放弃在巡逻的部分使用自动寻路系统
	private void MonsterAIManagement(){
		if (isPatrolling) {
			//巡逻
			MonsterPatrol ();
		} else {
			//不是在巡逻，说明在索敌中
			//如果索敌过程中靠近了目标
			if (NearTarget) {
				MonsterAttack ();
			} else {
			//仍是在索敌的过程中,更新对象的位置
				MonsterSearchTarget();
			}
		}
	}

	private void MonsterSearchTarget(){
		if(!gameObject.GetComponent<NavMeshAgent>().enabled){
			gameObject.GetComponent<NavMeshAgent> ().enabled = true;
			//索敌的时候播放Run的动画好还是当Rigidbody有位移的时候再播放动画好呢？
			MonsterAnimator.SetBool ("Run", true);
		}
		gameObject.GetComponent<NavMeshAgent> ().destination = AttackTarget.position;
		float distanceBetweenMonsterAndTarget = Vector3.Distance (gameObject.transform.position,AttackTarget.position);
		//如果monster跟target之间的距离超过了设定距离则让怪物返回，回到巡逻的状态
		if (distanceBetweenMonsterAndTarget > monsterFollowingDistance) {
			isPatrolling = true;
		}
		//还有一种情况是monster远离了巡逻的地方一定距离也要返回巡逻的位置
		if(Vector3.Distance(gameObject.transform.position,MonsterOriginPosition) > monsterPatrollMaxDistance){
			isPatrolling = true;
		}
	}
		


    private void MonsterPatrol()
    {
		//将巡逻的过程分为去跟回
		//开始巡逻的时候
		if (!PatrolBack) {
			
		} else {
			//到达巡逻的地方需要返回到初始的地方
		}


		if (MonsterIsMoving ()) {
			MonsterAnimator.SetBool ("Run", true);
		} else { 
			MonsterAnimator.SetBool ("Run", false);
			//set the next Patrol place
			if (Vector3.Distance (EndPosition, PatrolPosition) <= 0.1f) {
				Debug.Log ("Goal!");
				PatrolPosition = StartPosition;
				gameObject.GetComponent<NavMeshAgent> ().destination = PatrolPosition;
				return;
			} 
			if(Vector3.Distance (StartPosition, PatrolPosition) <= 0.1f){
				PatrolPosition = EndPosition;
				gameObject.GetComponent<NavMeshAgent> ().destination = PatrolPosition;
			}
		}
    }

	private bool MonsterIsMoving(){
		if (Vector3.Distance (gameObject.transform.position, PatrolPosition) > 0.3f) {
			return true;
		} else {
			Debug.Log ("Turn Round");
			return false;
		}
	}

	//monster attack action
	private void MonsterAttack(){
		//found the attack target
//		if (NearTarget) {
//            Debug.Log("Attack0!");
//            StartCoroutine(AttackAcion());
//            if(Vector3.Distance(AttackTarget.position, gameObject.transform.position) > AttackAreaDistance)
//            {
//                NearTarget = false;
//            }
//		} else {
//			if (AttackCoolDown <= 0.0f) {
//				MonsterAnimator.SetBool ("Run", true);
//				//navigation set target destination to Nav Mesh Agent
//				if (!gameObject.GetComponent<NavMeshAgent> ().enabled) {
//					gameObject.GetComponent<NavMeshAgent> ().enabled = true;
//				}
//				gameObject.GetComponent<NavMeshAgent> ().destination = AttackTarget.position;
//			}
//		}
		//NearTarget是true，目标在monster的攻击范围之内,则播放攻击动画(此时并没有播放攻击动画)
		if(!animationAttackIsPlaying){
			StartCoroutine(AttackAcion());
		}

		//monster的攻击动画播放完毕

	}
	//for the Script:MonsterAttackAreaTrigger.cs to set bool NearTarget
	public void setNearTargetToTrue(){
		//player is in attack area
        AttackAreaDistance= Vector3.Distance(AttackTarget.position,gameObject.transform.position);
		Debug.Log ("monster attack radium" + AttackAreaDistance);
		NearTarget = true;
		isPatrolling = false;
	}
	private IEnumerator AttackAcion(){
		animationAttackIsPlaying = true;
		//NearTarget = false;
		//这个地方的AttackCoolDown不知道有没有其他的用处
		if (AttackCoolDown <= 0.0f) {
			AttackCoolDown = 2.0f;
		}
        gameObject.GetComponent<NavMeshAgent> ().enabled = false;
        MonsterAnimator.SetBool("Run",false);
		MonsterAnimator.SetTrigger ("Attack01");
		yield return new WaitForSeconds(1.9f);
		animationAttackIsPlaying = false;
		//如果monster完成了攻击的动画此时检查与攻击目标之间的距离，如果超过了攻击的范围则需要将“靠近了攻击目标（NearTarget）”置为false
		if(Vector3.Distance(gameObject.transform.position,AttackTarget.position) > AttackAreaDistance){
			NearTarget = false;
		}
			
	}


    //when mosnter is attacked
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MonsterTrigger");
        if (other.tag == "PlayerWeapon")
        {
            Debug.Log("Monster is Attack");
            MonsterIsDead = true;
            isPatrolling = false;
            StartCoroutine(MonsterDamaged());
        }
    }

    private IEnumerator MonsterDamaged() {
        if (MonsterIsDead)
        {
            if (gameObject.GetComponent<NavMeshAgent>().enabled)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
            MonsterAnimator.SetBool("Run", false);
            //MonsterAnimator.SetTrigger("Dead");
            /*if (gameObject.GetComponent<MonsterWarriorController>().enabled) {
                gameObject.GetComponent<MonsterWarriorController>().enabled = false;
            }*/
            if (gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                MonsterAnimator.SetTrigger("Dead");
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
            
            yield return new WaitForSeconds(2.0f);
        }
        else
        {
            Debug.Log("MonsterDamage");
            if (gameObject.GetComponent<NavMeshAgent>().enabled)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
            MonsterAnimator.SetBool("Run", false);
            MonsterAnimator.SetTrigger("Damage");
            yield return new WaitForSeconds(1.0f);
        }

    }
    private void MonsterToDelete() {
        Destroy(gameObject);
    }
}
