using UnityEngine;

namespace Match3
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Tile _tile;

        public Tile Tile
        {
            get { return _tile; }
            set { _tile = value; }
        }
    }
}