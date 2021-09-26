using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    private string[] allSkins = {"Default", "ShellHacks", "Christmas", "Halloween"};
    public GameObject[] cabinetSkins;
    public Material[] skyBoxSkins;
    public GameObject[] starSkins;
    public AudioSource[] sfxSkins;

    private GameObject[] toDestroy;

    public FruitManager fruitManager;
    public PlayScript audioScript;

    public int counter = 0;

    public static string currentSkin = "Default";
    public Text SkinText;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        currentSkin = "Default";
    }

    // Update is called once per frame
    void Update()
    {
        SkinText.text = currentSkin.ToString();
    }

    public void ChangeSkinPressed()
    {
        counter++;
        if(counter >= allSkins.Length)
            counter = 0;
        currentSkin = allSkins[counter];

        fruitManager.fruitPrefabs[4] = starSkins[counter];

        if(cabinetSkins[counter] != null)
        {
            toDestroy = GameObject.FindGameObjectsWithTag("Cabinet");
            foreach (GameObject item in toDestroy)
            {
                Destroy(item);
            }
            Instantiate(cabinetSkins[counter]);
        }
        if(skyBoxSkins[counter] != null)
            RenderSettings.skybox = skyBoxSkins[counter];
    }


}
