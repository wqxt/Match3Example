using UnityEngine;

namespace Match3
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Transform _position;
        [SerializeField] private string _tileType;

        public string TileType
        {
            get { return _tileType; }
            set { _tileType = value; }
        }

        public Transform TileTransform
        {
            get { return _position; }
            set { _position = value; }
        }

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            SetType();
        }
        private void SetType() => _tileType = _sprite.name;
    }
}