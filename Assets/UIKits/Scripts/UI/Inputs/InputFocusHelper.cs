using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace VRUiKits.Utils
{
    public class InputFocusHelper : MonoBehaviour, ISelectHandler
    {
        private InputField input;

        void Awake()
        {
            input = GetComponent<InputField>();
            KeyboardManager.Target = input;
            StartCoroutine(ActivateInputFieldWithCaret());
        }

        public void OnSelect(BaseEventData eventData)
        {
            /*
            Set keyboard target explicitly for some 3rd party packages which lost focus when
            user click on keyboard.
            */
            //KeyboardManager.Target = input;
            //StartCoroutine(ActivateInputFieldWithCaret());
        }

        IEnumerator ActivateInputFieldWithCaret()
        {
            input.ActivateInputField();

            yield return new WaitForEndOfFrame();

            if (EventSystem.current.currentSelectedGameObject == input.gameObject)
            {
                input.MoveTextEnd(false);
            }
        }
    }
}