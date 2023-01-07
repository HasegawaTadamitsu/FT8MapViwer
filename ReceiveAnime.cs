
public class ReceiveAnime
{
    private GridLocator myGL;
    private GridLocator toGL;
    private const int MAX_COUNT = 50;
    private int counter = 0;
    private double location_Long = 0;
    private double location_Lat = 0;
    public ReceiveAnime(GridLocator my, GridLocator to)
    {
        this.myGL = my;
        this.toGL = to;
        counter = MAX_COUNT;
   }
    public double[] getToLongLat()
    {
        return (new double[] { location_Long, location_Lat });
    }
    public double[] getStartLongLat()
    {
        return (new double[] { toGL.getLong(),toGL.getLat() });
    }
    public int doExec()
    {
        location_Long = (myGL.getLong() - toGL.getLong()) / (double)MAX_COUNT * (double)(MAX_COUNT-counter) + toGL.getLong();
        location_Lat = (myGL.getLat() - toGL.getLat()) / (double)MAX_COUNT * (double)(MAX_COUNT - counter) + toGL.getLat();
        counter--;
        return counter;
    }
}

