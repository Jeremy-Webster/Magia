using System.Collections;
using UnityEngine;
using TMPro;

public class DisplayTip : MonoBehaviour
{
    public float scrollSpeed = 0.05f;
    public float delayBetweenLines = 0.4f;
    public float beginDelay = 1f;
    public float transistionDuration = 0.04f;

    public TextMeshProUGUI m_textMeshPro;
    private string[] lines;
    private CanvasGroup canvasGroup;

    public GameObject textContainer;

    IEnumerator Start()
    {
        if (beginDelay != 0f)
        {
            yield return new WaitForSeconds(beginDelay);
        }

        textContainer.SetActive(true);
        canvasGroup = textContainer.GetComponent<CanvasGroup>();

        lines = m_textMeshPro.text.Split('.');
        m_textMeshPro.SetText("");

        float tranCounter = 0f;
        while (tranCounter < transistionDuration)
        {
            tranCounter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, tranCounter / transistionDuration);
            yield return null;
        }

        for (int i = 0;i < lines.Length;i++)
        {
            string line = lines[i];
            if (line.Length < 4) break;

            m_textMeshPro.maxVisibleCharacters = 0;
            m_textMeshPro.SetText(line.Trim() + (i+1 < lines.Length ? "." : ""));

            yield return new WaitForSeconds(0.01f);
            int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount;
            int counter = 0;

            while (true)
            {
                counter++;

                m_textMeshPro.maxVisibleCharacters = counter;

                if (counter >= totalVisibleCharacters)
                {
                    yield return new WaitForSeconds(delayBetweenLines);
                    break;
                }

                yield return new WaitForSeconds(scrollSpeed);
            }
        }

        tranCounter = 0f;
        while (tranCounter < transistionDuration)
        {
            tranCounter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, tranCounter / transistionDuration);

            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}
