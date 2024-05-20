using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaguarEnemy : HP
{
    [Header("Enemy Settings")]
    public LayerMask belowLayer;
    public LayerMask frontLayer;
    public LayerMask attackLayer;
    public float belowDistance;
    public float frontDistance;
    
    public float frontAttackDistance;
    public Transform belowController;
    public Transform frontController;
    public Transform AttackController;
    public bool belowInfo;
    public bool frontInfo;
    public bool attackinfo;
    private float runspeed;

    private bool LookToRight = false;

    [Header("attack Setings")]
    [SerializeField] private Transform AttackOperator;
    [SerializeField] private float AttackRadio;
    [SerializeField] private float AttackDamage;
    [SerializeField] private float TimeBetweenAttack;
    [SerializeField] private float TimeNextAttack;
    [SerializeField] private float AttackDuration;
    [SerializeField] private float KBHitForece;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        runspeed=HorizontalMovement;
    }
    void Update()
    {
        //Move(HorizontalMovement,false);
        frontInfo = Physics2D.Raycast(frontController.position, transform.right, frontDistance, frontLayer);
        attackinfo = Physics2D.Raycast(AttackController.position, transform.right, frontAttackDistance, attackLayer);
        belowInfo = Physics2D.Raycast(belowController.position, transform.up * -1, belowDistance, belowLayer);
        animator.SetInteger("Walk",(int)HorizontalMovement);
        if (Input.GetKeyDown(KeyCode.I))
            {
                jump = true;
                animator.SetBool("Jump",true);
            }
        if(TimeNextAttack>0){
            TimeNextAttack -= Time.deltaTime;
        }
        if(attackinfo && TimeNextAttack <=0 && inFloor){
            animator.SetTrigger("AttackTrigger");
            Invoke("CharacterHit",AttackDuration);
            TimeNextAttack=TimeBetweenAttack;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("attack")){
            canMove=false;
            float FixedSpeed;
            if(LD){
                FixedSpeed= runspeed*1*.035f;
            }else{
                FixedSpeed= runspeed*-1*.035f;
            }
            rb.velocity=new Vector2(FixedSpeed,rb.velocityY);
        }else{
            canMove=true;
        }

        if(frontInfo || !belowInfo)
        {
            //Girar
            Girar();
        }
        if(!inFloor){
            if(rb.velocityY>0){
                animator.SetBool("Jump",true);
                animator.SetBool("Fall",false);
            }else{
                animator.SetBool("Jump",false);
                animator.SetBool("Fall",true);
            }

        }else{
                animator.SetBool("Fall",false);  
                animator.SetBool("Jump",false);
        }
        if(!Live){
            animator.SetTrigger("DIE");
            Destroy(gameObject);
        }
        
    }

    private void Girar()
    {
        LookToRight = !LookToRight;
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        HorizontalMovement *= -1;
    }
    private void CharacterHit(){
        Collider2D[] Objects = Physics2D.OverlapCircleAll(AttackOperator.position, AttackRadio);
        foreach (Collider2D colition in Objects){
            if(colition.CompareTag("Player")){
                colition.transform.GetComponent<HP>().Damage(AttackDamage,transform,KBHitForece);
                colition.transform.GetComponent<hpSystem>().hpBarChange();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(belowController.transform.position, belowController.transform.position + transform.up * -1 *  belowDistance);
        Gizmos.DrawLine(frontController.transform.position, frontController.transform.position + transform.right * frontDistance);
        Gizmos.DrawLine(AttackController.transform.position, AttackController.transform.position + transform.right * frontAttackDistance);
        Gizmos.DrawWireCube(OperadorSuelo.position, dimensionesCaja);
        Gizmos.DrawWireSphere(AttackOperator.position, AttackRadio);
    }
}