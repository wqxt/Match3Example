using UnityEngine;

namespace Match3
{
    public class TileFactory : MonoBehaviour
    {
        [SerializeField] private Tile[] _tilePull;

        public int GetPullLeght()
        {
            return _tilePull.Length;
        }


        public Tile CreateTile(int randomValue, Transform spawnTransform, Transform parentTransform) 
        {
            if (randomValue < 0 || randomValue >= _tilePull.Length)
            {
                Debug.LogError("random value is out of the array range!");
                return null;
            }

            var tileInstance = Instantiate(_tilePull[randomValue], spawnTransform.position, Quaternion.identity, parentTransform);
            return tileInstance;
        }
    }
}