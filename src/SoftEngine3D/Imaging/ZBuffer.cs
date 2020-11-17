namespace SoftEngine3D.Imaging
{
    public class ZBuffer
    {
        private readonly float[,] buffer;

        public ZBuffer(int width, int height)
        {
            this.buffer = new float[width, height];
            for (var i = 0; i < this.buffer.GetLength(0); i++)
            {
                for (var j = 0; j < this.buffer.GetLength(1); ++j)
                {
                    this.buffer[i, j] = float.MaxValue;
                }
            }
        }


        public float this[int pointX, int pointY]
        {
            get { return this.buffer[pointX, pointY]; }
            set { this.buffer[pointX, pointY] = value; }
        }
    }
}