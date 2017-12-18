using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour {
    
/*     private AudioSource audioSource;

    private GameObject catTarget; */

	private AudioSource audioSource;
    private int hitPoint = 3;

    private Quaternion _lookRotation;
    private Vector3 _direction;

	// Use this for initialization
	void Start () {
    }

     void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        /* catTarget = GameObject.FindGameObjectWithTag("Cat");
        InvokeRepeating("CatchTheCat", 0.0f, 0.2f); */
    } 

	
	// Update is called once per frame
	void Update () {
        
	}


/*    void CatchTheCat()
    {
        if ( catTarget != null )
        {
            this.transform.position = Vector3.MoveTowards(
                this.transform.position,
                catTarget.transform.position,
                speed_move * Time.deltaTime
                );

            Vector3 vectorTocatTarget = catTarget.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorTocatTarget.y, vectorTocatTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed_rotate);
        }
    } */

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
            audioSource.Play();
            if(hitPoint == 1)
            {
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                Destroy(this.gameObject,3);
                GameObject.Find("UIManager").SendMessage("CountKills");
            }else
            {
                hitPoint --;
                //GameObject.Find("human").SendMessage("hitPointMinus");
                this.gameObject.GetComponent<HumanAI>().hitpoint --;
            }
            
        }

        if(coll.gameObject.tag == "Cat"){
            GetComponent<Collider2D>().enabled = false;
        }

    }

}
