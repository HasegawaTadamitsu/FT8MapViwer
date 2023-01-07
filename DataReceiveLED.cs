public class DataReceiveLED
{
    private int counter = 0;
    private Boolean lightingFlg = false;

    public delegate void labelLEDDelegater(Boolean arg);
    public labelLEDDelegater setLEDLabel;

    public DataReceiveLED()
    {
    }
    public void doStop()
    {
        notReceiveing();
        counter = 0;
    }
    private void nowReceiveing()
    {
        setLEDLabel(true);
        lightingFlg = true;
    }
    private void notReceiveing()
    {
        setLEDLabel(false);
        lightingFlg = false;

    }
    public void doReceive()
    {
        notReceiveing();
        counter = 2;
    }

    public void doExec()
    {
        if (counter <= 0 && lightingFlg == true)
        {
            notReceiveing();
            counter = 0;
            return;
        }
        if (counter <= 0)
        {
            counter = 0;
            return;
        }

        counter--;
        if (counter == 0)
        {
            if (lightingFlg)
                notReceiveing();
            lightingFlg = false;
            return;
        }
        if (!lightingFlg) nowReceiveing();
        return;
    }

}
