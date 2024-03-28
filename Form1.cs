using System;
using System.Security.Cryptography;
using System.Net.Http;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;


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
        private int lives = 3;
        private System.Windows.Forms.Timer timer;
        private List<string> enteredWords = new List<string>();
        private HashSet<char> usedLetters = new HashSet<char>();




        public Form1()
        {
            InitializeComponent();

            

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1000 milliseconds = 1 second
            timer.Tick += Timer_Tick;


            initializeAlphabetLabels();
            promptGeneration();

            



        }
        private void initializeAlphabetLabels()
        {
            int rowCount = (int)Math.Ceiling((double)alphabet.Length / 2);

            for (int row = 0; row < rowCount; row++)
            {
                System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();
                label1.Text = alphabet[row];
                label1.Padding = new Padding(4);
                label1.AutoSize = false;
                label1.Size = new Size(25, 25); // Adjust size as needed
                label1.Top = row * 30; // Adjust spacing as needed
                label1.Left = 10;
                label1.TextAlign = ContentAlignment.MiddleCenter;
                label1.BorderStyle = BorderStyle.FixedSingle; // Optional: Add border for better visibility
                label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; // Anchor to maintain position

                // Rotate text 90 degrees
                label1.Paint += (sender, e) =>
                {
                    e.Graphics.TranslateTransform(label1.Width, label1.Height);
                    e.Graphics.RotateTransform(-90);
                    e.Graphics.DrawString(label1.Text, label1.Font, Brushes.Black, 0, 0);
                };

                Controls.Add(label1);

                if (row + rowCount < alphabet.Length)
                {
                    System.Windows.Forms.Label label2 = new System.Windows.Forms.Label();
                    label2.Text = alphabet[row + rowCount];
                    label2.Padding = new Padding(4);
                    label2.AutoSize = false;
                    label2.Size = new Size(25, 25); // Adjust size as needed
                    label2.Top = row * 30; // Adjust spacing as needed
                    label2.Left = label1.Width + 20;
                    label2.TextAlign = ContentAlignment.MiddleCenter;
                    label2.BorderStyle = BorderStyle.FixedSingle; // Optional: Add border for better visibility
                    label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; // Anchor to maintain position

                    // Rotate text 90 degrees
                    label2.Paint += (sender, e) =>
                    {
                        e.Graphics.TranslateTransform(label2.Width, label2.Height);
                        e.Graphics.RotateTransform(-90);
                        e.Graphics.DrawString(label2.Text, label2.Font, Brushes.Black, 0, 0);
                    };

                    Controls.Add(label2);
                }
            }
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
            label4.Text = ($"Lives: {lives}");

            // Check if the current number is 0
            if (bombTimer == 0)
            {
                timer.Stop();
                promptGeneration();
                

                lives = lives - 1;
                // Stop the timer
                

                

                



            }
            else if (/*bombTimer == 0 && */lives == -1)
            {
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

                if (response.IsSuccessStatusCode && CheckWordPresence(answer, wordPrompt) && bombTimer > 0 && !enteredWords.Contains(answer))
                {
                    if (usedLetters.Count == alphabet.Length)
                    {
                        lives++;
                        resetUsedLetters();
                    }

                    enteredWords.Add(answer);
                    updateUsedLetters(answer);
                    updateAlphabetColors();


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
        private void resetUsedLetters()
        {
            usedLetters.Clear();
        }
        private void updateUsedLetters(string word)
        {
            foreach (char c in word)
            {
                usedLetters.Add(c);
            }
        }
        private void updateAlphabetColors()
        {
            foreach (Control control in Controls)
            {
                if (control is System.Windows.Forms.Label label && alphabet.Contains(label.Text))
                {
                    if (usedLetters.Contains(label.Text[0]))
                    {
                        label.BackColor = System.Drawing.Color.Gray;


                    }
                    else
                    {
                        label.BackColor = System.Drawing.Color.Yellow;
                    }
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }

}











