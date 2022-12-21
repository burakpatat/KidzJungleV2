
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedFruitController : MonoBehaviour
{
    public GameObject[] fruits;
    int fruitIndex;
    float randomForce;
    Vector2 randomDirection;
    public float left;
    public float right;
    public float randomCreateMinTime;
    public float randomCreateMaxTime;
    float createTime;
    IEnumerator SpawnFruit()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            CreateFruit();
            yield return new WaitForSeconds(createTime); 
        }
    }


    private void Start()
    {
        StartCoroutine(SpawnFruit());
    }
    public void CreateFruit()
    {
        createTime = Random.Range(randomCreateMinTime,randomCreateMaxTime);
        fruitIndex = Random.Range(0, fruits.Length);
        randomForce = Random.Range(13f, 17f);
        randomDirection = new Vector2(Random.Range(left,right), 1f).normalized;

        GameObject fruit = Instantiate(fruits[fruitIndex], transform.position, fruits[fruitIndex].transform.rotation);

        fruit.GetComponent<Rigidbody2D>().AddForce(randomDirection * randomForce,ForceMode2D.Impulse);

        // fruit.transform.rotation = Random.rotation;

        BackMenuSc.fruitCount++;
    }
}
