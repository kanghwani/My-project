using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace UnityChan
{
    public class FaceUpdate : MonoBehaviour
    {
        public AnimationClip[] animations;
        private Animator anim;

        public float delayWeight;
        public bool isKeepFace = false;

        private float current = 0;

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        void OnGUI()
        {
            GUILayout.Box("Face Update", GUILayout.Width(170), GUILayout.Height(25 * (animations.Length + 2)));

            Rect screenRect = new Rect(10, 25, 150, 25 * (animations.Length + 1));

            GUILayout.BeginArea(screenRect);

            foreach (var animation in animations)
            {
                if (GUILayout.RepeatButton(animation.name))
                {
                    anim.CrossFade(animation.name, 0);
                }
            }

            isKeepFace = GUILayout.Toggle(isKeepFace, " Keep Face");

            GUILayout.EndArea();
        }

        void Update()
        {
            Mouse mouse = Mouse.current;

            if (mouse != null && mouse.leftButton.isPressed)
            {
                current = 1;
            }
            else if (!isKeepFace)
            {
                current = Mathf.Lerp(current, 0, delayWeight);
            }

            anim.SetLayerWeight(1, current);
        }

        public void OnCallChangeFace(string str)
        {
            int checkedCount = 0;

            foreach (var animation in animations)
            {
                if (str == animation.name)
                {
                    ChangeFace(str);
                    break;
                }
                else if (checkedCount <= animations.Length)
                {
                    checkedCount++;
                }
                else
                {
                    str = "default@unitychan";
                    ChangeFace(str);
                }
            }
        }

        void ChangeFace(string str)
        {
            isKeepFace = true;
            current = 1;
            anim.CrossFade(str, 0);
        }
    }
}