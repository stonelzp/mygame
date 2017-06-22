using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {
    private Animator MonsterAnimator;
	// Use this for initialization
	void Start () {
        MonsterAnimator = gameObject.GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update() {
        GetMonsterInput(); 
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
}
