using NavMeshPlus.Extensions;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;


    public float dashRange = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 2f;

    private float dashTimer;
    private float cooldownTimer;
    private Vector2 dashDirection;
    private bool isDashing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);
        


        if (player != null)
        {
            if(distance <= dashRange && cooldownTimer <= 0f)
            {
                startDash();
            }
            if (isDashing)
            {
                dashMove();
            }
            else
            {
                navMeshAgent.SetDestination(player.position);
                pointAtPlayer(player);
            }

                


        }
    }

    void pointAtPlayer(Transform player)
    {
        if (player != null)
        {
            Vector2 direction = player.position - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f,0f,angle);


            //Debug
            Vector2 dirNorm = direction.normalized;
            Debug.DrawLine(transform.position, (Vector2)transform.position + dirNorm * dashRange, Color.red);



        }
    }

    void startDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
        dashDirection = (player.position - transform.position).normalized;
        navMeshAgent.isStopped = true;
    }
    void dashMove()
    {
        if (dashTimer > 0f)
        {
            transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);
            dashTimer -= Time.deltaTime;
        }
        else
        {
            endDash();
        }
    }

    void endDash()
    {
        isDashing = false;
        navMeshAgent.isStopped = false;
    }
}
