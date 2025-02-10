using System.Collections;
using TMPro;
using UnityEngine;

namespace Match3
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentScoreValue;

        public IEnumerator  SetScore(string scoreValue)
        {
            yield return new WaitForSeconds(1f);
            _currentScoreValue.text = scoreValue; 
        }
    }
}