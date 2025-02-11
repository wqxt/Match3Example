using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class GridController : MonoBehaviour
    {
        protected internal List<Cell> _cellList = new List<Cell>();
        protected internal Cell[,] _cellGrid = new Cell[,] { };
        [SerializeField] private Transform _startSpawnPosition;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private GridConfiguration _gridConfig;
        [SerializeField] private TileAnimationController _tileAnimationController;
        [SerializeField] public TileFactory _tilePoolFactory;
        [SerializeField] private TilePool _tilePool;


        public void SpawnCellGrid()
        {
            _cellGrid = new Cell[_gridConfig._rows, _gridConfig._columns];  

            for (int x = 0; x < _gridConfig._columns; x++)
            {
                for (int y = 0; y < _gridConfig._rows; y++)
                {
                    if (_gridConfig._gridMask[y, x] == 1)
                    {
                        Vector3 cellPosition = new Vector3(_startSpawnPosition.position.x + x, _startSpawnPosition.position.y - y, 0);
                        var cellPrefab = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, transform);

          
                        _cellGrid[y, x] = cellPrefab;
                    }
                }
            }
        }


        public IEnumerator SpawnTiles(Cell[,] cellGrid)
        {
            Debug.Log("Enter to the spawn tiles");

            yield return new WaitForSeconds(1.5f);

            for (int x = 0; x < _gridConfig._columns; x++)
            {
                for (int y = 0; y < _gridConfig._rows; y++)
                {
                    // Get current cell
                    Cell currentCell = cellGrid[y, x];

                    
                    if (currentCell != null && currentCell.Tile == null)
                    {
                        // get random tile
                        int tileValue = Random.Range(0, _tilePoolFactory.GetPullLeght());

                        // Проверка на валидность пула тайлов
                        if (_tilePool == null)
                        {
                            Debug.LogError("Tile pool is null!");
                            yield break;
                        }

                        // get tile from pull
                        var tile = _tilePool.GetTileFromPool(tileValue, currentCell.transform, _cellList.Count, transform);

                        // log current tile pull length
                        Debug.LogWarning($"Current length = {_tilePool.GetTileListLenght()}");

                
                        yield return _tileAnimationController.PlaySpawnTileAnimation(tile, currentCell.transform);

                   
                        currentCell.Tile = tile;
                        currentCell.Tile.TileTransform = currentCell.transform;
                    }
                    else
                    {
               
                        Debug.Log($"Cell at [{y}, {x}] already has a tile or is null.");
                    }
                }
            }
        }




        public IEnumerator DropTiles(List<Cell> cellList)
        {

            yield return new WaitForSeconds(1f);
            Debug.Log("Enter to the drop tiles");
            int cellListIndexPointer = 0;

            for (int x = 0; x < _gridConfig._columns; x++)
            {
                int nullCounter = 0;
                int firstNullIndex = 0;

                for (int y = 0; y < _gridConfig._rows; y++)
                {
                    if (cellList[cellListIndexPointer].Tile == null)
                    {
                        if (nullCounter == 0)
                        {
                            firstNullIndex = cellListIndexPointer;
                        }
                        nullCounter++;
                    }
                    else if (nullCounter > 0) // if emptu cell is down
                    {
                        cellList[firstNullIndex].Tile = cellList[cellListIndexPointer].Tile; // overwriting tile
                        cellList[firstNullIndex].Tile.TileTransform = _cellList[firstNullIndex].transform; // overwriting tile transform

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
                    if (cellList[j] != null && matches[i]._tile.TileTransform == cellList[j].Tile.TileTransform && matches[i]._tile != null)// Looking for a match on the transform
                    {
                        if (matches[i]._isHorizontal)
                        {
                            for (int l = 0; l < matches[i]._length; l++) // Adding tiles to the list for deletion along the entire length of the match
                            {
                                int index = j + rows * l; // horizontal step

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
                                int index = j + l; // vertical step 

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