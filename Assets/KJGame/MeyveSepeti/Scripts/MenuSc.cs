using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSc : MonoBehaviour
{
    public GameObject colorGame;
    public GameObject puzzleGame;
    public GameObject dimensionGame;
    public GameObject gameButtons;
    public GameObject puzzleController;
    public GameObject dimensionController;
    public GameObject colorController;
    float time;
    public GameObject colorGameButtonTouchParticle;
    public GameObject dimensionGameButtonTouchParticle;
    public GameObject puzzleGameButtonTouchParticle;
    public GameObject backButton;
    public GameObject backGround;
    public Sprite menuBg;
    public GameObject menuTabela;
    public GameObject colorMask;
    public GameObject dimensionMask;
    public GameObject puzzleMask;
    private void Start()
    {
        MeyveSepeti_Sounds.aManager.FruitSceneSound(MeyveSepeti_Sounds.aManager.sounds[5]);
    }
    private void Update()
    {
        if (Input.touchCount <= 0)
        {
            time += Time.deltaTime;

            if (time > 10f)
            {
                colorGameButtonTouchParticle.SetActive(true);
                dimensionGameButtonTouchParticle.SetActive(true);
                puzzleGameButtonTouchParticle.SetActive(true);
            }
        }
        else
        {
            colorGameButtonTouchParticle.SetActive(false);
            dimensionGameButtonTouchParticle.SetActive(false);
            puzzleGameButtonTouchParticle.SetActive(false);
            time = 0f;
        } 
    }
    public void BackButtonClick()
    {
        MeyveSepeti_Sounds.aManager.ButtonClickSound();
        menuTabela.SetActive(true);
        colorMask.SetActive(true);
        dimensionMask.SetActive(true);
        puzzleMask.SetActive(true);
        backGround.GetComponent<Image>().sprite = menuBg;

        colorGame.SetActive(false);
        puzzleGame.SetActive(false);
        dimensionGame.SetActive(false);
        gameButtons.SetActive(true);
        backButton.SetActive(false);

        colorController.GetComponent<ColorController>().FinishOrPressBackButton();
        dimensionController.GetComponent<DimensionController>().FinishOrPressBackButton();
        puzzleController.GetComponent<PuzzleController>().FinishOrPressBackButton();

    }
}
