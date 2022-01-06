// GeneticsDevTwo Demonstrate Genetic Algorythm Techniques
// Copyright (C) 2006 pseudonym67
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

using System;
using BoardControl;
using System.Drawing;

namespace GeneticsDevTwo
{
	/// <summary>
	/// Summary description for GeneticsSquare.
	/// </summary>
	public class GeneticsSquare : BasicSquare
	{
		private bool bDrawPath;
		private bool bDrawUp;
		private bool bDrawRight;
		private bool bDrawDown;
		private bool bDrawLeft;
		private bool bBlocked;
		private bool bDrawFinish;
		private bool bDrawStart;
		private bool bIsStart;
		private bool bIsFinish;
		
		/// current attempt variables
		/// 
		private bool bDrawCurrentAttemptUp;
		private bool bDrawCurrentAttemptRight;
		private bool bDrawCurrentAttemptDown;
		private bool bDrawCurrentAttemptLeft;
		
		/// previous solution variables
		///
		private bool bDrawPreviousSolutionUp;
		private bool bDrawPreviousSolutionRight;
		private bool bDrawPreviousSolutionDown;
		private bool bDrawPreviousSolutionLeft;
		
		/// current solution variables
		/// 
		private bool bDrawCurrentSolutionUp;
		private bool bDrawCurrentSolutionRight;
		private bool bDrawCurrentSolutionDown;
		private bool bDrawCurrentSolutionLeft;
		
		private static bool bDrawPreviousSolution;
		private static bool bDrawCurrentSolution;
		private static bool bDrawCurrentAttempt;
        private static bool bDrawBlocked;

		private Color cPath;
		private Color cPreviousSolutionOne;
		private Color cPreviousSolutionTwo;
		private Color cCurrentSolutionOne;
		private Color cCurrentSolutionTwo;
		private Color cCurrentAttemptOne;
		private Color cCurrentAttemptTwo;
		private Color cBlocked;
		private Color cStart;
		private Color cFinish;

		private static Pen pPath = null;
		private static Pen pPreviousSolutionOne = null;
		private static Pen pPreviousSolutionTwo = null;
		private static Pen pCurrentSolutionOne = null;
		private static Pen pCurrentSolutionTwo = null;
		private static Pen pCurrentAttemptOne = null;
		private static Pen pCurrentAttemptTwo = null;
		private static Pen pBlocked = null;
		private static Brush brushStart = null;
		private static Brush brushFinish = null;
		private static Brush brushBlocked = null;

		private const int nWidth = 5;

		private int nCurrentSolutionIncrement = 1;
		private int nCurrentAttemptIncrement = 3;
		private int nPreviousSolutionIncrement = 5;

		public bool DrawPath
		{
			get
			{
				return bDrawPath;
			}
			set
			{
				bDrawPath = value;
			}
		}

		public bool DrawUp
		{
			get
			{
				return bDrawUp;
			}
			set
			{
				bDrawUp = value;
			}
		}

		public bool DrawRight
		{
			get
			{
				return bDrawRight;
			}
			set
			{
				bDrawRight = value;
			}
		}

		public bool DrawDown
		{
			get
			{
				return bDrawDown;
			}
			set
			{
				bDrawDown = value;
			}
		}

		public bool DrawLeft
		{
			get
			{
				return bDrawLeft;
			}
			set
			{
				bDrawLeft = value;
			}
		}

        public bool DrawBlocked
        {
            get
            {
                return bDrawBlocked;
            }
            set
            {
                bDrawBlocked = value;
            }
        }

		public bool Blocked
		{
			get
			{
				return bBlocked;
			}
			set
			{
				bBlocked = value;
			}
		}
		
		public bool DrawCurrentAttemptUp
		{
			get
			{
				return bDrawCurrentAttemptUp;
			}
			set
			{
				bDrawCurrentAttemptUp = value;
			}
		}
		
		public bool DrawCurrentAttemptRight
		{
			get
			{
				return bDrawCurrentAttemptRight;
			}
			set
			{
				bDrawCurrentAttemptRight = value;
			}
		}
		
		public bool DrawCurrentAttemptDown
		{
			get
			{
				return bDrawCurrentAttemptDown;
			}
			set
			{
				bDrawCurrentAttemptDown = value;
			}
		}
		
