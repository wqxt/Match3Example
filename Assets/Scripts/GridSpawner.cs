using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class GridSpawner : MonoBehaviour
    {
        [SerializeField] private Tile[] _tilePull;
        [SerializeField] private List<Tile> _tilesGrid;
        [SerializeField] protected internal List<Cell> _cellGrid;

        [SerializeField] private Transform _spawnStartPosition;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private int _columns = 8;
        [SerializeField] private int _rows = 6;
        [SerializeField] private float _animationDuration = 0.05f;

        private void Start()
        {
            _cellGrid = new List<Cell>();
            SpawnGrid();
        }

        private void SpawnGrid()
        {
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    Vector3 position = new Vector3(_spawnStartPosition.position.x + x, _spawnStartPosition.position.y + y, 0);
                    var cellPrefab = Instantiate(_cellPrefab, position, Quaternion.identity, transform);
                    _cellGrid.Add(cellPrefab);
                }
            }
            StartCoroutine(SpawnTile(_cellGrid));
        }

        private IEnumerator SpawnTile(List<Cell> cellGrid)
        {
            for (int i = 0; i < cellGrid.Count; i++)
            {
                if (cellGrid[i].Tile == null)
                {
                    System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
                    var tileValue = random.Next(_tilePull.Length);

                    var tile = Instantiate(_tilePull[tileValue], cellGrid[i].transform.position, Quaternion.identity, cellGrid[i].transform);
                    _tilesGrid.Add(tile);
                    PlayTileAnimation(tile, cellGrid[i].transform);

                    cellGrid[i].Tile = tile;
                    cellGrid[i].Tile.Position = cellGrid[i].transform.position;

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
    }
}