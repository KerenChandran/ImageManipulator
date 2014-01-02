using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

//ImageProcessing Application Student Version
//Created by Mr. Iannotta for ICS4U

namespace ImageProcessing
{
    public partial class frmMain : Form
    {
        private Color[,] original; //this is the original picture - never change the values stored in this array
        private Color[,] transformedPic;  //transformed picture that is displayed

        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //this method draws the transformed picture
            //what ever is stored in transformedPic array will
            //be displayed on the form

            base.OnPaint(e);

            Graphics g = e.Graphics;

            //only draw if picture is transformed
            if (transformedPic != null)
            {
                //get height and width of the transfrormedPic array
                int height = transformedPic.GetUpperBound(0)+1;
                int width = transformedPic.GetUpperBound(1) + 1;

                //create a new Bitmap to be dispalyed on the form
                Bitmap newBmp = new Bitmap(width, height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //loop through each element transformedPic and set the 
                        //colour of each pixel in the bitmalp
                        newBmp.SetPixel(j, i, transformedPic[i, j]);
                    }

                }
                //call DrawImage to draw the bitmap
                g.DrawImage(newBmp, 0, 20, width, height);
            }
            
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            //this method reads in a picture file and stores it in an array

            //try catch should handle any errors for invalid picture files
            try
            {

                //open the file dialog to select a picture file

                OpenFileDialog fd = new OpenFileDialog();

                //create a bitmap to store the file in
                Bitmap bmp;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    //store the selected file into a bitmap
                    bmp = new Bitmap(fd.FileName);

                    //create the arrays that store the colours for the image
                    //the size of the arrays is based on the height and width of the bitmap
                    //initially both the original and transformedPic arrays will be identical
                    original = new Color[bmp.Height, bmp.Width];
                    transformedPic = new Color[bmp.Height, bmp.Width];

                    //load each color into a color array
                    for (int i = 0; i < bmp.Height; i++)//each row
                    {
                        for (int j = 0; j < bmp.Width; j++)//each column
                        {
                            //assign the colour in the bitmap to the array
                            original[i, j] = bmp.GetPixel(j, i);
                            transformedPic[i, j] = original[i, j];
                        }
                    }
                    //this will cause the form to be redrawn and OnPaint() will be called
                    this.Refresh();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Picture File. \n" + ex.Message);
            }
            
        }

        private void mnuProcessDarken_Click(object sender, EventArgs e)
        {            
            if (transformedPic != null) //Runs only if picture is loaded
            {
                //Main Colours used
                int red, green, blue;
                int height = transformedPic.GetLength(0); //Gets the height of the picture
                int width = transformedPic.GetLength(1); //Gets the width of the picture

                //loop through each element in the array
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //Gets the current colour value, decreases it by 10 and stores it in appropriate variable
                        red = transformedPic[i, j].R - 10; 
                        blue = transformedPic[i, j].B - 10; 
                        green = transformedPic[i, j].G - 10; 
                        //Checks if colours are below 0 and forces them to be 0
                        if (red < 0) 
                            red = 0;
                        if (green < 0)
                            green = 0;
                        if (blue < 0)
                            blue = 0;
                        transformedPic[i, j] = Color.FromArgb(red, green, blue); //Rewrites pixel with new RGB values
                    }
                }
            }

            this.Refresh(); //Re-Draws picture
        }

        private void mnuProcessInvert_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                int red, green, blue; //Main Colours used
                int height = transformedPic.GetLength(0); //Gets the height of the picture
                int width = transformedPic.GetLength(1); //Gets the width of the picture

                //loop through each element in the array
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //Subtracts current colour value from 255 and stores it in appropriate variable
                        red = 255 - transformedPic[i, j].R;
                        blue = 255 - transformedPic[i, j].B;
                        green = 255 - transformedPic[i, j].G;
                        transformedPic[i, j] = Color.FromArgb(red, green, blue); //Rewrites pixel with new RGB values
                    }
                }
            }
            this.Refresh(); //Re-Draws picture
        }

        private void mnuProcessWhiten_Click(object sender, EventArgs e)
        {
            int red, green, blue; //Main Colours used
            if (transformedPic != null) //Runs only if a picture loaded
            {
                int height = transformedPic.GetLength(0); //Gets the height of the picture
                int width = transformedPic.GetLength(1); //Gets the width of the picture

                //loop through each element in the array
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //Adds 10 to current colour value and stores it in appropriate variable
                        red = transformedPic[i, j].R + 10;
                        blue = transformedPic[i, j].B + 10;
                        green = transformedPic[i, j].G + 10;
                        //Checks if colours are above 255 and forces them to be 255
                        if (red > 255)
                            red = 255;
                        if (green > 255)
                            green = 255;
                        if (blue > 255)
                            blue = 255;
                        transformedPic[i, j] = Color.FromArgb(red, green, blue); //Rewrites pixel with new RGB values
                    }
                }
            }
            this.Refresh(); //Re-Draws picture                   
        }

        private void mnuProcessReset_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                int height = original.GetLength(0); //Gets the height of the original picture
                int width = original.GetLength(1); //Gets the width of the original picture
                transformedPic = new Color[height, width]; //Re-Initializes transformedPic
                
                //Loads each pixel into 2D Color array
                for (int i = 0; i < height; i++)//each row
                {
                    for (int j = 0; j < width; j++)//each column
                    {
                        //Assign the colour in the bitmap to the array
                        transformedPic[i, j] = original[i, j];
                    }
                }
            }
            this.Refresh(); //Re-Draws the picture
        }

        private void mnuProcessScale50_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                Color[,] storage = new Color [transformedPic.GetLength(0)/2, transformedPic.GetLength(1)/2]; //Creates a storage array that is half the size of the image on screen

                //Loop through each element in the array
                for (int i = 0; i < storage.GetLength(0); i++)
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        storage [i, j] = transformedPic[i*2, j*2]; //Copies every other element in the array
                    }
                }
                transformedPic = new Color[storage.GetLength(0), storage.GetLength(1)]; //Reinitializes transformedPic
                //Loop through each element in the array
                for (int i = 0; i < storage.GetLength(0); i++) 
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        transformedPic[i, j] = storage[i, j]; //Copies new picture to transformedPic
                    }
                }
            }
            this.Refresh(); //Re-Draws picture
        }

        private void mnuProcessScale200_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                Color[,] storage = new Color[2 * transformedPic.GetLength(0), 2 * transformedPic.GetLength(1)]; //Creates a storage array that is doublw the size of the image on screen

                //Loop through each element in the array
                for (int i = 0; i < storage.GetLength(0); i++)
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        storage[i, j] = transformedPic[i / 2, j / 2]; //Copies every element twice in the array
                    }
                }
                transformedPic = new Color[storage.GetLength(0), storage.GetLength(1)]; //Reinitializes transformedPic
                for (int i = 0; i < storage.GetLength(0); i++)
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        transformedPic[i, j] = storage[i, j]; //Copies new picture to transformedPic
                    }
                }
            }
            this.Refresh();  //Re-Draws picture
        }

        private void mnuProcessFlipX_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                int height = transformedPic.GetLength(0); //Gets the height of the original picture
                int width = transformedPic.GetLength(1); //Gets the width of the original picture
                Color[,] storage = new Color[height, width]; //Initializes a storage array to be the same size as the one on screen

                //loop through each element in the array
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        storage[i, j] = transformedPic[i, (width-(j+1))]; //Stores flipped image in storage array
                    }
                }
                transformedPic = new Color[storage.GetLength(0), storage.GetLength(1)]; //Reinitializes array to same size of storage
                //loop through each element in the array
                for (int i = 0; i < storage.GetLength(0); i++)
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        transformedPic[i, j] = storage[i, j]; //Copies storage to transformedPic
                    }
                }
            }
            this.Refresh(); //Re-Draws transformedPic
        }

        private void mnuProcessFlipY_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                int height = transformedPic.GetLength(0); //Gets the height of the original picture
                int width = transformedPic.GetLength(1); //Gets the width of the original picture;
                Color[,] storage = new Color[height, width]; //Initializes a storage array to be the same size as the one on screen

                //loop through each element in the array
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        storage[i, j] = transformedPic[height - (i + 1), j]; //Stores flipped image in storage array
                    }
                }
                transformedPic = new Color[storage.GetLength(0), storage.GetLength(1)]; //Reinitializes array to same size of storage
                //loop through each element in the array
                for (int i = 0; i < storage.GetLength(0); i++)
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        transformedPic[i, j] = storage[i, j]; //Copies storage to transformedPic
                    }
                }
            }
            this.Refresh(); //Re-Draws transformedPic
        }

        private void mnuProcessMirrorH_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                int height = transformedPic.GetLength(0); //Gets the height of the original picture
                int width = transformedPic.GetLength(1); //Gets the width of the original picture
                Color[,] storage = new Color[height, 2*width]; //Creates a new array that is double in width than image on screen
                
                for (int i = 0; i < storage.GetLength(0); i++) //Loops through each element in the array
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        if (j < width) //Copies image to storage 
                            storage[i, j] = transformedPic[i, j];
                        else
                            storage[i, j] = storage[i, width - (j - width)]; //Copies flipped image to storage
                    }
                }

                transformedPic = new Color[storage.GetLength(0), storage.GetLength(1)]; //Reinitializes transformedPic
                
                for (int i = 0; i < transformedPic.GetLength(0); i++) //Loops through each element in the array
                {
                    for (int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        transformedPic[i, j] = storage[i, j]; //Copies array
                    }
                }
            }
            this.Refresh(); //Re-Draws transformedPic on image
        }

        private void mnuProcessMirrorV_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                int height = transformedPic.GetLength(0); //Gets the height of the original picture
                int width = transformedPic.GetLength(1); //Gets the width of the original picture;
                Color[,] storage = new Color[2 * height, width]; //Creates a new array that is double in height than image on screen

                for (int i = 0; i < storage.GetLength(0); i++) //Loops through each element in the array
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        if (i < height) //Copies image to storage 
                            storage[i, j] = transformedPic[i, j];
                        else //Copies flipped image to storage
                            storage[i, j] = storage[height - (i-height), j];
                    }
                }

                transformedPic = new Color[storage.GetLength(0), storage.GetLength(1)]; //Reinitializes transformedPic

                for (int i = 0; i < transformedPic.GetLength(0); i++) //Loops through each element in the array
                {
                    for (int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        transformedPic[i, j] = storage[i, j]; //Copies array
                    }
                }
            }
            this.Refresh(); //Re-Draws transformedPic on image
        }

        private void mnuProcessRotate_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                int height = transformedPic.GetLength(0); //Gets the height of the original picture
                int width = transformedPic.GetLength(1); //Gets the width of the original picture;
                Color[,] storage = new Color[width, height]; //Creates a storage that has switched dimensions

                for (int i = 0; i < storage.GetLength(0); i++) //Loops through each element in the array
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        storage[i, j] = transformedPic[height - (j+1), i]; //Copies rotated pixel to storage
                    }
                }

                transformedPic = new Color[storage.GetLength(0), storage.GetLength(1)]; //Reinitializes transformedPic

                for (int i = 0; i < transformedPic.GetLength(0); i++) //Loops through each element in the array
                {
                    for (int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        transformedPic[i, j] = storage[i, j]; //Copies array
                    }
                }
            }
            this.Refresh(); //Re-Draws transformedPic on image
        }

        private void mnuProcessBlur_Click(object sender, EventArgs e)
        {
            if (transformedPic != null) //Runs only if a picture loaded
            {
                int height = transformedPic.GetLength(0); //Gets the height of the original picture
                int width = transformedPic.GetLength(1); //Gets the width of the original picture;
                Color[,] storage = new Color[height, width]; //Creates an array with the same dimensions of storage

                int red, blue, green; //Initializes RGB variables

                for (int i = 0; i < storage.GetLength(0); i++) //Loops through each element in the array
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        storage[i, j] = transformedPic[i, j]; //Copies Picture to storage
                    }
                }

                for (int i = 0; i < storage.GetLength(0); i++) //Loops through each element in the array
                {
                    for (int j = 0; j < storage.GetLength(1); j++)
                    {
                        if (i == 0) //Checks if it is the left column
                        {
                            if (j == 0) //Top Left Pixel
                            {
                                red = (storage[i, j + 1].R + storage[i + 1, j].R + storage[i + 1, j + 1].R) / 3;
                                blue = (storage[i, j + 1].B + storage[1, j].B + storage[i + 1, j + 1].B) / 3;
                                green = (storage[i, j + 1].G + storage[1, j].G + storage[i + 1, j + 1].G) / 3;
                            }
                            else if (j == width - 1) //Bottom Right Pixel
                            {
                                red = (storage[i, j - 1].R + storage[i + 1, j].R + storage[i + 1, j - 1].R) / 3;
                                blue = (storage[i, j - 1].B + storage[i + 1, j].B + storage[i + 1, j - 1].B) / 3;
                                green = (storage[i, j - 1].G + storage[i + 1, j].G + storage[i + 1, j - 1].G) / 3;
                            }
                            else //Rest of the column
                            {
                                red = (storage[i, j - 1].R + storage[i + 1, j - 1].R + storage[i, j + 1].R + storage[i + 1, j].R + storage[i + 1, j + 1].R) / 5;
                                blue = (storage[i, j - 1].B + storage[i + 1, j - 1].B + storage[i, j + 1].B + storage[i + 1, j].B + storage[i + 1, j + 1].B) / 5;
                                green = (storage[i, j - 1].R + storage[i + 1, j - 1].R + storage[i, j + 1].R + storage[i + 1, j].G + storage[i + 1, j + 1].G) / 5;
                            }
                        }
                        else if (i == height - 1) //Checks if it is the right corner
                        {
                            if (j == 0) //Top Right Pixel
                            {
                                red = (storage[i - 1, j].R + storage[i, j + 1].R + storage[i - 1, j + 1].R) / 3;
                                blue = (storage[i - 1, j].B + storage[i, j + 1].B + storage[i - 1, j + 1].B) / 3;
                                green = (storage[i - 1, j].G + storage[i, j + 1].G + storage[i - 1, j + 1].G) / 3;
                            }
                            else if (j == width - 1) //Bottom Right Pixel
                            {
                                red = (storage[i - 1, j].R + storage[i, j - 1].R + storage[i - 1, j - 1].R) / 3;
                                blue = (storage[i - 1, j].B + storage[i, j - 1].B + storage[i - 1, j - 1].B) / 3;
                                green = (storage[i - 1, j].G + storage[i, j - 1].G + storage[i - 1, j - 1].G) / 3;
                            }
                            else //Rest of the column
                            {
                                red = (storage[i, j - 1].R + storage[i - 1, j - 1].R + storage[i - 1, j].R + storage[i - 1, j + 1].R + storage[i, j + 1].R) / 5;
                                blue = (storage[i, j - 1].B + storage[i - 1, j - 1].B + storage[i - 1, j].B + storage[i - 1, j + 1].B + storage[i, j + 1].B) / 5;
                                green = (storage[i, j - 1].G + storage[i - 1, j - 1].G + storage[i - 1, j].G + storage[i - 1, j + 1].G + storage[i, j + 1].G) / 5;
                            }
                        }
                        else if (j == 0) //Top row
                        {
                            red = (storage[i - 1, j].R + storage[i + 1, j].R + storage[i, j + 1].R) / 3;
                            blue = (storage[i - 1, j].B + storage[i + 1, j].B + storage[i, j + 1].B) / 3;
                            green = (storage[i - 1, j].G + storage[i + 1, j].G + storage[i, j + 1].G) / 3;
                        }
                        else if (j == width - 1) //Bottom Row
                        {
                            red = (storage[i - 1, j].R + storage[i - 1, j - 1].R + storage[i, j - 1].R + storage[i + 1, j - 1].R + storage[i + 1, j].R) / 5;
                            blue = (storage[i - 1, j].B + storage[i - 1, j - 1].B + storage[i, j - 1].B + storage[i + 1, j - 1].B + storage[i + 1, j].B) / 5;
                            green = (storage[i - 1, j].G + storage[i - 1, j - 1].G + storage[i, j - 1].G + storage[i + 1, j - 1].G + storage[i + 1, j].G) / 5;
                        }
                        else //Any other pixel
                        {
                            red = (storage[i - 1, j - 1].R + storage[i - 1, j].R + storage[i - 1, j + 1].R + storage[i, j - 1].R + storage[i, j + 1].R + storage[i + 1, j - 1].R + storage[i + 1, j].R + storage[i + 1, j + 1].R) / 8;
                            blue = (storage[i - 1, j - 1].B + storage[i - 1, j].B + storage[i - 1, j + 1].B + storage[i, j - 1].B + storage[i, j + 1].B + storage[i + 1, j - 1].B + storage[i + 1, j].B + storage[i + 1, j + 1].B) / 8;
                            green = (storage[i - 1, j - 1].G + storage[i - 1, j].G + storage[i - 1, j + 1].G + storage[i, j - 1].G + storage[i, j + 1].G + storage[i + 1, j - 1].G + storage[i + 1, j].G + storage[i + 1, j + 1].G) / 8;
                        }                        
                        transformedPic[i, j] = Color.FromArgb(red,green,blue); //Rewrites transforemdPic with new RGB values
                    }
                }
            }
            this.Refresh(); //Re-Draws image on screen
        }
    }
}
