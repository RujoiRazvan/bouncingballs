using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace Breakout_Ball
{
    public partial class joc : Form
    {
        bool muta_stanga;
        bool muta_dreapta;
        public int vSpeed;
        public int hSpeed;
        int Speed = 15;
        public int score = 0;
        public bool pauza = true;
        
        private Random rnd = new Random();

        SoundPlayer start = new SoundPlayer(Breakout_Ball.Properties.Resources.start);
        SoundPlayer iesire = new SoundPlayer(Breakout_Ball.Properties.Resources.quit);
        SoundPlayer bazaSunet = new SoundPlayer(Breakout_Ball.Properties.Resources.paddle_touch);
        SoundPlayer brickSound = new SoundPlayer(Breakout_Ball.Properties.Resources.Hit1);
        //SoundPlayer iesireMinge = new SoundPlayer(Breakout_Ball.Properties.Resources.out);

        public joc()
        {
            vSpeed = 3;
            hSpeed = 3;

            InitializeComponent();
            ImageList images = new ImageList();
            images.Images.Add(Image.FromFile(@"C:\Users\Razvan Rujoi\Desktop\bricks_galbe.png"));
            images.Images.Add(Image.FromFile(@"C:\Users\Razvan Rujoi\Desktop\bricks_rosu.png"));
            images.Images.Add(Image.FromFile(@"C:\Users\Razvan Rujoi\Desktop\bricks_albastru.png"));
            images.Images.Add(Image.FromFile(@"C:\Users\Razvan Rujoi\Desktop\bricks_verde.png"));

            foreach (Control z in this.Controls)
            {
                if (z is PictureBox && z.Tag == "block")
                {
                    z.BackgroundImage = images.Images[rnd.Next(0, images.Images.Count )]; ;
                }
            }
        }

        private void joc_Load(object sender, EventArgs e)
        {
            start.Play();
            mutaBaza((ClientRectangle.Width - baza.Width) / 2);
            meniuJoc.Location = new Point((ClientRectangle.Width - meniuJoc.Width) / 2, (ClientRectangle.Height - meniuJoc.Height) / 2);
            logo.Location = new Point((ClientRectangle.Width - logo.Width) / 2, (ClientRectangle.Height - logo.Height) / 2 - 150);
            p.Location= new Point((ClientRectangle.Width - p.Width) / 2, (ClientRectangle.Height - p.Height) / 2 + 15);
            aiPierdut.Location=new Point((ClientRectangle.Width - aiPierdut.Width) / 2, (ClientRectangle.Height - aiPierdut.Height) / 2 - 140);
            CenterToScreen();
            ascundeBlocuri();
            aiPierdut.Visible = false;
            scor.Visible = false;
            logo.Visible = true;
            baza.Visible = false;
            p.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Ball.Top += vSpeed;
            Ball.Left += hSpeed;
            if (muta_stanga)
                baza.Left -= Speed;
            if(muta_dreapta)
                baza.Left += Speed;
            if (baza.Left < 0)
                muta_stanga = false;
            if (baza.Left > ClientRectangle.Size.Width - baza.Width)
                muta_dreapta = false;
            if(Ball.Left<0 || Ball.Left> ClientRectangle.Width - Ball.Width)
                hSpeed = -hSpeed;
            if (Ball.Top < 0)
                vSpeed = -vSpeed;
            if (Ball.Top > ClientRectangle.Size.Height)
            {
                Ball.Visible = false;
                gameOver();
            }

            if (Ball.Bounds.IntersectsWith(baza.Bounds))
            {
                bazaSunet.Play();
                vSpeed = -vSpeed;
            }

            foreach(Control z in this.Controls)
            {
                if(z is PictureBox && z.Tag == "block")
                {
                    if(Ball.Bounds.IntersectsWith(z.Bounds))
                    {
                        brickSound.Play();
                        this.Controls.Remove(z);
                        score += 1;
                        scor.Text = "Scor: " + score.ToString();
                        vSpeed = -vSpeed;
                    }
                }
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                muta_stanga = false;
            if(e.KeyCode==Keys.Right)
                muta_dreapta = false;
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Left && baza.Left > 10)
            {
                muta_stanga = true;
            }
            if (e.KeyCode == Keys.Right && baza.Left + baza.Width < 440)
            {
                muta_dreapta = true;
            }
            switch (e.KeyCode)
            {
                case Keys.Escape:   
                    Pauza();
                    break;
                case Keys.Q:
                    Close();
                    break;
                case Keys.R:
                    Application.Restart();
                    break;
            }
        }
        
        private void Pauza()
        {
            pauza = !pauza;
            if (pauza == false)
            {
                p.Visible = true;
                timer1.Stop();
            }
            else
            {
                p.Visible = false;
                timer1.Start();
            }
        }

        private void meniu(bool Show = true)
        {
            meniuJoc.Visible = Show;
            Invalidate();
        }

        private void mutaBaza(int newXPos)
        {
            if (newXPos < 0)
                newXPos = 0;
            else if (newXPos > ClientRectangle.Width - baza.Width)
                newXPos = ClientRectangle.Width - baza.Width;

            baza.Left = newXPos;
        }
        
        private void ascundeBlocuri()
        {
            timer1.Stop();
            foreach (Control z in this.Controls)
            {
                if (z is PictureBox && z.Tag == "block")
                {
                    z.Visible = false;
                }
            }
            Ball.Visible = false;
        }

        private void jocNou_Click(object sender, EventArgs e)
        {
            foreach (Control z in this.Controls)
            {
                if (z is PictureBox && z.Tag == "block")
                {
                    z.Visible = true;
                }
            }
            meniu(false);
            
            scor.Visible = true;
            Ball.Visible = true;
            
            timer1.Start();
            logo.Visible = false;
            baza.Visible = true;
      
        }

        private void gameOver()
        {
            iesire.Play();
            ascundeBlocuri();
            baza.Visible = false;
            meniu(true);
            aiPierdut.Show();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Close();
        }
    }
}
