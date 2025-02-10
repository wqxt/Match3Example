using Unity.VisualScripting;
using UnityEngine;

namespace Match3
{
    public class ScoreController : MonoBehaviour
    {
        private int currentScore = 0;
        public CheckMatch _checkMatch;
        public ScoreView _scoreView;

        private void OnEnable()
        {
            _checkMatch.UpdateScore += SetCurrentScore;
        }

        private void OnDisable()
        {
            _checkMatch.UpdateScore -= SetCurrentScore;
        }

        public void SetCurrentScore(int scoreLenght)
        {
            Debug.Log("Invoke currentsetscore method");
            currentScore += scoreLenght;
            StartCoroutine(_scoreView.SetScore(currentScore.ToString()));
        }
    }
}