using UnityEngine;

namespace FMenu.Utils
{
    public class Timer
    {
        private float targetSeconds { get; set; }
        private float startTimestamp { get; set; }

        public Timer(float _targetSeconds)
        {

        }

        private void Reset(float _targetSeconds)
        {
            this.targetSeconds = _targetSeconds;
            this.startTimestamp = Time.time;
        }

        public bool isCompleted()
        {
            if (Time.time >= startTimestamp + targetSeconds)
            {
                Reset(targetSeconds);
                return true;
            }
            return false;
        }
    }
}