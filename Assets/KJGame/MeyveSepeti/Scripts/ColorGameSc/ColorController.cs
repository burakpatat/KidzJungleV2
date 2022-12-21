using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    //Geri butonuna bastýðýmýzda listemden silme iþlemi gerçekleþtirdiðim için asýl meyveler her oyun tekrarlandýðýnda fruitlerine eþitlenecek.
    public List<GameObject> red;
    public List<GameObject> yellow;
    public List<GameObject> green;


    public List<GameObject> redFruits;
    public List<GameObject> yellowFruits;
    public List<GameObject> greenFruits;

    public static List<GameObject> colorFruits = new List<GameObject>();
    public static List<GameObject> oldColorFruits = new List<GameObject>();

    public List<GameObject> colorFruitDots;

    int firstMember, secondMember, thirdMember;
    int x;

    public void FinishOrPressBackButton()
    {
        colorFruits.Clear();
        oldColorFruits.Clear();
        gameObject.GetComponent<ColorDrag>().totalSussecfulObject = 0;
        gameObject.GetComponent<ColorDrag>().total = 0;

        redFruits.Clear();

        foreach (GameObject redFruit in red)
        {
            redFruits.Add(redFruit);
            redFruit.SetActive(false);
        }

        yellowFruits.Clear();

        foreach (GameObject yellowFruit in yellow)
        {
            yellowFruits.Add(yellowFruit);
            yellowFruit.SetActive(false);
        }

        greenFruits.Clear();

        foreach (GameObject greenFruit in green)
        {
            greenFruits.Add(greenFruit);
            greenFruit.SetActive(false);
        }
    }

    IEnumerator FruitStartAnim(GameObject first,GameObject second,GameObject third)
    {
        yield return new WaitForSeconds(0.2f);
        first.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        second.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        third.SetActive(true);
    }


    public void CreateColorFruit()
    {
        oldColorFruits.Clear();
        colorFruits.Clear();

        firstMember = Random.Range(0,redFruits.Count);
        secondMember = Random.Range(0,yellowFruits.Count);
        thirdMember = Random.Range(0,greenFruits.Count);

        colorFruits.Add(redFruits[firstMember].gameObject);
        colorFruits.Add(yellowFruits[secondMember].gameObject);
        colorFruits.Add(greenFruits[thirdMember].gameObject);
        
        oldColorFruits.Add(redFruits[firstMember].gameObject);
        oldColorFruits.Add(yellowFruits[secondMember].gameObject);
        oldColorFruits.Add(greenFruits[thirdMember].gameObject);

        foreach(GameObject fruits in colorFruits)
        {
            fruits.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            fruits.gameObject.GetComponent<ColorDrop>().inRightPosition = false;
            fruits.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 12;
            fruits.gameObject.transform.localScale= new Vector3(1f, 1f, 1f);
        }

        for(int i = 0; i < colorFruitDots.Count; i++)
        {
            x = Random.Range(0,colorFruits.Count);
            colorFruits[x].transform.position = colorFruitDots[i].transform.position;
            colorFruits[x].GetComponent<ColorDrop>().startPos = colorFruitDots[i].transform.position;
            //colorFruits[x].SetActive(true);
            colorFruits.RemoveAt(x);
            
        }

        redFruits.RemoveAt(firstMember);
        yellowFruits.RemoveAt(secondMember);
        greenFruits.RemoveAt(thirdMember);

        StartCoroutine(FruitStartAnim(oldColorFruits[0].gameObject, oldColorFruits[1].gameObject, oldColorFruits[2].gameObject));

        
    }
    
}
