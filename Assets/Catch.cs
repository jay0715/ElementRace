using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : Photon.MonoBehaviour
{
    // public Transform catchTransform;
    // public ParticleSystem ps;

    public Transform attackPointLeft;
    public Transform attackPointRight;
    public float attackRange = 0.5f;
   // public LayerMask players;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("shoot");
                photonView.RPC("RPC_Catch", PhotonTargets.All);
            }
        }
    }

    [PunRPC]
    private void RPC_Catch()
    {
        //ps.Play();
        //Ray ray = new Ray(catchTransform.position, catchTransform.forward);
        // if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        /*float laserLength = 1f;
        RaycastHit2D hit = Physics2D.Raycast(catchTransform.position, catchTransform.forward, laserLength);
        if(hit.collider != null)
        {
            var enemyHealth = hit.collider.GetComponent<Health>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(100);
                Debug.Log("hit");
            }
        }
        //Debug.Log("RUN");
        */

        bool target = GetComponent<Player>().m_FacingLeft;
        if (target == true)
        {
            Collider2D[] hitEnemiesL = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange);
            foreach (Collider2D enemyL in hitEnemiesL)
            {
                enemyL.GetComponent<Health>().TakeDamage(100);
                Debug.Log("Hit Left");      
            }
        }
        else
        {
            Collider2D[] hitEnemiesR = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange);
            foreach (Collider2D enemyR in hitEnemiesR)
            {
                enemyR.GetComponent<Health>().TakeDamage(100);
                Debug.Log("Hit right");
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPointLeft == null)
            return;
        Gizmos.DrawWireSphere(attackPointLeft.position, attackRange);

        if (attackPointRight == null)
            return;
        Gizmos.DrawWireSphere(attackPointRight.position, attackRange);
    }
}
