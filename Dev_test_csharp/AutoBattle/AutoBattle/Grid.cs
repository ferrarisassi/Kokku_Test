using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        public List<GridBox> grids = new List<GridBox>();
        public int xLenght;
        public int yLength;
        public Grid(int Lines, int Columns)
        {
            xLenght = Lines;
            yLength = Columns;
            //I'm showing the size of the battlefield before drawing it
            Console.WriteLine("The battle field has been created, it size is {0} x {1}\n",Lines, Columns);
            for (int i = 0; i < Lines; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, (Columns * i + j));
                    //it isn't necessary to print the index for every box created
                    //Console.Write($"{newBox.Index}\n");
                    grids.Add(newBox);
                }
            }
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void drawBattlefield(Grid grid)
        {
            int Lines = grid.xLenght;
            int Columns = grid.yLength;
            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    //Everytime the beattlefield is drawn, it is used only new gridbox instead of the game's grid
                    GridBox currentgrid = grid.grids[i * Columns + j];
                    if (currentgrid.ocupied)
                    {
                        //if()
                        Console.Write("[X]\t");
                    }
                    else
                    {
                        Console.Write($"[ ]\t");
                    }
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }

    }
}
