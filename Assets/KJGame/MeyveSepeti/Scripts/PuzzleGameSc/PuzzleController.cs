using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public List<GameObject> red;
    public List<GameObject> yellow;
    public List<GameObject> green;
    public List<GameObject> shadowRed;
    public List<GameObject> shadowYellow;
    public List<GameObject> shadowGreen;


    public List<GameObject> redFruits;
    public List<GameObject> yellowFruits;
    public List<GameObject> greenFruits;

    public static List<GameObject> puzzleFruits = new List<GameObject>();
    public static List<GameObject> shadowfruits = new List<GameObject>();
    public static List<GameObject> oldPuzzleFruits = new List<GameObject>();

    public List<GameObject> fruitDots;
    public List<GameObject> shadowFruitDots;
    public List<GameObject> shadowRedFruits;
    public List<GameObject> shadowYellowFruits;
    public List<GameObject> shadowGreenFruits;
    int firstMember, secondMember, thirdMember;
    int x,y;


    IEnumerator FruitStartAnim(GameObject first, GameObject second, GameObject third)
    {
        yield return new WaitForSeconds(0.2f);
        first.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        second.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        third.SetActive(true);
    }

    public void FinishOrPressBackButton()
    {
        puzzleFruits.Clear();
        shadowfruits.Clear();
        oldPuzzleFruits.Clear();
        gameObject.GetComponent<PuzzleDrag>().totalSussecfulObject = 0;
        gameObject.GetComponent<PuzzleDrag>().total = 0;

        redFruits.Clear();

        foreach(GameObject redFruit in red)
        {
            redFruits.Add(redFruit);
            redFruit.SetActive(false);
            redFruit.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        yellowFruits.Clear();

        foreach (GameObject yellowFruit in yellow)
        {
            yellowFruits.Add(yellowFruit);
            yellowFruit.SetActive(false);
            yellowFruit.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        greenFruits.Clear();

        foreach (GameObject greenFruit in green)
        {
            greenFruits.Add(greenFruit);
            greenFruit.SetActive(false);
            greenFruit.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        shadowRedFruits.Clear();

        foreach (GameObject shadowred in shadowRed)
        {
            shadowRedFruits.Add(shadowred);
            shadowred.SetActive(false);
        }

        shadowYellowFruits.Clear();

        foreach (GameObject shadowyellow in shadowYellow)
        {
            shadowYellowFruits.Add(shadowyellow);
            shadowyellow.SetActive(false);
        }

        shadowGreenFruits.Clear();

        foreach (GameObject shadowgreen in shadowGreen)
        {
            shadowGreenFruits.Add(shadowgreen);
            shadowgreen.SetActive(false);
        }


    }

    public void CreatePuzzleFruit()
    {
        oldPuzzleFruits.Clear();
        puzzleFruits.Clear();

        firstMember = Random.Range(0,redFruits.Count);
        secondMember = Random.Range(0,yellowFruits.Count);
        thirdMember = Random.Range(0,greenFruits.Count);

        puzzleFruits.Add(redFruits[firstMember].gameObject);
        puzzleFruits.Add(yellowFruits[secondMember].gameObject);
        puzzleFruits.Add(greenFruits[thirdMember].gameObject);
        
        oldPuzzleFruits.Add(redFruits[firstMember].gameObject);
        oldPuzzleFruits.Add(yellowFruits[secondMember].gameObject);
        oldPuzzleFruits.Add(greenFruits[thirdMember].gameObject);

        shadowfruits.Add(shadowRedFruits[firstMember].gameObject);
        shadowfruits.Add(shadowYellowFruits[secondMember].gameObject);
        shadowfruits.Add(shadowGreenFruits[thirdMember].gameObject);

        foreach(GameObject fruits in puzzleFruits)
        {
            fruits.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            fruits.gameObject.GetComponent<PuzzleDrop>().inRightPosition = false;
            fruits.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 12;
            fruits.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        for (int i = 0;i<fruitDots.Count ; i++)
        {
            x = Random.Range(0, puzzleFruits.Count);

            puzzleFruits[x].transform.position = fruitDots[i].transform.position;
            puzzleFruits[x].GetComponent<PuzzleDrop>().startPos = fruitDots[i].transform.position;
           // puzzleFruits[x].SetActive(true);
            puzzleFruits.RemoveAt(x);
            
        }
        for(int i=0;i< shadowFruitDots.Count; i++)
        {
            y = Random.Range(0,shadowfruits.Count);

            shadowfruits[y].transform.position = shadowFruitDots[i].transform.position;
            shadowfruits[y].SetActive(true);
            shadowfruits.RemoveAt(y);
        }


        redFruits.RemoveAt(firstMember);
        yellowFruits.RemoveAt(secondMember);
        greenFruits.RemoveAt(thirdMember);

        shadowRedFruits.RemoveAt(firstMember);
        shadowYellowFruits.RemoveAt(secondMember);
        shadowGreenFruits.RemoveAt(thirdMember);


        StartCoroutine(FruitStartAnim(oldPuzzleFruits[0].gameObject,oldPuzzleFruits[1].gameObject,oldPuzzleFruits[2].gameObject));

    }
}
