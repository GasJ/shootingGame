﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    private AudioSource audioSource;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	}
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Human" || coll.gameObject.tag == "Wall")
        {
            audioSource.Play();
            //invisible the bullet to play the audio.
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject,3);
        }
    }
}
