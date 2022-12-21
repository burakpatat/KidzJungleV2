using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DimensionSc : MonoBehaviour,IPointerClickHandler
{
    public GameObject dimensionGame;
    public GameObject gameButtons;
    public GameObject dimensionController;
    public GameObject backButton;
    public GameObject menuTabela;
    public GameObject background;
    public Sprite gameBg;
    public GameObject colorMask;
    public GameObject dimensionMask;
    public GameObject puzzleMask;
    public void OnPointerClick(PointerEventData eventData)
    {
        DimensionDrag.timer = 0f;
        MeyveSepeti_Sounds.aManager.ButtonClickSound();
        colorMask.SetActive(false);
        dimensionMask.SetActive(false);
        puzzleMask.SetActive(false);
        menuTabela.SetActive(false);
        background.GetComponent<Image>().sprite = gameBg;
        dimensionGame.SetActive(true);
        gameButtons.SetActive(false);
        dimensionController.GetComponent<DimensionController>().CreateDimensionFruit();
        backButton.SetActive(true);
        DimensionDrag.t = 0;
        DimensionDrag.tt = 0;
    }
}
