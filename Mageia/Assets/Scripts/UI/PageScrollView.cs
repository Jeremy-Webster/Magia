using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MageiaGame
{
    public class PageScrollView : ScrollRect
    {
        public GameObject[] pages, nav_buttons;
        [Range(1, 5)]
        public int page = 1;

        private float scroll_to = 0f, scroll_distance = 0f, last_move = 0f;
        private float[] pos;

        private Animator[] anims;

        protected override void Start()
        {
            base.Start();

            anims = new Animator[4];

            if (nav_buttons != null && nav_buttons.Length > 0)
            {
                for (int i = 0; i < nav_buttons.Length; i++)
                {
                    anims[i] = nav_buttons[i].GetComponent<Animator>();
                }
            }

            if (pages != null && pages.Length > 0)
            {
                ChangePage(page, true);
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            ShowPages();

            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            pos = new float[pages != null ? pages.Length : 1];
            scroll_distance = 1f / (pos.Length - 1f);

            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = scroll_distance * i;
            }

            for (int i = 0; i < pos.Length; i++)
            {
                if (horizontalNormalizedPosition < pos[i] + (scroll_distance / 2) && horizontalNormalizedPosition > pos[i] - (scroll_distance / 2))
                {
                    ChangePage(i + 1);

                }
            }
        }


        private void Update()
        {
            if (Input.GetMouseButton(0))
                last_move = Time.time + 0.5f;

            if (!Input.GetMouseButton(0) && horizontalNormalizedPosition != scroll_to)
            {
                horizontalNormalizedPosition = Mathf.Lerp(horizontalNormalizedPosition, scroll_to, 0.1f);
                if (Mathf.Abs(horizontalNormalizedPosition - scroll_to) < 0.0001)
                    horizontalNormalizedPosition = scroll_to;

                last_move = Time.time + 0.5f;
            }

            if (last_move != 0f && Time.time >= last_move)
            {
                HidePages();
                last_move = 0f;
            }
        }

        public void ShowPages()
        {
            for (int i = 1; i <= pages.Length; i++)
            {
                pages[i - 1].transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        private void HidePages()
        {
            for (int i = 1; i <= pages.Length; i++)
            {
                pages[i - 1].transform.GetChild(0).gameObject.SetActive(i == page);
            }
        }

        public void ChangePage(int page, bool instant)
        {
            float t = ((float)page - 1f) / (pages.Length - 1);

            if (instant)
            {
                horizontalNormalizedPosition = t;
                HidePages();
            }
            else
            {
                ShowPages();
            }

            for (int i = 0; i < nav_buttons.Length; i++)
            {
                if (page == (i + 1))
                {
                    anims[i].Play("btn_navigation_select");
                }
                else
                {
                    anims[i].Play("btn_navigation_unselect");
                }
            }

            scroll_to = t;
            this.page = page;
        }
        public void ChangePage(int page) { ChangePage(page, false); }
    }

    /*
    [CustomEditor(typeof(PageScrollView))]
    public class PageScrollViewEditor : ScrollRectEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("pages"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("nav_buttons"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("page"), true);
            this.serializedObject.ApplyModifiedProperties();
        }
    }
    */
}