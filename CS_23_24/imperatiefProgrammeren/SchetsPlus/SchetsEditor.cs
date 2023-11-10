using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

public class SchetsEditor : Form
{
    private MenuStrip menuStrip;
    // Nog meer standaard sourcecode methodes, variabelen en andere leuke dingen
    public SchetsEditor()
    {   
        this.ClientSize = new Size(800, 800);
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
        MessageBox.Show ( "Schets++ versie 1.01\n(c) UU Informatica && DVDP Prod. 2023"
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
        s.Show();
    }
    private void afsluiten(object sender, EventArgs e)
    {
        this.Close();
    }
    // Dan iets leuks: de XML reader
    private void open(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.InitialDirectory = "c:\\";
        ofd.RestoreDirectory = true;
        ofd.Filter = "SchetsPlus XML|*.xml";

        if(ofd.ShowDialog() == DialogResult.OK)
        {
            // Check of er een XML bestand geopend wordt
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
                        // Lees voor iedere regel wat er precies gebeurd: is er een tool,
                        // punt, kleur aanwezig? Bij een nieuw element wordt er ook een nieuw
                        // TekenElement aangemaakt, nadat het oude TekenElement toegevoegd is
                        // aan een nieuwe TEM lijst
                        case "Element":
                            te = new TekenElement();
                            tem.TekenElementLijst.Add(te);
                            break;
                        case "Tool":
                            te.Tool = xtr.ReadString();
                            break;
                        case "Punt":
                            // Parse alle coördinaten en zet ze terug in punten
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
                                // Als er een '=' aanwezig is, is er sprake van een kleur met ARGB waarde.
                                // Parse deze en haal de kleur er weer uit
                                string[] ls = color.Split("=");
                                te.Kleur = Color.FromArgb(int.Parse(ls[0]), int.Parse(ls[1]), int.Parse(ls[2]), int.Parse(ls[3]));
                            }
                            else // Als er geen sprake is van een ARGB waarde, is er sprake van een naam. 
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

        // Sluit de reader, zodat het document overschreven kan worden als dat nodig is
        xtr.Close();
        SchetsWin s = new SchetsWin();
        s.Text = $"{naam}";
        s.MdiParent = this;
        s.schetscontrol.schets.TemSchrijven(tem);
        s.schetscontrol.OpnieuwTekenen(tem.TekenElementLijst);
        s.Wijzig = false;
        s.Show();
    }
    // De functie die daadwerkelijk de 'wijzig' bool zet
    public void Gewijzigd()
    {
        SchetsWin activeChild = (SchetsWin)this.ActiveMdiChild;
        activeChild.Wijzig = true;
    }
}