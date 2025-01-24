using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class CheckMatch : MonoBehaviour
    {
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] public TaskProcessor _taskProcessor;
        private List<Match> _matches = new List<Match>();

        public GridSpawner _gridSpawner;
        public TileSwapper _tileSwapper;


        public void CheckMatchesAndProcess(List<Tile> tileList)
        {
            _taskProcessor.AddTask(CheckMatches(tileList));
        }

        public IEnumerator CheckMatches(List<Tile> tileList)
        {

            Debug.Log("Enter the check grid");
            FindHorizontal(tileList, _gameConfiguration._rows, _gameConfiguration._columns);
            FindVertical(tileList, _gameConfiguration._rows, _gameConfiguration._columns);

            if (_matches.Count > 0)
            {

                _taskProcessor.AddTask(DeleteTiles(tileList, _gameConfiguration._rows, _gameConfiguration._columns));
                _taskProcessor.AddTask(_gridSpawner.DropTiles(tileList));
                _taskProcessor.AddTask(_gridSpawner.SpawnTiles(tileList));
                CheckMatchesAndProcess(tileList);


            }
            yield break;
        }

        public void FindVertical(List<Tile> tileList, int rows, int columns)
        {
            int tileListIndexPointer = 0;

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    int tileLength = 1;
                    int verticalStep = tileListIndexPointer + 1;
                    int localStep = j + 1;

                    if (localStep < rows)
                    {
                        if (tileList[tileListIndexPointer].TileType == tileList[verticalStep].TileType)
                        {
                            tileLength++;
                            CheckVerticalMatches(tileList, verticalStep, ref tileLength, localStep, rows);

                            if (tileLength >= 3)
                            {
                                Match verticalMatch = new Match(tileList[tileListIndexPointer], tileList[tileListIndexPointer].TileTransform.localPosition.x,
                                    tileList[tileListIndexPointer].TileTransform.localPosition.y, tileLength, false);
                                Debug.Log($"Match first pos X = {verticalMatch._tile.TileTransform.localPosition.x} ," +
                                        $"Match first pos Y = {verticalMatch._tile.TileTransform.localPosition.y} " +
                                        $"match type = {verticalMatch._tile.TileType} " +
                                        $"match length = {verticalMatch._length} " +
                                        $"match is hgorizontal= {verticalMatch._isHorizontal}");
                                _matches.Add(verticalMatch);

                            }

                            j = j + tileLength - 1;
                            tileListIndexPointer = tileListIndexPointer + (tileLength - 1);
                        }
                    }

                    tileListIndexPointer++;
                }
            }
        }

        public void CheckVerticalMatches(List<Tile> tileList, int tileLastIndexPointer, ref int tileLength, int localStepIndex, int rows)
        {
            int verticalStep = tileLastIndexPointer + 1;
            int localStep = localStepIndex + 1;

            while (localStep < rows && verticalStep < tileList.Count)
            {
                if (tileList[tileLastIndexPointer].TileType == tileList[verticalStep].TileType)
                {
                    tileLength++;
                    verticalStep++;
                    localStep++;
                }
                else
                {
                    break;
                }
            }
        }

        public void FindHorizontal(List<Tile> tileList, int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                int tileListIndexPointer = i;

                for (int j = 0; j < columns; j++)
                {
                    int secondElementIndex = tileListIndexPointer + rows;

                    int tileLength = 1;
                    int localStep = j + 1;

                    if (localStep < columns)
                    {
                        if (tileList[tileListIndexPointer].TileType == tileList[secondElementIndex].TileType)
                        {
                            tileLength++;
                            CheckHorizontalMatches(tileList, ref secondElementIndex, ref tileLength, localStep, rows, columns);

                            if (tileLength >= 3)
                            {
                                Match verticalMatch = new Match(tileList[tileListIndexPointer], tileList[tileListIndexPointer].TileTransform.localPosition.x,
                                    tileList[tileListIndexPointer].TileTransform.localPosition.y, tileLength, true);
                                Debug.Log($"Match first pos X = {verticalMatch._tile.TileTransform.localPosition.x} ," +
                                        $"Match first pos Y = {verticalMatch._tile.TileTransform.localPosition.y} " +
                                        $"match type = {verticalMatch._tile.TileType} " +
                                        $"match length = {verticalMatch._length} " +
                                        $"mathc ishorizontal = {verticalMatch._isHorizontal}");

                                _matches.Add(verticalMatch);
                            }

                            for (int x = 0; x < tileLength - 1; x++)
                            {
                                tileListIndexPointer = tileListIndexPointer + rows;
                            }
                            j = j + tileLength - 1;
                        }
                    }

                    tileListIndexPointer = tileListIndexPointer + rows;
                }
            }
        }

        public void CheckHorizontalMatches(List<Tile> tileList, ref int tileLastIndexPointer, ref int tileLength, int localStepIndex, int rows, int columns)
        {
            int horizontalStep = tileLastIndexPointer + rows;
            int localStep = localStepIndex + 1;

            while (localStep < columns && horizontalStep < tileList.Count)
            {
                if (tileList[tileLastIndexPointer].TileType == tileList[horizontalStep].TileType)
                {
                    tileLength++;
                    horizontalStep += rows;
                    localStep++;
                }
                else
                {
                    break;
                }
            }
        }


        public IEnumerator DeleteTiles(List<Tile> tileList, int rows, int columns)
        {
            yield return new WaitForSeconds(1f);


            HashSet<Tile> tilesToDelete = new HashSet<Tile>();

            for (int i = 0; i < _matches.Count; i++)
            {
                for (int j = 0; j < tileList.Count; j++)
                {
                    if (tileList[j] != null && _matches[i]._tile.TileTransform == tileList[j].TileTransform && _matches[i]._tile != null)
                    {
                        if (_matches[i]._isHorizontal)
                        {
                            for (int l = 0; l < _matches[i]._length; l++)
                            {
                                int index = j + rows * l;
                                if (index >= 0 && index < tileList.Count)
                                {
                                    Tile a = tileList[index];

                                    if (a.TileType != null)
                                    {
                    
                                        tilesToDelete.Add(a);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int l = 0; l < _matches[i]._length; l++)
                            {
                                if (j + l >= 0 && j + l < tileList.Count)
                                {
                                    Tile a = tileList[j + l];
                                    if (a.TileType != null)
                                    {
                                        
                                        tilesToDelete.Add(a);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Удаляем тайлы
            foreach (var a in tilesToDelete)
            {
                if (a != null && a.gameObject != null)
                {
         
                    Debug.Log($"DELETED Tile type {a.TileType}" +
                        $"Tile X = {a.TileTransform.localPosition.x}" +
                        $"Tile Y = {a.TileTransform.localPosition.y}");

                    _gridSpawner.PlayDeletedTileAnimation(a);

                    //a.transform
                    //.DOScale(2f, 1f)
                    //.SetEase(Ease.InOutQuad);


                    a.TileType = null;
                    //Destroy(a.gameObject);
                }
            }


            tilesToDelete.Clear();
            _matches.Clear();
        }
    }
}
