using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Match3
{
    public class CheckMatch : MonoBehaviour
    {
        private List<Match> _matches = new List<Match>();
        public event Action<List<Tile>> FillGrid;
        [SerializeField] private GameConfiguration _gameConfiguration;
        public GridSpawner _gridSpawner;
        public TileSwapper _swapper;

        private void OnEnable()
        {
            _swapper.Swap += CheckGrid;
        }

        private void OnDisable()
        {
            _swapper.Swap -= CheckGrid;
        }

        public void CheckGrid(List<Tile> tileList)
        {

            Debug.Log("Enter chek grid");
            FindHorizontal(tileList, _gameConfiguration._rows, _gameConfiguration._columns);
            FindVertical(tileList, _gameConfiguration._rows, _gameConfiguration._columns);
            //TestFindHorizontal(tileList, _gameConfiguration._rows, _gameConfiguration._columns);

            if (_matches.Count > 0)
            {
                //DeleteTiles(tileList, _gameConfiguration._rows, _gameConfiguration._columns);
                Debug.Log("Find match in the check ");
                //StartCoroutine(Delete(tileList, _gameConfiguration._rows, _gameConfiguration._columns));
            }
            else
            {
                Debug.Log("Mathces not find");
                return;
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
                            CheckVerticalMatches(tileList, verticalStep, ref tileLength, localStep, rows);

                            if (tileLength >= 3)
                            {
                                Match verticalMatch = new Match(tileList[tileListIndexPointer], tileList[tileListIndexPointer].TileTransform.localPosition.x,
                                    tileList[tileListIndexPointer].TileTransform.localPosition.y, tileLength, false);
                                Debug.Log($"Match first pos X = {verticalMatch._tile.TileTransform.localPosition.x}," +
                                        $"Match first pos Y = {verticalMatch._tile.TileTransform.localPosition.y}" +
                                        $"match type = {verticalMatch._tile.TileType}" +
                                        $"match length = {verticalMatch._length}");
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

            while (localStep < rows) // Проверяем границу строки
            {
                if (tileList[tileLastIndexPointer].TileType == tileList[verticalStep].TileType)
                {
                    tileLength++;
                    tileLastIndexPointer = verticalStep; // Обновляем указатель на последний совпавший тайл
                }
                else
                {
                    break; // Выходим, если тип тайлов не совпадает
                }

                verticalStep++;
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
                                Debug.Log($"Match first pos X = {verticalMatch._tile.TileTransform.localPosition.x}," +
                                        $"Match first pos Y = {verticalMatch._tile.TileTransform.localPosition.y}" +
                                        $"match type = {verticalMatch._tile.TileType}" +
                                        $"match length = {verticalMatch._length}");

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
            int horizontalStep = tileLastIndexPointer + columns; // Переход к следующему тайлу в строке
            int localStep = localStepIndex + 1;

            while (localStep < columns) // Проверяем, что не вышли за пределы столбцов
            {
                if (tileList[tileLastIndexPointer].TileType == tileList[horizontalStep].TileType)
                {
                    tileLength++;
                    tileLastIndexPointer = horizontalStep; // Обновляем последний индекс
                }
                else
                {
                    break; // Выходим, если тип тайлов не совпадает
                }

                horizontalStep += columns; // Переход к следующему тайлу
                localStep++; // Увеличиваем шаг
            }
        }


        public void DeleteTiles(List<Tile> tileList, int rows, int columns)
        {
            List<Tile> deletedTiles = new List<Tile>();
            //Debug.Log("Enter delete tile method");
            for (int i = 0; i < _matches.Count; i++)
            {
                for (int j = 0; j < tileList.Count; j++)
                {
                    if (_matches[i]._tile.TileTransform == tileList[j].TileTransform && _matches[i]._tile != null)
                    {
                        Debug.Log($"First delete element X = {_matches[i]._tile.TileTransform.localPosition.x}" +
                       $"First delete element Y = {_matches[i]._tile.TileTransform.localPosition.x}" +
                       $"Lenght delete element = {_matches[i]._length}");

                        //Debug.Log($"{}");

                        if (_matches[i]._isHorizontal == true)
                        {
                            //Debug.Log("Enter horizontal check ");
                            for (int l = 0; l < _matches[i]._length; l++)
                            {
                                try
                                {
                                    Tile a = tileList[j + rows * l];

                                    deletedTiles.Add(a);
                                }
                                catch
                                {
                                    //Debug.Log("Out of horizontalrange ");

                                }

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

                a.TileType = null;
                Destroy(a.gameObject);
            }

            deletedTiles.Clear();
            _matches.Clear();
            //Debug.Log($"Matches count = {_matches.Count}");
            _gridSpawner.FillFallGrid(tileList);
            //CheckGrid(tileList);
        }



        public IEnumerator Delete(List<Tile> tileList, int rows, int columns)
        {

            yield return new WaitForSeconds(1f);

            List<Tile> deletedTiles = new List<Tile>();
            //Debug.Log("Enter delete tile method");
            for (int i = 0; i < _matches.Count; i++)
            {
                for (int j = 0; j < tileList.Count; j++)
                {
                    if (_matches[i]._tile.TileTransform == tileList[j].TileTransform && _matches[i]._tile != null)
                    {
                        Debug.Log($"First delete element X = {_matches[i]._tile.TileTransform.localPosition.x}" +
                       $"First delete element Y = {_matches[i]._tile.TileTransform.localPosition.x}" +
                       $"Lenght delete element = {_matches[i]._length}");

                        //Debug.Log($"{}");

                        if (_matches[i]._isHorizontal == true)
                        {
                            //Debug.Log("Enter horizontal check ");
                            for (int l = 0; l < _matches[i]._length; l++)
                            {
                                try
                                {
                                    Tile a = tileList[j + rows * l];
                                    //Debug.Log($"Detele this element X  = {a.TileTransform.localPosition.x}" +
                                    //    $"Detele type element X  = {a}");
                                    //a.gameObject.SetActive(false);
                                    //a.TileType = null;
                                    //a = null;

                                    deletedTiles.Add(a);
                                    //Destroy(a.gameObject);

                                    //Debug.Log($"tile type after check = {a.TileType}");
                                }
                                catch
                                {
                                    //Debug.Log("Out of horizontalrange ");

                                }

                            }
                            //break;
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
                a.TileType = null;
                Destroy(a.gameObject);
            }

            deletedTiles.Clear();
            _matches.Clear();
            //Debug.Log($"Matches count = {_matches.Count}");
            //FillGrid?.Invoke(tileList);

            //CheckGrid(tileList);
        }


    }
}