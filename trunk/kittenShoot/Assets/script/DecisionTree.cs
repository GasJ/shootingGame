using UnityEngine;
using System.Collections;

public class DecisionTree {

	public delegate bool Decision();
	public delegate void Action();

//    int tmp;

	DecisionTree left;
	DecisionTree right;
	Decision myDecision;
	Action myAction;

	string name;

	// Constructor
	public DecisionTree()
	{
		left = null;
		right = null;
		myDecision = null;
		myAction = null;
		name = null;
	}

	public void SetDecision(Decision dec)
	{
		myDecision = dec;
	}

	public void setName(string name){
		this.name = name;
	}
	public void SetAction(Action act)
	{
		myAction = act;
	}

	public void SetLeft(DecisionTree t)
	{
		left = t;
	}

	public void SetRight(DecisionTree t)
	{
		right = t;
	}
		
	/****** Make Decisions *******/

	public bool Decide()
	{
		return myDecision();
	}

	public void goLeft()
	{
		left.Search();
	}

	public void goRight()
	{
		right.Search();
	}

	public void Search()
	{
		string hi = "hola world";
		Debug.Log("node's name " +  name + hi);
		// recursive base case
		if(myAction != null)
		{
			myAction();
		}			
		else if(Decide())
		{
			goLeft();
		}	
		else
		{
			goRight();
		}
	}
}
