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

   

            _checkMatch.CheckMatches(_tileList);

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

        public IEnumerator DropTiles(List<Tile> tileList)
        {
            yield return new WaitForSeconds(2f);
            int tileListIndexPointer = 0;

            for (int x = 0; x < _gameConfiguration._columns; x++)
            {
                int nullCounter = 0;
                int firstNullIndex = 0;

                for (int y = 0; y < _gameConfiguration._rows; y++)
                {
                    if (tileList[tileListIndexPointer].TileType == null) // ѕуста€ €чейка
                    {
                        if (nullCounter == 0)
                        {
                            firstNullIndex = tileListIndexPointer;
                        }
                        nullCounter++;
                    }
                    else if (nullCounter > 0) // ≈сть пустые €чейки ниже
                    {
                        // ѕеремещаем тайл вниз
                        tileList[firstNullIndex] = tileList[tileListIndexPointer];
                        _cellList[firstNullIndex].Tile = _cellList[tileListIndexPointer].Tile;
              

                        // ќбновл€ем позицию тайла
                        _cellList[firstNullIndex].Tile.transform.position = _cellList[firstNullIndex].transform.position;
                        tileList[firstNullIndex].TileTransform = _cellList[firstNullIndex].transform;
                        // ќсвобождаем текущую €чейку
                        _cellList[tileListIndexPointer].Tile = null;
                        tileList[tileListIndexPointer] = null;

                        firstNullIndex++;
                    }

                    tileListIndexPointer++;
                }
            }
            StartCoroutine(FillEmptyCells(tileList));
        }


        public IEnumerator FillEmptyCells(List<Tile> tileList)
        {
            yield return new WaitForSeconds(1.5f);
            System.Random random = new System.Random();
            for (int i = 0; i < _cellList.Count; i++)
            {
                if (tileList[i] == null && _cellList[i].Tile == null)
                {
      
                    int tileValue = random.Next(_tilePull.Length);
                    Tile tile = Instantiate(_tilePull[tileValue], _cellList[i].transform.position, Quaternion.identity, transform);

                    // ”станавливаем новый тайл
                    tileList[i] = tile;
                    _cellList[i].Tile = tile;
                    _cellList[i].Tile.TileTransform = _cellList[i].transform;
                }
            }

            _checkMatch.CheckMatches(tileList);
        }


    }
}