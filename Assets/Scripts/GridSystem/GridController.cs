using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Match3
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Tile[] _tilePull;
        [SerializeField] protected internal List<Tile> _tileList = new List<Tile>();
        [SerializeField] protected internal List<Cell> _cellList = new List<Cell>();
        [SerializeField] private Transform _startSpawnPosition;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private GridConfiguration _gameConfiguration;
        [SerializeField] private TileAnimationController _tileAnimationController;

        public void SpawnCellGrid()
        {
            for (int x = 0; x < _gameConfiguration._columns; x++)
            {
                for (int y = 0; y < _gameConfiguration._rows; y++)
                {

                    Vector3 cellPosition = new Vector3(_startSpawnPosition.position.x + x, _startSpawnPosition.position.y + y, 0);
                    var cellPrefab = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, transform);
                    _cellList.Add(cellPrefab);
                    _tileList.Add(null); // ��������� 
                }
            }
        }

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

                    // ����������� ��������
                    yield return _tileAnimationController.PlaySpawnTileAnimation(tile, _cellList[i].transform);
                    // ������������� ����� ����
                    tileList[i] = tile;


                    _cellList[i].Tile = tile;
                    _cellList[i].Tile.TileTransform = _cellList[i].transform;
                }
            }
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
                    if (tileList[tileListIndexPointer].TileType == null) // ������ ������
                    {
                        if (nullCounter == 0)
                        {
                            firstNullIndex = tileListIndexPointer;
                        }
                        nullCounter++;
                    }
                    else if (nullCounter > 0) // ���� ������ ������ ����
                    {

                        tileList[firstNullIndex] = tileList[tileListIndexPointer]; // �������������� ������ �����
                        _cellList[firstNullIndex].Tile = _cellList[tileListIndexPointer].Tile; // �������������� ���� � ������� ������
                        tileList[firstNullIndex].TileTransform = _cellList[firstNullIndex].transform; // �������������� ���������

                        // ����������� ��������
                        yield return _tileAnimationController.PlayDropTileAnimation(tileList[firstNullIndex], _cellList[firstNullIndex].transform);


                        // ����������� ������� ������
                        _cellList[tileListIndexPointer].Tile = null;
                        tileList[tileListIndexPointer] = null;

                        firstNullIndex++;
                    }

                    tileListIndexPointer++;
                }
            }
        }

        public IEnumerator DeleteTiles(List<Tile> tileList, int rows, int columns, List<Match> _matches)
        {
            yield return new WaitForSeconds(1f);


            HashSet<Tile> tilesToDelete = new HashSet<Tile>();

            for (int i = 0; i < _matches.Count; i++)
            {
                for (int j = 0; j < tileList.Count; j++)
                {
                    if (tileList[j] != null && _matches[i]._tile.TileTransform == tileList[j].TileTransform && _matches[i]._tile != null)
                    {
                        if (_matches[i]._isHorizontal)
                        {
                            for (int l = 0; l < _matches[i]._length; l++)
                            {
                                int index = j + rows * l;
                                if (index >= 0 && index < tileList.Count)
                                {
                                    Tile a = tileList[index];

                                    if (a.TileType != null)
                                    {

                                        //��������� ����� ��� �������� 
                                        tilesToDelete.Add(a);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int l = 0; l < _matches[i]._length; l++)
                            {
                                if (j + l >= 0 && j + l < tileList.Count)
                                {
                                    Tile a = tileList[j + l];
                                    if (a.TileType != null)
                                    {
                                        //��������� ����� ��� �������� 
                                        tilesToDelete.Add(a);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // ������� �����
            foreach (var a in tilesToDelete)
            {
                if (a != null && a.gameObject != null)
                {

                    yield return _tileAnimationController.PlayDeletedTileAnimation(a);

                    a.TileType = null;
                    Destroy(a.gameObject);
                }
            }
            tilesToDelete.Clear();
        }
    }
}