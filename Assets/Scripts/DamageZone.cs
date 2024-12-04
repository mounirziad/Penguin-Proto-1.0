//Code written by Jason Westcott and Mounir Ziad
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageZone : MonoBehaviour
{
   void OnTriggerStay2D(Collider2D other)
   {
       PlayerController controller = other.GetComponent<PlayerController>();



       if (controller != null)
       {
        
           controller.ChangeHealth(-20);
       }


   }
}