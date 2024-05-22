using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextGradientEffect : MonoBehaviour
{
    // 참고: https://www.youtube.com/watch?v=JJ6T3ROG_RA&t=433s

    [SerializeField] private Gradient gradient;
    [SerializeField] private float gradientSpeed;

    private TMP_Text m_TextComponent;
    private float _totalTime;

    private void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartCoroutine(AnimateVertexColors());
    }

    IEnumerator AnimateVertexColors()
    {
        yield return new WaitForSeconds(4f);
        m_TextComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        int currentCharacter = 0;

        Color32[] newVertexColors;
        Color32 c0 = gradient.Evaluate(0f);
        Color32 c1 = m_TextComponent.color;

        while(true)
        {
            int characterCount = textInfo.characterCount;

            if(characterCount == 0)
            {
                yield return new WaitForSeconds(0.25f);
                continue;
            }

            int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

            if(textInfo.characterInfo[currentCharacter].isVisible)
            {
                float offset = (currentCharacter / characterCount);
                c0 = gradient.Evaluate((_totalTime + offset) % 1);
                _totalTime += Time.deltaTime;

                newVertexColors[vertexIndex + 0] = c0;
                newVertexColors[vertexIndex + 1] = c0;
                newVertexColors[vertexIndex + 2] = c0;
                newVertexColors[vertexIndex + 3] = c1;

                c0 = c1;

                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
            currentCharacter = (currentCharacter + 1) % characterCount;

            yield return new WaitForSeconds(gradientSpeed);
        }
    }
}
