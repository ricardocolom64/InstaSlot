using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitManager : MonoBehaviour
{
    /*
        This script controls which fruits are able to spawn and the speed at which they move. When a fruit goes too "off screen" it despawns and
        a new fruit spawns back at the top of each slot.
    */

    enum GameStates
    {
        Idle,
        StartPressed,
        Rolling,
        StopPressed,
    }

    GameStates currentGameState;

    public Text debugText;

    public Text currencyText;
    public Text betText;

    public GameObject[] fruitPrefabs;

    public GameObject[] spawnedPrefabsSlot1;
    public GameObject[] spawnedPrefabsSlot2;
    public GameObject[] spawnedPrefabsSlot3;

    public Finish_Bar finishBar;

    // The max speed at which the fruits should travel.
    public float maxFruitSpeed = 15f;

    public float fruitSpeed = 0f;
    private float speedRef = 0f;

    private Vector3 scaleRef = Vector3.zero;

    // Sets the starting Y position of the fruits to be twice the length of the amount of fruits in a slot.
    public float startYPos;

    // The vertical distance between fruits in a slot
    public float distBetweenFruits = 2f;

    // Initialized to zero but modified in the PickRandomFruitIndex() method.
    int fruitIndex = 0;

    public bool lookAtFruits = false;

    public int currency;

    public int betAmount;

    //public InputField inputField;

    // lever animation gameObject
    public GameObject lever;

    // sound gameObject
    public PlayScript startSound;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the starting Y position of the fruits to be twice the length of the amount of fruits in a slot.
        startYPos = spawnedPrefabsSlot1.Length * 2;
        
        ClearAndSpawnFruits();

        currentGameState = GameStates.Idle;
        
        currency = 1000;

        betAmount = 50;
    }

    void ClearAndSpawnFruits()
    {
        foreach (GameObject fruit in spawnedPrefabsSlot1)
        {
            Destroy(fruit);
        }

        foreach (GameObject fruit in spawnedPrefabsSlot2)
        {
            //fruit.transform.localScale = Vector3.SmoothDamp(fruit.transform.localScale, Vector3.zero, ref scaleRef, 0.5f);
            //if(fruit.transform.localScale.magnitude <= 0.1f)
                Destroy(fruit);
        }

        foreach (GameObject fruit in spawnedPrefabsSlot3)
        {
            //fruit.transform.localScale = Vector3.SmoothDamp(fruit.transform.localScale, Vector3.zero, ref scaleRef, 0.5f);
            //if(fruit.transform.localScale.magnitude <= 0.1f)
                Destroy(fruit);
        }

        // Places 6 fruits of random order in the first slot
        for(int i = 0; i < spawnedPrefabsSlot1.Length; ++i)
        {
            PickRandomFruitIndex();
            spawnedPrefabsSlot1[i] = Instantiate(fruitPrefabs[fruitIndex], new Vector3(-4f, startYPos - i*distBetweenFruits, 0f), fruitPrefabs[fruitIndex].transform.rotation);
        }

        // Places 6 fruits of random order in the second slot
        for(int i = 0; i < spawnedPrefabsSlot2.Length; ++i)
        {
            PickRandomFruitIndex();
            spawnedPrefabsSlot2[i] = Instantiate(fruitPrefabs[fruitIndex], new Vector3(0f, startYPos - i*distBetweenFruits, 0f), fruitPrefabs[fruitIndex].transform.rotation);
        }

        // Places 6 fruits of random order in the third slot
        for(int i = 0; i < spawnedPrefabsSlot3.Length; ++i)
        {
            PickRandomFruitIndex();
            spawnedPrefabsSlot3[i] = Instantiate(fruitPrefabs[fruitIndex], new Vector3(4f, startYPos - i*distBetweenFruits, 0f), fruitPrefabs[fruitIndex].transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = "Current GameState = " + currentGameState.ToString();

        currencyText.text = "Currency = " + currency.ToString();
        betText.text = "Current Bet = " + betAmount.ToString();

        if(currentGameState == GameStates.StartPressed)
        {
            foreach(GameObject fruit in spawnedPrefabsSlot1)
            {
                fruit.transform.localScale = Vector3.SmoothDamp(fruit.transform.localScale, Vector3.zero, ref scaleRef, 0.1f);
                if(fruit.transform.localScale.magnitude <= 0.1f)
                    fruit.transform.localScale = Vector3.zero;
            }
            foreach(GameObject fruit in spawnedPrefabsSlot2)
            {
                fruit.transform.localScale = Vector3.SmoothDamp(fruit.transform.localScale, Vector3.zero, ref scaleRef, 0.1f);
                if(fruit.transform.localScale.magnitude <= 0.1f)
                    fruit.transform.localScale = Vector3.zero;
            }
            foreach(GameObject fruit in spawnedPrefabsSlot3)
            {
                fruit.transform.localScale = Vector3.SmoothDamp(fruit.transform.localScale, Vector3.zero, ref scaleRef, 0.1f);
                if(fruit.transform.localScale.magnitude <= 0.1f)
                    fruit.transform.localScale = Vector3.zero;
            }
        }

        if(currentGameState == GameStates.Rolling)
        {
            fruitSpeed = Mathf.SmoothDamp(fruitSpeed, maxFruitSpeed, ref speedRef, 0.25f);
        }
        
        if(currentGameState == GameStates.StopPressed)
        {
            fruitSpeed = Mathf.SmoothDamp(fruitSpeed, 0f, ref speedRef, 0.5f);
            if(fruitSpeed <= 0.01f)
                fruitSpeed = 0f;
        }

        // Spawns and destroys the fruits in each slot
        SpawnAndDestroyFruitsSlot1();
        SpawnAndDestroyFruitsSlot2();
        SpawnAndDestroyFruitsSlot3();
    }


    // This method simply picks a random integer from 0 to the amount of fruits
    //------- TODO: Change odds of specific fruits, for example, a "lucky 7" should be much rarer than an apple ----------
    void PickRandomFruitIndex()
    {
        // fruitIndex = Random.Range(0, fruitPrefabs.Length);
        int selection = Random.Range(0, 1000);

        // 1/1000 chance of getting star. 1 star is an instant win. -> 1000X multiplier
        if(selection <= 0)
        {
            fruitIndex = 4;
        }
        // 7% chance of getting a Lucky7. 3 7s is a win. (+13% chance of Bonus below) (~8/1000 chance of win) -> 100X multiplier
        else if(selection <= 70)
        {
            fruitIndex = 6;
        }
        // 129/1000 (~13%) of getting a Bonus. (Bonus counts as anything except a star) 3 Bonus counts as a win (~2/1000) -> 100X multiplier
        else if(selection < 200)
        {
            fruitIndex = 5;
        }
        // (80% remaining: 4 fruits = 20% each) (+13% chance of Bonus below) (3/100 chance of win) -> 25X multiplier
        else
        {
            fruitIndex = Random.Range(0, 4);
        }
    }

    public void StartPressed()
    {
        if(currentGameState == GameStates.Idle)
        {         
            // Play Lever animation after the start button is pressed.
            lever.GetComponent<Animator>().Play("Lever_Pull");
            // Play the start sound after the start button is pressed.
            startSound.playSound();

            currentGameState = GameStates.StartPressed;
            finishBar.ResetCounts();

            //int.TryParse(inputField.text, out userInput);

            // Runs a function called StartRolling that has the purpose of setting a short delay before the currentGameState is changed to Rolling
            StartCoroutine(StartRolling());

            // Deduct the bet amount from currency
            currency -= betAmount;
        }
        
    }

    public void StopPressed()
    {
        if(fruitSpeed >= maxFruitSpeed - 0.1f && currentGameState == GameStates.Rolling)
        {
            currentGameState = GameStates.StopPressed;

            // Sets a short delay before setting the currentGameState to idle, and therefore before being able to start the game again
            StartCoroutine(delayToIdle(false));
        }
    }

    // Sets a short delay before the currentGameState is changed to Rolling
    IEnumerator StartRolling()
    {
        yield return new WaitForSeconds(1);
        ClearAndSpawnFruits();
        currentGameState = GameStates.Rolling;
    }

    IEnumerator delayToIdle(bool didWin)
    {
        if(didWin)
            yield return new WaitForSeconds(4);
        else
            yield return new WaitForSeconds(3);
        currentGameState = GameStates.Idle;
        lookAtFruits = true;
    }

    public void Subtract10ButtonPressed()
    {
        if(betAmount != 10)
            betAmount -= 10;
    }

    public void Add10ButtonPressed()
    {
        if(betAmount != currency)
            betAmount += 10;
    }


    /* -------------------------------------------
    
    These 3 methods below all control how the last fruit is destroyed once it reaches a certain y position in each slot, and then a new one is spawned at the top of each slot.

    ---------------------------------------------- */

    #region

    void SpawnAndDestroyFruitsSlot1()
    {
        // If the last fruit in the column exists
        if(spawnedPrefabsSlot1[5] != null)
        {
            PickRandomFruitIndex();
            // If the last fruit has a y value that is less than 0
            if(spawnedPrefabsSlot1[5].transform.position.y <= 0f)
            {
                //Destroys last fruit in column
                Destroy(spawnedPrefabsSlot1[5]);

                // Shifts every remaining fruit before it upwards by 1 in the spawnedPrefabsSlot1 array
                for(int i = spawnedPrefabsSlot1.Length - 2; i >= 0; --i)
                {
                    spawnedPrefabsSlot1[i+1] = spawnedPrefabsSlot1[i];
                }

                // Create a new prefab at the top and at the 0th position.
                spawnedPrefabsSlot1[0] = Instantiate(fruitPrefabs[fruitIndex], new Vector3(-4f, 12f, 0f), fruitPrefabs[fruitIndex].transform.rotation);
            }
        }
    }

    void SpawnAndDestroyFruitsSlot2()
    {
        // If the last fruit in the column exists
        if(spawnedPrefabsSlot2[5] != null)
        {
            PickRandomFruitIndex();
            // If the last fruit has a y value that is less than 0
            if(spawnedPrefabsSlot2[5].transform.position.y <= 0f)
            {
                //Destroys last fruit in column
                Destroy(spawnedPrefabsSlot2[5]);

                // Shifts every remaining fruit before it upwards by 1 in the spawnedPrefabsSlot2 array
                for(int i = spawnedPrefabsSlot2.Length - 2; i >= 0; --i)
                {
                    spawnedPrefabsSlot2[i+1] = spawnedPrefabsSlot2[i];
                }

                // Create a new prefab at the top and at the 0th position.
                spawnedPrefabsSlot2[0] = Instantiate(fruitPrefabs[fruitIndex], new Vector3(0f, 12f, 0f), fruitPrefabs[fruitIndex].transform.rotation);
            }
        }
    }

    void SpawnAndDestroyFruitsSlot3()
    {
        // If the last fruit in the column exists
        if(spawnedPrefabsSlot3[5] != null)
        {
            PickRandomFruitIndex();
            // If the last fruit has a y value that is less than 0
            if(spawnedPrefabsSlot3[5].transform.position.y <= 0f)
            {
                //Destroys last fruit in column
                Destroy(spawnedPrefabsSlot3[5]);

                // Shifts every remaining fruit before it upwards by 1 in the spawnedPrefabsSlot3 array
                for(int i = spawnedPrefabsSlot3.Length - 2; i >= 0; --i)
                {
                    spawnedPrefabsSlot3[i+1] = spawnedPrefabsSlot3[i];
                }

                // Create a new prefab at the top and at the 0th position.
                spawnedPrefabsSlot3[0] = Instantiate(fruitPrefabs[fruitIndex], new Vector3(4f, 12f, 0f), fruitPrefabs[fruitIndex].transform.rotation);
            }
        }
    }

    #endregion
}
