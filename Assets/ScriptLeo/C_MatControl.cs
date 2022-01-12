using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Custom.MaterialController
{
    public class C_MatControl:MonoBehaviour
    {
        public static C_MatControl instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != null)
            {
                Destroy(gameObject);
            }
        }

        public static void FadeOutlineColorTo(Outline outline, Color to_c, float duration)
        {
            Color from_c = outline.effectColor;
            instance.StartCoroutine(ie_FadeOutlineColorTo(outline, from_c, to_c, duration));
        }

        static IEnumerator ie_FadeOutlineColorTo(Outline outline, Color fromColor, Color toColor, float duration)
        {
            float timepass = 0;
            float realDuration = 1 / duration;
            while (timepass < 1)
            {
                timepass += Time.deltaTime * realDuration;
                outline.effectColor = Color.Lerp(fromColor, toColor, timepass);
                yield return null;
            }
        }


        public static void FadeMaterialColorTo(Material mat, Color toColor, float duration)
        {
            Color fromColor = mat.color;
            instance.StartCoroutine(ie_FadeMaterialColorTo(mat, fromColor, toColor, duration));
        }

        static IEnumerator ie_FadeMaterialColorTo(Material mat,Color fromColor, Color toColor, float duration)
        {
            float timepass = 0;
            float realDuration = 1 / duration;
            while (timepass < 1)
            {
                timepass += Time.deltaTime* realDuration;
                mat.color = Color.Lerp(fromColor, toColor, timepass);
                yield return null;
            }
        }
    }
}
