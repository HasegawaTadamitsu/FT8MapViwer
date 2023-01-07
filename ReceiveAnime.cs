
public class ReceiveAnime
{
    private GridLocator myGR;
    private GridLocator toGr;
    private const int MAX_COUNT = 50;
    private int counter = 0;
    private double location_Long = 0;
    private double location_Lat = 0;
    public ReceiveAnime(GridLocator my, GridLocator to)
    {
        this.myGR = my;
        this.toGr = to;
        counter = MAX_COUNT;

    }
    public double getLong()
    {
        return location_Long;

    }
    public double getLat()
    {
        return location_Lat;
    }

    public double[] getLongLat()
    {
        return (new double[] { location_Long, location_Lat });
    }
    public int doExec()
    {
        location_Long = (myGR.getLong() - toGr.getLong()) / (double)MAX_COUNT * (double)counter + toGr.getLong();
        location_Lat = (myGR.getLat() - toGr.getLat()) / (double)MAX_COUNT * (double)counter + toGr.getLat();
        counter--;
        return counter;
    }
}

