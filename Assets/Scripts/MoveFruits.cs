using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFruits : MonoBehaviour
{
    /*
        This script doesn't do much, but it is attached to each fruit prefab to have them move automatically when spawned.
    */

    public static FruitManager fruitManagerScript;

    
    // Start is called before the first frame update
    void Start()
    {
        fruitManagerScript = GameObject.Find("/FruitManager").GetComponent<FruitManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Translates the object (moves it) in a direction
        transform.Translate(new Vector3(0f, -1f, 0f) * fruitManagerScript.fruitSpeed * Time.deltaTime);
    }
}
