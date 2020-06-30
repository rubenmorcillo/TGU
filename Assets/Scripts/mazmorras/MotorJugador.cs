using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]


public class MotorJugador : MonoBehaviour
{
    NavMeshAgent agenteNav;
    Animator anim;
    void Start()
    {
        agenteNav = GetComponent<NavMeshAgent>();
        agenteNav.speed = 4f;
        agenteNav.acceleration = 18f;
        agenteNav.angularSpeed = 1000f;
        agenteNav.stoppingDistance = 1f;

        anim = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
       //if (agenteNav.isActiveAndEnabled)
       // {
           
       //         if (gameObject.transform.position == agenteNav.pathEndPosition)
       //         {
       //             anim.SetBool("moving", false);
       //         }
       // }
    }

    public void MoverAlPunto(Vector3 punto)
    {
        //anim.SetBool("moving", true);
		agenteNav.SetDestination(punto);		
        
    }
}
