using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public Sprite catSprite;
	public Sprite humanSprite;
	public RuntimeAnimatorController catAnimator;
    public AudioClip catCatchedSound;
    public AudioClip humanDeadSound;
    public float catMoveSpeed = 6f;
	public float catRotationSpeed = 130f;
	public float catFireSpeed = 1f;
    public int humanHitPoint = 3;
	public float humanMoveSpeed = 5f;
	public float humanRotatSpeed = 50f;


/* 
	we do the initialization in the Awake() function
 */
	void Awake(){
		/* this is  sprite renderer for human or cat */
		SpriteRenderer spRender = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        
        /* this is Rigidbody2D for human or cat */
        Rigidbody2D RBody2D = gameObject.AddComponent<Rigidbody2D>() as Rigidbody2D;
		/* set gravity scale of RBody2D to 0 for cat or human
			then it won't drop out of the screen. 
		*/
        RBody2D.gravityScale = 0;
		RBody2D.simulated = true;

		/* this is Audio Source for human or cat */
        AudioSource auSource = gameObject.AddComponent<AudioSource>() as AudioSource;

		/* set things for cat */
		if(tag == "Cat"){

			/* this part is sprite and sortingLayer for cat */
            spRender.sprite = catSprite;
            spRender.sortingLayerName = "characters";

			/*  this part is Collider2D  for cat */
            CircleCollider2D cirCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
            cirCollider.offset = new Vector2(-0.1131339f, -0.4774529f);
            cirCollider.radius = 0.4398445f;

			BoxCollider2D boxColCat = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
			boxColCat.offset = new Vector2(0.6331149f, -0.4399056f);
			boxColCat.size = new Vector2(0.627313f, 0.1023629f);
            
			//this collider is only for fun, but we unable it at most time
			PolygonCollider2D polyCol = gameObject.AddComponent<PolygonCollider2D>() as PolygonCollider2D;
			polyCol.enabled = false;
			polyCol.isTrigger = true;
			polyCol.usedByEffector = true;

			/* this part is animator  for cat  */
            Animator catAnim = gameObject.AddComponent<Animator>() as Animator;
            catAnim.runtimeAnimatorController = catAnimator;

			/* this part is Audio Source  for cat  */
            auSource.clip = catCatchedSound;
            auSource.playOnAwake = false;

			/* this part is adding script (controller) for cat  */
            PlayerController catMove = gameObject.AddComponent<PlayerController>() as PlayerController;
            catMove.movement_speed = catMoveSpeed;
			catMove.rotation_speed = 130f;
			catMove.fire_Rate = 1f;
			return;
		}

		/* now we set for human */

			/* set rigid body setting for human */
			RBody2D.mass = 10000;
			RBody2D.freezeRotation = true;
			RBody2D.angularDrag = 0.05f;
			RBody2D.bodyType = RigidbodyType2D.Dynamic;


			/* this part is sprite and sortingLayer for human */
            spRender.sprite = humanSprite;
            spRender.sortingLayerName = "human";

            /* this part is Collider2D  for human */
            BoxCollider2D boxCol = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
            boxCol.size = new Vector2(0.746667f, 0.553333f);

            /* this part is Audio Source  for human  */
            auSource.clip = humanDeadSound;
            auSource.playOnAwake = false;

            // add script: zombieAI for zombies
            HumanAI aiHuman = gameObject.AddComponent<HumanAI>() as HumanAI;
            aiHuman.hitpoint = humanHitPoint;
			aiHuman.speed_move = humanMoveSpeed;
			aiHuman.speed_rotate = humanRotatSpeed;

			HumanController humanControl = gameObject.AddComponent<HumanController>() as HumanController;

	}
}
