using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFollow : Enemy
{
    //keep the move function 

    protected override void Awake()
    {
        //This class is instantiated upon another enemy's death, have that enemya add to the room manager
        //base.Awake();
    }
}
