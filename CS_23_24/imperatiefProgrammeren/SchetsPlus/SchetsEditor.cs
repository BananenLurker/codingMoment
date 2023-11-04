using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

public class SchetsEditor : Form
{
    public List<SchetsWin> windows = new List<SchetsWin>();
    private MenuStrip menuStrip;

    public SchetsEditor()
    {   
        this.ClientSize = new Size(800, 600);
        menuStrip = new MenuStrip();
        this.Controls.Add(menuStrip);
        this.maakFileMenu();
        this.maakHelpMenu();
        this.Text = "Schets editor";
        this.IsMdiContainer = true;
        this.MainMenuStrip = menuStrip;
    }
    private void maakFileMenu()
    {   
        ToolStripDropDownItem menu = new ToolStripMenuItem("File");
        menu.DropDownItems.Add("Nieuw", null, this.nieuw);
        menu.DropDownItems.Add("Exit", null, this.afsluiten);
        menu.DropDownItems.Add("Open", null, this.open);
        menuStrip.Items.Add(menu);
    }
    private void maakHelpMenu()
    {   
        ToolStripDropDownItem menu = new ToolStripMenuItem("Help");
        menu.DropDownItems.Add("Over \"Schets\"", null, this.about);
        menuStrip.Items.Add(menu);
    }
    private void about(object o, EventArgs ea)
    {   
        MessageBox.Show ( "Schets versie 2.0\n(c) UU Informatica 2022"
                        , "Over \"Schets\""
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Information
                        );
    }

    private void nieuw(object sender, EventArgs e)
    {   
        SchetsWin s = new SchetsWin();
        s.MdiParent = this;
        s.Wijzig = false;
        windows.Add(s);
        s.Show();
    }
    private void afsluiten(object sender, EventArgs e)
    {
        this.Close();
    }
    private void open(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.InitialDirectory = "c:\\";
        ofd.RestoreDirectory = true;
        ofd.Filter = "png files (*.png)|*.png|jpg files(*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|xml files (*.xml)|*.xml|All files (*.*)|*.*";

        if(ofd.ShowDialog() == DialogResult.OK)
        {
            this.lees(ofd.FileName);
        }
    }
    private void lees(string naam)
    {
        if (naam.EndsWith("xml"))
        {
            OpenXml(naam);
        }
        else
        {
            SchetsWin sw = new SchetsWin();
            Bitmap bmp = new Bitmap(naam);
            PictureBox pb = new PictureBox();

            pb.Image = bmp;
            sw.MdiParent = this;
            sw.Show();
        }
    }
    private void OpenXml(string naam)
    {
        XmlTextReader textReader = new XmlTextReader($"{naam}");
        textReader.Read();
        while (textReader.Read())
        {
            textReader.MoveToElement();
            Debug.WriteLine("XmlTextReader properties list");
            Debug.WriteLine("================================");
            Debug.WriteLine("Name:" + textReader.Name);
            Debug.WriteLine("Base URI:" + textReader.BaseURI);
            Debug.WriteLine("Local Name:" + textReader.LocalName);
            Debug.WriteLine("Attribute Count:" + textReader.AttributeCount.ToString());
            Debug.WriteLine("Depth:" + textReader.Depth.ToString());
            Debug.WriteLine("Line Number:" + textReader.LineNumber.ToString());
            Debug.WriteLine("Node Type:" + textReader.NodeType.ToString());
            Debug.WriteLine("Attribute Count:" + textReader.Value.ToString());
        }
    }
    public void Gewijzigd()
    {
        SchetsWin activeChild = (SchetsWin)this.ActiveMdiChild;
        activeChild.Wijzig = true;
    }
}