public class CoordinateSpace
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Depth { get; private set; }

    public CoordinateSpace(int x, int y) : this(x, y, 0) { }

    public CoordinateSpace(int x, int y, int z)
    {
        Width = x;
        Height = y;
        Depth = z;
    }

    public int GetSize()
    {
        int total = Width;
        if (Height != 0) total *= Height;
        if (Depth != 0) total *= Depth;
        return total;

    }

}