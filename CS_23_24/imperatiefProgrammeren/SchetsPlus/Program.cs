using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;

static class Program
{
    public static SchetsEditor se = new SchetsEditor();
    [STAThreadAttribute]
    static void Main()
    {
        Application.Run(se);
    }
}