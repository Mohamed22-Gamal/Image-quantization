using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class DistinctColours
    {
        public static int get_distinct_colour(RGBPixel[,] ImageMatrix)
        {
            for (int i = 0; i < ImageOperations.GetHeight(ImageMatrix); i++)//O(n)
            {
                for (int j = 0; j < ImageOperations.GetWidth(ImageMatrix); j++)//O(n)
                {
                    int red = ImageMatrix[i, j].red;//O(1)
                    int green = ImageMatrix[i, j].green;//O(1)
                    int blue = ImageMatrix[i, j].blue;//O(1)

                    if (!(ImageOperations.arrColour[red, green, blue] == true))//O(1)
                    {
                        ImageOperations.arrColour[red, green, blue] = true;//O(1)
                        ImageOperations.dist_colours.Add(ImageMatrix[i, j]);//O(1)
                    }
                }
            }
            ImageOperations.number_of_coulurs = ImageOperations.dist_colours.Count;//O(1)
            return ImageOperations.number_of_coulurs;//O(1)
        }
    }
}
