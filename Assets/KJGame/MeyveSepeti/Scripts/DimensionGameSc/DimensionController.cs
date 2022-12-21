using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionController : MonoBehaviour
{
    public List<GameObject> large;
    public List<GameObject> medium;
    public List<GameObject> small;

    public List<GameObject> largeFruits;
    public List<GameObject> mediumFruits;
    public List<GameObject> smallFruits;
    public List<GameObject> dimensionFruitDots;
    public static List<GameObject> fruits = new List<GameObject>();
    public static List<GameObject> oldFruits = new List<GameObject>();

    int firstMember;
    int x;

    IEnumerator FruitStartAnim(GameObject first, GameObject second, GameObject third)
    {
        yield return new WaitForSeconds(0.2f);
        first.SetActive(true);
        first.transform.localScale = first.GetComponent<DimensionDrop>().startLocalScale; 
        yield return new WaitForSeconds(0.2f);
        second.SetActive(true);
        second.transform.localScale = second.GetComponent<DimensionDrop>().startLocalScale;
        yield return new WaitForSeconds(0.2f);
        third.SetActive(true);
        third.transform.localScale = third.GetComponent<DimensionDrop>().startLocalScale;
    }

    public void FinishOrPressBackButton()
    {
        fruits.Clear();
        oldFruits.Clear();
        gameObject.GetComponent<DimensionDrag>().totalSussecfulObject = 0;
        gameObject.GetComponent<DimensionDrag>().total = 0;

        largeFruits.Clear();

        foreach(GameObject largeFruit in large)
        {
            largeFruits.Add(largeFruit);
            largeFruit.SetActive(false);
        }

        mediumFruits.Clear();

        foreach (GameObject mediumFruit in medium)
        {
            mediumFruits.Add(mediumFruit);
            mediumFruit.SetActive(false);
        }

        smallFruits.Clear();

        foreach (GameObject smallFruit in small)
        {
            smallFruits.Add(smallFruit);
            smallFruit.SetActive(false);
        }


    }

    public void CreateDimensionFruit()
    {
        oldFruits.Clear();
        fruits.Clear();

        firstMember = Random.Range(0, largeFruits.Count);

        fruits.Add(largeFruits[firstMember].gameObject);
        fruits.Add(mediumFruits[firstMember].gameObject);
        fruits.Add(smallFruits[firstMember].gameObject);

        oldFruits.Add(largeFruits[firstMember].gameObject);
        oldFruits.Add(mediumFruits[firstMember].gameObject);
        oldFruits.Add(smallFruits[firstMember].gameObject);

        foreach(GameObject fruit in fruits)
        {
            fruit.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            fruit.gameObject.GetComponent<DimensionDrop>().inRightPosition = false;
            fruit.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 12;
            //fruit.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        for (int i=0;i<dimensionFruitDots.Count;i++)
        {
            x = Random.Range(0,fruits.Count);
            fruits[x].transform.position = dimensionFruitDots[i].transform.position;
            fruits[x].GetComponent<DimensionDrop>().startPos = dimensionFruitDots[i].transform.position;
            //fruits[x].SetActive(true);
            fruits.RemoveAt(x);
        }

        largeFruits.RemoveAt(firstMember);
        mediumFruits.RemoveAt(firstMember);
        smallFruits.RemoveAt(firstMember);

        StartCoroutine(FruitStartAnim(oldFruits[0].gameObject,oldFruits[1].gameObject,oldFruits[2].gameObject));

    }   
}
