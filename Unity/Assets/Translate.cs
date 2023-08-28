using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translate : MonoBehaviour
{
    [SerializeField]
    private Text ReadKoreaText;
    [SerializeField]
    private Text ReadJapanText;
    [SerializeField]
    private Text ReadEnglishText;
    [SerializeField]
    private AudioSource audioSource;


    private string english = "En";
    private string japan = "Ja";
    private string Korea = "ko";

    private string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=";

    // Start is called before the first frame update
    void Start()
    {
    }

    IEnumerator PlaySpeak(string str)
    {
        Debug.Log(str);
        WWW www = new WWW(str);
        yield return www;

        audioSource.clip = www.GetAudioClip(false, true, AudioType.MPEG);
        audioSource.Play();
    }

    private string GetString(string text, string stateName)
    {
        return text + "&tl=" + stateName + "-gb";
    }
    public void SpeakGPT(string Question)
    {
        StartCoroutine(PlaySpeak(url + GetString(Question, Korea)));
    }


    public void KoreaButton()
    {
        StartCoroutine(PlaySpeak(url + GetString(ReadKoreaText.text, Korea)));         
    }
    public void JapanButton()
    {
        StartCoroutine(PlaySpeak(url + GetString(ReadJapanText.text, japan)));
    }
    public void EnglishButton()
    {
        StartCoroutine(PlaySpeak(url + GetString(ReadEnglishText.text, english)));
    }
}
