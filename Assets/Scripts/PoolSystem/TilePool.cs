using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class TilePool : MonoBehaviour
    {
        [SerializeField] private TileFactory _factory;
        private List<Tile> _tileList = new List<Tile>();

        public Tile GetTileFromPool(int randomValue, Transform spawnTransform, int required, Transform parentTransform)
        {
            if (_tileList.Count == 0 || Random.value < 0.2f) // 20% шанс создать новый тайл
            {
                var tileInstance = _factory.CreateTile(randomValue, spawnTransform, parentTransform);
                return tileInstance;
            }

            ShuffleTileList();


            for (int i = 0; i < _tileList.Count; i++)
            {
                if (randomValue == i)
                {
                    var randomPoolTile = _tileList[randomValue];
                    randomPoolTile.gameObject.SetActive(true);
                    _tileList.Remove(randomPoolTile);
                    return randomPoolTile;
                }
            }

            ShuffleTileList();

            var defaultPoolTile = _tileList[0];
            defaultPoolTile.gameObject.SetActive(true);
            _tileList.Remove(defaultPoolTile);


            return defaultPoolTile;
        }

        public int GetTileListLenght()
        {
            return _tileList.Count;
        }
        private void ShuffleTileList()
        {
            for (int i = _tileList.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (_tileList[i], _tileList[j]) = (_tileList[j], _tileList[i]);
            }
        }
        public void ReturnToPool(Tile tile)
        {
            tile.TileTransform = null;
            _tileList.Add(tile);
            tile.gameObject.SetActive(false);
        }
    }
}
