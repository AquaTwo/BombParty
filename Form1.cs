using System;
using System.Security.Cryptography;
using System.Net.Http;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using static BombParty.StartScreen;
using Label = System.Windows.Forms.Label;


namespace BombParty
{
    
    public partial class Form1 : Form
    {
        private string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private Label livesLabel;
        private int letter1;
        private int letter2;
        private string wordPrompt;
        private string char1;
        private string char2;
        private string answer;
        private int initialBombtimer = 7+1;
        private int bombTimer = 7 + 1;
        public int lives = GlobalVariables.Lives;
        
       
        private System.Windows.Forms.Timer timer;
        private List<string> enteredWords = new List<string>();
        private HashSet<char> usedLetters = new HashSet<char>();

        


        public Form1()
        {
            InitializeComponent();

            

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1000 milliseconds = 1 second
            timer.Tick += Timer_Tick;

            livesLabel = new Label();

            livesLabel.Text = $"Lives: {GlobalVariables.Lives} ";
            livesLabel.Location = new Point(385, 65);
            // Add the label to the form's Controls collection
            Controls.Add(livesLabel);
        



            initializeAlphabetLabels();
            updateAlphabetColors();
            promptGeneration();

            



        }
        private void initializeAlphabetLabels()
        {
            int rowCount = (int)Math.Ceiling((double)alphabet.Length / 2);
            int topPadding = 20; // Adjust the top padding as needed

            

                for (int outerRow = 0; outerRow < rowCount; outerRow++)
                {
                    System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();
                    label1.Text = alphabet[outerRow];
                    label1.Padding = new Padding(4);
                    label1.AutoSize = false;
                    label1.Size = new Size(25, 25); // Adjust size as needed
                    label1.Top = topPadding + outerRow * 30; // Adjust spacing as needed
                    label1.Left = 10;
                    label1.TextAlign = ContentAlignment.MiddleCenter;
                    label1.BorderStyle = BorderStyle.FixedSingle; // Optional: Add border for better visibility
                    //label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; // Anchor to maintain position

                    // Rotate text 90 degrees
                    label1.Paint += (sender, e) =>
                    {
                        e.Graphics.TranslateTransform(label1.Width, label1.Height);
                        e.Graphics.RotateTransform(-90);
                        e.Graphics.DrawString(label1.Text, label1.Font, Brushes.Black, 0, 0);
                    };

                    Controls.Add(label1);

                    if (outerRow + rowCount < alphabet.Length)
                    {
                        System.Windows.Forms.Label label2 = new System.Windows.Forms.Label();
                        label2.Text = alphabet[outerRow + rowCount];
                        label2.Padding = new Padding(4);
                        label2.AutoSize = false;
                        label2.Size = new Size(25, 25); // Adjust size as needed
                        label2.Top = topPadding + outerRow * 30; // Adjust spacing as needed
                        label2.Left = label1.Width + 20;
                        label2.TextAlign = ContentAlignment.MiddleCenter;
                        label2.BorderStyle = BorderStyle.FixedSingle; // Optional: Add border for better visibility
                       // label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; // Anchor to maintain position

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
            livesLabel.Text = ($"Lives: {GlobalVariables.Lives} ");

            // Check if the current number is 0
            if (bombTimer == 0)
            {
                timer.Stop();
                textBox1.Clear();
                promptGeneration();


                GlobalVariables.Lives = GlobalVariables.Lives - 1;
                // Stop the timer



                

                


            }
            else if(/*bombTimer == 0 && */GlobalVariables.Lives == 0)
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

            if (!button1.Enabled)
            {
                return; 
            }


            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://api.dictionaryapi.dev/api/v2/entries/en_US/{answer}";
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode && CheckWordPresence(answer, wordPrompt) && bombTimer > 0 && !enteredWords.Contains(answer))
                {
                    if (usedLetters.Count == alphabet.Length)
                    {
                        GlobalVariables.Lives++;
                        resetAlphabetColors();

                    }

                    enteredWords.Add(answer);

                    updateUsedLetters(answer);
                    
                    updateAlphabetColors(); 


                    label2.Text = ($"The word '{answer}' is valid.");

                    textBox1.Clear();
                    



                    promptGeneration();







                }

                else if (enteredWords.Contains(answer))
                {
                    label2.Text = ($"The word '{answer}' has already been used.");
                }

                else
                {
                    // If the API returns an error response, the word is considered invalid
                    label2.Text = ($"The word '{answer}' is invalid.");
                }


            }

        }
        private void resetAlphabetColors()
        {
            foreach (Control control in Controls)
            {
                if (control is System.Windows.Forms.Label label && alphabet.Contains(label.Text))
                {
                    label.BackColor = Color.Yellow; // Reset all alphabet labels to yellow
                }
            }
        }


        private void updateAlphabetColors()
        {
            int count = 0; // Initialize count variable to track used alphabet letters
            foreach (Control control in Controls)
            {
                if (control is System.Windows.Forms.Label label && alphabet.Contains(label.Text))
                {
                    char letter = label.Text[0];
                    if (usedLetters.Contains(letter))
                    {
                        label.BackColor = Color.Gray;
                        count++;
                    }
                    else
                    {
                        label.BackColor = Color.Yellow;
                    }
                }
            }



            // Check if all alphabet letters are used, if yes, increment lives
            if (count == alphabet.Length)
            {
                GlobalVariables.Lives++;
                resetAlphabetColors(); // Reset alphabet colors to yellow when lives are incremented
            }
        }
        private void updateUsedLetters(string word) // Corrected method name
        {
            foreach (char c in word)
            {
                usedLetters.Add(c);
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

    
        public void ResetGame()
        {
            // Reset variables
            
            bombTimer = initialBombtimer;
            enteredWords.Clear();
            usedLetters.Clear();
            GlobalVariables.Lives = 3;

            // Reset UI elements
            label1.Text = string.Empty; // Clear the word prompt
            label2.Text = string.Empty; // Clear any messages
            textBox1.Clear(); // Clear the input textbox

            // Reset alphabet labels colors
            resetAlphabetColors();

            // Start the game again
            timer.Start();
            promptGeneration();
            
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











