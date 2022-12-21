using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DimensionDrag : MonoBehaviour
{
    public static DimensionDrag Instance;
    Touch touch;
    Vector3 vec;
    public GameObject selectedObject;
    float gecenSure;
    float gittigiSure = 0.5f;
    public int totalSussecfulObject = 0;
    int totalObject = 3;
    public int total = 0;
    public GameObject gameButtons;
    public GameObject dimensionGame;
    public GameObject backButton;
    public static bool isWrongBucket;
    public static float timer;
    IEnumerator FinishGame()
    {
        

        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene("MeyveSepeti_BonusScene");
        gameObject.GetComponent<DimensionController>().FinishOrPressBackButton();
        //backButton.SetActive(false);
        //dimensionGame.SetActive(false);
        //gameButtons.SetActive(true);
    }
    private void Start()
    {
        isWrongBucket = false;
    }
    public void TruePos()
    {
        MeyveSepeti_Sounds.aManager.TrueSound();
        totalSussecfulObject++;

        if (totalSussecfulObject == totalObject)
        {
            total++;

            //foreach (GameObject oldFruits in DimensionController.oldFruits)
            //{
            //    oldFruits.gameObject.SetActive(false);
            //}

            if (total == 12)
            {
                Debug.Log("Bitti");
                
                StartCoroutine(FinishGame());
                total = 0;
                
            }
            else
            {
                gameObject.GetComponent<DimensionController>().CreateDimensionFruit();
                totalSussecfulObject = 0;
               
            }
        }
    }
    public static int t = 0;
    public static int tt = 0;
    IEnumerator ParcaYukari(GameObject x)
    {
        Vector2 vec = x.GetComponent<DimensionDrop>().bucketMask.transform.position + new Vector3(0f, 1.15f, 0f);

        while (gecenSure < gittigiSure)
        {
            x.transform.position = Vector3.Lerp(x.transform.position, vec, gecenSure / gittigiSure);
            gecenSure += Time.deltaTime;
            yield return null;
        }
        x.transform.position = vec;
        yield return null;
        gecenSure = 0f;
        x.GetComponent<SpriteRenderer>().sortingOrder = 9;

        Vector2 vec2 = x.GetComponent<DimensionDrop>().bucketMask.transform.position + new Vector3(0f,-0.2f,0f);
        vec2 = vec2 + new Vector2(0.4f *tt-0.4f ,0f);
        t++;
        if (t == 3)
        {
            tt++;
            t = 0;
            if(tt == 3)
            {
                tt = 0;
            }
        }


        while (gecenSure < gittigiSure)
        {
            x.transform.position = Vector3.Lerp(x.transform.position, vec2, gecenSure / gittigiSure);
            gecenSure += Time.deltaTime;
            yield return null;
        }
        x.transform.position = vec2;
        yield return null;
        gecenSure = 0f;
    }

    IEnumerator ParcaGeriDon(GameObject x)
    {
        x.GetComponent<BoxCollider2D>().enabled = false;
        Vector3 vec = x.GetComponent<DimensionDrop>().startPos;

        while (gecenSure < gittigiSure)
        {
            x.transform.position = Vector3.Lerp(x.transform.position, vec, gecenSure / gittigiSure);
            gecenSure += Time.deltaTime;
            yield return null;
        }
        x.transform.position = x.GetComponent<DimensionDrop>().startPos;
        yield return null;
        gecenSure = 0f;
        x.GetComponent<BoxCollider2D>().enabled = true;
        x.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 12;
    }
    IEnumerator ParcaKucul(GameObject x)
    {
        //Vector3 vec = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 vec = x.GetComponent<DimensionDrop>().fruitSize;

        while (gecenSure < gittigiSure)
        {
            x.transform.localScale = Vector3.Lerp(x.transform.localScale, vec, gecenSure / gittigiSure);
            gecenSure += Time.deltaTime;
            yield return null;
        }
        x.transform.localScale = vec;
        yield return null;
        gecenSure = 0f;
    }
    IEnumerator ParcaGeriDon2(GameObject x)
    {
        x.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(0.3f);
        Vector2 vec = x.GetComponent<DimensionDrop>().startPos;

        while (gecenSure < gittigiSure)
        {
            x.transform.position = Vector3.Lerp(x.transform.position, vec, gecenSure / gittigiSure);
            gecenSure += Time.deltaTime;
            yield return null;
        }
        x.transform.position = x.GetComponent<DimensionDrop>().startPos;
        yield return null;
        gecenSure = 0f;
        x.GetComponent<BoxCollider2D>().enabled = true;
        x.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 12;
    }
    void Update()
    {
        timer += Time.deltaTime;


        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            vec = Camera.main.ScreenToWorldPoint(touch.position);

            RaycastHit2D ray2d = Physics2D.Raycast(vec, Vector2.zero);

            if (ray2d.collider != null && selectedObject == null && !ray2d.collider.name.Equals("LargeBucket") && !ray2d.collider.name.Equals("MediumBucket") && !ray2d.collider.name.Equals("SmallBucket") )
            {
                selectedObject = ray2d.transform.gameObject;
            }
            else if (selectedObject != null)
            {
                selectedObject.transform.position = new Vector2(vec.x, vec.y);
                selectedObject.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 13;

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (selectedObject.GetComponent<DimensionDrop>().inRightPosition)
                    {
                        //   selectedObject.transform.position = selectedObject.GetComponent<DimensionDrop>().bucket.transform.position;
                        //  selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 9;
                        StartCoroutine(ParcaKucul(selectedObject.gameObject));
                        StartCoroutine(ParcaYukari(selectedObject.gameObject));
                        selectedObject.GetComponent<BoxCollider2D>().enabled = false;
                        TruePos();
                    }
                    else
                    {
                        MeyveSepeti_Sounds.aManager.FalseSound();
                        if (isWrongBucket == true)
                        {
                            selectedObject.GetComponent<DimensionDrop>().anim.SetTrigger("wrongBucket");
                            StartCoroutine(ParcaGeriDon2(selectedObject.gameObject));
                        }
                        else
                        {
                            
                            StartCoroutine(ParcaGeriDon(selectedObject));
                        }
                        //selectedObject.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 12;
                    }
                    selectedObject = null;
                }
            }
        }
    }
}
