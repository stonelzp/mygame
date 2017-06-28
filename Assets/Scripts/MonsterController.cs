using UnityEngine;
using System.Collections;
using UnityEngine.AI;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class MonsterController : MonoBehaviour {
    public Transform AttackTarget;
    
    private Animator MonsterAnimator;
    private bool isPatrolling;
	//near the player to attack;
	public bool NearTarget;
	//the monster next to Patrol position
    private Vector3 PatrolPosition;
	private Vector3 StartPosition;
	private Vector3 EndPosition;

    //the distance of Monster Attack Area
    private float AttackAreaDistance = 0.0f;
	private float AttackCoolDown = 0.0f;


	// Use this for initialization
	void Start () {
        MonsterAnimator = gameObject.GetComponent<Animator>();
		StartPosition = gameObject.transform.position;
		PatrolPosition=new Vector3 (transform.position.x,transform.position.y,transform.position.z-15.0f);
		EndPosition = PatrolPosition;
		gameObject.GetComponent<NavMeshAgent> ().destination = PatrolPosition;
		isPatrolling = false;
		NearTarget = false;
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
		if (Vector3.Distance (gameObject.transform.position, PatrolPosition) > 0.1f) {
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

}
