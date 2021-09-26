using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayScript : MonoBehaviour
{
    public AudioSource[] sources;

    int count;

    public SkinManager skinManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count = skinManager.counter;
    }

    public void playSound(){

        sources[count].Play();
    }
}
