using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship_Game_WinForm
{
    public partial class Form1 : Form
    {
        private static int WIN_SCORE = 7;

        private List<Button> playerPositionButtons;
        private List<Button> enemyPositionButtons;

        // for the enemy to randomly pick a location against the player
        private Random rand = new Random();

        private int totalShips = 13;
        private int round = 26;
        private int playerScore;
        private int enemyScore;


        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        /// <summary>
        /// Handles the enemy timer event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnemyPlayTimerEvent(object sender, EventArgs e)
        {
            // if the game is still playable
            if (playerPositionButtons.Count > 0 && round > 0)
            {
                round -= 1;
                lblRounds.Text = "Round: " + round;

                int index = rand.Next(playerPositionButtons.Count);

                // if the enemy (AI) hits the player ship
                if ((string)playerPositionButtons[index].Tag == "playerShip")
                {
                    playerPositionButtons[index].BackgroundImage = Properties.Resources.hit_ship_icon;
                    playerPositionButtons[index].BackColor = Color.Red;
                    lblEnemyMove.Text = playerPositionButtons[index].Name.ToUpper();
                    playerPositionButtons[index].Enabled = false;
                    playerPositionButtons.RemoveAt(index);

                    enemyScore += 1;
                    lblEnemyWins.Text = enemyScore.ToString();
                    EnemyPlayTimer.Stop();
                }
                // if enemy misses the player's ship
                else
                {
                    playerPositionButtons[index].BackgroundImage = Properties.Resources.missed_ship_icon;
                    playerPositionButtons[index].BackColor = Color.Blue;
                    lblEnemyMove.Text = playerPositionButtons[index].Name.ToUpper();
                    playerPositionButtons[index].Enabled = false;
                    playerPositionButtons.RemoveAt(index);

                    EnemyPlayTimer.Stop();

                }

            }

            DetermineEndGameResult();


        }

        /// <summary>
        /// Dtermine who wins, loses or draws in the game
        /// </summary>
        private void DetermineEndGameResult()
        {
            if (round < 1 || enemyScore > WIN_SCORE || playerScore > WIN_SCORE)
            {
                if (playerScore > enemyScore)
                {
                    MessageBox.Show("You Win", "Winning");
                    RestartGame();
                }
                else if (enemyScore > playerScore)
                {
                    MessageBox.Show("Haha, I sank your battle ship Win", "Lost");
                    RestartGame();
                }
                else if (enemyScore == playerScore)
                {
                    MessageBox.Show("No one wins", "Draw");
                    RestartGame();
                }

            }
        }

        /// <summary>
        /// Handles the click event for the attack button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void AttackButtonEvent(object sender, EventArgs e)
        {
            // if user selects a postion from the combo list box
            if (cmBoxEnemyLocation.Text != "")
            {
                // get that selected postion
                string attackPostion = cmBoxEnemyLocation.Text.ToLower();

                int index = enemyPositionButtons.FindIndex(a => a.Name == attackPostion);

                // the player hasnt attacked this postion before and still have more round to play
                if (enemyPositionButtons[index].Enabled && round > 0)
                {
                    round -= 1;
                    lblRounds.Text = "Round: " + round;

                    // if the player hit the enemy ship
                    if ((string)enemyPositionButtons[index].Tag == "enemyShip")
                    {
                        enemyPositionButtons[index].Enabled = false;
                        enemyPositionButtons[index].BackgroundImage = Properties.Resources.hit_ship_icon;
                        enemyPositionButtons[index].BackColor = Color.Red;

                        playerScore += 1;
                        lblPlayerWins.Text = playerScore.ToString();

                        EnemyPlayTimer.Start();
                    }
                    // if the player missed the enemy ship
                    else
                    {
                        enemyPositionButtons[index].Enabled = false;
                        enemyPositionButtons[index].BackgroundImage = Properties.Resources.missed_ship_icon;
                        enemyPositionButtons[index].BackColor = Color.Blue;

                        EnemyPlayTimer.Start();

                    }

                }
            }
            else
            {
                MessageBox.Show("Choose a location from the drop down first", "Information");
            }

        }

        /// <summary>
        /// Handles the click event for the player buttons
        /// </summary>
        /// <param name="sender">the button clicked</param>
        /// <param name="e"></param>

        private void PlayerPositionButtonsEvent(object sender, EventArgs e)
        {
            // allow the player to select the maximum number of ships
            if (totalShips > 0)
            {
                // get the button thats clicked
                Button button = (Button)sender;

                
                button.Enabled = false;
                button.Tag = "playerShip";
                button.BackColor = Color.Orange;
                totalShips -= 1;
            }

            // if player selects all his ships on the baord then allow the player to attck the enemy
            if (totalShips == 0)
            {
                btnAttack.Enabled = true;
                btnAttack.BackColor = Color.Red;
                btnAttack.ForeColor = Color.White;

                lblHelp.Text = "2.Pick the attack postion from the drop down";
            }

        }

        /// <summary>
        /// load up the default functionalities needed for this game
        /// Resets everything back to default upon restart
        /// </summary>
        private void RestartGame()
        {
            // instantiate a new list with all the player buttons
            playerPositionButtons = new List<Button> 
            {
                a1, a2, a3, a4, a5, a6, a7, a8, a9, a0,
                b1, b2, b3, b4, b5, b6, b7, b8, b9, b0,
                c1, c2, c3, c4, c5, c6, c7, c8, c9, c0,
                d1, d2, d3, d4, d5, d6, d7, d8, d9, d0,
                e1, e2, e3, e4, e5, e6, e7, e8, e9 ,e0,
                f1, f2, f3, f4, f5, f6, f7, f8, f9, f0,
                g1, g2, g3, g4, g5, g6, g7, g8 ,g9, g0,
                h1, h2, h3, h4, h5, h6, h7, h8, h9, h0,
                i1, i2, i3, i4, i5, i6, i7, i8, i9, i0,
                j1, j2, j3, j4, j5, j6, j7, j8, j9, j0
            };

            // instantiate a new list with all the player buttons
            enemyPositionButtons = new List<Button>
            {
                k1, k2, k3, k4, k5, k6, k7, k8, k9, k0,
                l1, l2, l3, l4, l5, l6, l7, l8, l9, l0,
                m1, m2, m3, m4, m5, m6, m7, m8, m9, m0,
                n1, n2, n3, n4, n5, n6, n7, n8, n9, n0,
                o1, o2, o3, o4, o5, o6, o7, o8, o9, o0,
                p1, p2, p3, p4, p5, p6, p7, p8, p9, p0,
                q1, q2, q3, q4, q5, q6, q7, q8, q9, q0,
                r1, r2, r3, r4, r5, r6, r7, r7, r9, r0,
                s1, s2, s3, s4, s5, s6, s7, s8, s9, s0

            };

            // clear all the items from the enemy list box
            cmBoxEnemyLocation.Items.Clear();
            cmBoxEnemyLocation.Text = null;

            lblHelp.Text = "1. Click on 13 locations from above to start";

            // loop through all the enemy postion buttons and set its values to default
            for (int i = 0; i < enemyPositionButtons.Count; i++)
            {
                // set the enemy postion buttons to default
                enemyPositionButtons[i].Enabled = true;
                enemyPositionButtons[i].Tag = null;
                enemyPositionButtons[i].BackColor = Color.Gray;
                enemyPositionButtons[i].BackgroundImage = null;

                // add all the enemy postion buttons to the list box
                cmBoxEnemyLocation.Items.Add(enemyPositionButtons[i].Name.ToUpper());


            }

            // loop through all the player postion buttons and set its values to default
            for (int i = 0; i < playerPositionButtons.Count; i++)
            {
                // set the enemy postion buttons to default
                playerPositionButtons[i].Enabled = true;
                playerPositionButtons[i].Tag = null;
                playerPositionButtons[i].BackColor = Color.Gray;
                playerPositionButtons[i].BackgroundImage = null;

            }

            playerScore = 0;
            enemyScore = 0;
            round = 26;
            totalShips = 13;

            lblPlayerWins.Text = playerScore.ToString();
            lblEnemyWins.Text = enemyScore.ToString();
            lblEnemyMove.Text = "K1";

            // disable it so the player is not able to attack the enemy yet 
            btnAttack.Enabled = false;

            // allow the enemy to pick a location
            EnemyLocationPicker();

        }

        /// <summary>
        /// Enemy selects its locations on the board map
        /// </summary>
        private void EnemyLocationPicker()
        {

            for (int i = 0; i < 13; i++)
            {
                // generate random numbers between the amount of enemy location buttons
                int index = rand.Next(enemyPositionButtons.Count);


                // so the enemy (AI) doesnt pick the same button twice
                if (enemyPositionButtons[index].Enabled == true && (string)enemyPositionButtons[index].Tag == null)
                {
                    enemyPositionButtons[index].Tag = "enemyShip";

                    Debug.WriteLine("Enemey Postion: " + enemyPositionButtons[index].Name);
                }
                else
                {
                    // pick another random number until the loop finishes
                    index = rand.Next(enemyPositionButtons.Count);
                }
            }
        }


    }
}
