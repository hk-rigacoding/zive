using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zive
{

    //1. mums ir pašu vadīts personāžs  -   ievade ar taustiņiem <- PA KREISI; PA LABI ->; UZ AUGSHU; UZ LEJU
    //2. datora vadīti personāži    -   jāuzdod Randoms virziens un pie katras izvēles nogriezties Random();
    //3. sienas ? loopojams masīvs/kolekcija, jo personāža vadīšanā jānosaka vai nesaskarās !


    public partial class Form1 : Form
    {
        Pacman pac;
        List<Npc> boti;


        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
            if (level == null)
                return;

            foreach (PictureBox pb in level.Controls)
            {
                MessageBox.Show(pb.Name);
            }
            */
        }


        List<Npc> spawn (List<Npc> botu_liste, int botu_skaits, ref GroupBox level)
        {

            Npc npc = null;
            Random rX = new Random();
            Random rY = new Random();
            Random rheading = new Random();

            for (int i = 0; i < botu_skaits; i++)
            {

                //kamēr ir nederīgs bots
                do
                {
                    //randoma koordināta 
                    int x = rX.Next(1, level.Bounds.Right - 50);

                    //randoma koordināta 
                    int y = rY.Next(1, level.Bounds.Bottom - 50);

                    int virziens = rheading.Next(0,4);

                    npc = new Npc(new Point(x, y), 10, virziens, new Size(50, 50), level);

                    //ja paks ir sienā, tyad pac = null
                    if (Overlap(npc))
                        npc = null;

                } while (npc == null);

                level.Controls.Add(npc);
                botu_liste.Add(npc);
            }

            return botu_liste;
        }

        Pacman spawn (Pacman p)
        {

            p = null;
            Random rX = new Random();
            Random rY = new Random();

            //kamēr ir nederīgs pac
            do
            {
                //randoma koordināta 
                int x = rX.Next(1, level.Bounds.Right - 50);

                //randoma koordināta 
                int y = rY.Next(1, level.Bounds.Bottom - 50);

                p = new Pacman(new Point(x, y), 10, 0, new Size(50, 50));

                //ja paks ir sienā, tyad pac = null
                if (Overlap(p))
                    p = null;

            } while (p == null);


            return p;
        }


        private void Form1_Load(object sender, EventArgs e)
        {


            boti = new List<Npc>();

            Random b_sk = new Random();
            int botu_skaits = b_sk.Next(4,10);

            level.Controls.Add(pac = spawn(pac));

            //spawn loops listei ar NPC
            //level.Controls.Add(spawn(npc));
            //this.Controls.Add(pac);
            pac.BringToFront();


            boti = spawn(boti, botu_skaits, ref level);

            //foreach (Npc bots in boti)
              //  level.Controls.Add(bots);


        }

        //pac.Left - pac.solis, pac.Right - pac.solis,  pac.Bottom, pac.Top
        bool Overlap (Pacman p)
        {
            
            if (level == null)
             return false;

            foreach (PictureBox pb in level.Controls)
            {
               

                if (pb != p)
                if (pb.Bounds.IntersectsWith(p.Bounds))
                {
                    //log.Text += pb.Name + " : TRUE" + Environment.NewLine;
                    return true;
                }

                //log.Text += "PacY =[" + pac.Bounds.Y + "]; " + pb.Name + " PbY = [" + pb.Bounds.Bottom + "]" + Environment.NewLine;

                
            }



                return false;
        }


        bool Overlap(Npc p)
        {

            if (level == null)
                return false;

            foreach (PictureBox pb in level.Controls)
            {
                if (pb != p)
                    if (pb.Bounds.IntersectsWith(p.Bounds))
                        return true;
            }
            return false;
        }


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

            switch (e.KeyChar)
            {
                case 'a'://PA KREISI
                    pac.virziens = 0;
                    //ja ir iespēja, mainīt lokāciju 

                    pac.last_loc = pac.Location;
                    pac.Location = new Point(pac.Location.X - pac.solis, pac.Location.Y);

                    if (Overlap(pac))
                        pac.Location = pac.last_loc;
                    break;

                case 'd'://PA LABI 
                    pac.virziens = 1;

                    pac.last_loc = pac.Location;
                    pac.Location = new Point(pac.Location.X + pac.solis, pac.Location.Y);

                    if (Overlap(pac))
                        pac.Location = pac.last_loc;
                    break;

                case 'w'://UZ AUGŠU
                    pac.virziens = 2;

                    pac.last_loc = pac.Location;
                    pac.Location = new Point(pac.Location.X, pac.Location.Y - pac.solis);
                    if (Overlap(pac))
                        pac.Location = pac.last_loc;
                    break;

                case 's':
                    pac.virziens = 3;

                    pac.last_loc = pac.Location;
                    pac.Location = new Point(pac.Location.X, pac.Location.Y + pac.solis);

                    if (Overlap(pac))
                        pac.Location = pac.last_loc;

                    break;

            }

            e.Handled = true;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
             switch (e.KeyCode)
            {
                case Keys.Left:
                    pac.virziens = 0;
                    break;

                case Keys.Right:
                    pac.virziens = 1;
                    break;

                case Keys.Up:
                    pac.virziens = 2;
                    break;

                case Keys.Down:
                    pac.virziens = 3;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }


    class Pacman : PictureBox
    {

        //visas bildes
        Image[,] bildes;

        //virziens (pa kreisi, pa labi, augša, leja)
        public int virziens;

        //solis ??
        public int solis;

        //taimeris, kas mainīs spraitus
        Timer t;

        //spraita indekss 0 - 3
        int spraits;

        public Point last_loc;

        public Pacman (Point loc, int step, int heading, Size izm)
        {
            bildes = new Image[4,4];
            spraits = 0;

            this.Location = loc;
            solis = step;
            virziens = heading;


            //pa kreisi
            bildes[0, 0] = Properties.Resources.pacman_left_1;
            bildes[0, 1] = Properties.Resources.pacman_left_2;
            bildes[0, 2] = Properties.Resources.pacman_left_3;
            bildes[0, 3] = Properties.Resources.pacman_left_4;

            //pa labi
            bildes[1, 0] = Properties.Resources.pacman_right_1;
            bildes[1, 1] = Properties.Resources.pacman_right_2;
            bildes[1, 2] = Properties.Resources.pacman_right_3;
            bildes[1, 3] = Properties.Resources.pacman_right_4;

            //uz augšu
            bildes[2, 0] = Properties.Resources.pacman_up_1;
            bildes[2, 1] = Properties.Resources.pacman_up_2;
            bildes[2, 2] = Properties.Resources.pacman_up_3;
            bildes[2, 3] = Properties.Resources.pacman_up_4;

            //uz leju
            bildes[3, 0] = Properties.Resources.pacman_down_1;
            bildes[3, 1] = Properties.Resources.pacman_down_2;
            bildes[3, 2] = Properties.Resources.pacman_down_3;
            bildes[3, 3] = Properties.Resources.pacman_down_4;


            this.Size = izm;
            this.SizeMode = PictureBoxSizeMode.StretchImage;

            //spraitu mainīšanai
            t = new Timer();
            t.Interval = 150;//50 ms koriģējams !!!
            t.Tick += T_Tick;
            t.Start();

        }




        private void T_Tick(object sender, EventArgs e)
        {
            this.Image = bildes[virziens, spraits++];

            if (spraits == 4)
                spraits = 0;

        }
    }



    class  Npc : PictureBox
    {
        //visas bildes
        Image[,] bildes;

        //tips ?


        //virziens (pa kreisi, pa labi, augša, leja)
        public int virziens;

        //solis ??
        public int solis;

        //taimeris, kas mainīs spraitus
        Timer t;

        //spraita indekss 0 - 1
        int spraits;

        public Point last_loc;

        GroupBox level;

        public Npc(Point loc, int step, int heading, Size izm, GroupBox _level)
        {
            level = _level;

            bildes = new Image[4, 2];
            spraits = 0;

            this.Location = loc;
            solis = step;
            virziens = heading;


            //pa kreisi
            bildes[0, 0] = Properties.Resources.enemy_left_1;
            bildes[0, 1] = Properties.Resources.enemy_left_2;


            //pa labi
            bildes[1, 0] = Properties.Resources.enemy_right_1;
            bildes[1, 1] = Properties.Resources.enemy_right_2;


            //uz augšu
            bildes[2, 0] = Properties.Resources.enemy_up_1;
            bildes[2, 1] = Properties.Resources.enemy_up_2;


            //uz leju
            bildes[3, 0] = Properties.Resources.enemy_down_1;
            bildes[3, 1] = Properties.Resources.enemy_down_2;



            this.Size = izm;
            this.SizeMode = PictureBoxSizeMode.StretchImage;

            //spraitu mainīšanai
            t = new Timer();
            t.Interval = 100;//50 ms koriģējams !!!
            t.Tick += T_Tick;
            t.Start();

        }

        bool Overlap(Npc p)
        {

            if (level == null)
                return false;

            foreach (PictureBox pb in level.Controls)
            {
                if (pb != p)
                    if (pb.Bounds.IntersectsWith(p.Bounds))
                        return true;
            }
            return false;
        }

        private void T_Tick(object sender, EventArgs e)
        {

            this.Image = bildes[virziens, spraits++];

            if (spraits == 2)
                spraits = 0;

            //tikšķis veidos kustību

            //ja neatduros sienā : Overlap()
            //taisu vienu gājienu uz priekšu patreizējā virzienā

            Random v_rnd = new Random();
            

            switch (virziens)
            {
                case 0: // PA KREISI
                    last_loc = Location;
                    Location = new Point(Location.X - solis, Location.Y);

                    if (Location.X < level.Bounds.Left)
                        Location = new Point(level.Bounds.Right - solis, Location.Y);


                    if (Overlap(this))
                    {
                        Location = last_loc;
                        virziens = v_rnd.Next(0, 4);
                    }
                    break;

                case 1: // PA LABI
                    last_loc = Location;
                    Location = new Point(Location.X + solis, Location.Y);

                    if (Location.X > level.Bounds.Right)
                        Location = new Point(level.Bounds.Left, Location.Y);


                    if (Overlap(this))
                    {
                        Location = last_loc;
                        virziens = v_rnd.Next(0, 4);
                    }
                    break;


                case 2: // UZ AUGSHU
                    last_loc = Location;
                    Location = new Point(Location.X, Location.Y - solis);


                    if (Location.Y < level.Bounds.Top)
                        Location = new Point(Location.X, level.Bounds.Bottom - solis);

                    if (Overlap(this))
                    {
                        Location = last_loc;
                        virziens = v_rnd.Next(0, 4); 
                    }
                    break;

                case 3: // UZ LEJU
                    last_loc = Location;
                    Location = new Point(Location.X, Location.Y + solis);

                    if (Location.Y > level.Bounds.Bottom)
                        Location = new Point(Location.X, level.Bounds.Top);

                    if (Overlap(this))
                    {
                        Location = last_loc;
                        virziens = v_rnd.Next(0, 4);
                    }
                    break;




            }


        }
    }

    }
