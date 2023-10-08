using System;
using System.Drawing;
using System.Windows.Forms;

namespace DAUGDAUGLOWLEVELGRAPHICS
{
    public partial class Form1 : Form
    {
        private int score = 0;
        private int circleX = 330;
        private int circleY = 350;
        private int circleX2 = 430; // X-coordinate for the second circle
        private int circleY2 = 350; // Y-coordinate for the second circle
        private int lineX = 365;
        private int lineY = 370;
        private int lineX2 = 465;
        private int lineY2 = 370;
        private int circleSpeedX = 9; // Horizontal speed
        private int circleSpeedY = 7; // Vertical speed
        private bool moveLeft = true; // Flag to control the direction of movement
        private bool moveRight = true; // Flag to control the direction of movement
        private bool isAnimating = false; // Flag to indicate if the animation is running
        private bool complete = false;
        private bool complete2 = false;
        private bool spacebarHeldDown = false;
        private bool isPressed = false;
        Color[] customColors = new Color[]
        {
            Color.FromArgb(0x85, 0xD7, 0xD1), // #85D7D1
            Color.FromArgb(0xA8, 0x85, 0xD7), // #a885d7
            Color.FromArgb(0x85, 0xB4, 0xD7), // #85b4d7
            Color.FromArgb(0x85, 0xD7, 0xA8),  // #85d7a8
            Color.FromArgb(0xD7, 0xD1, 0x85)   // #d7d185 
        };
        private int currentColorIndex = 0;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you ready to play? Press the spacebar to play the game.", "Ready to Play", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                timer1.Start();
                timer2.Start();
            }
            else
            {
                // Handle the case where the player is not ready, e.g., close the application or show a message
                MessageBox.Show("Okay, come back when you're ready!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Close the form or exit the application
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.White, 3);
            Brush brush = new SolidBrush(customColors[currentColorIndex]);

            g.DrawLine(pen, 420, 70, lineX, lineY);
            g.DrawEllipse(pen, circleX, circleY, 70, 70);
            g.FillEllipse(brush, circleX, circleY, 70, 70);

            g.DrawLine(pen, 420, 70, lineX2, lineY2);
            g.DrawEllipse(pen, circleX2, circleY2, 70, 70);
            g.FillEllipse(brush, circleX2, circleY2, 70, 70);
        }
        
        
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if(isAnimating)
            {
                // Update the circle's X-coordinate
                if (moveLeft)
                {
                    circleX -= circleSpeedX;
                    circleY -= circleSpeedY; // Move higher while moving left
                    lineX -= circleSpeedX;
                    lineY -= circleSpeedY;
                }
                else
                {
                    circleX += circleSpeedX;
                    lineX += circleSpeedX;
                    if (circleY < 350)
                    {
                        circleY += circleSpeedY; // Move back down to initial Y-coordinate while moving right
                        lineY += circleSpeedY;
                    }
                }

                // Check if the circle reaches the left edge
                if (circleX <= 20)
                {
                    moveLeft = false; // Change direction to move right
                }

                // Check if the circle reaches the right edge
                if (circleX >= 330)
                {
                    moveLeft = true; // Change direction to move left
                    complete = true;

                }


                //circle2

                // Update the circle's X-coordinate
                if (moveRight)
                {
                    circleX2 += circleSpeedX;
                    circleY2 -= circleSpeedY; // Move higher while moving left
                    lineX2 += circleSpeedX;
                    lineY2 -= circleSpeedY;
                }
                else
                {
                    lineX2 -= circleSpeedX;
                    circleX2 -= circleSpeedX;
                    if (circleY2 < 350)
                    {
                        circleY2 += circleSpeedY; // Move back down to initial Y-coordinate while moving right
                        lineY2 += circleSpeedY;
                    }
                }

                // Check if the circle reaches the left edge
                if (circleX2 >= 740)
                {
                    moveRight = false; // Change direction to move right
                }

                // Check if the circle reaches the right edge
                if (circleX2 <= 430)
                {
                    moveRight = true; // Change direction to move left
                    complete2 = true;
                }

                if(complete && complete2)
                {
                    Brush brush = new SolidBrush(customColors[currentColorIndex]);
                    currentColorIndex = (currentColorIndex + 1) % customColors.Length;
                    scorebox.Text = " Score: " + ++score;
                    isAnimating = false;
                    complete = false;
                    complete2 = false;
                    timer1.Stop();
                }

                // Redraw the form to reflect the new circle position
                this.Invalidate();
            }
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the spacebar key is pressed and the animation is not already running
            if (e.KeyCode == Keys.Space && !spacebarHeldDown)
            {
                spacebarHeldDown = true;
                isPressed = true;

                if (!isAnimating)
                {
                    isAnimating = true; // Start the animation
                    timer1.Start();
                }
                else
                {
                    timer2.Stop();
                    isAnimating = false; // Stop the animation
                    timer1.Stop();
                    DialogResult result = MessageBox.Show("You have failed successfully! Please try again.", "You failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reset();
                    if (result == DialogResult.OK)
                    {
                        reset();
                        timer2.Start();
                    }
                }
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                spacebarHeldDown = false;
            }   
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if(isPressed)
            {
                isPressed = false;
            } else
            {
                timer2.Stop();
                DialogResult result = MessageBox.Show("Time's up! Please try again.", "You failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    reset();
                    timer2.Start();
                }
            }
        }

        private void reset()
        {
            circleX = 330;
            circleY = 350;
            circleX2 = 430;
            circleY2 = 350;
            moveLeft = true;
            moveRight = true;
            complete = false;
            complete2 = false;
            score = 0;
            scorebox.Text = " Score: 0";
            lineX = 365;
            lineY = 370;
            lineX2 = 465;
            lineY2 = 370;
            this.Invalidate();
        }
    }
}
