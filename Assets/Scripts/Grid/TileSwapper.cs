using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class TileSwapper : MonoBehaviour
    {
        [SerializeField] private InputController _inputController;
        [SerializeField] private Cell _firstClickableObject = null;
        public List<Tile> _tileList;
        public event Action<List<Tile>> Swap;
        public List<Cell> _cellList;
        public CheckMatch _checkmatch;
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
                if (_firstClickableObject.Tile.TileType != clickableObject.Tile.TileType)
                {
                    if (CheckMove(_firstClickableObject, clickableObject))
                    {
                        SwapClickableObject(_tileList, _firstClickableObject, clickableObject);


                        PlayUnSelectAnimation(clickableObject);


                        _firstClickableObject = null;
                        _checkmatch.CheckGrid(_tileList);
                    }
                    else
                    {
                        //PlayUnSelectAnimation(_firstClickableObject);
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

        public bool CheckMove(Cell firstCell, Cell secondCell)
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

        private void SwapClickableObject(List<Tile> tileList, Cell firstClickableObject, Cell secondClickableObject)
        {
            // Перемещение тайлов в мире
            Vector3 tempTransformPosition = firstClickableObject.Tile.transform.position;
            firstClickableObject.Tile.transform.position = secondClickableObject.Tile.transform.position;
            secondClickableObject.Tile.transform.position = tempTransformPosition;

            // Обмен тайлов в ячейках
            Tile tempTile = firstClickableObject.Tile;
            firstClickableObject.Tile = secondClickableObject.Tile;
            secondClickableObject.Tile = tempTile;

            // Найти индексы элементов в tileList
            int firstObjectIndex = tileList.IndexOf(firstClickableObject.Tile);
            int secondObjectIndex = tileList.IndexOf(secondClickableObject.Tile);

            // Убедиться, что индексы найдены
            if (firstObjectIndex >= 0 && secondObjectIndex >= 0)
            {
                // Обмен тайлов в списке
                var temp = tileList[firstObjectIndex];
                tileList[firstObjectIndex] = tileList[secondObjectIndex];
                tileList[secondObjectIndex] = temp;

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