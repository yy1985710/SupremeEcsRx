using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EcsRx.Utility;
using UniRx;
using UnityEngine.UI;

namespace EcsRx.UI
{
    public class RollingMessage
    {
        public string Message;
        public int Count;
    }

    public class RollMessage : MonoBehaviour
    {
        public Queue<RollingMessage> Messages;
        public Text MessageText;
        public float RollingWidth;
        private bool isRolling;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Messages?.Count > 0)
            {
                if (!isRolling)
                {
                    var message = Messages.Dequeue();
                    Rolling(message.Message, message.Count);
                    isRolling = true;
                }
            }
        }

        private void Rolling(string text, int count)
        {
            MessageText.text = text;
            var rectTransform = MessageText.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 0);
            var width = MessageText.preferredWidth;
            rectTransform.DOAnchorPosX(-width - RollingWidth, 3).SetLoops(count).SetEase(Ease.Linear).OnComplete(() => isRolling = false);
        }
    }
}

