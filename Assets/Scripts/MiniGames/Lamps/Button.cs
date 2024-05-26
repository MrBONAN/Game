using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

namespace LampsMiniGame
{
    public class Button : MonoBehaviour
    {
        private SpriteRenderer buttonBase;
        private SpriteRenderer buttonText;

        private ButtonContent _bg;
        private ButtonContent _txt;
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
        
        public SpriteAtlas spriteAtlas;

        private void Start()
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

        private void SetButton(ButtonContent txt, ButtonContent bg)
        {
            ButtonBackground = bg;
            ButtonText = txt;
        }
    }
}