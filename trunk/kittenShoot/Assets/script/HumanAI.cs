using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/* 
AI Value Descepline
threat zone:
if the cat in the area of 3.8f x 3.8f
if it is, we say it is in the threat zone.
this value is suitable for the whole map.

in range:
if the cat in the area of 2.5f x 2.5f
if it is, we say it is in the range.
this value is suitable for the whole map.

friends distance:
if there is another human in radius 3.0f, distance < 2, 
we say this human have a friend near him.
this value is suitable for the whole map.

spawn distance:
if the distance between this human and his spawn point > 5
we ask him to go back.
this value is suitable for the whole map.

random walk or idle:
60% probilaty walk
40% idle
choose this value, because then this human can both walk and idle

hp low:
hitpoint <= 1 means hp is low.
this value is suitable, because if one more shooted by the cat, this human will die.

 */
public class HumanAI : MonoBehaviour 
{
    private DecisionTree root;
	public int hitpoint = 3;
    public float speed_move = 1f;
    public float speed_rotate = 50f;

    public float number;

	public Vector3 spawnTransform;

    private GameObject catTarget;

    //private bool isActive = false;

    //private Rigidbody2D human;
    // Use this for initialization
    void Start () {
        //isActive = true;
        //human = gameObject.GetComponent<Rigidbody2D>();
        number = Time.deltaTime;
        BuildDecisionTree();
	}

    private void BuildDecisionTree()
    {
        /******  Decision Nodes  ******/
		DecisionTree isTreateNode = new DecisionTree();
		isTreateNode.SetDecision(CheckInTreatZone);
        isTreateNode.setName("isTreateNode");

		DecisionTree isInRangeNode = new DecisionTree();
		isInRangeNode.SetDecision(CheckCatInRange);
        isInRangeNode.setName("isInRangeNodee");

        DecisionTree tooFarSpwanNode = new DecisionTree();
		tooFarSpwanNode.SetDecision(CheckSpwanDist);
        tooFarSpwanNode.setName("tooFarSpwanNode");

        DecisionTree randomMovement = new DecisionTree();
		randomMovement.SetDecision(ARandomMovement);
        randomMovement.setName("randomMovement");

        DecisionTree FriendsNear = new DecisionTree();
		FriendsNear.SetDecision(checkFriendsNear);
        FriendsNear.setName("FriendsNear");

        DecisionTree hpLowAorR = new DecisionTree();
		hpLowAorR.SetDecision(checkHp);
        hpLowAorR.setName("hpLowAorR");

        DecisionTree hpLowRorAD = new DecisionTree();
		hpLowRorAD.SetDecision(checkHp);
        hpLowRorAD.setName("hpLowRorAD");

        DecisionTree actGoSpawnNode = new DecisionTree();
		actGoSpawnNode.SetAction(goBacktoSpawn);
        actGoSpawnNode.setName("actGoSpawnNode");

		DecisionTree actIdleNode = new DecisionTree();
		actIdleNode.SetAction(Idle);
        actIdleNode.setName("actIdleNode");

		DecisionTree actWalkNode = new DecisionTree();
		actWalkNode.SetAction(Walk);
        actWalkNode.setName("actWalkNode");

		DecisionTree actAttackNode = new DecisionTree();
		actAttackNode.SetAction(Attack);
        actAttackNode.setName("actAttackNode");

        DecisionTree actRetreatNode = new DecisionTree();
		actRetreatNode.SetAction(Retreat);
        actRetreatNode.setName("actRetreatNode");

        DecisionTree actAdvanceNode = new DecisionTree();
		actAdvanceNode.SetAction(Advance);
        actAdvanceNode.setName("actAdvanceNode");



		/******  Assemble Tree  ******/
        isTreateNode.SetLeft(isInRangeNode);
        isTreateNode.SetRight(tooFarSpwanNode);

        isInRangeNode.SetLeft(FriendsNear);
        isInRangeNode.SetRight(hpLowRorAD);

        tooFarSpwanNode.SetLeft(actGoSpawnNode);
        tooFarSpwanNode.SetRight(randomMovement);

        FriendsNear.SetLeft(actAttackNode);
        FriendsNear.SetRight(hpLowAorR);


        hpLowAorR.SetLeft(actRetreatNode);
        hpLowAorR.SetRight(actAttackNode);

        hpLowRorAD.SetLeft(actRetreatNode);
        hpLowRorAD.SetRight(actAdvanceNode);

        randomMovement.SetLeft(actWalkNode);
        randomMovement.SetRight(actIdleNode);

		root =  isTreateNode;
    }

