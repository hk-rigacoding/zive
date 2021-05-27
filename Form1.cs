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

        private void Form1_Load(object sender, EventArgs e)
        {
            pac = new Pacman(new Point(100, 100), 10, 0, new Size(50,50));
            this.Controls.Add(pac);
            pac.BringToFront();
        }

        //pac.Left - pac.solis, pac.Right - pac.solis,  pac.Bottom, pac.Top
        bool Overlap ()
        {
            
            if (level == null)
             return false;

            /*
            foreach (PictureBox pb in level.Controls)
            {
                if (left >= pb.Left && left <= pb.Right)//kreisā mala ir iekšā svešā kontrolā
                    if (right >= pb.Left && left <= pb.Right)//kreisā mala ir iekšā svešā kontrolā
                        return true;
            }

            */

            foreach (PictureBox pb in level.Controls)
            {
                if (pac.Bounds.IntersectsWith(pb.Bounds))
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
                    if (!Overlap())
                    {
                        pac.last_loc = pac.Location;
                        pac.Location = new Point(pac.Location.X - pac.solis, pac.Location.Y);
                        
                    } else pac.Location = pac.last_loc;

                    break;

                case 'd'://PA LABI 
                    pac.virziens = 1;

                    if (!Overlap())
                    {
                        pac.last_loc = pac.Location;
                        pac.Location = new Point(pac.Location.X + pac.solis, pac.Location.Y);
                    } else pac.Location = pac.last_loc;

                    break;

                case 'w'://UZ AUGŠU
                    pac.virziens = 2;
                    if (!Overlap())
                    {
                        pac.last_loc = pac.Location;
                        pac.Location = new Point(pac.Location.X, pac.Location.Y - pac.solis);
                    } else pac.Location = pac.last_loc;


                    break;

                case 's':
                    pac.virziens = 3;

                    if (!Overlap())
                    {
                        pac.last_loc = pac.Location;
                        pac.Location = new Point(pac.Location.X, pac.Location.Y + pac.solis);
                    }
                    else pac.Location = pac.last_loc;


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


}
