using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorButtonSc : MonoBehaviour, IPointerClickHandler
{
    public GameObject colorGame;
    public GameObject gameButton;
    public GameObject colorController;
    public GameObject backButton;
    public GameObject menuTabela;
    public Sprite gameBg;
    public GameObject background;
    public GameObject colorMask;
    public GameObject dimensionMask;
    public GameObject puzzleMask;
    public void OnPointerClick(PointerEventData eventData)
    {
        ColorDrag.timer = 0f;
        MeyveSepeti_Sounds.aManager.ButtonClickSound();
        colorGame.SetActive(true);
        colorMask.SetActive(false);
        dimensionMask.SetActive(false);
        puzzleMask.SetActive(false);
        ColorDrag.tt = 0;
        ColorDrag.t = 0;
        gameButton.SetActive(false);
        colorController.GetComponent<ColorController>().CreateColorFruit();
        backButton.SetActive(true);
        menuTabela.SetActive(false);
        background.GetComponent<Image>().sprite = gameBg;    
    }
}
