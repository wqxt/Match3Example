using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class CheckMatch : MonoBehaviour
    {
        [SerializeField] private GridConfiguration _gameConfiguration;
        [SerializeField] public TaskController _taskProcessor;
        private List<Match> _matches = new List<Match>();

        public GridController _gridSpawner;
        public TileSwapper _tileSwapper;
        public TileAnimationController _animatorController;

        public void CheckMatchesAndProcess(List<Tile> tileList)
        {
            _taskProcessor.AddTask(CheckMatches(tileList));
        }

        private IEnumerator CheckMatches(List<Tile> tileList)
        {

            Debug.Log("Enter the check grid");
            FindHorizontal(tileList, _gameConfiguration._rows, _gameConfiguration._columns);
            FindVertical(tileList, _gameConfiguration._rows, _gameConfiguration._columns);

            if (_matches.Count > 0)
            {

                _taskProcessor.AddTask(_gridSpawner.DeleteTiles(tileList, _gameConfiguration._rows, _gameConfiguration._columns, _matches));
                _taskProcessor.AddTask(_gridSpawner.DropTiles(tileList));
                _taskProcessor.AddTask(_gridSpawner.SpawnTiles(tileList));
                CheckMatchesAndProcess(tileList);


            }
            yield break;
        }

        private void FindVertical(List<Tile> tileList, int rows, int columns)
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

        private void CheckVerticalMatches(List<Tile> tileList, int tileLastIndexPointer, ref int tileLength, int localStepIndex, int rows)
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

       private void FindHorizontal(List<Tile> tileList, int rows, int columns)
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

        private void CheckHorizontalMatches(List<Tile> tileList, ref int tileLastIndexPointer, ref int tileLength, int localStepIndex, int rows, int columns)
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
    }
}
