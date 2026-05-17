using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace UnityChan
{
    [RequireComponent(typeof(Animator))]
    public class IdleChanger : MonoBehaviour
    {
        private Animator anim;
        private AnimatorStateInfo currentState;
        private AnimatorStateInfo previousState;

        public bool _random = false;
        public float _threshold = 0.5f;
        public float _interval = 10f;

        void Start()
        {
            anim = GetComponent<Animator>();
            currentState = anim.GetCurrentAnimatorStateInfo(0);
            previousState = currentState;

            StartCoroutine(RandomChange());
        }

        void Update()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard == null)
                return;

            if (keyboard.upArrowKey.wasPressedThisFrame )
            {
                anim.SetBool("Next", true);
            }

            if (keyboard.downArrowKey.wasPressedThisFrame)
            {
                anim.SetBool("Back", true);
            }

            if (anim.GetBool("Next"))
            {
                currentState = anim.GetCurrentAnimatorStateInfo(0);

                if (previousState.nameHash != currentState.nameHash)
                {
                    anim.SetBool("Next", false);
                    previousState = currentState;
                }
            }

            if (anim.GetBool("Back"))
            {
                currentState = anim.GetCurrentAnimatorStateInfo(0);

                if (previousState.nameHash != currentState.nameHash)
                {
                    anim.SetBool("Back", false);
                    previousState = currentState;
                }
            }
        }

        void OnGUI()
        {
            GUI.Box(new Rect(Screen.width - 110, 10, 100, 90), "Change Motion");

            if (GUI.Button(new Rect(Screen.width - 100, 40, 80, 20), "Next"))
                anim.SetBool("Next", true);

            if (GUI.Button(new Rect(Screen.width - 100, 70, 80, 20), "Back"))
                anim.SetBool("Back", true);
        }

        IEnumerator RandomChange()
        {
            while (true)
            {
                if (_random)
                {
                    float seed = Random.Range(0.0f, 1.0f);

                    if (seed < _threshold)
                    {
                        anim.SetBool("Back", true);
                    }
                    else
                    {
                        anim.SetBool("Next", true);
                    }
                }

                yield return new WaitForSeconds(_interval);
            }
        }
    }
}