		public bool DrawCurrentAttemptLeft
		{
			get
			{
				return bDrawCurrentAttemptLeft;
			}
			set
			{
				bDrawCurrentAttemptLeft = value;
			}
		}
		
		public bool DrawPreviousSolutionUp
		{
			get
			{
				return bDrawPreviousSolutionUp;
			}
			set
			{
				bDrawPreviousSolutionUp = value;
			}
		}
		
		public bool DrawPreviousSolutionRight
		{
			get
			{
				return bDrawPreviousSolutionRight;
			}
			set
			{
				bDrawPreviousSolutionRight = value;
			}
		}
		
		public bool DrawPreviousSolutionDown
		{
			get
			{
				return bDrawPreviousSolutionDown;
			}
			set
			{
				bDrawPreviousSolutionDown = value;
			}
		}
		
		public bool DrawPreviousSolutionLeft
		{
			get
			{
				return bDrawPreviousSolutionLeft;
			}
			set
			{
				bDrawPreviousSolutionLeft = value;
			}
		}
		
		public bool DrawCurrentSolutionUp
		{
			get
			{
				return bDrawCurrentSolutionUp;
			}
			set
			{
				bDrawCurrentSolutionUp = value;
			}
		}
		
		public bool DrawCurrentSolutionRight
		{
			get
			{
				return bDrawCurrentSolutionRight;
			}
			set
			{
				bDrawCurrentSolutionRight = value;
			}
		}
		
		public bool DrawCurrentSolutionDown 
		{
			get
			{
				return bDrawCurrentSolutionDown;
			}
			set
			{
				bDrawCurrentSolutionDown = value;	
			}
		}
		
		public bool DrawCurrentSolutionLeft
		{
			get
			{
				return bDrawCurrentSolutionLeft;
			}
			set
			{
				bDrawCurrentSolutionLeft = value;
			}
		}

		public bool DrawPreviousSolution
		{
			get
			{
				return bDrawPreviousSolution;
			}
			set
			{
				bDrawPreviousSolution = value;
			}
		}

		public bool DrawCurrentSolution
		{
			get
			{
				return bDrawCurrentSolution;
			}
			set
			{
				bDrawCurrentSolution = value;
			}
		}

		public bool DrawCurrentAttempt
		{
			get
			{
				return bDrawCurrentAttempt;
			}
			set
			{
				bDrawCurrentAttempt = value;
			}
		}

		public bool DrawStart
		{
			get
			{
				return bDrawStart;
			}
			set
			{
				bDrawStart = value;
				if( bDrawStart == true )
				{
					bIsStart = true; 
				}
				else
					bIsStart = false;
			}
		}

		public bool DrawFinish
		{
			get
			{
				return bDrawFinish;
			}
			set
			{
				bDrawFinish = value;
				if( bDrawFinish == true )
				{
					bIsFinish = true;
				}
				else
				{
					bIsFinish = false;
				}
			}
		}

		public bool IsStart
		{
			get
			{
				return bIsStart;
			}
			set
			{
				bIsStart = value;
			}
		}

		public bool IsFinish
		{
			get
			{
				return bIsFinish;
			}
			set
			{
				bIsFinish = value;
			}
		}

		private GeneticsSquare()
		{
		}

