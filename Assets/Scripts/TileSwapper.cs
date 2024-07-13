using DG.Tweening;
using UnityEngine;

namespace Match3
{
    public class TileSwapper : MonoBehaviour
    {
        [SerializeField] private InputController _inputController;
        private Cell _firstClickableObject = null;

        private void OnEnable()
        {
            _inputController.ClickObject += CheckClickableObject;
        }

        private void OnDisable()
        {
            _inputController.ClickObject -= CheckClickableObject;
        }

        public void CheckClickableObject(Cell clickableObject)
        {
            if (_firstClickableObject == null)
            {
                _firstClickableObject = clickableObject;
                PlaySelectAnimation(clickableObject);
            }
            else
            {
                SwapClickableObject(_firstClickableObject, clickableObject);
                PlayUnSelectAnimation(clickableObject);

                _firstClickableObject = null;
            }
        }

        private void SwapClickableObject(Cell firstClickableObject, Cell secondClickableObject)
        {
            Vector3 tempTransformPosition = firstClickableObject.Tile.transform.position;
            firstClickableObject.Tile.transform.position = secondClickableObject.Tile.transform.position;
            secondClickableObject.Tile.transform.position = tempTransformPosition;


            Tile tempFruitPostion = firstClickableObject.Tile;
            firstClickableObject.Tile = secondClickableObject.Tile;
            secondClickableObject.Tile = tempFruitPostion;
        }

        private void PlaySelectAnimation(Cell clickableObject)
        {
            clickableObject.Tile.transform
                .DOScale(0.7f, 0.2f)
                .SetEase(Ease.InOutQuad);
        }

        private void PlayUnSelectAnimation(Cell clickableObject)
        {
            clickableObject.Tile.transform
                .DOScale(0.5f, 0.2f)
                .SetEase(Ease.InOutQuad);
        }
    }
}