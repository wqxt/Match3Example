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
        protected internal Cell[,] _cellGrid;
        //public GridController _gridController;

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
            Debug.Log($"Clickable cell pos = {clickableObject.transform.position}");


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


                        SwapClickableObject(_cellGrid, _firstClickableObject, clickableObject);

                        PlayUnSelectAnimation(clickableObject);

                        _firstClickableObject = null;
                        Debug.Log("Check in the Swap");
                        //_checkMatch.CheckMatchesAndProcess(_cellList);
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

        private void SwapClickableObject(Cell[,] cellGrid, Cell firstClickableCell, Cell secondClickableCell)
        {

            // swap tile position on the grid
            Vector3 tempTransformPosition = firstClickableCell.Tile.transform.position;
            firstClickableCell.Tile.transform.position = secondClickableCell.Tile.transform.position;
            secondClickableCell.Tile.transform.position = tempTransformPosition;

            // swap tile reference to cell
            Transform tempTransform = firstClickableCell.Tile.TileTransform;
            firstClickableCell.Tile.TileTransform = secondClickableCell.Tile.TileTransform;
            secondClickableCell.Tile.TileTransform = tempTransform;

            //swap tile sprite on the grid
            Tile tempTile = firstClickableCell.Tile;
            firstClickableCell.Tile = secondClickableCell.Tile;
            secondClickableCell.Tile = tempTile;

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