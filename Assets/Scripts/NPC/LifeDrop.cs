using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDrop : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            other.GetComponent<HP>().Cure(Random.Range(1,4));
            other.GetComponent<hpSystem>().hpBarChange();
            Destroy(gameObject);
        }
    }
}