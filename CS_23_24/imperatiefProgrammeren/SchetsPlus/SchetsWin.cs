using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

public class SchetsWin : Form
{
    public bool wijzig;
    MenuStrip menuStrip;
    public SchetsControl schetscontrol;
    public ISchetsTool huidigeTool;
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
        this.FormClosing -= this.afsluiten;
        if (Wijzig == true)
        {
            DialogResult dr = MessageBox.Show("You have unsaved changes, save before quitting?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                opslaanAls(null, null);
                Wijzig = false;
            }
            else if (dr == DialogResult.No)
            {
                this.Close();
            }
            else
            {
                if(fcea != null)
                {
                    this.FormClosing += this.afsluiten;
                    fcea.Cancel = true;
                }
            }
        }
        else
        {
            this.Close();
        }
        this.FormClosing += this.afsluiten;
    }
    private void SluitenButton_Click(object obj, EventArgs ea)
    {
        afsluiten(this, null);
    }

    private void opslaanAls(object obj, EventArgs ea)
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "png files (*.png)|*.png|jpg files(*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|xml files (.xml)|*.xml|All files (*.*)|*.*";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            if (sfd.FileName.EndsWith("xml"))
            {
                SchrijfXml(sfd.FileName);
            }
            else
            {
                Bitmap map = new Bitmap(schetscontrol.Size.Width, schetscontrol.Size.Height);
                schetscontrol.DrawToBitmap(map, new Rectangle(0, 0, schetscontrol.Size.Width, schetscontrol.Size.Height));
                map.Save(sfd.FileName);

                this.Text = $"{sfd.FileName}";
                Wijzig = false;
                if (obj == null)
                {
                    this.Close();
                }
                return;
            }
        }
    }
    private void SchrijfXml(string naam)
    {
        XmlTextWriter tw = new XmlTextWriter(naam, null);

        tw.WriteStartDocument();
        tw.WriteStartElement("TekenElementen");
        foreach (TekenElement te in schetscontrol.Ophalen.TekenElementLijst)
        {
            string kleurstring = te.Kleur.ToString().Split(" ")[1].Remove(0, 1);

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
            tw.WriteString($"{kleurstring.Remove(kleurstring.Length - 1, 1)}");
            tw.WriteEndElement();
            tw.WriteStartElement("Letters");
            tw.WriteString($"{te.Letters}");
            tw.WriteEndElement();
            tw.WriteStartElement("Hoek");
            tw.WriteString($"{te.Hoek}");
            tw.WriteEndElement();
            tw.WriteEndElement();
        }
        tw.WriteEndElement();
        tw.WriteEndDocument();
        tw.Close();
        Wijzig = false;
        return;
    }
    public bool Wijzig
    {
        get { return wijzig; }
        set { wijzig = value; }
    }
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
                                };
        String[] deKleuren = { "Black", "Red", "Green", "Blue", "Yellow", "Magenta", "Cyan" };

        this.ClientSize = new Size(700, 500);
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
        this.maakActieMenu(deKleuren);
        this.maakToolButtons(deTools);
        this.maakActieButtons(deKleuren);
        this.Resize += this.veranderAfmeting;
        this.FormClosing += this.afsluiten;
        this.veranderAfmeting(null, null);
    }
    private void maakFileMenu()
    {   
        ToolStripMenuItem menu = new ToolStripMenuItem("File");
        menu.MergeAction = MergeAction.MatchOnly;
        menu.DropDownItems.Add("Sluiten", null, this.SluitenButton_Click);
        menu.DropDownItems.Add("Opslaan als...", null, this.opslaanAls);
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

    private void maakActieMenu(String[] kleuren)
    {   
        ToolStripMenuItem menu = new ToolStripMenuItem("Actie");
        menu.DropDownItems.Add("Clear", null, schetscontrol.Schoon );
        menu.DropDownItems.Add("Roteer", null, schetscontrol.Roteer );
        ToolStripMenuItem submenu = new ToolStripMenuItem("Kies kleur");
        foreach (string k in kleuren)
            submenu.DropDownItems.Add(k, null, schetscontrol.VeranderKleurViaMenu);
        menu.DropDownItems.Add(submenu);
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

    private void maakActieButtons(String[] kleuren)
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

        Button save = new Button(); paneel.Controls.Add(save);
        save.Text = "Save";
        save.Location = new Point(160, 0);
        save.Click += schetscontrol.Opslaan;

        Button undo = new Button(); paneel.Controls.Add(undo);
        undo.Text = "Undo";
        undo.Location = new Point(240, 0);
        undo.Click += schetscontrol.Undo;

        Label penkleur = new Label(); paneel.Controls.Add(penkleur);
        penkleur.Text = "Penkleur:"; 
        penkleur.Location = new Point(260, 3); 
        penkleur.AutoSize = true;               
            
        ComboBox cbb = new ComboBox(); paneel.Controls.Add(cbb);
        cbb.Location = new Point(320, 0); 
        cbb.DropDownStyle = ComboBoxStyle.DropDownList; 
        cbb.SelectedValueChanged += schetscontrol.VeranderKleur;
        foreach (string k in kleuren)
            cbb.Items.Add(k);
        cbb.SelectedIndex = 0;
    }
}