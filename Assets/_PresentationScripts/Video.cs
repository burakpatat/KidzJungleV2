using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using UnityEngine.UI;
using TMPro;

using RenderHeads.Media.AVProVideo;
using RenderHeads.Media.AVProVideo.Demos;

public enum QuestionTime
{
    InTime,
    OutTime,
    SATime,
    FirstWrongTime,
    SecondWrongTime,
    TrueTime,
}
public class Video : MonoBehaviour
{
    public VCR vCR;
    public TextMeshProUGUI fpsText;
    private float fps;

    public Transform VideoCanvas;
    public GameObject InteractiveButton;
    public Transform VideoSeekSlider;
    public Transform VideoPlayButton;
    public Transform VideoPauseButton;

    public List<Transform> ButtonList;

    List<Video_Data> _datas = new List<Video_Data>();

    bool b_InTime = false;
    bool b_SATime = false;
    bool b_FWTime = false;
    bool b_SWTime = false;
    bool b_TTime = false;
    bool b_OutTime = false;
    float InTime;
    float SATime;
    float FWTime;
    float SWTime;
    float TTime;
    float OutTime;

    public bool NewQuestion = false;
    int _QuestionCount;
    int _QuestionCountIn = 0;

    [SerializeField] float currCountdownValue = 10;
    [SerializeField] int QuestionRowCount;
    [SerializeField] int QuestionWrongState = 0;
    [SerializeField] bool b_QuestionFWrongState = false;
    [SerializeField] bool b_QuestionSWrongState = false;
    [SerializeField] bool b_QuestionTrueState = false;
    bool b_WrongQuestionOutTime = false;

