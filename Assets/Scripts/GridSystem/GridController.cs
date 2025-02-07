using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class GridController : MonoBehaviour
    {
        protected internal List<Cell> _cellList = new List<Cell>();
        [SerializeField] private Transform _startSpawnPosition;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private GridConfiguration _gameConfiguration;
        [SerializeField] private TileAnimationController _tileAnimationController;
        [SerializeField] public TileFactory _tilePoolFactory;
        [SerializeField] private TilePool _tilePool;
        public void SpawnCellGrid()
        {
            Debug.Log("Enter to the spawn cell grid");

            for (int x = 0; x < _gameConfiguration._columns; x++)
            {
                for (int y = 0; y < _gameConfiguration._rows; y++)
                {
                    Vector3 cellPosition = new Vector3(_startSpawnPosition.position.x + x, _startSpawnPosition.position.y + y, 0);
                    var cellPrefab = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, transform);
                    _cellList.Add(cellPrefab);
                }
            }
        }

        public IEnumerator SpawnTiles(List<Cell> cellList)
        {
            Debug.Log("Enter to the spawn tiles");

            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < _cellList.Count; i++)
            {
                if (_cellList[i].Tile == null)
                {
                    int tileValue = Random.Range(0, _tilePoolFactory.GetPullLeght());


                    var tile = _tilePool.GetTileFromPool(tileValue, _cellList[i].transform, _cellList.Count, transform);
                    Debug.LogWarning($"Current length = {_tilePool.GetTileListLenght()}");

                    // Проигрываем анимацию
                    yield return _tileAnimationController.PlaySpawnTileAnimation(tile, _cellList[i].transform);

                    _cellList[i].Tile = tile;
                    _cellList[i].Tile.TileTransform = _cellList[i].transform;
                }
            }
        }


        public IEnumerator DropTiles(List<Cell> cellList)
        {

            yield return new WaitForSeconds(1f);
            Debug.Log("Enter to the drop tiles");
            int cellListIndexPointer = 0;

            for (int x = 0; x < _gameConfiguration._columns; x++)
            {
                int nullCounter = 0;
                int firstNullIndex = 0;

                for (int y = 0; y < _gameConfiguration._rows; y++)
                {
                    if (cellList[cellListIndexPointer].Tile == null) // Пустая ячейка
                    {
                        if (nullCounter == 0)
                        {
                            firstNullIndex = cellListIndexPointer;
                        }
                        nullCounter++;
                    }
                    else if (nullCounter > 0) // Есть пустые ячейки ниже
                    {
                        cellList[firstNullIndex].Tile = cellList[cellListIndexPointer].Tile; // Перезаписываем тайл в клетке
                        cellList[firstNullIndex].Tile.TileTransform = _cellList[firstNullIndex].transform; // Перезаписываем трансформ клетки в тайл

                        // Проигрываем анимацию
                        yield return _tileAnimationController.PlayDropTileAnimation(cellList[firstNullIndex].Tile, _cellList[firstNullIndex].transform);

                        // Освобождаем текущую ячейку
                        cellList[cellListIndexPointer].Tile = null;
                        firstNullIndex++;
                    }

                    cellListIndexPointer++;
                }
            }
        }

        public IEnumerator DeleteTiles(List<Cell> cellList, int rows, int columns, List<Match> matches)
        {
            Debug.Log("Enter to the Delete tiles");
            yield return new WaitForSeconds(1f);


            HashSet<Tile> tilesToDelete = new HashSet<Tile>();
            HashSet<Cell> cellToRefresh = new HashSet<Cell>();


            for (int i = 0; i < matches.Count; i++)
            {
                for (int j = 0; j < cellList.Count; j++)
                {
                    if (cellList[j] != null && matches[i]._tile.TileTransform == cellList[j].Tile.TileTransform && matches[i]._tile != null) // Ищем совпадения по трансформу
                    {
                        if (matches[i]._isHorizontal)
                        {
                            for (int l = 0; l < matches[i]._length; l++) // По всей длине совпадения добавляем тайлы в список на удаление 
                            {
                                int index = j + rows * l; // горизонтальный шаш

                                if (index >= 0 && index < cellList.Count)
                                {
                                    Tile tile = cellList[index].Tile;

                                    if (tile.TileType != null)
                                    {
                                        tilesToDelete.Add(tile);
                                        cellToRefresh.Add(cellList[index]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int l = 0; l < matches[i]._length; l++)
                            {
                                int index = j + l; // Вертикальный шаг

                                if (index >= 0 && index < cellList.Count)
                                {
                                    Tile tile = cellList[index].Tile;
                                    if (tile.TileType != null)
                                    {
                                        tilesToDelete.Add(tile);
                                        cellToRefresh.Add(cellList[index]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Очищаем клетки от тайлов
            foreach (Cell cell in cellToRefresh)
            {
                cell.Tile = null;
            }

            // Добавляем тайлы в пул
            foreach (var a in tilesToDelete)
            {
                if (a != null && a.gameObject != null)
                {
                    yield return _tileAnimationController.PlayDeletedTileAnimation(a);
                    _tilePool.ReturnToPool(a);
                }
            }

            matches.Clear();
            tilesToDelete.Clear();
        }
    }
}