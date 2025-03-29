using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public Transform player;
    public Vector3 offset=new Vector3(0f,0f,-10f);
   void LateUpdate() {
    if (player!=null){
        transform.position=player.position+offset;
    }
   }
}
