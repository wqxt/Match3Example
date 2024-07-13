using UnityEngine;

namespace Match3
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Vector3 _position;

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }
    }
}