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
        public event Action<List<Cell>> FillTiles;
        [SerializeField] private GameConfiguration _gameConfiguration;

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
                StartCoroutine(DeleteTiles(tileList, _gameConfiguration._rows, _gameConfiguration._columns));
                Debug.Log("Find match in the check ");
                //StartCoroutine(Delete(tileList, _gameConfiguration._rows, _gameConfiguration._columns));


            }
            else
            {
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
            var verticalStep = tileLastIndexPointer + 1;
            int localStep = localStepIndex + 1;

            if (localStep < rows)
            {
                if (tileList[tileLastIndexPointer].TileType == tileList[verticalStep].TileType)
                {
                    tileLength++;

                    CheckVerticalMatches(tileList, verticalStep, ref tileLength, localStep, rows);
                }
            }
        }

        public void TestFindHorizontal(List<Tile> tileList, int rows, int columns)
        {


            for (int i = 0; i < rows; i++)
            {
                int tileListIndexPointer = i;
                int tileLength = 1;
                int firstStepIndex = 0;
                for (int j = 0; j < columns - 2; j++)
                {

                    var step = tileListIndexPointer + rows;
                    Debug.Log($"Current element index = {tileListIndexPointer}");
                    Debug.Log($"Current step index = {step}");

                    if (tileList[tileListIndexPointer].TileType == tileList[step].TileType)
                    {
                        tileLength++;
                        if (tileLength == 2)
                        {
                            firstStepIndex = tileListIndexPointer;
                            Debug.Log($"Tile first Y = {tileList[tileListIndexPointer].TileTransform.localPosition.y}" +
                                $"Tile first X = {tileList[tileListIndexPointer].TileTransform.localPosition.x}" +
                            $"Tile type = {tileList[tileListIndexPointer].TileType}");
                        }

                        //if (tileLength >= 3 )
                        //{
                        //    Debug.Log($"New match!" +
                        //        $"Tile first X = {tileList[firstStepIndex].TileTransform.localPosition.x}" +
                        //        $"Tile first Y = {tileList[firstStepIndex].TileTransform.localPosition.y}" +
                        //    $"Tile type = {tileList[firstStepIndex].TileType}" +
                        //    $"Tile length = {tileLength}");
                        //    tileLength = 1;
                        //}

                    }
                    else
                    {
                        if (tileLength >= 3)
                        {
                            Debug.Log($"new match in else " +
                                $"Tile first X = {tileList[firstStepIndex].TileTransform.localPosition.x}" +
                                $"Tile first Y = {tileList[firstStepIndex].TileTransform.localPosition.y}" +
                            $"Tile type = {tileList[firstStepIndex].TileType}" +
                            $"Tile length = {tileLength}");
                        }
                        tileLength = 1;
                    }

                    tileListIndexPointer += rows;


                    Debug.Log($"Tile list index pointer = {tileListIndexPointer}");
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
                                //Debug.Log($"Detele this element X  = {a.TileTransform.localPosition.x}" +
                                //    $"Detele type element X  = {a}");
                                //a.gameObject.SetActive(false);
                                //a.TileType = null;
                                //a = null;
                                deletedTiles.Add(a);
                                //Destroy(a.gameObject);
                                //Debug.Log($"tile type after check = {a.TileType}");

                                //try
                                //{
                                //    Tile a = tileList[j + l];
                                //    //Debug.Log($"Detele this element X  = {a.TileTransform.localPosition.x}" +
                                //    //    $"Detele type element X  = {a}");
                                //    //a.gameObject.SetActive(false);
                                //    //a.TileType = null;
                                //    //a = null;
                                //    deletedTiles.Add(a);
                                //    //Destroy(a.gameObject);
                                //    //Debug.Log($"tile type after check = {a.TileType}");
                                //}
                                //catch
                                //{
                                //    //Debug.Log("Out of vertical range ");

                                //}

                            }
                            //break;
                        }
                    }

                }
            }
            foreach (var a in deletedTiles)
            {
                //Debug.Log($"Deleted tile X = {a.TileTransform.localPosition.x}" +
                //    $"Deleted tile Y = {a.TileTransform.localPosition.y}");
                a.TileType = null;
                Destroy(a.gameObject);
            }

            deletedTiles.Clear();
            _matches.Clear();
            //Debug.Log($"Matches count = {_matches.Count}");
            //FillGrid?.Invoke(tileList);
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
                                //Debug.Log($"Detele this element X  = {a.TileTransform.localPosition.x}" +
                                //    $"Detele type element X  = {a}");
                                //a.gameObject.SetActive(false);
                                //a.TileType = null;
                                //a = null;
                                deletedTiles.Add(a);
                                //Destroy(a.gameObject);
                                //Debug.Log($"tile type after check = {a.TileType}");

                                //try
                                //{
                                //    Tile a = tileList[j + l];
                                //    //Debug.Log($"Detele this element X  = {a.TileTransform.localPosition.x}" +
                                //    //    $"Detele type element X  = {a}");
                                //    //a.gameObject.SetActive(false);
                                //    //a.TileType = null;
                                //    //a = null;
                                //    deletedTiles.Add(a);
                                //    //Destroy(a.gameObject);
                                //    //Debug.Log($"tile type after check = {a.TileType}");
                                //}
                                //catch
                                //{
                                //    //Debug.Log("Out of vertical range ");

                                //}

                            }
                            //break;
                        }
                    }

                }
            }
            foreach (var a in deletedTiles)
            {
                //Debug.Log($"Deleted tile X = {a.TileTransform.localPosition.x}" +
                //    $"Deleted tile Y = {a.TileTransform.localPosition.y}");
                a.TileType = null;
                Destroy(a.gameObject);
            }

            deletedTiles.Clear();
            _matches.Clear();
            //Debug.Log($"Matches count = {_matches.Count}");
            FillGrid?.Invoke(tileList);
            //CheckGrid(tileList);
        }


    }
}