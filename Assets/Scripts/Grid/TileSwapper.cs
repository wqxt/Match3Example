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

            Debug.Log($"CheckValue normalized X = {checkValue.normalized.x}" +
                $" CheckValue normalized Y  ={checkValue.normalized.y}");


            if (checkValue.normalized.x == 1 || checkValue.normalized.y == 1 || checkValue.normalized.x == -1 || checkValue.normalized.y == -1)
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

            int firstObjectIndex = -1;
            int secondObjectIndex = -1;


            Vector3 tempTransformPosition = firstClickableObject.Tile.transform.position;
            firstClickableObject.Tile.transform.position = secondClickableObject.Tile.transform.position;
            secondClickableObject.Tile.transform.position = tempTransformPosition;


            Tile tempFruitTile = firstClickableObject.Tile;

            firstClickableObject.Tile.TileTransform = secondClickableObject.Tile.TileTransform;

            firstClickableObject.Tile = secondClickableObject.Tile;

            secondClickableObject.Tile.TileTransform = tempFruitTile.TileTransform;

            secondClickableObject.Tile = tempFruitTile;




            for (int i = 0; i < tileList.Count; i++)
            {

                if (tileList[i].transform.position == firstClickableObject.Tile.transform.position)
                {
                    Debug.Log($"First type = {tileList[i]}" +
                        $"First index = {i}");
                    firstObjectIndex = i;
                }

                if (tileList[i].transform.position == secondClickableObject.Tile.transform.position)
                {
                    Debug.Log($"Second index = {tileList[i]}" +
                        $"First index = {i}");
                    secondObjectIndex = i;
                }


            }

            if (firstObjectIndex >= 0 && secondObjectIndex >= 0)
            {

                Debug.Log("Enter in the swap ");
                var temp = tileList[firstObjectIndex];
                tileList[firstObjectIndex] = tileList[secondObjectIndex];
                tileList[secondObjectIndex] = temp;

                Swap?.Invoke(tileList);
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