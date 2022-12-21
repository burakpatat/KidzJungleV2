using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorDrag : MonoBehaviour
{
    Touch touch;
    Vector2 vec;
    public GameObject selectedObject;
    float gecenSure;
    float gittigiSure = 0.5f;
    public int totalSussecfulObject = 0;
    int totalObject=3;
    public int total=0;
    public GameObject gameButtons;
    public GameObject colorGame;
    public GameObject backButton;
    public static bool isWrongBucket;
    public static float timer;
    
    IEnumerator ParcaKucul(GameObject x)
    {
        Vector3 vec = new Vector3(0.60f, 0.60f, 0.60f);

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
    public static int t=0;
    public static int tt = 0;

    IEnumerator ParcaYukari(GameObject x)
    {
        Vector2 vec = x.GetComponent<ColorDrop>().bucketMask.transform.position + new Vector3(0f,1.15f,0f);

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

        Vector2 vec2 = x.GetComponent<ColorDrop>().bucketMask.transform.position + new Vector3(0f,-0.3f,0f);
        vec2 = vec2 +  new Vector2( 0.65f * tt - 0.8f ,0f);
        t++;
        if(t == 3)
        {
            tt++;
            t = 0;
        }
        while (gecenSure < 0.2f)
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
        Vector2 vec = x.GetComponent<ColorDrop>().startPos;

        while (gecenSure < gittigiSure)
        {
            x.transform.position = Vector3.Lerp(x.transform.position, vec, gecenSure / gittigiSure);
            gecenSure += Time.deltaTime;
            yield return null;
        }
        x.transform.position = x.GetComponent<ColorDrop>().startPos;
        yield return null;
        gecenSure = 0f;
        x.GetComponent<BoxCollider2D>().enabled = true;
        x.GetComponent<SpriteRenderer>().sortingOrder = 12;
    }
    IEnumerator ParcaGeriDon2(GameObject x)
    {
        x.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(0.3f);
        Vector2 vec = x.GetComponent<ColorDrop>().startPos;

        while (gecenSure < gittigiSure)
        {
            x.transform.position = Vector3.Lerp(x.transform.position, vec, gecenSure / gittigiSure);
            gecenSure += Time.deltaTime;
            yield return null;
        }
        x.transform.position = x.GetComponent<ColorDrop>().startPos;
        yield return null;
        gecenSure = 0f;
        x.GetComponent<BoxCollider2D>().enabled = true;
        x.GetComponent<SpriteRenderer>().sortingOrder = 12;
    }
    IEnumerator FinishGame()
    {

       


        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene("MeyveSepeti_BonusScene");
        
        gameObject.GetComponent<ColorController>().FinishOrPressBackButton();
        //colorGame.SetActive(false);
        //backButton.SetActive(false);
        //gameButtons.SetActive(true);
        
        
    }

    public void TruePos()
    {
        MeyveSepeti_Sounds.aManager.TrueSound();
        totalSussecfulObject++;
        if(totalSussecfulObject == totalObject)
        {
            total++;
            if (total == 4)
            {
                total = 0;
                Debug.Log("Bitti");
                StartCoroutine(FinishGame());
                
            }
            else
            {
                gameObject.GetComponent<ColorController>().CreateColorFruit();
                totalSussecfulObject = 0;
            }
        }
    }
    private void Start()
    {
        isWrongBucket = false;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            vec = Camera.main.ScreenToWorldPoint(touch.position);

            RaycastHit2D ray2d = Physics2D.Raycast(vec, Vector2.zero);

            if (ray2d.collider != null && selectedObject == null && !ray2d.collider.name.Equals("GreenBucket") && !ray2d.collider.name.Equals("YellowBucket")&& !ray2d.collider.name.Equals("RedBucket"))
            {
                selectedObject = ray2d.transform.gameObject;
            }
            
           else if(selectedObject != null)
            {
                selectedObject.transform.position = new Vector2(vec.x, vec.y);
                selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 13;

                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (selectedObject.GetComponent<ColorDrop>().inRightPosition)
                    {
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
                            selectedObject.GetComponent<ColorDrop>().anim.SetTrigger("wrongBucket");
                            StartCoroutine(ParcaGeriDon2(selectedObject.gameObject));
                        }
                        else
                        {
                            //MeyveSepeti_Sounds.aManager.FalseSound();
                            StartCoroutine(ParcaGeriDon(selectedObject));
                        }
                    }
                    isWrongBucket = false;
                    //selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 12;
                    selectedObject = null;
                }
           }
        }
    }
}
