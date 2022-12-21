using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDrop : MonoBehaviour
{
    public GameObject shadowFruit;
    public bool inRightPosition=false;
    public Vector2 startPos;
    public Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PuzzleDrag.isWrongBucket = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PuzzleDrag.isWrongBucket = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PuzzleDrag.isWrongBucket = false;
    }


    void Update()
    {
        if (Vector2.Distance(transform.position, shadowFruit.transform.position) < 1.2f)
        {
            inRightPosition = true;
        }
        else
        {
            inRightPosition = false;
        }
    }
}
