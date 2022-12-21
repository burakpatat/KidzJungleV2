using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleDrag : MonoBehaviour
{
    Scene scene;
    Touch touch;
    Vector3 vec;
    public GameObject selectedObject;
    float gecenSure;
    float gittigiSure = 0.5f;
    public int totalSussecfulObject = 0;
    int totalObject = 3;
    public int total = 0;
    public GameObject gameButtons;
    public GameObject puzzleGame;
    public GameObject backButton;
    public static bool isWrongBucket;
    public List<GameObject> KutuyaGirecekNesneler = new List<GameObject>();
    float scaleSpeed = 1f;
    public float scaleVelocity = 0.1f;
    public static float timer;
    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(1.5f);
    }
    IEnumerator ParcaKucul(GameObject x)
    {
        Vector3 vec = new Vector3(0.63f, 0.63f, 0.63f);

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
    public static int t = 0;
    public static int tt = 0;
    IEnumerator ParcaYukari(GameObject x)
    {
        Vector2 vec = x.GetComponent<PuzzleDrop>().shadowFruit.transform.position + new Vector3(0f, 3.15f, 0f);

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

        Vector2 vec2 = new Vector3(x.transform.position.x,-1.70f,x.transform.position.z);
        vec2 = vec2 + new Vector2(0.60f * tt - 0.8f, 0f);
        t++;
        if (t == 3)
        {
            tt++;
            t = 0;
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

    IEnumerator FinishGame()
    {
        yield return new WaitForSecondsRealtime(3f);
        gameObject.GetComponent<PuzzleController>().FinishOrPressBackButton();
        puzzleGame.SetActive(false);
        backButton.SetActive(false);
        gameButtons.SetActive(true);
    }

    private void Start()
    {
        isWrongBucket = false;
    }
    IEnumerator CreatePuzzleFruits()
    {
        yield return new WaitForSecondsRealtime(1f);
        gameObject.GetComponent<PuzzleController>().CreatePuzzleFruit();

    }

    IEnumerator objeKutuya()
    {
        for (int i = 0; i < KutuyaGirecekNesneler.Count; i++)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            StartCoroutine(ParcaKucul(KutuyaGirecekNesneler[i].gameObject));
            StartCoroutine(ParcaYukari(KutuyaGirecekNesneler[i].gameObject));
        }
        KutuyaGirecekNesneler.Clear();
    }

    IEnumerator fruitSceneCross()
    {
       

        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene("MeyveSepeti_BonusScene");
        
    }
    public void TruePos()
    {
        MeyveSepeti_Sounds.aManager.TrueSound();
        totalSussecfulObject++;

        if (totalSussecfulObject == totalObject)
        {
            total++;

            //foreach (GameObject oldFruits in PuzzleController.oldPuzzleFruits)
            //{
            //    oldFruits.gameObject.SetActive(false);
            //}
            

            if (total == 4)
            {
                
                Debug.Log("Bitti");
                StartCoroutine(fruitSceneCross());
                StartCoroutine(FinishGame());
                total = 0;
                
            }
            else
            {
                StartCoroutine(Wait());
                StartCoroutine(CreatePuzzleFruits());
                totalSussecfulObject = 0;
               
            }
            StartCoroutine(objeKutuya());
            
            StartCoroutine(Wait());
          
            
        }
    }
    IEnumerator ParcaGeriDon(GameObject x)
    {
        x.GetComponent<BoxCollider2D>().enabled = false;
        Vector3 vec = x.GetComponent<PuzzleDrop>().startPos;

        while (gecenSure < gittigiSure)
        {
            x.transform.position = Vector3.Lerp(x.transform.position, vec, gecenSure / gittigiSure);
            gecenSure += Time.deltaTime;
           yield return null;
        }
         x.transform.position = x.GetComponent<PuzzleDrop>().startPos;
        yield return null;
        gecenSure = 0f;
        x.GetComponent<BoxCollider2D>().enabled = true;
        x.GetComponent<SpriteRenderer>().sortingOrder = 12;
    }

    IEnumerator ParcaGeriDon2(GameObject x)
    {
        x.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(0.3f);
        Vector2 vec = x.GetComponent<PuzzleDrop>().startPos;

        while (gecenSure < gittigiSure)
        {
            x.transform.position = Vector3.Lerp(x.transform.position, vec, gecenSure / gittigiSure);
            gecenSure += Time.deltaTime;
            yield return null;
        }
        x.transform.position = x.GetComponent<PuzzleDrop>().startPos;
        yield return null;
        gecenSure = 0f;
        x.GetComponent<BoxCollider2D>().enabled = true;
        x.GetComponent<SpriteRenderer>().sortingOrder = 12;
    }

    private void Update()
    {
        timer += Time.deltaTime;


        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            vec = Camera.main.ScreenToWorldPoint(touch.position);

            RaycastHit2D ray2d = Physics2D.Raycast(vec, Vector2.zero);

            if (ray2d.collider != null && selectedObject == null && !ray2d.collider.name.Equals("Kova1") && !ray2d.collider.name.Equals("Kova2") && !ray2d.collider.name.Equals("Kova3"))
            {
                selectedObject = ray2d.transform.gameObject;
            }
            else if (selectedObject != null)
            {
                //if (Vector3.Distance(new Vector3(0f, selectedObject.transform.position.y, 0f), new Vector3(0f, selectedObject.GetComponent<PuzzleDrop>().shadowFruit.transform.position.y, 0f)) < 8f)
                //{
                //    if (scaleSpeed! < 1f && touch.deltaPosition.y < 0f && scaleSpeed > 0.73f)
                //    {
                //        scaleSpeed -= scaleVelocity * Time.deltaTime;
                //    }
                //    else
                //    {
                //        scaleSpeed = Mathf.Abs(Mathf.Clamp(scaleSpeed, scaleSpeed, 0.73f));
                //    }

                //    Vector3 scaleVec = new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
                //    selectedObject.transform.localScale = scaleVec;
                //}
                //else
                //{

                //    if (scaleSpeed <1f && touch.deltaPosition.y > 0f && scaleSpeed > 0.73f)
                //    {
                //        scaleSpeed += scaleVelocity * Time.deltaTime;
                //    }
                //    else
                //    {
                //        scaleSpeed = Mathf.Abs(Mathf.Clamp(scaleSpeed, scaleSpeed, 1f));
                //    }
                //    Vector3 scaleVec = new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
                //    selectedObject.transform.localScale = scaleVec;
                //}

                if (touch.deltaPosition.y > 0f)
                {
                    if (scaleSpeed < 1f)
                        scaleSpeed += scaleVelocity * Time.deltaTime;
                    else
                        scaleSpeed = Mathf.Abs(Mathf.Clamp(scaleSpeed, scaleSpeed, 1f));
                    Debug.Log("yUKARIIIIIIIII");
                }
                else if (touch.deltaPosition.y < 0f && selectedObject.transform.position.y > selectedObject.GetComponent<PuzzleDrop>().shadowFruit.transform.position.y)
                {

                    Debug.Log("A?a??");
                    if (scaleSpeed > 0.73f && scaleSpeed < 1f)
                        scaleSpeed -= scaleVelocity * Time.deltaTime;
                    else
                        scaleSpeed = Mathf.Abs(Mathf.Clamp(scaleSpeed, scaleSpeed, 0.73f));
                }

                if (Vector3.Distance(new Vector3(0f, selectedObject.transform.position.y, 0f), new Vector3(0f, selectedObject.GetComponent<PuzzleDrop>().shadowFruit.transform.position.y, 0f)) < 0.5f)
                {
                    scaleSpeed = 0.73f;
                }
                else if (Vector3.Distance(new Vector3(0f, selectedObject.GetComponent<PuzzleDrop>().startPos.y, 0f), new Vector3(0f, selectedObject.transform.position.y, 0f)) < 0.1f)
                {
                    scaleSpeed = 1f;
                }


                Vector3 scaleVec = new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
                selectedObject.transform.localScale = scaleVec;


                selectedObject.transform.position = new Vector2(vec.x, vec.y);
                selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 13;

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (selectedObject.GetComponent<PuzzleDrop>().inRightPosition)
                    {
                        //selectedObject.transform.position = selectedObject.GetComponent<PuzzleDrop>().shadowFruit.transform.position;
                        //selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 9;
                        //   StartCoroutine(ParcaKucul(selectedObject.gameObject));
                        //   StartCoroutine(ParcaYukari(selectedObject.gameObject));
                        selectedObject.transform.position = selectedObject.GetComponent<PuzzleDrop>().shadowFruit.transform.position;

                        selectedObject.transform.localScale = new Vector3(0.73f,0.73f,0.73f);

                        selectedObject.GetComponent<PuzzleDrop>().shadowFruit.SetActive(false);
                        selectedObject.GetComponent<BoxCollider2D>().enabled = false;
                        KutuyaGirecekNesneler.Add(selectedObject);
                        selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 12;
                        TruePos();
                    }
                    else
                    {
                        MeyveSepeti_Sounds.aManager.FalseSound();

                        if (isWrongBucket == true)
                        {
                            selectedObject.GetComponent<PuzzleDrop>().anim.SetTrigger("wrongBucket");
                            StartCoroutine(ParcaGeriDon2(selectedObject.gameObject));
                        }
                        else
                        {
                            
                            StartCoroutine(ParcaGeriDon(selectedObject));
                        }
                       // selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 12;
                        selectedObject.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    selectedObject = null;
                }
            }
            
              
            
            
        }
    }
}