		public GeneticsSquare( int squareWidth, int squareHeight, int squareHorizontalLocation, int squareVerticalLocation, string identifier ) : base( squareWidth, squareHeight, squareHorizontalLocation, squareVerticalLocation, identifier )
		{
			bDrawPath = true;
			DrawUp = false;
			DrawRight = false;
			DrawDown = false;
			DrawLeft = false;
			DrawCurrentAttemptUp = false;
			DrawCurrentAttemptRight = false;
			DrawCurrentAttemptDown = false;
			DrawCurrentAttemptLeft = false;
			DrawPreviousSolutionUp = false;
			DrawPreviousSolutionRight = false;
			DrawPreviousSolutionDown = false;
			DrawPreviousSolutionLeft = false;
			DrawCurrentSolutionUp = false;
			DrawCurrentSolutionRight = false;
			DrawCurrentSolutionDown = false;
			DrawCurrentSolutionLeft = false;
			DrawPreviousSolution = false;
			DrawCurrentSolution = false;
			DrawCurrentAttempt = false;
			Blocked = false;
			bIsStart = false;
			bIsFinish = false;
			DrawStart = false;
			DrawFinish = false;

			cPath = Color.Gold;
			cPreviousSolutionOne = Color.Red;
			cPreviousSolutionTwo = Color.PowderBlue;
			cCurrentSolutionOne = Color.Yellow;
			cCurrentSolutionTwo = Color.Green;
			cCurrentAttemptOne = Color.Brown;
			cCurrentAttemptTwo = Color.White;
			cBlocked = Color.Black;
			cStart = Color.Azure;
			cFinish = Color.Azure;

			if( pPath == null )
			{
				pPath = new Pen( cPath );
				pPath.Width = 10;
			}

			if( pPreviousSolutionOne == null )
			{
				pPreviousSolutionOne = new Pen( cPreviousSolutionOne );
				pPreviousSolutionOne.Width = nWidth;
			}

			if( pPreviousSolutionTwo == null )
			{
				pPreviousSolutionTwo = new Pen( cPreviousSolutionTwo );
				pPreviousSolutionTwo.Width = nWidth;
			}

			if( pCurrentSolutionOne == null )
			{
				pCurrentSolutionOne = new Pen( cCurrentSolutionOne );
				pCurrentSolutionOne.Width = nWidth;
			}

			if( pCurrentSolutionTwo == null )
			{
				pCurrentSolutionTwo = new Pen( cCurrentSolutionTwo );
				pCurrentSolutionTwo.Width = nWidth;
			}

			if( pCurrentAttemptOne == null )
			{
				pCurrentAttemptOne = new Pen( cCurrentAttemptOne );
				pCurrentAttemptOne.Width = nWidth;
			}

			if( pCurrentAttemptTwo == null )
			{
				pCurrentAttemptTwo = new Pen( cCurrentAttemptTwo );
				pCurrentAttemptTwo.Width = nWidth;
			}

			if( pBlocked == null )
			{
				pBlocked = new Pen( cBlocked );
			}

			if( brushStart == null )
			{
				brushStart = new SolidBrush( cStart );
			}

			if( brushFinish == null )
			{
				brushFinish = new SolidBrush( cFinish );
			}

			if( brushBlocked == null )
			{
				brushBlocked = new SolidBrush( cBlocked );
			}
		}
		
		public void ClearCurrentAttempt()
		{
			DrawCurrentAttemptUp = false;
			DrawCurrentAttemptRight = false;
			DrawCurrentAttemptDown = false;
			DrawCurrentAttemptLeft = false;
		}
		
		public void ClearCurrentSolution()
		{
			DrawCurrentSolutionUp = false;
			DrawCurrentSolutionRight = false;
			DrawCurrentSolutionDown = false;
			DrawCurrentSolutionLeft = false;
		}
		
		public void ClearPreviousSolution()
		{
			DrawPreviousSolutionUp = false;
			DrawPreviousSolutionRight = false;
			DrawPreviousSolutionDown = false;
			DrawPreviousSolutionLeft = false;
		}

		public override void DrawSquare(Graphics grfx)
		{
			if( IsValid == true )
				return;

			base.DrawSquare (grfx);

			float x1 = 0;
			float y1 = 0;
			float x2 = 0;
			float y2 = 0;

            bool bOriginalUp = DrawUp;
            bool bOriginalRight = DrawRight;
            bool bOriginalDown = DrawDown;
            bool bOriginalLeft = DrawLeft;

			if( bDrawPath == true )
			{
                /// special finish conditions
                /// 
                if( IsFinish == true )
                {
                    if( DrawUp == true )
                        DrawDown = true;
                    if( DrawRight == true )
                        DrawLeft = true;
                    if( DrawDown == true )
                        DrawUp = true;
                    if( DrawLeft == true )
                        DrawRight = true;
                }

				if( DrawUp == true )
				{
                    if( bOriginalUp == true )
                    {
					    x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
					    y1 = SquareVerticalLocation;
					    x2 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
					    y2 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );

					    if( DrawLeft == true )
					    {
			    ///			if( DrawPreviousSolution == false || DrawCurrentSolution == false || DrawCurrentAttempt == false )
						    {
							    y2 = SquareVerticalLocation + SquareHeight/2 - nWidth;
						    }
					    }

					    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );

					    x1 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
					    y1 = SquareVerticalLocation;
					    x2 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
					    y2 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );

					    if( DrawRight == true )
					    {
				    ///		if( DrawPreviousSolution == false || DrawCurrentSolution == false || DrawCurrentAttempt == false )
						    {
							    y2 = SquareVerticalLocation + SquareHeight/2 - nWidth;
						    }
					    }

					    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );

