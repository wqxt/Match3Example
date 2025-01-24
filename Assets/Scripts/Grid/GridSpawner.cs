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
        [SerializeField] protected internal List<Tile> _tileList;
        [SerializeField] protected internal List<Cell> _cellList = new List<Cell>();
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _spawnStartPosition;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private float _animationDuration = 0.05f;
        [SerializeField] private GameConfiguration _gameConfiguration;


        public void SpawnCellGrid()
        {
            for (int x = 0; x < _gameConfiguration._columns; x++)
            {
                for (int y = 0; y < _gameConfiguration._rows; y++)
                {

                    Vector3 cellPosition = new Vector3(_spawnStartPosition.position.x + x, _spawnStartPosition.position.y + y, 0);
                    var cellPrefab = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, transform);
                    _cellList.Add(cellPrefab);
                    _tileList.Add(null); // добавляем 
                }
            }
        }


        //public void SpawnTiles(List<Cell> cellGrid)
        //{
        //    System.Random random1 = new System.Random();
        //    for (int i = 0; i < cellGrid.Count; i++)
        //    {
        //        if (cellGrid[i].Tile == null)
        //        {

        //            var tileValue = random1.Next(_tilePull.Length);
        //            Tile tile = Instantiate(_tilePull[tileValue], cellGrid[i].transform.position, Quaternion.identity, transform);//, cellGrid[i].transform);

        //            _tileList.Add(tile);
        //            //PlayTileAnimation(tile, cellGrid[i].transform);

        //            cellGrid[i].Tile = tile;
        //            cellGrid[i].Tile.TileTransform = cellGrid[i].transform;

        //        }
        //    }
        //}

        public IEnumerator SpawnTiles(List<Tile> tileList)
        {
            yield return new WaitForSeconds(1.5f);
            System.Random random = new System.Random();
            for (int i = 0; i < _cellList.Count; i++)
            {
                if (tileList[i] == null && _cellList[i].Tile == null)
                {

                    int tileValue = random.Next(_tilePull.Length);
                    Tile tile = Instantiate(_tilePull[tileValue], _cellList[i].transform.position, Quaternion.identity, transform);
                    PlaySpawnTileAnimation(tile, _cellList[i].transform);
                    // Устанавливаем новый тайл
                    tileList[i] = tile;


                    _cellList[i].Tile = tile;
                    _cellList[i].Tile.TileTransform = _cellList[i].transform;
                }
            }
            yield return new WaitForSeconds(1f);
        }

        private void PlaySpawnTileAnimation(Tile tile, Transform targetTransform)
        {
            tile.transform.localScale = new Vector3(0, 0);
            tile.transform.position = new Vector3(targetTransform.position.x, 6);

            tile.transform
                .DOScale(0.5f, 0.2f)
                .SetEase(Ease.InOutQuad);

            tile.transform
                .DOMove(targetTransform.position, 0.5f)
                .SetEase(Ease.InOutQuad)
                .WaitForCompletion();

        }

        private void PlayDropTileAnimation(Tile tile, Transform targetTransform)
        {


            tile.transform
                .DOMove(targetTransform.position, 0.5f)
                .SetEase(Ease.InOutQuad)
                .WaitForCompletion();
        }

        public void PlayDeletedTileAnimation(Tile tile)
        {
            Debug.Log("Enter to the delete animation");

            tile.transform
                .DOScale(0f, 0.2f)
                .SetEase(Ease.InOutQuad)
                .OnComplete( () => Destroy(tile.gameObject));
        }

        public IEnumerator DropTiles(List<Tile> tileList)
        {
            yield return new WaitForSeconds(1f);

            int tileListIndexPointer = 0;

            for (int x = 0; x < _gameConfiguration._columns; x++)
            {
                int nullCounter = 0;
                int firstNullIndex = 0;

                for (int y = 0; y < _gameConfiguration._rows; y++)
                {
                    if (tileList[tileListIndexPointer].TileType == null) // Пустая ячейка
                    {
                        if (nullCounter == 0)
                        {
                            firstNullIndex = tileListIndexPointer;
                        }
                        nullCounter++;
                    }
                    else if (nullCounter > 0) // Есть пустые ячейки ниже
                    {

                        tileList[firstNullIndex] = tileList[tileListIndexPointer]; // Перезаписываем ссылку тайла
                        _cellList[firstNullIndex].Tile = _cellList[tileListIndexPointer].Tile; // Перезаписываем тайл в таблице клеток
                        tileList[firstNullIndex].TileTransform = _cellList[firstNullIndex].transform; // Перезаписываем трансформ

                        // Проигрываем анимацию
                        PlayDropTileAnimation(tileList[firstNullIndex], _cellList[firstNullIndex].transform);

                        // Освобождаем текущую ячейку
                        _cellList[tileListIndexPointer].Tile = null;
                        tileList[tileListIndexPointer] = null;

                        firstNullIndex++;
                    }

                    tileListIndexPointer++;
                }
            }
        }
    }
}