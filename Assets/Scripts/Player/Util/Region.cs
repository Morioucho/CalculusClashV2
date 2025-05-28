[System.Serializable]
public class Region {
    public int x1, x2, y1, y2;
    public int cameraX, cameraY;

    public Region(int x1, int x2, int y1, int y2, int cameraX, int cameraY) {
        this.x1 = x1;
        this.x2 = x2;
        this.y1 = y1;
        this.y2 = y2;

        this.cameraX = cameraX;
        this.cameraY = cameraY;
    }

    public bool Contains(float x, float y) {
        return x >= x1 && x <= x2 && y >= y1 && y <= y2;
    }
}