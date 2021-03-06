﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {

        // firstClicked points to the first Label control 
        // that the player clicks, but it will be null 
        // if the player hasn't clicked a label yet
        Label firstClicked = null;

        // secondClicked points to the second Label control 
        // that the player clicks
        Label secondClicked = null;

        Random random = new Random();

        int time = 0;


        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        public Form1()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        /// <summary>
        /// Every label's Click event is handled by this event handler
        /// </summary>
        /// <param name="sender">The label that was clicked</param>
        /// <param name="e"></param>
        private void label_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // The timer is only on after two non-matching 
                // icons have been shown to the player, 
                // so ignore any clicks if the timer is running
                if (timer1.Enabled == true)
                    return;


                    
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                // If firstClicked is null, this is the first icon 
                // in the pair that the player clicked,
                // so set firstClicked to the label that the player 
                // clicked, change its color to black, and return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;

                    return;
                }

                // If the player gets this far, the timer isn't
                // running and firstClicked isn't null,
                // so this must be the second icon the player clicked
                // Set its color to black
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                CheckForWinner();

                // If the player clicked two matching icons, keep them 
                // black and reset firstClicked and secondClicked 
                // so the player can click another icon
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // If the player gets this far, the player 
                // clicked two different icons, so start the 
                // timer (which will wait three quarters of 
                // a second, and then hide the icons)
                timer1.Start();

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            //Hide both icons
            
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;


            //Reset firstClicked and secondClicked
            //so that the next time a label is clicked,
            //the program knows it's the first click
            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Log(time, w);
            }

            timer2.Stop();
            if (HighScore() < time)
            {
                MessageBox.Show("wow. good job. great effort.\n it only took you " + time + " seconds.\nHighscore: " + HighScore(),
                "you're really going places.");
                Close();
            }
            else
            {
                MessageBox.Show("NEW HIGH SCORE HOLY SHIT!!!\nHigh Score: " + HighScore());
            }
            
        }

        private static void Log(int logMessage, TextWriter w)
        {
            w.WriteLine(logMessage);

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            time++;
            timeTaken.Text = "Time : " + time;
        }

        private static int HighScore()
        {
            int highscore = int.MaxValue;
            string line = "";

            StreamReader sr = new StreamReader(@"log.txt");
            
            while (sr.Peek() != -1)
            {
                line = sr.ReadLine();
                int tempHighScore;
                tempHighScore = int.Parse(line);
                if (tempHighScore < highscore)
                {
                    highscore = tempHighScore;
                }


            }
            return highscore;

        }
    }
}
