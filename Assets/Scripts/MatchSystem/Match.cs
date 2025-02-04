namespace Match3
{
    public struct Match
    {
        private float _x;
        private float _y;

        internal   Tile _tile;

        internal int _length;
        internal bool _isHorizontal;

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