using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Start() {
            canvasGroup = GetComponent<CanvasGroup>();
            // StartCoroutine(FadeOutIn());

        }

        public IEnumerator FadeOutIn()
        {
            yield return FadeOut(1f);
            print("Faded out");
            yield return FadeIn(1f);
            print("Faded in");

        }

        public IEnumerator FadeOut(float time)
        {
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            } //

        }

        public IEnumerator FadeIn(float time)
        {
            while(canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            } //

        }

    }
}