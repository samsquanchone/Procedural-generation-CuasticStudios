using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NpcController : MonoBehaviour
{
    public float lookRadius = 10f;
    public float moveSpeed = .9f;
    float gravity;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float distanceFromGround;
    float attackRange;
    float attackSpeed;
    GameObject thisObject;

    Transform target;
    CharacterController characterController;
    CharacterStats stats;
    float timeSinceAttack;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        characterController = GetComponent<CharacterController>();
        stats = GetComponent<CharacterStats>();
        attackRange = stats.attackRange;
        attackSpeed = stats.attackSpeed;

    }

    // Update is called once per frame
    void Update()
    {        
        float distanceFromTarget = Vector3.Distance(target.position, transform.position);       

        if(distanceFromTarget > lookRadius)
        {
            timeSinceAttack += Time.deltaTime; // make sure the attack speed stuff is still done even thought completely out of range
            return;
        }

        if(distanceFromTarget > attackRange)
        {
            NpcMotor(distanceFromTarget);
        }
        else if (distanceFromTarget <= attackRange && timeSinceAttack >= attackSpeed)
        {
            NpcAttack(distanceFromTarget);
            timeSinceAttack = 0;
        }

        timeSinceAttack += Time.deltaTime;
    }

    void NpcMotor(float distanceFromTarget)
    {

        Vector3 offset = target.position - transform.position; // simple way to calculate object => player offset
        float targetAngle = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (offset.magnitude > 0.1f && lookRadius >=  distanceFromTarget)
        {            
            offset = offset.normalized * moveSpeed;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            characterController.Move(offset * Time.deltaTime);
        }

        if (!characterController.isGrounded)
        {
            Vector3 dir = new Vector3();
            gravity -= (float)9.81 * Time.deltaTime;
            dir.y = gravity;
            characterController.Move(dir * Time.deltaTime);
            if (characterController.isGrounded) gravity = 0;
        }
    }

    void NpcAttack(float distanceFromTarget)
    {
        if (distanceFromTarget <= attackRange)
        {
            target.GetComponent<CharacterStats>().TakeDamage(stats.damage.GetValue()); // Player takes damage when npc is in attackrange
        }
    }

    // Gizmos for debuggin
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        
    }
}
