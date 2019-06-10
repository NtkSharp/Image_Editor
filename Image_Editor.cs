using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image_Editor : MonoBehaviour
{
    //The texture2D that you want to edit goes here
    public Texture2D InputImage;
    //Here we will store the width, height of the InputImage
    int Width=0;
    int Height=0;

    //The matrix with the colors of the image
    Color[,] ColorMatrix;
    //Temporary ColorMatrix for the application of the convolution
    Color[,] TempColorMatrix;
    //The edited image will be stored here
    Texture2D OutputImage;

    //The matrix that we will convolve our image with
    //---------------Do not change the size of this array in the editor----------------
    public Vector3[] ConvolutionMatrix = new Vector3[3];
    public bool Convolve=false;


    


    void Start()
    {
        Width = InputImage.width;
        Height = InputImage.height;
        ColorMatrix = new Color[Width, Height];
        GetColors();
        OutputImage = SetColors(ColorMatrix);
        SetTexture2DAsSprite();

    }

   
    void Update()
    {
        if (Convolve)
        {
            ApplyConvolution();
            OutputImage = SetColors(ColorMatrix);
            SetTexture2DAsSprite();
            Convolve = false;
        }

        
        
    }


   
    //Gets the color values out of a Textrure2D and stores them to the ColorMatrix
    void GetColors()
    {
        for (int i = 0; i < Width; i++)
        {

            for (int j = 0; j < Height; j++)
            {
                ColorMatrix[i, j] = InputImage.GetPixel(i, j);
              
            }
        }

      
    }

    //Renders the ColorMatrix to a Texture2D
    Texture2D SetColors(Color[,] InputMatrix)
    {
        Texture2D OutputTexture= new Texture2D(InputMatrix.GetLength(0), InputMatrix.GetLength(1)); 

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
               
                OutputTexture.SetPixel(i, j, InputMatrix[i, j]);

            }
        }

        OutputTexture.Apply();
        return OutputTexture;

    }

   


    //Sets the Texture2D as a sprite in the gameobject
    void SetTexture2DAsSprite()
    {
       
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(OutputImage, new Rect(0.0f, 0.0f, OutputImage.width, OutputImage.height), new Vector2(0.5f, 0.5f), 100.0f);


    }

    void ApplyConvolution()
    {
        CopyColorMatrix();

        for(int i = 1; i < Width - 1; i++)
        {
            for(int j = 1; j < Height - 1; j++)
            {

                TempColorMatrix[i, j] = SetPointwiseConvolution(i, j); 

            }
        }

        ColorMatrix = TempColorMatrix;

    }

    void CopyColorMatrix()
    {
        TempColorMatrix = new Color[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                TempColorMatrix[i, j] = ColorMatrix[i, j];

            }
        }
    }

    //Calculates the convolution for a pixel
    Color SetPointwiseConvolution(int PositionX,int PositionY)
    {
        
        Color OutputColor=new Color(0,0,0);
        OutputColor.a = 1;
        Vector3[] CM = ConvolutionMatrix;
        //Setting the red values
        OutputColor.r = CM[0].x * ColorMatrix[PositionX - 1, PositionY - 1].r + CM[0].y * ColorMatrix[PositionX, PositionY - 1].r + CM[0].z* ColorMatrix[PositionX + 1, PositionY - 1].r;
        OutputColor.r = OutputColor.r + CM[1].x * ColorMatrix[PositionX - 1, PositionY].r + CM[1].y * ColorMatrix[PositionX, PositionY].r + CM[1].z * ColorMatrix[PositionX + 1, PositionY].r;
        OutputColor.r = OutputColor.r + CM[2].x * ColorMatrix[PositionX - 1, PositionY+1].r + CM[2].y * ColorMatrix[PositionX, PositionY+1].r + CM[2].z * ColorMatrix[PositionX + 1, PositionY+1].r;
        //Setting the green values
        OutputColor.g = CM[0].x * ColorMatrix[PositionX - 1, PositionY - 1].g + CM[0].y * ColorMatrix[PositionX, PositionY - 1].g + CM[0].z * ColorMatrix[PositionX + 1, PositionY - 1].g;
        OutputColor.g = OutputColor.g + CM[1].x * ColorMatrix[PositionX - 1, PositionY].g + CM[1].y * ColorMatrix[PositionX, PositionY].g + CM[1].z * ColorMatrix[PositionX + 1, PositionY].g;
        OutputColor.g = OutputColor.g + CM[2].x * ColorMatrix[PositionX - 1, PositionY + 1].g + CM[2].y * ColorMatrix[PositionX, PositionY + 1].g + CM[2].z * ColorMatrix[PositionX + 1, PositionY + 1].g;
        //Setting the blue values
        OutputColor.b = CM[0].x * ColorMatrix[PositionX - 1, PositionY - 1].b + CM[0].y * ColorMatrix[PositionX, PositionY - 1].b + CM[0].z * ColorMatrix[PositionX + 1, PositionY - 1].b;
        OutputColor.b = OutputColor.b + CM[1].x * ColorMatrix[PositionX - 1, PositionY].b + CM[1].y * ColorMatrix[PositionX, PositionY].b + CM[1].z * ColorMatrix[PositionX + 1, PositionY].b;
        OutputColor.b = OutputColor.b + CM[2].x * ColorMatrix[PositionX - 1, PositionY + 1].b + CM[2].y * ColorMatrix[PositionX, PositionY + 1].b + CM[2].z * ColorMatrix[PositionX + 1, PositionY + 1].b;

        return OutputColor;

    }



}
