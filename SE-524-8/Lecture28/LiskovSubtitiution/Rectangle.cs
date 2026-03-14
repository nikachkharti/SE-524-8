namespace Lecture28.LiskovSubtitiution
{
    public interface IShape ///კონტრაქტი
    {
        int GetArea();
    }

    public class Rectangle : IShape
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int GetArea()
        {
            return Width * Height;
        }
    }

    public class Square : IShape
    {
        public int Side { get; set; }

        public int GetArea()
        {
            return Side * Side;
        }
    }


}
