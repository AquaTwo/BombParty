using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Automation;
using static BombParty.Form1;

namespace BombParty
{

    public partial class StartScreen : Form
    {
        int lives = GlobalVariables.Lives;
        private Form1 form1Instance;
        

        public static class GlobalVariables
        {
        
            public static int Lives = 3;
            
        }

        

        public StartScreen()
        {
            InitializeComponent();
            
            
        }


        private void startScreenButton_Click(object sender, EventArgs e)
        {

            gameStart();

            
           
            



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (GlobalVariables.Lives == 0)
            {
               panel1.Visible = false;
               panel1.SendToBack();

            }
        }



        private void gameStart()
        {
            Form1 form1 = new Form1();

            form1Instance = form1;
            form1.ResetGame();



            panel1.BringToFront();
            panel1.Visible = true;
            panel1.Controls.Clear();

            

            form1.TopLevel = false;
            form1.FormBorderStyle = FormBorderStyle.None;
            form1.Dock = DockStyle.Fill;
            form1.Parent = panel1;
            form1.Show();

            


        }


    }
}
