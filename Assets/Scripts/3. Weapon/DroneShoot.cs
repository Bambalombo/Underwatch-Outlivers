using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneShoot : MonoBehaviour
{
    // Start is called before the first frame update
 [SerializeField] float shootingDistance = 7f;
    [SerializeField] float speedProjectile = 5f;
    [SerializeField] float fireRate = 3f;
    public GameObject laserProjectile;
    GameObject target;
    bool canShoot = true;
 

    private void Start()
    {
        KillTimer();
    }

    void Update ()
    {
      
        if (canShoot) {
            canShoot = false;
            //Coroutine for delay between shooting
            StartCoroutine("AllowToShoot");
            //array with enemies
            //you can put in start, iff all enemies are in the level at beginn (will be not spawn later)
            GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Enemy");
            if (allTargets != null)
            {
                target = allTargets[0];
                //look for the closest
                foreach (GameObject tmpTarget in allTargets)
                {
                    if (Vector2.Distance(transform.position, tmpTarget.transform.position) < Vector2.Distance(transform.position, target.transform.position))
                    {
                        target = tmpTarget;
                    }
                }
                //shoot if the closest is in the fire range
                if (Vector2.Distance(transform.position, target.transform.position) < shootingDistance)
                {
                    Fire();
                }
            }
        }
    }
 
    void Fire ()
    {
        Debug.Log("skyder");
        Vector2 direction = target.transform.position - transform.position;
        //link to spawned arrow, you dont need it, if the arrow has own moving script
        GameObject tmpArrow = Instantiate(laserProjectile, transform.position, transform.rotation);
        tmpArrow.transform.right = direction;
        tmpArrow.GetComponent<Rigidbody2D>().velocity = direction.normalized * speedProjectile;
    }
 
    IEnumerator AllowToShoot ()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    

}