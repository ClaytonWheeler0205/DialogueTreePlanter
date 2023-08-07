using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueTreePlanter.Elements
{
    public class DialogueTreeGroup : Group
    {
        public string OldTitle;
        private Color _defaultBorderColor;
        private float _defaultBorderWidth;

        public DialogueTreeGroup(string groupTitle, Vector2 position)
        {
            title = groupTitle;
            OldTitle = groupTitle;
            SetPosition(new Rect(position, Vector2.zero));
            _defaultBorderColor = contentContainer.style.borderBottomColor.value;
            _defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
        }

        public void SetErrorStyle(Color color)
        {
            contentContainer.style.borderBottomColor = color;
            contentContainer.style.borderBottomWidth = 2f;
        }

        public void ResetStyle()
        {
            contentContainer.style.borderBottomColor = _defaultBorderColor;
            contentContainer.style.borderBottomWidth = _defaultBorderWidth;
        }
    }
}