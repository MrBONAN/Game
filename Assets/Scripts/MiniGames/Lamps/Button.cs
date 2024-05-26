using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

namespace LampsMiniGame
{
    public class Button : MonoBehaviour
    {
        public SpriteAtlas spriteAtlas;
        private SpriteRenderer buttonBase;
        private SpriteRenderer buttonText;

        private ButtonContent _bg;
        private ButtonContent _txt;

        private Vector3 _pos;

        public Vector3 Position
        {
            get => _pos;
            set
            {
                _pos = value;
                transform.localPosition = value;
            }
        }

        public ButtonContent ButtonBackground
        {
            get => _bg;
            set
            {
                _bg = value;
                buttonBase.sprite = spriteAtlas.GetSprite(value.ToString());
            }
        }

        public ButtonContent ButtonText
        {
            get => _txt;
            set
            {
                _txt = value;
                buttonText.sprite = spriteAtlas.GetSprite(value.ToString());
            }
        }

        private void Awake()
        {
            foreach (var component in GetComponentsInChildren<SpriteRenderer>())
            {
                if (component.name is "Base")
                    buttonBase = component;
                else if (component.name is "Text")
                    buttonText = component;
            }

            SetButton(ButtonContent.One, ButtonContent.Default);
        }

        public void SetButton(ButtonContent txt, ButtonContent bg)
        {
            ButtonBackground = bg;
            ButtonText = txt;
        }

        public void SetButtonTextColor(ButtonContent textColor)
        {
            _txt = textColor;
            buttonText.sprite = spriteAtlas.GetSprite(textColor + "-Text");
        }
    }
}