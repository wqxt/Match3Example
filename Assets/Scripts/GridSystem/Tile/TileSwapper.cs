using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class TileSwapper : MonoBehaviour
    {
        [SerializeField] private InputController _inputController;
        [SerializeField] private Cell _firstClickableObject = null;
        [SerializeField] protected internal CheckMatch _checkMatch;
        protected internal List<Cell> _cellList;

        private void OnEnable()
        {
            _inputController.ClickObject += CheckClickableObject;
        }

        private void OnDisable()
        {
            _inputController.ClickObject -= CheckClickableObject;
        }

        private void CheckClickableObject(Cell clickableObject)
        {
            if (_firstClickableObject == null)
            {
                _firstClickableObject = clickableObject;
                PlaySelectAnimation(clickableObject);
            }
            else
            {
                if (_firstClickableObject.Tile.TileType != clickableObject.Tile.TileType)
                {
                    if (CheckMove(_firstClickableObject, clickableObject))
                    {
                        SwapClickableObject(_cellList, _firstClickableObject, clickableObject);

                        PlayUnSelectAnimation(clickableObject);

                        _firstClickableObject = null;
                        Debug.Log("Check in the Swap");
                        _checkMatch.CheckMatchesAndProcess(_cellList);
                    }
                    else
                    {

                        Debug.Log($"Current move not correct");
                    }
                }
                else
                {
                    PlayUnSelectAnimation(_firstClickableObject);
                    _firstClickableObject = null;

                    return;
                }

            }
        }

        private bool CheckMove(Cell firstCell, Cell secondCell)
        {

            Vector3 firstCellVector = new Vector3(secondCell.transform.localPosition.x, secondCell.transform.localPosition.y, secondCell.transform.localPosition.z);
            Vector3 secondCellvector = new Vector3(firstCell.transform.localPosition.x, firstCell.transform.localPosition.y, firstCell.transform.localPosition.z);

            Vector3 checkValue = firstCellVector - secondCellvector;

            if (checkValue.normalized.x == 1 || checkValue.normalized.y == 1
                || checkValue.normalized.x == -1 || checkValue.normalized.y == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SwapClickableObject(List<Cell> cellList, Cell firstClickableObject, Cell secondClickableObject)
        {
            // Перемещение тайлов в мире
            Vector3 tempTransformPosition = firstClickableObject.Tile.transform.position;
            firstClickableObject.Tile.transform.position = secondClickableObject.Tile.transform.position;
            secondClickableObject.Tile.transform.position = tempTransformPosition;

            // Обмен тайлов в ячейках
            Tile tempTile = firstClickableObject.Tile;
            firstClickableObject.Tile = secondClickableObject.Tile;
            secondClickableObject.Tile = tempTile;

            // Найти индексы элементов в cellList
            int firstObjectIndex = cellList.IndexOf(firstClickableObject);
            int secondObjectIndex = cellList.IndexOf(secondClickableObject);

            if (firstObjectIndex >= 0 && secondObjectIndex >= 0)
            {
                // Обмен тайлов в списке
                var temp = cellList[firstObjectIndex].Tile.TileTransform;
                cellList[firstObjectIndex].Tile.TileTransform = cellList[secondObjectIndex].Tile.TileTransform;
                cellList[secondObjectIndex].Tile.TileTransform = temp;

                Debug.Log("Tiles swapped successfully.");
            }
            else
            {
                Debug.LogWarning("Failed to find one or both tiles in the list.");
            }
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