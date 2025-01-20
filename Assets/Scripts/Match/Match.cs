namespace Match3
{
    public struct Match
    {
        public float _x;
        public float _y;

        public Tile _tile;

        public int _length;
        public bool _isHorizontal;

        public Match(Tile tile, float x, float y, int length, bool isHorizontal)
        {
            _tile = tile;
            _x = x;
            _y = y;
            this._length = length;
            this._isHorizontal = isHorizontal;
        }

        public bool IsValid()
        {
            return _tile != null; // Проверяем валидность структуры по полю _tile
        }
    }
}