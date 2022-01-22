using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Photon.MonoBehaviour,IPunObservable
{
     public int maxhealth = 100;
     public int currentHealth;

     public HealthBar healthBar;
     public GameObject checkPoint;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(maxhealth);
        }
        else
        {
            maxhealth = (int)stream.ReceiveNext();
        }
    }

    IEnumerator Respawn()
    {
        maxhealth = 100;
        currentHealth = maxhealth;
        healthBar.SetMaxHealth(maxhealth);
        transform.position = new Vector2(checkPoint.transform.position.x, checkPoint.transform.position.y);
        yield return new WaitForSecondsRealtime(3);
 

    }
    void Start()
    {
       currentHealth = maxhealth;
       healthBar.SetMaxHealth(maxhealth);
       
    }

    // Update is called once per frame
    void Update()
    {         
          if (currentHealth <= 0)
        {
            Died();

        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }


    public void Died()
    {
        StartCoroutine(Respawn());
    }


}
