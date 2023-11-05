using System;
using System.Collections.Generic;
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
        ofd.Filter = "xml files (*.xml)|*.xml|png files (*.png)|*.png|jpg files (*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";

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
        TekenElementMaster tem = new TekenElementMaster();
        TekenElement te = new TekenElement();
        XmlTextReader tr = new XmlTextReader($"{naam}");
        tr.Read();
        while (tr.Read())
        {
            if (tr.IsStartElement())
            {
                switch (tr.Name.ToString())
                {
                    case "Element":
                        te = new TekenElement();
                        tem.TekenElementLijst.Add(te);
                        break;
                    case "Tool":
                        te.Tool = tr.ReadString();
                        break;
                    case "Punt":
                        string[] xy = tr.ReadString().Split(",");
                        int x = 0; int y = 0;
                        try
                        {
                            x = int.Parse(xy[0]);
                            y = int.Parse(xy[1]);
                        }
                        catch
                        {
                            MessageBox.Show("This file appears to be corrupt. Image may not be accurate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Point p = new Point(x, y);
                        te.Punten.Add(p);
                        break;
                    case "Kleur":
                        string color = tr.ReadString();
                        if (color.Contains("="))
                        {
                            string[] ls = color.Split("=");
                            try
                            {
                                te.Kleur = Color.FromArgb(int.Parse(ls[0]), int.Parse(ls[1]), int.Parse(ls[2]), int.Parse(ls[3]));
                            }
                            catch
                            {
                                MessageBox.Show("This file appears to be corrupt. Image may not be accurate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            te.Kleur = Color.FromName(color);
                        }
                        break;
                    case "Letters":
                        te.Letters = tr.ReadString();
                        break;
                    case "Hoek":
                        try
                        {
                            te.Hoek = int.Parse(tr.ReadString());
                        }
                        catch
                        {
                            MessageBox.Show("This file appears to be corrupt. Image may not be accurate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                }
            }
        }
        SchetsWin s = new SchetsWin();
        s.MdiParent = this;
        s.schetscontrol.schets.temSchrijven(tem);
        s.schetscontrol.OpnieuwTekenen(tem.TekenElementLijst);
        s.Wijzig = false;
        windows.Add(s);
        s.Show();
    }
    public void Gewijzigd()
    {
        SchetsWin activeChild = (SchetsWin)this.ActiveMdiChild;
        activeChild.Wijzig = true;
    }
}