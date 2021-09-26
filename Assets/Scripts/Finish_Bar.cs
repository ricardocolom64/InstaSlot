using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish_Bar : MonoBehaviour
{
    public float x, y, z;
    
    public string[] Winbox={"Cherry", "Apple", "Banana","Lemon"};
    public string[] Special={"Star", "Bonus", "Lucky7"};

    public GameObject[] endModels;

    private GameObject theEndModel;

    // 0 is a loss, 1 is a normal win, 2 is a star win, 3 is a lucky 7 win, 4 is a free spin (in the event that winbar lands on nothing)
    public int winType = 0;
    public string resultMsg = "";
    
    public FruitManager FruitManagerScript;
    public Text subText;

    private Vector3 scaleRef = Vector3.zero;

    // initialize all fruit counts to 0.
    public int cherryCount = 0, appleCount = 0, bananaCount = 0, lemonCount = 0, lucky7Count = 0;
    
    public bool win = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the bounds of the box collider to conform with the bounds of the object it is on
        x = GetComponent<BoxCollider>().bounds.extents.x;
        y = GetComponent<BoxCollider>().bounds.extents.y;
        z = GetComponent<BoxCollider>().bounds.extents.z;
    }


    void Update()
    {
        // Set subText to the appropriate message.
        subText.text = resultMsg;

        // Create a new OverlapBox collider
        Collider[] Winbar;
        Winbar = Physics.OverlapBox(transform.position, new Vector3(x,y,z));

        // If lookAtFruits == true (If the user has pressed stop and the fruits have come to a complete stop)
        if(FruitManagerScript.lookAtFruits == true)
        {
            // For each collision with the winbar, add 1 to the appropriate counter, or immediately set win to true if a star is colliding with Winbar
            foreach(Collider collision in Winbar)
            {
                if(collision.tag == Special[0])
                {
                    win = true;
                    winType = 2;
                    theEndModel = Instantiate(endModels[2], endModels[2].transform.position, endModels[2].transform.rotation);
                    resultMsg = "Special Star!";
                }
                if(collision.tag == Winbox[0])
                    cherryCount += 1;
                if(collision.tag == Winbox[1])
                    appleCount += 1;
                if(collision.tag == Winbox[2])
                    bananaCount += 1;
                if(collision.tag == Winbox[3])
                    lemonCount += 1;
                if(collision.tag == Special[2])
                    lucky7Count += 1;
                if(collision.tag == Special[1])
                {
                    cherryCount += 1;
                    appleCount += 1;
                    bananaCount += 1;
                    lemonCount += 1;
                    lucky7Count += 1;
                }
            }
            if(lucky7Count == 3)
            {
                win = true;
                winType = 3;
                theEndModel = Instantiate(endModels[3], endModels[3].transform.position, endModels[3].transform.rotation);
                resultMsg = "3 Lucky 7's!";
            }
            else if(cherryCount == 3 || appleCount == 3 || bananaCount == 3 || lemonCount == 3)
            {
                win = true;
                winType = 1;
                theEndModel = Instantiate(endModels[1], endModels[1].transform.position, endModels[1].transform.rotation);
                resultMsg = "3 Fruits In a Row!";
            }
            else if(cherryCount == 0 && appleCount == 0 && bananaCount == 0 && lemonCount == 0 && lucky7Count == 0 && winType != 2)
            {
                winType = 4;
                theEndModel = Instantiate(endModels[4], endModels[4].transform.position, endModels[4].transform.rotation);
                resultMsg = "Free Spin!";
            }

            if(winType == 0)
            {
                theEndModel = Instantiate(endModels[0], endModels[0].transform.position, endModels[0].transform.rotation);
            }

            // ADD MODEL FOR FREE SPIN OR IT WILL BREAK IN THAT CASE
            theEndModel.transform.Translate(Vector3.down * 15f);
            theEndModel.transform.localScale = Vector3.zero;
            theEndModel.transform.Translate(Vector3.up * 15f);

            FruitManagerScript.lookAtFruits = false;

            if(winType == 0)
            {
                // User Lost -> User lost their bet.
            }
            else if(winType == 1)
            {
                // User won with 3x fruits (including Bonus)-> Give the user 25X their bet.
                FruitManagerScript.currency += FruitManagerScript.betAmount * 25;
            }
            else if(winType == 2)
            {
                // User won with special star -> Give the user 1000X their bet.
                FruitManagerScript.currency += FruitManagerScript.betAmount * 1000;
            }
            else if(winType == 3)
            {
                // User won with Lucky 7s and/or Bonus -> Give the user 100X their bet.
                FruitManagerScript.currency += FruitManagerScript.betAmount * 100;
            }
            else if(winType == 4)
            {
                // Free Spin -> Give the user back their bet.
                FruitManagerScript.currency += FruitManagerScript.betAmount;
            }
        }

        if(theEndModel != null)
        {
            theEndModel.transform.localScale = Vector3.SmoothDamp(theEndModel.transform.localScale, new Vector3(100f, 100f, 100f), ref scaleRef, 0.1f);
        }
    }

    // Resets the counts of all variables. (Is called in FruitManager.cs when the Start button is pressed.)
    public void ResetCounts()
    {
        cherryCount = 0;
        appleCount = 0;
        bananaCount = 0;
        lemonCount = 0;
        lucky7Count = 0;
        win = false;
        winType = 0;
        resultMsg = "";
        Destroy(theEndModel);
    }
}