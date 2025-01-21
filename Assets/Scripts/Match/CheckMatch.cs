using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class CheckMatch : MonoBehaviour
    {
        private List<Match> _matches = new List<Match>();
        public event Action<List<Tile>> FillGrid;
        public event Action<List<Cell>> FillTiles;
        [SerializeField] private GameConfiguration _gameConfiguration;
        public GridSpawner _spawner;
        public TileSwapper _swapper;

        private void OnEnable()
        {
            //_swapper.Swap += CheckMatches;
        }

        private void OnDisable()
        {
            //_swapper.Swap -= CheckMatches;
        }

        public void CheckMatches(List<Tile> tileList)
        {

            Debug.Log("Enter check grid");
            FindHorizontal(tileList, _gameConfiguration._rows, _gameConfiguration._columns);
            FindVertical(tileList, _gameConfiguration._rows, _gameConfiguration._columns);

            if (_matches.Count > 0)
            {
                StartCoroutine(DeleteTiles(tileList, _gameConfiguration._rows, _gameConfiguration._columns));

                StartCoroutine(_spawner.DropTiles(tileList));

            }
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
                            CheckVerticalMatches(tileList, ref verticalStep, ref tileLength, localStep, rows);

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

        public void CheckVerticalMatches(List<Tile> tileList, ref int tileLastIndexPointer, ref int tileLength, int localStepIndex, int rows)
        {
            int verticalStep = tileLastIndexPointer + 1;
            int localStep = localStepIndex + 1;


            while (localStep < rows && verticalStep < tileList.Count && tileList[tileLastIndexPointer].TileType == tileList[verticalStep].TileType)
            {
                tileLength++;
                tileLastIndexPointer = verticalStep;
                verticalStep = tileLastIndexPointer + 1;
                localStep++;
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
            var horizontalStep = tileLastIndexPointer + rows;
            int localStep = localStepIndex + 1;

            if (localStep < columns)
            {
                if (tileList[tileLastIndexPointer].TileType == tileList[horizontalStep].TileType)
                {
                    tileLength++;

                    CheckHorizontalMatches(tileList, ref horizontalStep, ref tileLength, localStep, rows, columns);

                }
            }
        }


        public IEnumerator DeleteTiles(List<Tile> tileList, int rows, int columns)
        {
            yield return new WaitForSeconds(1f);

            List<Tile> deletedTiles = new List<Tile>();


            for (int i = 0; i < _matches.Count; i++)
            {
                for (int j = 0; j < tileList.Count; j++)
                {
                    if (_matches[i]._tile.TileTransform == tileList[j].TileTransform && _matches[i]._tile != null)
                    {
                        Debug.Log($"First delete element X = {_matches[i]._tile.TileTransform.localPosition.x}" +
                       $"First delete element Y = {_matches[i]._tile.TileTransform.localPosition.x}" +
                       $"Lenght delete element = {_matches[i]._length}" +
                       $"Tile type element = {_matches[i]._tile.TileType}");



                        if (_matches[i]._isHorizontal == true)
                        {
                            for (int l = 0; l < _matches[i]._length; l++)
                            {
                                Tile a = tileList[j + rows * l];

                                deletedTiles.Add(a);
                            }
                        }
                        else
                        {
                            for (int l = 0; l < _matches[i]._length; l++)
                            {
                                Tile a = tileList[j + l];

                                deletedTiles.Add(a);

                            }
                        }
                    }
                }
            }


            foreach (var a in deletedTiles)
            {
                Debug.Log(a.TileType);
                a.TileType = null;
                Destroy(a.gameObject);
            }

            deletedTiles.Clear();
            _matches.Clear();
        }



    }
}
