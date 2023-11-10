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
        ofd.Filter = "SchetsPlus XML|*.xml";

        if(ofd.ShowDialog() == DialogResult.OK)
        {
            if (ofd.FileName.EndsWith(".xml"))
                this.OpenXml(ofd.FileName);
            else
                MessageBox.Show("File could not be read. Please make sure a SchetsPlus XML file was selected.", "Error: File not read", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void OpenXml(string naam)
    {
        TekenElementMaster tem = new TekenElementMaster();
        TekenElement te = new TekenElement();
        XmlTextReader xtr = new XmlTextReader($"{naam}");
        try
        {
            xtr.Read();
            while (xtr.Read())
            {
                if (xtr.IsStartElement())
                {
                    switch (xtr.Name.ToString())
                    {
                        case "Element":
                            te = new TekenElement();
                            tem.TekenElementLijst.Add(te);
                            break;
                        case "Tool":
                            te.Tool = xtr.ReadString();
                            break;
                        case "Punt":
                            string[] xy = xtr.ReadString().Split(",");
                            int x = int.Parse(xy[0]);
                            int y = int.Parse(xy[1]);
                            Point p = new Point(x, y);
                            te.Punten.Add(p);
                            break;
                        case "Kleur":
                            string color = xtr.ReadString();
                            if (color.Contains("="))
                            {
                                string[] ls = color.Split("=");
                                te.Kleur = Color.FromArgb(int.Parse(ls[0]), int.Parse(ls[1]), int.Parse(ls[2]), int.Parse(ls[3]));
                            }
                            else
                                te.Kleur = Color.FromName(color);
                            break;
                        case "Letters":
                            te.Letters = xtr.ReadString();
                            break;
                        case "Hoek":
                            te.Hoek = int.Parse(xtr.ReadString());
                            break;
                    }
                }
            }
        }
        catch { MessageBox.Show("This file may be corrupt and can not be read correctly. Image may not be accurate. Please make sure you have selected a SchetsPlus XML file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        xtr.Close();
        SchetsWin s = new SchetsWin();
        s.Text = $"{naam}";
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