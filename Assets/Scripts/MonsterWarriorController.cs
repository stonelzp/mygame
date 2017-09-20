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

    //the distance of Monster Attack Area
    private float AttackAreaDistance = 0.0f;
	private float AttackCoolDown = 0.0f;

	//the monster origin position
	private Vector3 MonsterOriginPosition;


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

		if (isPatrolling) {
			MonsterPatrol ();
		} else {
			MonsterAttack ();
		}
        Debug.Log(MonsterIsDead);
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
    private void MonsterPatrol()
    {
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
		if (NearTarget) {
            Debug.Log("Attack0!");
            StartCoroutine(AttackAcion());
            if(Vector3.Distance(AttackTarget.position, gameObject.transform.position) > AttackAreaDistance)
            {
                NearTarget = false;
            }
		} else {
			if (AttackCoolDown <= 0.0f) {
				MonsterAnimator.SetBool ("Run", true);
				//navigation set target destination to Nav Mesh Agent
				if (!gameObject.GetComponent<NavMeshAgent> ().enabled) {
					gameObject.GetComponent<NavMeshAgent> ().enabled = true;
				}
				gameObject.GetComponent<NavMeshAgent> ().destination = AttackTarget.position;
			}
		}

	}
	//for the Script:MonsterAttackAreaTrigger.cs to set bool NearTarget
	public void setBoolNearTarget(bool sign){
        AttackAreaDistance= Vector3.Distance(AttackTarget.position,gameObject.transform.position);
		NearTarget = sign;
		isPatrolling = false;
	}
	private IEnumerator AttackAcion(){
		if (AttackCoolDown <= 0.0f) {
			AttackCoolDown = 2.0f;
		}
        Debug.Log("Attack!");
        gameObject.GetComponent<NavMeshAgent> ().enabled = false;
        MonsterAnimator.SetBool("Run",false);
		MonsterAnimator.SetTrigger ("Attack01");
		yield return new WaitForSeconds(1.5f);
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
