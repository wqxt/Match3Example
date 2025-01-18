using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Match3
{
    public class GridSpawner : MonoBehaviour
    {
        [SerializeField] private Tile[] _tilePull;
        [SerializeField] private List<Tile> _tileList;
        [SerializeField] protected internal List<Cell> _cellList;
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _spawnStartPosition;
        [SerializeField] private Cell _cellPrefab;

        [SerializeField] private float _animationDuration = 0.05f;

        [SerializeField] private GameConfiguration _gameConfiguration;

        public CheckMatch _checkMatch;

        public TileSwapper _tileSwapper;
        private void OnEnable()
        {

            //_checkMatch.FillGrid += FillRandomGrid;
            //_checkMatch.FillGrid += FillFallGrid;
        }

        private void OnDisable()
        {
            //_tileSwapper.Swap -= FillFallGrid;
            //_checkMatch.FillGrid -= FillRandomGrid;
            //_checkMatch.FillGrid -= FillFallGrid;
        }
        private void Start()
        {
            _cellList = new List<Cell>();
            _tileSwapper._tileList = _tileList;
            _tileSwapper._cellList = _cellList;

            SpawnCellGrid();
            SpawnTiles(_cellList);


            _checkMatch.CheckGrid(_tileList);

        }

        private void SpawnCellGrid()
        {
            for (int x = 0; x < _gameConfiguration._columns; x++)
            {
                for (int y = 0; y < _gameConfiguration._rows; y++)
                {

                    Vector3 cellPosition = new Vector3(_spawnStartPosition.position.x + x, _spawnStartPosition.position.y + y, 0);
                    var cellPrefab = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, transform);
                    _cellList.Add(cellPrefab);
                }
            }
            //StartCoroutine(SpawnTile(_cellGrid));



        }


        public void SpawnTiles(List<Cell> cellGrid)
        {
            System.Random random1 = new System.Random();
            for (int i = 0; i < cellGrid.Count; i++)
            {
                if (cellGrid[i].Tile == null)
                {

                    var tileValue = random1.Next(_tilePull.Length);
                    Tile tile = Instantiate(_tilePull[tileValue], cellGrid[i].transform.position, Quaternion.identity, transform);//, cellGrid[i].transform);

                    _tileList.Add(tile);
                    //PlayTileAnimation(tile, cellGrid[i].transform);

                    cellGrid[i].Tile = tile;
                    cellGrid[i].Tile.TileTransform = cellGrid[i].transform;

                }
            }
        }

        private IEnumerator SpawnTile(List<Cell> cellGrid)
        {
            for (int i = 0; i < cellGrid.Count; i++)
            {
                if (cellGrid[i].Tile == null)
                {
                    System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
                    var tileValue = random.Next(_tilePull.Length);

                    var tile = Instantiate(_tilePull[tileValue], cellGrid[i].transform.position, Quaternion.identity);//, cellGrid[i].transform);
                    _tileList.Add(tile);
                    PlayTileAnimation(tile, cellGrid[i].transform);

                    cellGrid[i].Tile = tile;
                    cellGrid[i].Tile.TileTransform = cellGrid[i].transform;

                    yield return new WaitForSeconds(_animationDuration);
                }
            }
        }

        private void PlayTileAnimation(Tile tile, Transform transform)
        {
            tile.transform.localScale = new Vector3(0, 0);
            tile.transform.position = new Vector3(transform.position.x, 4);

            tile.transform
                .DOScale(0.5f, 0.2f)
                .SetEase(Ease.InOutQuad);

            tile.transform
                .DOMove(transform.position, 0.2f)
                .SetEase(Ease.InOutQuad);
        }

        public void FillRandomGrid(List<Tile> tileList)
        {
            //Debug.Log("Enter in the fill random grid");
            for (int i = 0; i < tileList.Count; i++)
            {
                if (tileList[i] == null)
                {
                    System.Random random1 = new System.Random();
                    var tileValue = random1.Next(_tilePull.Length);
                    Tile tile = Instantiate(_tilePull[tileValue], _cellList[i].transform.position, Quaternion.identity);

                    _cellList[i].Tile = tile;
                    _cellList[i].Tile.TileTransform = _cellList[i].transform;

                    tileList[i] = tile;
                    _cellList[i].Tile = tile;
                    //_tilesGrid.Add(tile);
                }
            }
        }

        public void FillFallGrid(List<Tile> tileList)
        {
            int tileListIndexPointer = 0;

            // Проходим по каждому столбцу
            for (int x = 0; x < _gameConfiguration._columns; x++)
            {
                int nullCounter = 0; // Счётчик пустых ячеек
                int firstNullIndex = 0;

                // Проходим по строкам в текущем столбце
                for (int y = 0; y < _gameConfiguration._rows; y++)
                {
                    if (tileList[tileListIndexPointer].TileType == null) // Если ячейка пуста
                    {
                        if (nullCounter == 0)
                        {
                            firstNullIndex = tileListIndexPointer; // Запоминаем первый пустой индекс
                        }

                        nullCounter++;
                    }
                    else if (nullCounter > 0) // Если есть пустые ячейки ниже
                    {
                        // Перемещаем текущий тайл вниз
                        tileList[firstNullIndex] = tileList[tileListIndexPointer];
                        _cellList[firstNullIndex].Tile = _cellList[tileListIndexPointer].Tile;

                        // Обновляем позицию тайла
                        _cellList[firstNullIndex].Tile.transform.position = _cellList[firstNullIndex].transform.position;

                        // Освобождаем текущую ячейку
                        _cellList[tileListIndexPointer].Tile = null;
                        tileList[tileListIndexPointer] = null;

                        firstNullIndex++; // Сдвигаем индекс для следующего пустого места
                    }

                    tileListIndexPointer++; // Переход к следующей ячейке
                }
            }

            // Заполняем оставшиеся пустые ячейки новыми тайлами
            for (int i = 0; i < _cellList.Count; i++)
            {
                if (tileList[i] == null && _cellList[i].Tile == null)
                {
                    System.Random random = new System.Random();
                    int tileValue = random.Next(_tilePull.Length);
                    Tile tile = Instantiate(_tilePull[tileValue], _cellList[i].transform.position, Quaternion.identity, transform);

                    // Устанавливаем новый тайл
                    tileList[i] = tile;
                    _cellList[i].Tile = tile;
                    _cellList[i].Tile.TileTransform = _cellList[i].transform;
                }
            }

            //_checkMatch.CheckGrid(tileList);
        }




        public IEnumerator StartFill(List<Tile> tileList)
        {
            yield return new WaitForSeconds(1f);
            int tileListIndexPointer = 0;



            for (int x = 0; x < _gameConfiguration._columns; x++)
            {

                int nullCounter = 0;
                int firstNullIndex = 0;

                for (int y = 0; y < _gameConfiguration._rows; y++)
                {

                    if (tileList[tileListIndexPointer].TileType == null)
                    {
                        if (nullCounter == 0)
                        {
                            //Debug.Log($"current columns = {y}");
                            firstNullIndex = tileListIndexPointer;
                            //Debug.Log($"First null index = {firstNullIndex}");

                        }
                        //deletedTiles.Add(tileList[tileListIndexPointer]);
                        nullCounter++;

                    }
                    else
                    {
                        //Debug.Log("Enter in the swap else block");
                        //Debug.Log($"null counter = {nullCounter}");
                        if (nullCounter != 0 && tileList[tileListIndexPointer].TileType != null)
                        {
                            //Debug.Log("Enter in the swap");

                            tileList[firstNullIndex] = tileList[tileListIndexPointer];


                            _cellList[tileListIndexPointer].Tile.transform.position = _cellList[firstNullIndex].transform.position;
                            _cellList[firstNullIndex].Tile = _cellList[tileListIndexPointer].Tile;
                            //_cellList[firstNullIndex].Tile.TileTransform = _cellList[tileListIndexPointer].Tile.TileTransform;

                            _cellList[tileListIndexPointer].Tile = null;

                            tileList[tileListIndexPointer] = null;

                            firstNullIndex++;

                        }

                        //nullCounter--;


                    }

                    tileListIndexPointer++;
                }
            }


            for (int i = 0; i < _cellList.Count; i++)
            {

                if (tileList[i] == null && _cellList[i].Tile == null)
                {
                    //Debug.Log("Enter in the fill  grid");
                    System.Random random1 = new System.Random();
                    var tileValue = random1.Next(_tilePull.Length);
                    Tile tile = Instantiate(_tilePull[tileValue], _cellList[i].transform.position, Quaternion.identity, transform);//, _cellList[i].transform);
                    tileList[i] = tile;
                    _cellList[i].Tile = tile;
                    _cellList[i].Tile.TileTransform = _cellList[i].transform;

                    //_cellList[i].Tile = tile;

                }
            }

        }


    }
}