					    if( DrawRight == false && DrawDown == false && DrawLeft == false )
					    {
						    x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
						    y1 = SquareVerticalLocation + SquareHeight/2 + ( nWidth );
						    x2 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
						    y2 = SquareVerticalLocation + SquareHeight/2 + ( nWidth );

						    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
					    }
                    }

					if( DrawPreviousSolution == true && DrawPreviousSolutionUp == true )
					{
						x1 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
						y1 = SquareVerticalLocation;
						x2 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
						y2 = SquareVerticalLocation + SquareHeight/2 - ( nWidth/2 );

						for( int i=( int )y1 + LegendWidth;i<y2+LegendWidth;i+=nPreviousSolutionIncrement)
						{
							if( i%2 == 0 )
							{
								if( ( i+nPreviousSolutionIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pPreviousSolutionOne, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pPreviousSolutionOne, x1 + LegendWidth, i, x2 + LegendWidth, i+nPreviousSolutionIncrement );
							}
							else
							{
								if( ( i+nPreviousSolutionIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pPreviousSolutionTwo, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pPreviousSolutionTwo, x1 + LegendWidth, i, x2 + LegendWidth, i+nPreviousSolutionIncrement );
							}
						}
					}

					if( DrawCurrentSolution == true && DrawCurrentSolutionUp == true )
					{
						x1 = SquareHorizontalLocation + SquareWidth/2;
						y1 = SquareVerticalLocation;
						x2 = SquareHorizontalLocation + SquareWidth/2;
						y2 = SquareVerticalLocation + SquareHeight/2 + nWidth;

						if( DrawLeft == true || DrawRight == true && DrawPreviousSolution == false || DrawCurrentSolution == false || DrawCurrentAttempt == false )
						{
							y2 = SquareVerticalLocation + SquareHeight/2;
						}

						for( int i=( int )y1 + LegendWidth;i<y2+LegendWidth;i+=nCurrentSolutionIncrement )
						{
							if( i%2 == 0 )
							{
								if( ( i+nCurrentSolutionIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentSolutionOne, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentSolutionOne, x1 + LegendWidth, i, x2 + LegendWidth, i+nCurrentSolutionIncrement );
							}
							else
							{
								if( ( i+nCurrentSolutionIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentSolutionTwo, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentSolutionTwo, x1 + LegendWidth, i, x2 + LegendWidth, i+nCurrentSolutionIncrement );
							}
						}
					}

					if( DrawCurrentAttempt == true && DrawCurrentAttemptUp == true )
					{
						x1 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						y1 = SquareVerticalLocation;
						x2 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						y2 = SquareVerticalLocation + SquareHeight/2 + nWidth;
/*
						if( DrawLeft == true || DrawRight == true && DrawPreviousSolution == false || DrawCurrentSolution == false || DrawCurrentAttempt == false )
						{
							y2 = SquareVerticalLocation + SquareHeight/2;
						}*/

						for( int i=( int )y1+LegendWidth;i<y2+LegendWidth;i+=nCurrentAttemptIncrement )
						{
							if( i%2 == 0 )
							{
								if( ( i+nCurrentAttemptIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentAttemptOne, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentAttemptOne, x1 + LegendWidth, i, x2 + LegendWidth, i+nCurrentAttemptIncrement );
							}
							else
							{
								if( ( i+nCurrentAttemptIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentAttemptTwo, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentAttemptTwo, x1 + LegendWidth, i, x2 + LegendWidth, i+nCurrentAttemptIncrement );
							}
						}
					}
				}

				if( DrawRight == true )
				{
                    if( bOriginalRight == true )
                    {
					    x1 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
					    y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );
					    x2 = SquareHorizontalLocation + SquareWidth;
					    y2 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );

					    if( DrawUp == false )
					    {
						    x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*3 );
					    }

					    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );

					    x1 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
					    y1 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );
					    x2 = SquareHorizontalLocation + SquareWidth;
					    y2 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );

					    if( DrawDown == false )
					    {
						    x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*3 );
					    }

					    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );

					    if( DrawUp == false && DrawDown == false && DrawLeft == false )
					    {
						    x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
						    y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );
						    x2 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
						    y2 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );

						    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
					    }
                    }

					if( DrawPreviousSolution == true && DrawPreviousSolutionRight == true )
					{
						x1 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						y1 = SquareVerticalLocation + SquareHeight/2 - nWidth;
						x2 = SquareHorizontalLocation + SquareWidth;
						y2 = SquareVerticalLocation + SquareHeight/2 - nWidth;

						if( DrawLeft == true || DrawDown == true )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2  );
						}

						if( DrawUp == false && DrawDown == false && DrawLeft == false )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						}

						if( DrawUp == true )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth/2 );
						}

						if( DrawDown == true && DrawUp == false && DrawLeft == false )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
						}

						for( int i=( int )x1+LegendWidth;i<( int )x2+LegendWidth;i+=nPreviousSolutionIncrement )
						{
							if( i%2 == 0 )
							{
								if( ( i+nPreviousSolutionIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pPreviousSolutionOne, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pPreviousSolutionOne, i, y1 + LegendWidth, i + nPreviousSolutionIncrement, y2 + LegendWidth );
							}
							else
							{
								if( ( i+nPreviousSolutionIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pPreviousSolutionTwo, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pPreviousSolutionTwo, i, y1 + LegendWidth, i + nPreviousSolutionIncrement, y2 + LegendWidth );
							}
						}
					}

					if( DrawCurrentSolution == true && DrawCurrentSolutionRight == true )
					{
						x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth/2 );
						y1 = SquareVerticalLocation + SquareHeight/2;
						x2 = SquareHorizontalLocation + SquareWidth;
						y2 = SquareVerticalLocation + SquareHeight/2;

						if( DrawLeft == true || DrawDown == true )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
						}

						if( DrawUp == false && DrawDown == false && DrawLeft == false )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						}

						if( DrawUp == true )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth/2 );
						}

						if( DrawDown == true && DrawUp == false && DrawLeft == false )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2;
						}

						for( int i=( int )x1+LegendWidth; i<( int )x2+LegendWidth; i+=nCurrentSolutionIncrement )
						{
							if( i%2 == 0 )
							{
								if( ( i+nCurrentSolutionIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentSolutionOne, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentSolutionOne, i, y1 + LegendWidth, i + nCurrentSolutionIncrement, y2 + LegendWidth );
							}
							else
							{
								if( ( i+nCurrentSolutionIncrement ) > ( x2 + LegendWidth ) )
								{
									grfx.DrawLine( pCurrentSolutionTwo, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentSolutionTwo, i, y1 + LegendWidth, i + nCurrentSolutionIncrement, y2 + LegendWidth );
							}
						}
					}

					if( DrawCurrentAttempt == true && DrawCurrentAttemptRight == true )
					{
						x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth/2 );
						y1 = SquareVerticalLocation + SquareHeight/2 + nWidth;
						x2 = SquareHorizontalLocation + SquareWidth;
						y2 = SquareVerticalLocation + SquareHeight/2 + nWidth;

						if( DrawLeft == true || DrawDown == true )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
						}

						if( DrawUp == false && DrawDown == false && DrawLeft == false )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						}

						if( DrawUp == true )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						}

						if( DrawDown == true && DrawUp == false && DrawLeft == false )
						{
							x1 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						}

						for( int i=( int )x1+LegendWidth; i<( int )x2+LegendWidth; i+=nCurrentAttemptIncrement )
						{
							if( i%2 == 0 )
							{
								if( ( i+nCurrentAttemptIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentAttemptOne, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth  );
								}
								else
									grfx.DrawLine( pCurrentAttemptOne, i, y1 + LegendWidth, i+nCurrentAttemptIncrement, y2 + LegendWidth );
							}
							else
							{
								if( ( i+nCurrentAttemptIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentAttemptTwo, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentAttemptTwo, i, y1 + LegendWidth, i + nCurrentAttemptIncrement, y2 + LegendWidth );
							}
						}
					}
				}

				if( DrawDown == true )
				{
                    if( bOriginalDown == true )
                    {
					    x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
					    y1 = SquareVerticalLocation + SquareHeight/2 + nWidth;
					    x2 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
					    y2 = SquareVerticalLocation + SquareHeight;

					    if( DrawLeft == false )
					    {
						    y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*3 );
					    }

					    if( DrawUp == true )
					    {
						    y1 = SquareVerticalLocation + SquareHeight/2 + nWidth;
					    }

					    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );

					    x1 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
					    y1 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );
					    x2 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
					    y2 = SquareVerticalLocation + SquareHeight;

					    if( DrawRight == false )
					    {
						    y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*3 );
					    }

					    grfx.DrawLine( pPath,  x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );

					    if( DrawUp == false && DrawRight == false && DrawLeft == false )
					    {
						    x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
						    y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );
						    x2 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
						    y2 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );

						    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
					    }
                    }

					if( DrawPreviousSolution == true && DrawPreviousSolutionDown == true )
					{
						x1 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
						y1 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );
						x2 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
						y2 = SquareVerticalLocation + SquareHeight;

						if( DrawUp == true || DrawLeft == true )
						{
							y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );
						}

						if( DrawRight == true )
						{
							y1 = SquareVerticalLocation + SquareHeight/2 - nWidth;
						}

						if( DrawUp == false && DrawRight == false && DrawLeft == false )
						{
							y1 = SquareVerticalLocation + SquareHeight/2 - nWidth;
						}

						for( int i=( int )y1 + LegendWidth;i<y2+LegendWidth;i+=nPreviousSolutionIncrement)
						{
							if( i%2 == 0 )
							{
								if( ( i+nPreviousSolutionIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pPreviousSolutionOne, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pPreviousSolutionOne, x1 + LegendWidth, i, x2 + LegendWidth, i+nPreviousSolutionIncrement );
							}
							else
							{
								if( ( i+nPreviousSolutionIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pPreviousSolutionTwo, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pPreviousSolutionTwo, x1 + LegendWidth, i, x2 + LegendWidth, i+nPreviousSolutionIncrement );
							}
						}
					}

					if( DrawCurrentSolution == true && DrawCurrentSolutionDown == true )
					{
						x1 = SquareHorizontalLocation + SquareWidth/2;
						y1 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );
						x2 = SquareHorizontalLocation + SquareWidth/2;
						y2 = SquareVerticalLocation + SquareHeight;

						if( DrawUp == true || DrawLeft == true )
						{
							y1 = SquareVerticalLocation + SquareHeight/2 - nWidth;
						}

						if( DrawRight == true || DrawLeft == true )
						{
							y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth/2 );
						}

						if( DrawUp == false && DrawRight == false && DrawLeft == false )
						{
							y1 = SquareVerticalLocation + SquareHeight/2 - nWidth;
						}

						for( int i=( int )y1 + LegendWidth;i<y2+LegendWidth;i+=nCurrentSolutionIncrement )
						{
							if( i%2 == 0 )
							{
								if( ( i+nCurrentSolutionIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentSolutionOne, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentSolutionOne, x1 + LegendWidth, i, x2 + LegendWidth, i+nCurrentSolutionIncrement );
							}
							else
							{
								if( ( i+nCurrentSolutionIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentSolutionTwo, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentSolutionTwo, x1 + LegendWidth, i, x2 + LegendWidth, i+nCurrentSolutionIncrement );
							}
						}

						if( DrawRight == true && DrawUp == false && DrawDown == false )
						{
						/*	if( DrawPreviousSolution == true && DrawCurrentAttempt == true )
							{
								y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth + nWidth/2 );
								y2 = SquareVerticalLocation + SquareHeight/2 - ( nWidth/3 );
								grfx.DrawLine( pBlocked, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
							}
						*/
						}
					}

					if( DrawCurrentAttempt == true && DrawCurrentAttemptDown == true )
					{
						x1 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						y1 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );
						x2 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						y2 = SquareVerticalLocation + SquareHeight;

						if(  DrawUp == true || DrawLeft == true )
						{
							y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );
						}

						if( DrawRight == true )
						{
							y1 = SquareVerticalLocation + SquareHeight/2 + nWidth;
						}

						if( DrawUp == false && DrawRight == false || DrawLeft == false )
						{
							y1 = SquareVerticalLocation + SquareHeight/2 + ( nWidth/2 );
						}

						for( int i=( int )y1+LegendWidth;i<y2+LegendWidth;i+=nCurrentAttemptIncrement )
						{
							if( i%2 == 0 )
							{
								if( ( i+nCurrentAttemptIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentAttemptOne, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentAttemptOne, x1 + LegendWidth, i, x2 + LegendWidth, i+nCurrentAttemptIncrement );
							}
							else
							{
								if( ( i+nCurrentAttemptIncrement ) > ( y2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentAttemptTwo, x1 + LegendWidth, i, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentAttemptTwo, x1 + LegendWidth, i, x2 + LegendWidth, i+nCurrentAttemptIncrement );
							}
						}

						if( DrawRight == true && DrawUp == false && DrawLeft == false )
						{
						/*	if( DrawCurrentSolution == true && DrawPreviousSolution == true )
							{
								y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth + nWidth/2 );
								y2 = SquareVerticalLocation + SquareHeight/2 + ( nWidth + nWidth/3 );
								grfx.DrawLine( pBlocked, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
							}
							*/
						}
					}
				}

				if( DrawLeft == true )
				{
                    if( bOriginalLeft == true )
                    {
					    x1 = SquareHorizontalLocation;
					    y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );
					    x2 = SquareHorizontalLocation + SquareWidth/2 -  nWidth;
					    y2 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );

					    if( DrawUp == false )
					    {
						    x2 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*3 );
					    }

					    if( DrawUp == true )
					    {
						    x2 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*2 );
					    }

					    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );

					    x1 = SquareHorizontalLocation;
					    y1 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );
					    x2 = SquareHorizontalLocation + SquareWidth/2;
					    y2 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );

					    if( DrawDown == false && DrawRight == false )
					    {
						    x2 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*3 );
					    }

					    if( DrawDown == true )
					    {
						    x2 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth + nWidth/2 );
					    }

					    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );

					    if( DrawUp == false && DrawRight == false && DrawDown == false )
					    {
						    x1 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
						    y1 = SquareVerticalLocation + SquareHeight/2 - ( nWidth*2 );
						    x2 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
						    y2 = SquareVerticalLocation + SquareHeight/2 + ( nWidth*2 );

						    grfx.DrawLine( pPath, x1 + LegendWidth, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
					    }
                    }

					if( DrawPreviousSolution == true && DrawPreviousSolutionLeft == true )
					{
						x1 = SquareHorizontalLocation;
						y1 = SquareVerticalLocation + SquareHeight/2 - nWidth;
						x2 = SquareHorizontalLocation + SquareWidth/2;
						y2 = SquareVerticalLocation + SquareHeight/2 - nWidth;

						if( DrawDown == true )
						{
							x2 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
						}

						if( DrawRight == true )
						{
							x2 = SquareHorizontalLocation + SquareWidth;
						}

						if( DrawUp == true )
						{
							x2 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth*2 );
						}

						if( DrawUp == false && DrawRight == false && DrawDown == false )
						{
							x2 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
						}

						for( int i=( int )x1+ LegendWidth; i<( int )x2+LegendWidth;  i+=nPreviousSolutionIncrement )
						{
							if( i%2 == 0 )
							{
								if( ( i+nPreviousSolutionIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pPreviousSolutionOne, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pPreviousSolutionOne, i, y1 + LegendWidth, i+nPreviousSolutionIncrement, y2 + LegendWidth );
							}
							else
							{
								if( ( i+nPreviousSolutionIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pPreviousSolutionTwo, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pPreviousSolutionTwo, i, y1 + LegendWidth, i+nPreviousSolutionIncrement, y2 + LegendWidth );
							}
						}
					}

					if( DrawCurrentSolution == true && DrawCurrentSolutionLeft == true )
					{
						x1 = SquareHorizontalLocation;
						y1 = SquareVerticalLocation + SquareHeight/2;
						x2 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						y2 = SquareVerticalLocation + SquareHeight/2;

						if( DrawUp == true || DrawRight == true || DrawDown == true )
						{
							x2 = SquareHorizontalLocation + SquareWidth/2 + ( nWidth/2 );
						}

						if( DrawRight == true )
						{
							x2 = SquareHorizontalLocation + SquareWidth;
						}

						if( DrawUp == false && DrawRight == false && DrawDown == false )
						{
							x2 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
						}

						for( int i=( int )x1 + LegendWidth; i<( int )x2+LegendWidth; i+=nCurrentSolutionIncrement )
						{
							if( i%2 == 0 )
							{
								if( ( i+nCurrentSolutionIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentSolutionOne, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentSolutionOne, i, y1 + LegendWidth, i+nCurrentSolutionIncrement, y2 + LegendWidth );
							}
							else
							{
								if( ( i+nCurrentSolutionIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentSolutionTwo, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentSolutionTwo, i, y1 + LegendWidth, i+nCurrentSolutionIncrement, y2 + LegendWidth );
							}
						}
					}

					if( DrawCurrentAttempt == true && DrawCurrentAttemptLeft == true )
					{
						x1 = SquareHorizontalLocation;
						y1 = SquareVerticalLocation + SquareHeight/2 + nWidth;
						x2 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						y2 = SquareVerticalLocation + SquareHeight/2 + nWidth;

						if( DrawRight == true || DrawDown == true )
						{
							x2 = SquareHorizontalLocation + SquareWidth/2 - nWidth;
						}

						if( DrawRight == true )
						{
							x2 = SquareHorizontalLocation + SquareWidth;
						}

						if( DrawUp == true )
						{
							x2 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth/2 );
						}

						if( DrawUp == false && DrawRight == false && DrawDown == false )
						{
							x2 = SquareHorizontalLocation + SquareWidth/2 + nWidth;
						}

						for( int i=( int )x1 + LegendWidth; i<( int )x2+LegendWidth; i+=nCurrentAttemptIncrement )
						{
							if( i%2 == 0 )
							{
								if(  ( i+nCurrentAttemptIncrement ) > ( x2+LegendWidth ) )
								{
									grfx.DrawLine( pCurrentAttemptOne, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentAttemptOne, i, y1 + LegendWidth, i+nCurrentAttemptIncrement, y2 + LegendWidth );
							}
							else
							{
								if( ( i+nCurrentAttemptIncrement ) > ( x2 + LegendWidth ) )
								{
									grfx.DrawLine( pCurrentAttemptTwo, i, y1 + LegendWidth, x2 + LegendWidth, y2 + LegendWidth );
								}
								else
									grfx.DrawLine( pCurrentAttemptTwo, i, y1 + LegendWidth, i+nCurrentAttemptIncrement, y2 + LegendWidth );
							}
						}
					}
				}
			}

            if( Blocked == true )
            {
                y1 = SquareVerticalLocation + SquareHeight/2 - nWidth;
				x1 = SquareHorizontalLocation + SquareWidth/2 - nWidth;

                grfx.FillEllipse( brushBlocked, x1, y1, ( SquareWidth/2 ) -1, ( SquareHeight/2 ) -1 ); 
            }

			Font font = new Font( "Arial", 12 );
			x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth * 2 );
			y1 = SquareVerticalLocation + SquareHeight/2;
			if( DrawStart == true )
			{
				grfx.FillRectangle( brushBlocked, x1, y1, nWidth*8, font.Height );
				grfx.DrawString( "Start", font, brushStart, x1, y1 );
			}

			x1 = SquareHorizontalLocation + SquareWidth/2 - ( nWidth*3 );
			y1 = SquareVerticalLocation + SquareHeight/2;
			if( DrawFinish == true )
			{
                DrawUp = bOriginalUp;
                DrawRight = bOriginalRight;
                DrawDown = bOriginalDown;
                DrawLeft = bOriginalLeft;

				grfx.FillRectangle( brushBlocked, x1, y1,  nWidth*10, font.Height  );
				grfx.DrawString( "Finish", font, brushFinish, x1, y1 );
			}

			IsValid = true;
		}

	}
}
