using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMenuSc : MonoBehaviour
{
    public static int fruitCount;
    public GameObject leftSpawner;
    public GameObject midSpawner;
    public GameObject rightSpawner;
    float timer;
    public float finishTime=30f;
    IEnumerator SceneCross()
    {
        MeyveSepeti_Sounds.aManager.ButtonClickSound();
        leftSpawner.SetActive(false);
        midSpawner.SetActive(false);
        rightSpawner.SetActive(false);
        yield return new WaitForSecondsRealtime(3.8f);
        SceneManager.LoadScene("MeyveSepeti_Game");
    }
    private void Start()
    {
        fruitCount = 0;
      //  MeyveSepeti_Sounds.aManager.FruitSceneSound();
    }
    public void TurnMenu()
    {
        SceneManager.LoadScene("MeyveSepeti_Game");
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > finishTime)
        {
            StartCoroutine(SceneCross());
        }
        //if(fruitCount >= 50)
        //{
        //    StartCoroutine(SceneCross());
        //}
    }
}
