﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PolyPerfect;

public class AnimalBehavior : MonoBehaviour
{
    private AIStats animalStats;      //Variable to hold a reference to the animal stats such as aggression and toughness
    private Animal_WanderScript wanderScript;     //Variable to hold refernce to animal wander script
    private Animator animalAnimator;      //Variable to hold a refrence to the animator attached to the animal
    private GameObject player;    //Variable to hold refernce to the player in the scene
    public float animalReach;    //Variable to hold how far away the animal can attack
    private bool attacked = false;      //Variable to hold if the animal has already attacked during this attack animation state

    

    // Start is called before the first frame update
    void Start()
    {

        //Setting the variables
        wanderScript = this.GetComponent<Animal_WanderScript>();
        animalStats = wanderScript.stats;
        animalAnimator = this.GetComponent<Animator>();
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        //Checking if the animal is attacking
        if (animalAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            AnimalAttack();
        }
        else if(attacked)
        {
            attacked = false;
        }


        //Add check to see if player is attacking animal

    }

    /// <summary>
    /// This method handles checking if the animal is attacking the player and dealing damage to them if it hits them
    /// </summary>
    void AnimalAttack()
    {
        if (attacked) return;
        //Getting a direction vector between the player and animal
        Vector3 AnimaltoPlayer = player.transform.position - this.transform.position;
        //finding the distance between the player and animal
        float AtoPDistance = AnimaltoPlayer.magnitude;


        //Checking if the animal can reach the player
        if (AtoPDistance <= animalReach)
        {

            //Checking if the player is in front of the animal
            if (Vector3.Dot(AnimaltoPlayer, this.transform.forward) > 0)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(Random.Range(.001f, 3f) * (.5f * animalStats.power), Categories.DAMAGE_TYPE.ANIMAL);
                attacked = true;
            }
        }
        
    }
}