   void Advance()
    {
        Debug.Log("Advance");
        speed_move = 1f;
        CatchTheCat();
    }

    void Retreat()
    {
        Debug.Log("Retreat");
        speed_move = 2f;
        RetreatingBack();
    }

    void Attack()
    {
        Debug.Log("Attack");
        speed_move = 2f;
        CatchTheCat();
    }

    private float timeToChangeDirection = 0f;
     void Walk()
    {
        Debug.Log("we are random walking.");
        Vector3 randomPoint = new Vector3();
        timeToChangeDirection -= Time.deltaTime;
        randomPoint = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f), 0);
        timeToChangeDirection = 1.5f;
        randomPoint += this.transform.position;
        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            randomPoint
            , speed_move * Time.deltaTime
            );
        
        Vector3 vectorToTarget = randomPoint - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed_rotate);
    }

    void Idle()
    {
        Debug.Log("idle");
        if(ARandomMovement()){
            Quaternion q = Quaternion.AngleAxis(180, Vector3.forward);
            this.transform.rotation =  Quaternion.Slerp(transform.rotation, q, 0);
        }
    }

    void goBacktoSpawn()
    {
        Debug.Log("we are going back");
        this.transform.position = Vector3.MoveTowards(
                this.transform.position,
                spawnTransform,
                speed_move * Time.deltaTime);
        
        Vector3 vectorToSpawn = spawnTransform - transform.position;
        float angle = Mathf.Atan2(vectorToSpawn.y, vectorToSpawn.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed_rotate);
        
    }

     bool checkHp()
    {
        Debug.Log("hitpoint is " + hitpoint);
        return hitpoint <= 1;
    }

     bool checkFriendsNear()
    {
        return checkDist("Human", 3.0f);
    }

         bool CheckSpwanDist()
    {
        return Vector2.Distance(this.gameObject.transform.position, spawnTransform) > 5; 
    }

        bool checkDist(String tag, float radius){
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, radius);
            foreach(Collider2D things in colliders){
                Debug.Log("cheking " + things.tag);
                if(things.tag == tag 
                && things.GetComponent<HumanAI>().number != this.number
                && Vector2.Distance(this.transform.position, things.transform.position) < 2){
                    Debug.Log("near friends");
                    return true;
                }
            }
            if(tag == "Human") Debug.Log("not near friends");
            Debug.Log("going out from decisiong.");
            return false;
        }


     bool ARandomMovement()
    {
        System.Random moveDecision = new System.Random();
        return moveDecision.Next(100)  <= 60;
    }

     bool CheckCatInRange()
    {
        //the cat attack range should be a 3 x 3 rectangle 
       return checkZone(2.5f, 2.5f);
       //return Vector2.Distance(this.transform.position, catTarget.transform.position) < 5;
    }

    bool CheckInTreatZone()
    {
        //the threat range should be a 5 x 5 rectangle 
      return checkZone(3.8f, 3.8f);
       //return Vector2.Distance(this.transform.position, catTarget.transform.position) < 3;
    }

    bool checkZone(float xRange, float yRange){
        //get position valuse of this human and the cat
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float catX = catTarget.transform.position.x;
        float catY = catTarget.transform.position.y;
        
        //the range should be a 20 x 20 rectangle 
/*         if((catX + (xRange/2) >= x || catX - (xRange/2) <= x) 
        && (catY + (yRange/2) >= y || catY - (yRange/2) <= y)){
            return true;
        } */

        /* 
        yxxxxxxxxxxxxy
        y                      y
        y        cat         y 5
        y                      y 2
        yxxxxxxxxxxxxx 0

        caty - 
         */
         if(Math.Abs(catY - y) <= yRange && Math.Abs(catX - x) <= xRange){
             Debug.Log("cat in range");
             return true;
         }
         Debug.Log("cat not in range");
        return false;
    }

    void Awake()
    {
        catTarget = GameObject.FindGameObjectWithTag("Cat");
        /* InvokeRepeating("CatchTheCat", 0.0f, 0.2f); */
    }
	
	// Update is called once per frame
	void Update () {
        root.Search();	
	}

    
	   void CatchTheCat()
    {
        Debug.Log("catching the cat");
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
    }

    void RetreatingBack()
    {
        Debug.Log("retreating.");
        if (catTarget != null)
        {
            this.transform.position =  Vector3.MoveTowards(
                this.transform.position,
                catTarget.transform.position,
                -speed_move * Time.deltaTime
                );

            Vector3 vectorToTarget = catTarget.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed_rotate);
        }
    }
}
