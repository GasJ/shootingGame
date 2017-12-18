using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_player : MonoBehaviour {

    private GameObject catTarget;
    public float speed_move = 5f;
    public float speed_rotate = 50f;

    private Quaternion _lookRotation;
    private Vector3 _direction;


    void Awake()
    {
        catTarget = GameObject.FindGameObjectWithTag("Cat");
        InvokeRepeating("CatchTheCat", 0.0f, 0.2f);
    }

    void CatchTheCat()
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
            Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed_rotate);
        }
    }
}