    bool b_IsInteractive = false;
    void Start()
    {
        Invoke("InitData", 1.2f);
        StartCoroutine(Fps());
    }
    void InitData()
    {
        _datas = GetVideo.VideoClass.data;

        foreach (var row in _datas)
        {
            if (row.id == KovukList.Instance.ClickPosterID)
            {
                //openVideo
                vCR._videoFiles.Add(GetVideo.GetMedia() + row.videofile);
                vCR.OnOpenVideoFile();

                //IS INTERACTIVE
                for (int ina = 0; ina < row.isInteractive.Count; ina++)
                {
                    if (row.isInteractive[ina] == "isinteractive")
                    {
                        b_IsInteractive = true;

                        _QuestionCount = (row.questions.Count + 1) - row.questions.Count;
                        GetButtton();
                    }
                }
                
            }
        }
    }
    void GetButtton()
    {
        foreach (var row in _datas)
        {
            if (row.isInteractive != null)
            {
                for ( int i = 0; i < row.questions.Count; i++)
                {
                    for (int a = 0; a < row.questions[i].pos.Count; a++)
                    {
                        if(row.questions[i].pos[a].vid == row.questions[i].id)
                        {
                            var ins = Instantiate(InteractiveButton, Vector3.zero, Quaternion.identity) as GameObject;
                            ins.GetComponent<RectTransform>().SetParent(VideoCanvas);

                            ins.GetComponent<RectTransform>().anchoredPosition =
                                    new Vector3(row.questions[i].pos[a].x.ToFloat(), -row.questions[i].pos[a].y.ToFloat(), 0f);

                            ins.gameObject.name = "IntBut-" + i;
                            ins.GetComponent<InteractiveButtonIn>().ButtonState = row.questions[i].pos[a].isAnswer;

                            ins.GetComponent<RectTransform>().localScale = Vector3.one;

                            ins.gameObject.SetActive(false);

                            ButtonList.Add(ins.gameObject.transform);
                        }
                    }
                }
            }
        } 
    }
    private void Update()
    {
        fpsText.text = "fps: " + Mathf.Round(fps).ToString() + "\n" + "vfr: " + vCR._mediaPlayerB.Info.GetVideoFrameRate();

        foreach (var row in _datas)
        {

            if (row.id == KovukList.Instance.ClickPosterID && b_IsInteractive)
            {    
                if (!NewQuestion)
                {
                    NewQuestion = true;
                    if (_QuestionCount <= row.questions.Count)
                    {
                        for (int i = _QuestionCountIn; i < _QuestionCount; i++)
                        {
                            InTime = vCR.ConvertTimeByMs(WriteQuestionTime(QuestionTime.InTime, i));
                            SATime = vCR.ConvertTimeByMs(WriteQuestionTime(QuestionTime.SATime, i));
                            FWTime = vCR.ConvertTimeByMs(WriteQuestionTime(QuestionTime.FirstWrongTime, i));
                            SWTime = vCR.ConvertTimeByMs(WriteQuestionTime(QuestionTime.SecondWrongTime, i));
                            TTime = vCR.ConvertTimeByMs(WriteQuestionTime(QuestionTime.TrueTime, i));
                            OutTime = vCR.ConvertTimeByMs(WriteQuestionTime(QuestionTime.OutTime, i));

                            QuestionRowCount = i;
                        }
                    }
                }
                

                if (vCR.PlayingPlayer.Control.GetCurrentTimeMs() >= InTime && !b_InTime)
                {
                    b_InTime = true;

                    VideoSeekSlider.gameObject.SetActive(false);
                    VideoPauseButton.gameObject.SetActive(false);
                    VideoPlayButton.gameObject.SetActive(false);

                    vCR.OnPlayButton();

                    Debug.Log("Question InTime " + QuestionRowCount);
                }
                if (vCR.PlayingPlayer.Control.GetCurrentTimeMs() >= SATime && !b_SATime)
                {
                    b_SATime = true;
                    StartCoroutine(StartCountdown());

                    if(QuestionWrongState <= 0)
                    {
                        for (int but = 0; but < ButtonList.Count; but++)
                        {
                            if (ButtonList[but].name.Contains(QuestionRowCount.ToString()))
                            {
                                ButtonList[but].gameObject.SetActive(true);

                                if (ButtonList[but].GetComponent<InteractiveButtonIn>().ButtonState == false)
                                {
                                    ButtonList[but].GetComponent<Button>().onClick.AddListener(() => QuestionFalseTranslation(FWTime));
                                }
                                else
                                {
                                    ButtonList[but].GetComponent<Button>().onClick.AddListener(() => QuestionTrueTranslation(TTime));
                                }
                            }
                        }
                    }
                    Debug.Log("Button Set Active " + QuestionRowCount);
                }
                if (!b_OutTime)
                {
                    if (currCountdownValue <= 0)
                    {
                        b_OutTime = true;
                        QuestionTranslation(OutTime);

                        AllReset();
                    }
                }
            }
        }

        //Question State
        if (QuestionWrongState >= 1 && !b_QuestionSWrongState && b_IsInteractive)
        {
            b_QuestionSWrongState = true;
            for (int but = 0; but < ButtonList.Count; but++)
            {
                if (ButtonList[but].name.Contains(QuestionRowCount.ToString()))
                {
                    ButtonList[but].GetComponent<Button>().onClick.RemoveAllListeners();
                }
            }
            QuestionWrongState += 1;
        }
        if (QuestionWrongState >= 2 && b_IsInteractive)
        {
            b_QuestionFWrongState = false;

            for (int but = 0; but < ButtonList.Count; but++)
            {
                if (ButtonList[but].name.Contains(QuestionRowCount.ToString()))
                {
                    if (ButtonList[but].GetComponent<InteractiveButtonIn>().ButtonState == false)
                    {
                        ButtonList[but].GetComponent<Button>().onClick.AddListener(() => QuestionFalseTranslation(SWTime));
                    }
                    else
                    {
                        ButtonList[but].GetComponent<Button>().onClick.AddListener(() => QuestionTrueTranslation(TTime));
                    }
                }
            }
        }
        if(QuestionWrongState >= 3 && !b_WrongQuestionOutTime && b_IsInteractive)
        {
            b_WrongQuestionOutTime = true;

            ResetButton();
            StopAllCoroutines();
            currCountdownValue = 20;

            Invoke("ExitQuestionAfterWrongAnswer", 8f);
        }
        //End Question State
    }
    void QuestionTranslation(float _trans)
    {
        float time = _trans;
        float duration = vCR.PlayingPlayer.Info.GetDurationMs();
        float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);
        vCR._videoSeekSlider.value = d;
    }
    void QuestionFalseTranslation(float _trans)
    {
        float time = _trans;
        float duration = vCR.PlayingPlayer.Info.GetDurationMs();
        float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);
        vCR._videoSeekSlider.value = d;

        if (!b_QuestionFWrongState)
        {
            b_QuestionFWrongState = true;
            QuestionWrongState += 1;
        }
    }
    void QuestionTrueTranslation(float _trans)
    {
        float time = _trans;
        float duration = vCR.PlayingPlayer.Info.GetDurationMs();
        float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);
        vCR._videoSeekSlider.value = d;

        if (!b_QuestionTrueState)
        {
            b_QuestionTrueState = true;
            AllReset();
        }
    }
    void AllReset()
    {
        VideoSeekSlider.gameObject.SetActive(true);
        VideoPauseButton.gameObject.SetActive(true);
        VideoPlayButton.gameObject.SetActive(true);

        StopAllCoroutines();
        currCountdownValue = 20;

        Invoke("ResetButton", .8f);
        Invoke("ResetBool", 1f);
    }
    void ResetBool()
    {
        b_InTime = false;
        b_SATime = false;
        b_FWTime = false;
        b_SWTime = false;
        b_TTime = false;
        b_OutTime = false;

        NewQuestion = false;
        _QuestionCount += 1;
        _QuestionCountIn += 1;

        QuestionWrongState = 0;
        b_QuestionFWrongState = false;
        b_QuestionSWrongState = false;
        b_WrongQuestionOutTime = false;
        b_QuestionTrueState = false;
    }
    private void ResetButton()
    {
        for (int but = 0; but < ButtonList.Count; but++)
        {
            if (ButtonList[but].name.Contains(QuestionRowCount.ToString()))
            {
                ButtonList[but].gameObject.SetActive(false);
            }
        }
    }
    void ExitQuestionAfterWrongAnswer()
    {
        AllReset();
        QuestionTranslation(OutTime);
    }
    public string WriteQuestionTime(QuestionTime time, int _selectedId)
    {
        string timeText = "";

        foreach (var row in _datas)
        {
            if (row.id == 1)
            {
                for (int i = 0; i < row.questions.Count; i++)
                {
                    if (row.questions[i].id == row.questions[_selectedId].id)
                    {
                        switch (time)
                        {
                            case QuestionTime.InTime:
                                timeText += row.questions[_selectedId].intime;
                                break;
                            case QuestionTime.SATime:
                                timeText += row.questions[_selectedId].satime;
                                break;
                            case QuestionTime.FirstWrongTime:
                                timeText += row.questions[_selectedId].firstwrongtime;
                                break;
                            case QuestionTime.SecondWrongTime:
                                timeText += row.questions[_selectedId].secondwrongtime;
                                break;
                            case QuestionTime.TrueTime:
                                timeText += row.questions[_selectedId].truetime;
                                break;
                            case QuestionTime.OutTime:
                                timeText += row.questions[_selectedId].outtime;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        return timeText;
    }
    public IEnumerator StartCountdown()
    {
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
    }
    private IEnumerator Fps()
    {
        GUI.depth = 2;
        while (true)
        {
            fps = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
