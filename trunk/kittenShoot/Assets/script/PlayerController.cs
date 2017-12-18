using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public GameObject spawnBullet;

    private AudioSource audioSource;

    public bool leave = false;
    public float fire_Rate = 1f;
    private float firePreparation = 0.0f;
    public float movement_speed = 6f;
    public float rotation_speed = 130f;
    Animator anim;
    private GameObject bullet;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        this.gameObject.transform.position = new Vector3(0f, 0f, 0f);
        anim = gameObject.GetComponent<Animator>();
    }
    void Update()
    {

        anim.SetBool("Walk", false);
        anim.SetBool("Shoot", false);

        if(!leave){
        // MOVEMENT
        float horizontal_input = -Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        // Horizontal is rotation
        this.transform.Rotate(new Vector3(0, 0, horizontal_input * rotation_speed * Time.deltaTime ));

        // Vertical is forwards/backwards
        if (vertical_input != 0 ||  horizontal_input != 0)
        {
            anim.SetBool("Walk", true);
            this.transform.Translate(Vector2.right * vertical_input * movement_speed * Time.deltaTime);
        }

        // SHOOTING
        bool  is_shooting = Input.GetButton("Fire1");
        bool  fier2 = Input.GetButton("Fire2");
         bool  fier3 = Input.GetButton("Fire3");


        
        // Is the user holding down LEFT CLICK?
        if ((is_shooting || fier2 || fier3 ) &&  Time.time > firePreparation)
        {
            // Create the bullet
            bullet = (GameObject)Instantiate(spawnBullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 1)));
            bullet.transform.rotation = transform.rotation;

            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());


            StartCoroutine(bulletVanish());
            // Give it a velocity
            Rigidbody2D bullet_phsics = bullet.GetComponent<Rigidbody2D>();
            bullet_phsics.velocity = 8 * this.transform.right;
            //8 is the speed of the bullet
            
            
            // Set the shooting cooldown 
            firePreparation = Time.time + fire_Rate;
            anim.SetBool("Shoot", true);
        }

        }
    }
    
    IEnumerator bulletVanish()
    {
        bullet.GetComponent<SpriteRenderer>().enabled=false;
        yield return new WaitForSeconds(0.2f);
        bullet.GetComponent<SpriteRenderer>().enabled=true;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Human")
        {
            leave = true;
            StartCoroutine(CatReset());
            
        }

    }

     IEnumerator CatReset()
    {
        audioSource.Play();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

