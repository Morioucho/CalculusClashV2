public class Position {
    public float X { get; }
    public float Y { get; }

    public Position(float x, float y) {
        this.X = x;
        this.Y = y;
    }

    public Position(Position position) {
        this.X = position.X;
        this.Y = position.Y;
    }
}