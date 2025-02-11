using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class CheckMatch : MonoBehaviour
    {
        [SerializeField] private GridConfiguration _gameConfiguration;
        [SerializeField] private List<Match> _matches = new List<Match>();

        [SerializeField] public TaskController _taskProcessor;
        public GridController _gridController;
        public TileSwapper _tileSwapper;
        public TileAnimationController _animatorController;

        public Action<int> UpdateScore; // �������� ����� �������� ����������

        public void CheckMatchesAndProcess(List<Cell> cellList)
        {
            _taskProcessor.AddTask(CheckMatches(cellList));
        }

        private IEnumerator CheckMatches(List<Cell> cellList)
        {
            Debug.Log("Enter the check grid");
            FindHorizontal(cellList, _gameConfiguration._rows, _gameConfiguration._columns);
            FindVertical(cellList, _gameConfiguration._rows, _gameConfiguration._columns);

            if (_matches.Count > 0)
            {
                _taskProcessor.AddTask(_gridController.DeleteTiles(cellList, _gameConfiguration._rows, _gameConfiguration._columns, _matches));
                _taskProcessor.AddTask(_gridController.DropTiles(cellList));
                //_taskProcessor.AddTask(_gridController.SpawnTiles(cellList));

                CheckMatchesAndProcess(cellList);
            }

            yield break;
        }

        private void FindVertical(List<Cell> cellList, int rows, int columns)
        {
            int cellListIndexPointer = 0;

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    int tileLength = 1;
                    int verticalStep = cellListIndexPointer + 1;
                    int localStep = j + 1;

                    if (localStep < rows)
                    {
                        if (cellList[cellListIndexPointer].Tile.TileType == cellList[verticalStep].Tile.TileType)
                        {
                            tileLength++;
                            CheckVerticalMatches(cellList, verticalStep, ref tileLength, localStep, rows);

                            if (tileLength >= 3)
                            {
                                Match verticalMatch = new Match(cellList[cellListIndexPointer].Tile, cellList[cellListIndexPointer].Tile.TileTransform.localPosition.x,
                                    cellList[cellListIndexPointer].Tile.TileTransform.localPosition.y, tileLength, false);
                                Debug.Log($"Match first pos X = {verticalMatch._tile.TileTransform.localPosition.x} ," +
                                        $"Match first pos Y = {verticalMatch._tile.TileTransform.localPosition.y} " +
                                        $"match type = {verticalMatch._tile.TileType} " +
                                        $"match length = {verticalMatch._length} " +
                                        $"match is horizontal= {verticalMatch._isHorizontal}");

                 
                                UpdateScore?.Invoke(tileLength);
                                _matches.Add(verticalMatch);
                            }

                            j = j + tileLength - 1;
                            cellListIndexPointer = cellListIndexPointer + (tileLength - 1);
                        }

                    }

                    cellListIndexPointer++;
                }
            }
        }

        private void CheckVerticalMatches(List<Cell> cellList, int tileLastIndexPointer, ref int tileLength, int localStepIndex, int rows)
        {
            int verticalStep = tileLastIndexPointer + 1;
            int localStep = localStepIndex + 1;

            while (localStep < rows && verticalStep < cellList.Count)
            {
                if (cellList[tileLastIndexPointer].Tile.TileType == cellList[verticalStep].Tile.TileType)
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
        private void FindHorizontal(List<Cell> cellList, int rows, int columns)
        {
            for (int i = 0; i < rows; i++) // ��� �� �������
            {
                for (int j = 0; j < columns - 1; j++) // ��� �� ��������, �� ������� �� �������
                {
                    // ������ ������� ������ � ���������� ������
                    int currentIndex = i * columns + j;

                    // ������ ��������� ������ �� �����������
                    int nextIndex = currentIndex + 1;

                    // ����� ������� ��������� ������
                    int tileLength = 1;

                    // �������� ���������� �� �����������
                    if (nextIndex < cellList.Count && cellList[currentIndex].Tile.TileType == cellList[nextIndex].Tile.TileType)
                    {
                        tileLength++;
                        CheckHorizontalMatches(cellList, ref nextIndex, ref tileLength, j + 1, rows, columns);

                        // ���� ����� ���������� ������ ��� ����� 3, ��������� � ������ ������
                        if (tileLength >= 3)
                        {
                            Match horizontalMatch = new Match(cellList[currentIndex].Tile,
                                cellList[currentIndex].Tile.TileTransform.localPosition.x,
                                cellList[currentIndex].Tile.TileTransform.localPosition.y,
                                tileLength, true);

                            Debug.Log($"Match first pos X = {horizontalMatch._tile.TileTransform.localPosition.x} ," +
                                    $"Match first pos Y = {horizontalMatch._tile.TileTransform.localPosition.y} " +
                                    $"match type = {horizontalMatch._tile.TileType} " +
                                    $"match length = {horizontalMatch._length} " +
                                    $"match is horizontal = {horizontalMatch._isHorizontal}");
                            UpdateScore?.Invoke(tileLength);
                            _matches.Add(horizontalMatch);
                        }

                        // ���������� ����������� ������ � ������� �������
                        j += tileLength - 1;
                    }
                }
            }
        }

        private void CheckHorizontalMatches(List<Cell> cellList, ref int tileLastIndexPointer, ref int tileLength, int localStepIndex, int rows, int columns)
        {
            // ��� �� ����������� ��� ��������� ������
            int horizontalStep = tileLastIndexPointer + 1;  // +1, ��� ��� �� �������� �� �������
            int localStep = localStepIndex + 1;

            while (localStep < columns && horizontalStep < cellList.Count)
            {
                // ���������, ��������� �� ������� ������ � �������� �� �����������
                if (cellList[tileLastIndexPointer].Tile.TileType == cellList[horizontalStep].Tile.TileType)
                {
                    tileLength++;
                    horizontalStep++;  // ������������ �� �����������
                    localStep++;
                }
                else
                {
                    break;  // ����� �� �����, ���� ������ �� ���������
                }
            }

            // ��������� ������ � �������� �����
            tileLastIndexPointer = horizontalStep - 1; // ����� ���������� ����� ������ ����� ��������� �� ��������� ������
        }

    }
}
