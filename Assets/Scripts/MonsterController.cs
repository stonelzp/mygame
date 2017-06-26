using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class MonsterController : MonoBehaviour {
    public Transform AttackTarget;
    


    private Animator MonsterAnimator;
    private bool isPatrolling;
	//the monster next to Patrol position
    private Vector3 PatrolPosition;
	private Vector3 StartPosition;
	private Vector3 EndPosition;


	// Use this for initialization
	void Start () {
        MonsterAnimator = gameObject.GetComponent<Animator>();
		StartPosition = gameObject.transform.position;
		PatrolPosition=new Vector3 (transform.position.x,transform.position.y,transform.position.z-15.0f);
		EndPosition = PatrolPosition;
		gameObject.GetComponent<NavMeshAgent> ().destination = PatrolPosition;
		isPatrolling = true;
	}

    // Update is called once per frame
    void Update() {
        //sGetMonsterInput(); 

		if (isPatrolling) {
			MonsterPatrol ();
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


}
