﻿using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TextSequence
{
    public string sequenceName; // Identifikátor pro SignalReceiver
    public TextMeshProUGUI targetText;
    public string textToDisplay;
    public float fadeInTime = 1f;
    public float displayTime = 3f;
    public float fadeOutTime = 1f;
}

public class MultiTextManager : MonoBehaviour
{
    [Header("Text Sequences")]
    public List<TextSequence> sequences = new List<TextSequence>();

    private Dictionary<string, Coroutine> activeCoroutines = new Dictionary<string, Coroutine>();

    public void PlaySequence(string sequenceName)
    {
        TextSequence seq = sequences.Find(s => s.sequenceName == sequenceName);
        if (seq == null || seq.targetText == null) return;


        if (activeCoroutines.ContainsKey(sequenceName))
        {
            StopCoroutine(activeCoroutines[sequenceName]);
            activeCoroutines.Remove(sequenceName);
        }

        activeCoroutines[sequenceName] = StartCoroutine(RunTextSequence(seq));
    }

    private IEnumerator RunTextSequence(TextSequence seq)
    {
        seq.targetText.text = seq.textToDisplay;

        yield return StartCoroutine(FadeText(seq.targetText, 0f, 1f, seq.fadeInTime));

        yield return new WaitForSeconds(seq.displayTime);

        yield return StartCoroutine(FadeText(seq.targetText, 1f, 0f, seq.fadeOutTime));

        seq.targetText.text = "";

        if (activeCoroutines.ContainsKey(seq.sequenceName))
        {
            activeCoroutines.Remove(seq.sequenceName);
        }
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float startAlpha, float endAlpha, float duration)
    {
        text.gameObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            SetTextAlpha(text, newAlpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetTextAlpha(text, endAlpha);
        if (endAlpha < 0.1f) text.gameObject.SetActive(false);
    }

    private void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = Mathf.Clamp01(alpha);
        text.color = color;
    }
}
