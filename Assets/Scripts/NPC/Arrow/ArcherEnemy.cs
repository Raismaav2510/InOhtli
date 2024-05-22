using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ArcherEnemy : ParabolArrow
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
    public bool belowInfo;
    public bool frontInfo;
    private Transform player;

    private bool LookToRight = false;
    

    [Header("attack Setings")]
    [SerializeField] private float AttackRadio;
    [SerializeField] private float AttackDamage;
    [SerializeField] private float TimeBetweenAttack;
    [SerializeField] private float TimeNextAttack;
    [SerializeField] private float AttackDuration;
    private string str;    public float detectionRatio = 15;
    private float distancex;
    private float distancey;
    [Header("Sound Settings")]
    [SerializeField] private AudioClip AttackAudioClip;
    [SerializeField] private AudioSource audioSource;
    private bool canSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
    }
    void Update()
    {
        if(invincibleTime>0){
            invincibleTime -= Time.deltaTime;
        }
        //Move(HorizontalMovement,false);
        frontInfo = Physics2D.Raycast(frontController.position, transform.right, frontDistance, frontLayer);
        belowInfo = Physics2D.Raycast(belowController.position, transform.up * -1, belowDistance, belowLayer);
        animator.SetInteger("Walk",(int)HorizontalMovement);
        if (Input.GetKeyDown(KeyCode.I))
        {
            jump = true;
            animator.SetBool("Jump",true);
        }
        if(!player.IsDestroyed()){
            distancex = player.transform.position.x - transform.position.x;
            distancey = player.transform.position.y - transform.position.y;
            if(distancex < detectionRatio && distancex > -1*detectionRatio && distancey < detectionRatio && distancey > -1*detectionRatio){
                if(TimeNextAttack <=0 && inFloor){
                    animator.SetTrigger("AttackTrigger");
                    Invoke("shot",AttackDuration);
                    TimeNextAttack=TimeBetweenAttack;
                    canSound=true;
                }
            }
        }
        if(TimeNextAttack>0){
            TimeNextAttack -= Time.deltaTime;
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
            //animator.SetTrigger("DIE");
            Destroy(gameObject);
        }
        
    }

    private void Girar()
    {
        LookToRight = !LookToRight;
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        HorizontalMovement *= -1;
    }
    private IEnumerator soundWait(){
        yield return new WaitForSeconds(1);
        if(!audioSource.isPlaying&&canSound){
            canSound=false;
            //audioSource.PlayOneShot(AttackAudioClip);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(belowController.transform.position, belowController.transform.position + transform.up * -1 *  belowDistance);
        Gizmos.DrawLine(frontController.transform.position, frontController.transform.position + transform.right * frontDistance);
        Gizmos.DrawWireCube(OperadorSuelo.position, dimensionesCaja);
    }
}