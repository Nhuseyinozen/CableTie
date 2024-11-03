using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionControl : MonoBehaviour
{

    public GameManager GameManager;
    public int CollisionIndex;



    void Update()
    {


        Collider[] HitColl = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

        for (int i = 0; i < HitColl.Length; i++)
        {
            if (HitColl[i].CompareTag("CablePiece"))
            {
                GameManager.CollisionControl(CollisionIndex, false);
            }
            else
            {
                GameManager.CollisionControl(CollisionIndex, true);
            }
        }



    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale / 2);
    }
}
