using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject puzzleGame;
    public GameObject gameButtons;
    public GameObject puzzleController;
    public GameObject backButton;
    public GameObject menuTabela;
    public GameObject background;
    public Sprite gameBg;
    public GameObject colorMask;
    public GameObject dimensionMask;
    public GameObject puzzleMask;
    public void OnPointerClick(PointerEventData eventData)
    {
        PuzzleDrag.timer = 0f;
        MeyveSepeti_Sounds.aManager.ButtonClickSound();
        menuTabela.SetActive(false);
        colorMask.SetActive(false);
        dimensionMask.SetActive(false);
        puzzleMask.SetActive(false);
        puzzleGame.SetActive(true);
        gameButtons.SetActive(false);
        puzzleController.GetComponent<PuzzleController>().CreatePuzzleFruit();
        backButton.SetActive(true);
        background.GetComponent<Image>().sprite = gameBg;
        PuzzleDrag.tt = 0;
        PuzzleDrag.t = 0;
    }
}
