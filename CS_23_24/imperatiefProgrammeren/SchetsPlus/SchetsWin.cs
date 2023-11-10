using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

public class SchetsWin : Form
{
    // Standaardvariabelen en methoden die in de source code staan.
    // Wel is public bool wijzig toegevoegd, die voor ieder SchetsWin bijhoudt
    // of er sprake is van een wijziging. Zo ja, zal de Opslaan() methode
    // hier rekening mee houden
    public bool wijzig;
    MenuStrip menuStrip;
    public SchetsControl schetscontrol;
    ISchetsTool huidigeTool;
    Panel paneel;
    bool vast;

    private void veranderAfmeting(object o, EventArgs ea)
    {
        schetscontrol.Size = new Size ( this.ClientSize.Width  - 70
                                      , this.ClientSize.Height - 50);
        paneel.Location = new Point(64, this.ClientSize.Height - 30);
    }

    private void klikToolMenu(object obj, EventArgs ea)
    {
        this.huidigeTool = (ISchetsTool)((ToolStripMenuItem)obj).Tag;
    }

    private void klikToolButton(object obj, EventArgs ea)
    {
        this.huidigeTool = (ISchetsTool)((RadioButton)obj).Tag;
    }

    private void afsluiten(object sender, FormClosingEventArgs fcea)
    {
        // Zorg dat this.Close() zijn normale functionaliteit terug krijgt gedurende deze methode
        this.FormClosing -= this.afsluiten;
        if (Wijzig == true)
        {
            DialogResult dr = MessageBox.Show("You have unsaved changes, save before quitting?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                // Als er wordt opgeslagen voor het afsluiten, handelt methode Opslaan() dat af
                Opslaan(null, null);
                Wijzig = false;
            }
            else if (dr == DialogResult.No)
            {
                // Als er niet wordt opgeslagen voor het afsluiten, mag het scherm weg
                this.Close();
            }
            else
            {
                if(fcea != null)
                {
                    // Als er wordt gecancelled, mag het scherm niet weg, maar moet het afsluiten wel stoppen.
                    // FormClosing krijgt zijn oude EventHandler weer en wordt gecancelled
                    this.FormClosing += this.afsluiten;
                    fcea.Cancel = true;
                    return;
                }
            }
        }
        else
        {
            // Als er geen wijzigingen zijn, mag het scherm sowieso dicht
            this.Close();
        }
        // Voeg de EventHandler weer toe
        this.FormClosing += this.afsluiten;
    }
    private void SluitenButton_Click(object obj, EventArgs ea)
    {
        afsluiten(this, null);
    }
    private void Opslaan(object obj, EventArgs ea)
    {
        // Als er nog geen filenaam is, is het een nieuw bestand en mag
        // OpslaanAls() aangeroepen worden
        string fileNaam = this.Text;
        if(fileNaam == "")
        {
            OpslaanAls(this, null);
        }
        else
        {
            // Als er al wel een naam is, bestaat het bestand al
            // en hoeft het alleen maar overschreven te worden
            if (fileNaam.EndsWith(".xml"))
                SchrijfXml(fileNaam);
            else
                SaveBitmap(fileNaam, false);
        }
    }
    private void OpslaanAls(object obj, EventArgs ea)
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "xml files (.xml)|*.xml|png files (*.png)|*.png|jpg files(*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            if (sfd.FileName.EndsWith("xml"))
            {
                // De filenaam eindigt op xml, dus wordt de xml methode aangeroepen
                SchrijfXml(sfd.FileName);
            }
            else
            {
                // De filenaam eindigt niet op xml, dus wordt als image
                // opgeslagen. De bool 'controle' wordt meegegeven om bij te houden
                // of er afgesloten moet worden na het opslaan van de afbeelding.
                bool controle = false;
                if (obj == null)
                    controle = true;
                SaveBitmap(sfd.FileName, controle);
            }
        }
    }
    private void SchrijfXml(string naam)
    {
        // De structuur van een xml document opbouwen...
        XmlTextWriter tw = new XmlTextWriter(naam, null);
        tw.WriteStartDocument();
        tw.WriteStartElement("TekenElementen");
        try
        {
            foreach (TekenElement te in schetscontrol.Ophalen.TekenElementLijst)
            {
                tw.WriteStartElement("Element");
                tw.WriteStartElement("Tool");
                tw.WriteString($"{te.Tool}");
                tw.WriteEndElement();
                tw.WriteStartElement("Punten");
                foreach (Point p in te.Punten)
                {
                    tw.WriteStartElement("Punt");
                    tw.WriteString($"{p.X},{p.Y}");
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                tw.WriteStartElement("Kleur");
                if (te.Kleur.ToString().Contains("="))
                {
                    // Als de kleur een ARGB waarde heeft: split hem en haal de kleur er weer uit
                    string str = te.Kleur.ToString();
                    str = Regex.Replace(str, "[^0-9=]", "");
                    tw.WriteString($"{str.Remove(0, 1)}");
                }
                else
                {
                    // Als de kleur geen ARGB waarde heeft, is er sprake van een naam.
                    // Haal de overbodige characters weg, zodat dit niet
                    // allemaal opgeslagen wordt in de file
                    string kleurstring = te.Kleur.ToString().Split(" ")[1].Remove(0, 1);
                    tw.WriteString($"{kleurstring.Remove(kleurstring.Length - 1, 1)}");
                }
                tw.WriteEndElement();
                tw.WriteStartElement("Letters");
                tw.WriteString($"{te.Letters}");
                tw.WriteEndElement();
                tw.WriteStartElement("Hoek");
                tw.WriteString($"{te.Hoek}");
                tw.WriteEndElement();
                tw.WriteEndElement();
            }
            // en weer afsluiten.
            tw.WriteEndElement();
            tw.WriteEndDocument();
            tw.Close();
            Wijzig = false;
            // Zet de bestandslocatie als text van het window, zodat dit voor de
            // gebruiker duidelijk is, maar deze later ook nog gebruikt
            // kan worden
            this.Text = $"{naam}";
            OpslaanPopup();
        }
        catch
        {
            MessageBox.Show("Error saving the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void SaveBitmap(string naam, bool controle)
    {
        // Maak een nieuwe bitmap, sla die op als image
        Bitmap map = new Bitmap(schetscontrol.Size.Width, schetscontrol.Size.Height);
        schetscontrol.DrawToBitmap(map, new Rectangle(0, 0, schetscontrol.Size.Width, schetscontrol.Size.Height));
        map.Save(naam);

        // Nogmaals het trucje met de window naam
        this.Text = $"{naam}";
        Wijzig = false;
        if (controle)
        {
            this.Close();
        }
        OpslaanPopup();
    }
    private void OpslaanPopup()
    {
        // Geef een notificatie als er opgeslagen is. Voornamelijk handig voor
        // de save button. Spam deze maar eens en open je tray icons, ziet er leuk uit.
        NotifyIcon nf = new NotifyIcon();
        nf.Icon = SystemIcons.Information;
        nf.BalloonTipTitle = "Your Schets was saved!";
        nf.BalloonTipText = " ";
        nf.BalloonTipIcon = ToolTipIcon.Info;
        nf.Visible = true;
        nf.ShowBalloonTip(1000);
    }
    // De property die bijhoudt of er wijzigingen gedaan zijn
    public bool Wijzig
    {
        get { return wijzig; }
        set { wijzig = value; }
    }
    // Allemaal methodes zoals ze ook in de source code staan, met wat tools bijgevoegd
    public SchetsWin()
    {
        ISchetsTool[] deTools = { new PenTool()
                                , new LijnTool()
                                , new RechthoekTool()
                                , new VolRechthoekTool()
                                , new TekstTool()
                                , new GumTool()
                                , new OvaalTool()
                                , new VolOvaalTool()
                                , new BovenopTool()
                                , new OnderopTool()
                                , new MoveTool()
                                };

        this.ClientSize = new Size(700, 700);
        huidigeTool = deTools[0];

        schetscontrol = new SchetsControl();
        schetscontrol.Location = new Point(64, 10);
        schetscontrol.MouseDown += (object o, MouseEventArgs mea) =>
                                    {   vast=true;  
                                        huidigeTool.MuisVast(schetscontrol, mea.Location); 
                                    };
        schetscontrol.MouseMove += (object o, MouseEventArgs mea) =>
                                    {   if (vast)
                                        huidigeTool.MuisDrag(schetscontrol, mea.Location); 
                                    };
        schetscontrol.MouseUp   += (object o, MouseEventArgs mea) =>
                                    {   if (vast)
                                        huidigeTool.MuisLos (schetscontrol, mea.Location);
                                        vast = false; 
                                    };
        schetscontrol.KeyPress +=  (object o, KeyPressEventArgs kpea) => 
                                    {   huidigeTool.Letter  (schetscontrol, kpea.KeyChar); 
                                    };
        this.Controls.Add(schetscontrol);

        menuStrip = new MenuStrip();
        menuStrip.Visible = false;
        this.Controls.Add(menuStrip);
        this.maakFileMenu();
        this.maakToolMenu(deTools);
        this.maakActieMenu();
        this.maakToolButtons(deTools);
        this.maakActieButtons();
        this.Resize += this.veranderAfmeting;
        this.FormClosing += this.afsluiten;
        this.veranderAfmeting(null, null);
    }
    private void maakFileMenu()
    {   
        ToolStripMenuItem menu = new ToolStripMenuItem("File");
        menu.MergeAction = MergeAction.MatchOnly;
        menu.DropDownItems.Add("Sluiten", null, this.SluitenButton_Click);
        menu.DropDownItems.Add("Opslaan", null, this.Opslaan);
        menu.DropDownItems.Add("Opslaan als...", null, this.OpslaanAls);
        menuStrip.Items.Add(menu);
    }

    private void maakToolMenu(ICollection<ISchetsTool> tools)
    {   
        ToolStripMenuItem menu = new ToolStripMenuItem("Tool");
        foreach (ISchetsTool tool in tools)
        {   ToolStripItem item = new ToolStripMenuItem();
            item.Tag = tool;
            item.Text = tool.ToString();
            item.Image = new Bitmap($"../../../Icons/{tool.ToString()}.png");
            item.Click += this.klikToolMenu;
            menu.DropDownItems.Add(item);
        }
        menuStrip.Items.Add(menu);
    }

    private void maakActieMenu()
    {   
        ToolStripMenuItem menu = new ToolStripMenuItem("Actie");
        menu.DropDownItems.Add("Clear", null, schetscontrol.Schoon );
        menu.DropDownItems.Add("Roteer", null, schetscontrol.Roteer );
        menu.DropDownItems.Add("Kies kleur", null, schetscontrol.VeranderKleur);
        menuStrip.Items.Add(menu);
    }

    private void maakToolButtons(ICollection<ISchetsTool> tools)
    {
        int t = 0;
        foreach (ISchetsTool tool in tools)
        {
            RadioButton b = new RadioButton();
            b.Appearance = Appearance.Button;
            b.Size = new Size(45, 62);
            b.Location = new Point(10, 10 + t * 62);
            b.Tag = tool;
            b.Text = tool.ToString();
            b.Image = new Bitmap($"../../../Icons/{tool.ToString()}.png");
            b.TextAlign = ContentAlignment.TopCenter;
            b.ImageAlign = ContentAlignment.BottomCenter;
            b.Click += this.klikToolButton;
            this.Controls.Add(b);
            if (t == 0) b.Select();
            t++;
        }
    }

    private void maakActieButtons()
    {   
        paneel = new Panel(); this.Controls.Add(paneel);
        paneel.Size = new Size(600, 24);
            
        Button clear = new Button(); paneel.Controls.Add(clear);
        clear.Text = "Clear";  
        clear.Location = new Point(  0, 0); 
        clear.Click += schetscontrol.Schoon;        
            
        Button rotate = new Button(); paneel.Controls.Add(rotate);
        rotate.Text = "Rotate"; 
        rotate.Location = new Point( 80, 0); 
        rotate.Click += schetscontrol.Roteer;

        // en hier wat buttons toegevoegd
        Button save = new Button(); paneel.Controls.Add(save);
        save.Text = "Save";
        save.Location = new Point(160, 0);
        save.Click += this.Opslaan;

        Button undo = new Button(); paneel.Controls.Add(undo);
        undo.Text = "Undo";
        undo.Location = new Point(240, 0);
        undo.Click += schetscontrol.Undo;

        Button redo = new Button(); paneel.Controls.Add(redo);
        redo.Text = "Redo";
        redo.Location = new Point(320, 0);
        redo.Click += schetscontrol.Redo;

        Button Kleur = new Button(); paneel.Controls.Add(Kleur);
        Kleur.Text = "Kleur";
        Kleur.Location = new Point(400, 0);
        Kleur.Click += schetscontrol.VeranderKleur;
    }
}