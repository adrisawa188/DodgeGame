using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DodgeGame
{
    public partial class Form1 : Form
    {
        //global variables 
        Rectangle hero;
        int heroSpeed = 5;

        List<Rectangle> leftObstacles = new List<Rectangle>();
        List<Rectangle> rightObstacles = new List<Rectangle>();

        int obstacleHeight = 50;
        int obstacleWidth = 10;
        int leftObstacleX = 150;
        int rightObstacleX = 350;

        int leftObstacleSpeed = 6;
        int rightObstacleSpeed = -7;

        int newLeftObstacle;
        int newRightObstacle;

        string gameState = "waiting"; 

        bool upArrowDown;
        bool leftArrowDown;
        bool downArrowDown;
        bool rightArrowDown;


        SolidBrush heroBrush = new SolidBrush(Color.Goldenrod);
        SolidBrush obstacleBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();        
        }

        public void GameInit()
        {
            titleLabel.Text = "";
            subTitleLabel.Text = "";
            gameState = "running";

            hero = new Rectangle(10, (this.Height / 2) - 10, 20, 20);
            leftObstacles.Clear();
            leftObstacles.Add(new Rectangle(leftObstacleX, 0 - obstacleHeight, obstacleWidth, obstacleHeight));
            rightObstacles.Clear();
            rightObstacles.Add(new Rectangle(rightObstacleX, this.Height, obstacleWidth, obstacleHeight));

            gameTimer.Enabled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over" || gameState == "win")
                    {
                        GameInit();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over" || gameState == "win")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move hero
            if (upArrowDown == true && hero.Y > 0)
            {
                hero.Y -= heroSpeed;
            }
            if (leftArrowDown == true && hero.X > 0)
            {
                hero.X -= heroSpeed;
            }
            if (downArrowDown == true && hero.Y < this.Height - hero.Height)
            {
                hero.Y += heroSpeed;
            }
            if (rightArrowDown == true && hero.X < this.Width - hero.Height)
            {
                hero.X += heroSpeed;
            }

            //move left and right obstacles
            for (int i = 0; i < leftObstacles.Count(); i++)
            {
                //find the new postion of y based on speed
                int y = leftObstacles[i].Y + leftObstacleSpeed;

                //replace the rectangle in the list with updated one using new y
                leftObstacles[i] = new Rectangle(leftObstacles[i].X, y, obstacleWidth, obstacleHeight);
            }
            for (int i = 0; i < rightObstacles.Count(); i++)
            {
                //find the new postion of y based on speed
                int y = rightObstacles[i].Y + rightObstacleSpeed;

                //replace the rectangle in the list with updated one using new y
                rightObstacles[i] = new Rectangle(rightObstacles[i].X, y, obstacleWidth, obstacleHeight);
            }

            //increase new obstacle counter and check if time to add new
            newLeftObstacle++;

            if (newLeftObstacle == 22)
            {
                leftObstacles.Add(new Rectangle(leftObstacleX, 0 - obstacleHeight, obstacleWidth, obstacleHeight));

                newLeftObstacle = 0;
            }

            newRightObstacle++;

            if (newRightObstacle == 20)
            {
                rightObstacles.Add(new Rectangle(rightObstacleX, this.Height, obstacleWidth, obstacleHeight));

                newRightObstacle = 0;
            }

            //remove obstacles off screen

            for (int i = 0; i < leftObstacles.Count(); i++)
            {

                if (leftObstacles[i].Y > this.Height)
                {
                    leftObstacles.RemoveAt(i);
                }
            }
            for (int i = 0; i < rightObstacles.Count(); i++)
            {

                if (rightObstacles[i].Y > this.Height)
                {
                    rightObstacles.RemoveAt(i);
                }
            }

            //stop if player collides with obstacles

            for (int i = 0; i < leftObstacles.Count(); i++)
            {
                if (hero.IntersectsWith(leftObstacles[i]))
                {
                    gameTimer.Enabled = false;
                    gameState = "over";
                }
            }
            for (int i = 0; i < rightObstacles.Count(); i++)
            {
                if (hero.IntersectsWith(rightObstacles[i]))
                {
                    gameTimer.Enabled = false;
                    gameState = "over";
                }
            }

            //stop if player touches right side

            if (hero.X == this.Width - hero.Width)
            {
                gameTimer.Enabled = false;
                gameState = "win";
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                titleLabel.Text = "Dodger";
                subTitleLabel.Text = "Press Space to Start or Escape to Exit";
            }
            else if (gameState == "running")
            {
                //draw hero
                e.Graphics.FillRectangle(heroBrush, hero);

                //draw obstacles
                for (int i = 0; i < leftObstacles.Count(); i++)
                {
                    e.Graphics.FillRectangle(obstacleBrush, leftObstacles[i]);
                }
                for (int i = 0; i < rightObstacles.Count(); i++)
                {
                    e.Graphics.FillRectangle(obstacleBrush, rightObstacles[i]);
                }
            }
            else if (gameState == "over")
            {
                gameTimer.Enabled = false;
                titleLabel.Text = "Game Over";
                subTitleLabel.Text = "Press Space to try Again or Escape to Exit";
            }    
            else if (gameState == "win")
            {
                gameTimer.Enabled = false;
                titleLabel.Text = "You Made It!!";
                subTitleLabel.Text = "Press Space to try Again or Escape to Exit";
            }
        }
    }
}
