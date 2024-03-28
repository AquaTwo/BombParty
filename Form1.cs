using System;
using System.Security.Cryptography;
using System.Net.Http;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Reflection.Emit;


namespace BombParty
{
    public partial class Form1 : Form
    {
        private string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        private int letter1;
        private int letter2;
        private string wordPrompt;
        private string char1;
        private string char2;
        private string answer;
        private int initialBombtimer = 7+1;
        private int bombTimer = 7 + 1;
        private System.Windows.Forms.Timer timer;



        public Form1()
        {
            InitializeComponent();

            

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1000 milliseconds = 1 second
            timer.Tick += Timer_Tick;

            

            promptGeneration();

            



        }

        private void promptGeneration()
        {
            bombTimer = initialBombtimer;

            timer.Start();

            Random random = new Random();


            letter1 = random.Next(25);
            letter2 = random.Next(25);

            


            //wordPrompt = alphabet[letter1] + alphabet[letter2];

            char1 = alphabet[letter1];
            char2 = alphabet[letter2];

            wordPrompt = char1 + char2;

            label1.Text = wordPrompt;

            

        }

        private void Timer_Tick(object sender, EventArgs e)

        {



            // Decrease the current number by 1


            bombTimer--;

            // Display the current number
            label3.Text = bombTimer.ToString();

            // Check if the current number is 0
            if (bombTimer == 0)
            {
                // Stop the timer
                timer.Stop();

                label2.Text = ("The bomb exploded");

                button1.Enabled = false;
                


            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

            answer = textBox1.Text.ToLower();

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://api.dictionaryapi.dev/api/v2/entries/en_US/{answer}";
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode && CheckWordPresence(answer, wordPrompt) && bombTimer > 0)
                {
                    label2.Text = ($"The word '{answer}' is valid.");


                    textBox1.Clear();



                    promptGeneration();





                }



                else
                {
                    // If the API returns an error response, the word is considered invalid
                    label2.Text = ($"The word '{answer}' is invalid.");
                }


            }

        }
        static bool CheckWordPresence(string answer, string wordPrompt)
        {
            wordPrompt = wordPrompt.ToLower();
            answer = answer.ToLower();

            // Loop through each character in the answer
            for (int i = 0; i <= answer.Length - wordPrompt.Length; i++)
            {
                bool found = true;
                // Check if the substring starting at position i matches wordPrompt
                for (int j = 0; j < wordPrompt.Length; j++)
                {
                    if (answer[i + j] != wordPrompt[j])
                    {
                        found = false;
                        break; // Break the inner loop if characters don't match
                    }
                }
                if (found)
                {
                    return true; // Return true if wordPrompt is found
                }
            }
            return false; // Return false if wordPrompt is not found
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && bombTimer > 0)
            {
                button1_Click(sender, e);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
           
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
    }

}











