//Code written by Jason Westcott and Mounir Ziad
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    // Start is called before the first frame update
    //When game object collides 
    void OnTriggerEnter2D(Collider2D other)
    {
        //Get playercontroller components. 
       PlayerController controller = other.GetComponent<PlayerController>();
       //If playercontroller has the components of the player, as in, this function will only work if the controller
       //variable has a value. 
       
       if (controller != null && controller.health < controller.maxHealth)
       {
          //changes health by increasing it by 20. References the playercontrollers changehealth function.
          controller.ChangeHealth(20);
          Destroy(gameObject);  
       }
      
    }
    


    
}
