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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using BoardControl;
using System.Text;
using UsefulClasses;

namespace GeneticsDevTwo
{
	/// <summary>
	/// Summary description for GeneticsBoard.
	/// </summary>
	public class GeneticsBoard : Board
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ArrayList arraySquareInformation;
		private ThreadSafeTextBox tsTextBox = null;
		private bool bShowCurrentAttemptPath = false;
		private bool bShowCurrentSolutionPath = false;
		private bool bShowPreviousSolutionPath = false;
		
		public bool ShowCurrentAttemptPath
		{
			get
			{
				return bShowCurrentAttemptPath;
			}
			set
			{
				bShowCurrentAttemptPath = value;
			}
		}
		
		public bool ShowCurrentSolutionPath 
		{
			get
			{
				return bShowCurrentSolutionPath;
			}
			set
			{
				bShowCurrentSolutionPath = value;
			}
		}
		
		public bool ShowPreviousSolutionPath
		{
			get
			{
				return bShowPreviousSolutionPath;
			}
			set
			{
				bShowPreviousSolutionPath = value;
			}
		}

		public GeneticsBoard()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			if( Designing() == true )
				return;

			arraySquareInformation = new ArrayList();

		}
		
		public GeneticsBoard( ThreadSafeTextBox textBox ) : this()
		{
			tsTextBox = textBox;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// GeneticsBoard
			// 
			this.Name = "GeneticsBoard";
			this.Size = new System.Drawing.Size(360, 360);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);


		}
		#endregion

		public void InitialiseBoard()
		{
			Clear();

			GetHashTable.Add( "A8", new GeneticsSquare( SquareWidth, SquareHeight, 0, 0, "A8" ) );
			GetHashTable.Add( "A7", new GeneticsSquare( SquareWidth, SquareHeight, 0, SquareHeight, "A7" ) );
			GetHashTable.Add( "A6", new GeneticsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 2, "A6" ) );
			GetHashTable.Add( "A5", new GeneticsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 3, "A5" ) );
			GetHashTable.Add( "A4", new GeneticsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 4, "A4" ) );
			GetHashTable.Add( "A3", new GeneticsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 5, "A3" ) );
			GetHashTable.Add( "A2", new GeneticsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 6, "A2" ) );
			GetHashTable.Add( "A1", new GeneticsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 7, "A1" ) );
			GetHashTable.Add( "B8", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth, 0, "B8" ) );
			GetHashTable.Add( "B7", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight, "B7" ) );
			GetHashTable.Add( "B6", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 2, "B6" ) );
			GetHashTable.Add( "B5", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 3, "B5" ) );
			GetHashTable.Add( "B4", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 4, "B4" ) );
			GetHashTable.Add( "B3", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 5, "B3" ) );
			GetHashTable.Add( "B2", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 6, "B2" ) );
			GetHashTable.Add( "B1", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 7, "B1" ) );
			GetHashTable.Add( "C8", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 2, 0, "C8" ) );
			GetHashTable.Add( "C7", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight, "C7" ) );
			GetHashTable.Add( "C6", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 2, "C6" ) );
			GetHashTable.Add( "C5", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 3, "C5" ) );
			GetHashTable.Add( "C4", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 4, "C4" ) );
			GetHashTable.Add( "C3", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 5, "C3" ) );
			GetHashTable.Add( "C2", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 6, "C2" ) );
			GetHashTable.Add( "C1", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 7, "C1" ) );
			GetHashTable.Add( "D8", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 3, 0, "D8" ) );
			GetHashTable.Add( "D7", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight, "D7" ) );
			GetHashTable.Add( "D6", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 2, "D6" ) );
			GetHashTable.Add( "D5", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 3, "D5" ) );
			GetHashTable.Add( "D4", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 4, "D4" ) );
			GetHashTable.Add( "D3", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 5, "D3" ) );
			GetHashTable.Add( "D2", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 6, "D2" ) );
			GetHashTable.Add( "D1", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 7, "D1" ) );
			GetHashTable.Add( "E8", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 4, 0, "E8" ) );
			GetHashTable.Add( "E7", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight, "E7" ) );
			GetHashTable.Add( "E6", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 2, "E6" ) );
			GetHashTable.Add( "E5", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 3, "E5" ) );
			GetHashTable.Add( "E4", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 4, "E4" ) );
			GetHashTable.Add( "E3", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 5, "E3" ) );
			GetHashTable.Add( "E2", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 6, "E2" ) );
			GetHashTable.Add( "E1", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 7, "E1" ) );
			GetHashTable.Add( "F8", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 5, 0, "F8" ) );
			GetHashTable.Add( "F7", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight, "F7" ) );
			GetHashTable.Add( "F6", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 2, "F6" ) );
			GetHashTable.Add( "F5", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 3, "F5" ) );
			GetHashTable.Add( "F4", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 4, "F4" ) );
			GetHashTable.Add( "F3", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 5, "F3" ) );
			GetHashTable.Add( "F2", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 6, "F2" ) );
			GetHashTable.Add( "F1", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 7, "F1" ) );
			GetHashTable.Add( "G8", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 6, 0, "G8" ) );
			GetHashTable.Add( "G7", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight, "G7" ) );
			GetHashTable.Add( "G6", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 2, "G6" ) );
			GetHashTable.Add( "G5", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 3, "G5" ) );
			GetHashTable.Add( "G4", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 4, "G4" ) );
			GetHashTable.Add( "G3", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 5, "G3" ) );
			GetHashTable.Add( "G2", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 6, "G2" ) );
			GetHashTable.Add( "G1", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 7, "G1" ) );
			GetHashTable.Add( "H8", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 7, 0, "H8" ) );
			GetHashTable.Add( "H7", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight, "H7" ) );
			GetHashTable.Add( "H6", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 2, "H6" ) );
			GetHashTable.Add( "H5", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 3, "H5" ) );
			GetHashTable.Add( "H4", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 4, "H4" ) );
			GetHashTable.Add( "H3", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 5, "H3" ) );
			GetHashTable.Add( "H2", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 6, "H2" ) );
			GetHashTable.Add( "H1", new GeneticsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 7, "H1" ) );

			DrawLegend( true );
			SetBackGroundColor( Color.DarkGreen, Color.DarkGreen );
			SetHighlightColor( Color.BurlyWood );
			LegendColor = Color.Beige;

			SetDisplayMode( "APPLICATION" );

			/// Set this line to true to draw square borders
			/// 

			( ( BasicSquare )GetHashTable[ "A1" ] ).DrawBorder = false;

			Invalidate();
			Update();

		}

		private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{

			Graphics grfx = e.Graphics;

			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				BasicSquare square = ( BasicSquare )dicEnt.Value;

				square.DrawSquare( grfx );
			}
		}

		public void InvalidateBoard()
		{
			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				BasicSquare square = ( BasicSquare )dicEnt.Value;

				square.IsValid = false;
			}
		}

		public override void SetBackGroundColor( Color colorOne, Color colorTwo )
		{
			char firstChar = 'A';
			char secondChar = '8';

			bool bFirstColor = true;

			StringBuilder strString = new StringBuilder();
			strString.Append( firstChar );
			strString.Append( secondChar );

			BasicSquare squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

			while( squareTest != null )
			{
				if( bFirstColor == true )
				{
					squareTest.BackGroundColor = colorOne;
					bFirstColor = false;
				}
				else
				{
					squareTest.BackGroundColor = colorTwo;
					bFirstColor = true;
				}

				firstChar++;

				strString.Remove( 0, strString.Length );
				strString.Append( firstChar );
				strString.Append( secondChar );
				squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

				if( squareTest == null )
				{
					firstChar = 'A';
					secondChar--;
					strString.Remove( 0, strString.Length );
					strString.Append( firstChar );
					strString.Append( secondChar );
					squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

					if( bFirstColor == true )
						bFirstColor = false;
					else
						bFirstColor = true;
				}
			}
		}

		/// <summary>
		/// Draw the Legend
		/// </summary>
		/// <param name="drawLegend"></param>
		public override void DrawLegend( bool drawLegend )
		{
			if( GetHashTable[ "A8" ] != null )
				( ( BasicSquare )GetHashTable[ "A8" ] ).DrawLegend = drawLegend;

			SetEdges();
		}

		public override void SetHighlightColor( Color color )
		{
			if( GetHashTable[ "A8" ] != null )
				( ( BasicSquare )GetHashTable[ "A8" ] ).HighlightColor = color;
		}

		/// <summary>
		/// Set the edges for drawing the legends
		/// </summary>
		protected override void SetEdges()
		{
			char firstChar = 'A';
			char secondChar = '1';

			StringBuilder strString = new StringBuilder();
			strString.Append( firstChar );
			strString.Append( secondChar );

			BasicSquare squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

			while( squareTest != null )
			{
				firstChar++;

				strString.Remove( 0, strString.Length );
				strString.Append( firstChar );
				strString.Append( secondChar );
				squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

				if( squareTest == null )
				{
					firstChar--;
					strString.Remove( 0, strString.Length );
					strString.Append( firstChar );
					strString.Append( secondChar );
					squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

					squareTest.IsRightEdgeSquare = true;

					firstChar = 'A';
					secondChar++;
					strString.Remove( 0, strString.Length );
					strString.Append( firstChar );
					strString.Append( secondChar );
					squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];
				}
			}

			firstChar = 'A';
			secondChar = '1';
			strString.Remove( 0, strString.Length );
			strString.Append( firstChar );
			strString.Append( secondChar );

			squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

			while( squareTest != null )
			{
				squareTest.IsBottomSquare = true;

				firstChar++;

				strString.Remove( 0, strString.Length );
				strString.Append( firstChar );
				strString.Append( secondChar );

				squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];
			}
		}


		public bool AddSquareInformation( SquareInformation squareInfo )
		{
			if( squareInfo == null )
				return false;

			arraySquareInformation.Add( squareInfo );

			return true;
		}

        public void ClearSquareInformation()
        {
            if( arraySquareInformation != null && arraySquareInformation.Count > 0 )
                arraySquareInformation.Clear();
        }

		public bool DrawBoard()
		{
			GeneticsSquare square = null;
			foreach( SquareInformation squareInfo in arraySquareInformation )
			{
				square = ( GeneticsSquare )GetHashTable[ squareInfo.Square ];
				if( square == null )
				{
					MessageBox.Show( "Error unable to get square " + squareInfo.Square );
					return false;
				}

				square.DrawStart = squareInfo.Start;
				square.DrawFinish = squareInfo.Finish;
				square.DrawUp = squareInfo.Up;
				square.DrawRight = squareInfo.Right;
				square.DrawDown = squareInfo.Down;
				square.DrawLeft = squareInfo.Left;
				square.IsValid = false;
			}

			Invalidate();
			Update();

			return true;
		}
		
		public string GetStartSquare()
		{
			foreach( SquareInformation squareInfo in arraySquareInformation )
			{
				if( squareInfo.Start == true )
					return squareInfo.Square;
			}
			
			return null;
		}
		
		public SquareInformation GetSquareInformation( string square )
		{
			foreach( SquareInformation squareInfo in arraySquareInformation )
			{
				if( squareInfo.Square == square )
					return squareInfo;
			}
			
			return null;
		}
		
		public SquareInformation GetSquareInformation( int square )
		{
			return ( SquareInformation )arraySquareInformation[ square ];
		}
		
		public void DrawCurrentSolution( bool draw )
		{
			if( GetHashTable[ "A8" ] != null )
				( ( GeneticsSquare )GetHashTable[ "A8" ] ).DrawCurrentSolution = draw;
		}
		
		public void DrawCurrentAttempt( bool draw )
		{
			if( GetHashTable[ "A8" ] != null )
				( ( GeneticsSquare )GetHashTable[ "A8" ] ).DrawCurrentAttempt = draw;
		}
		
		public void DrawPreviousSolution( bool draw )
		{
			if( GetHashTable[ "A8" ] != null )
				( ( GeneticsSquare )GetHashTable[ "A8" ] ).DrawPreviousSolution = draw;
		}
		
		public void ResetCurrentSolution()
		{
            GeneticsSquare square = null;

			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
                square = ( GeneticsSquare )dicEnt.Value;
                if( square == null )
                    return;

             /*   if( square.DrawCurrentSolutionUp == true 
                    || square.DrawCurrentSolutionRight == true 
                    || square.DrawCurrentSolutionDown == true
                    || square.DrawCurrentSolutionLeft == true )
                    square.IsValid = false; */

				square.DrawCurrentSolutionUp = false;
				square.DrawCurrentSolutionRight = false;
				square.DrawCurrentSolutionDown = false;
				square.DrawCurrentSolutionLeft = false;
                square.IsValid = false;
			}

		}
		
		public void ResetCurrentAttempt()
		{
            GeneticsSquare square = null;

			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
                square = ( GeneticsSquare )dicEnt.Value;
                if( square == null )
                    return;
              /* 
                if( square.DrawCurrentAttemptUp == true 
                    || square.DrawCurrentAttemptRight == true 
                    || square.DrawCurrentAttemptDown == true 
                    || square.DrawCurrentAttemptLeft == true )
                    square.IsValid = false; */

				square.DrawCurrentAttemptUp = false;
				square.DrawCurrentAttemptRight = false;
				square.DrawCurrentAttemptDown = false;
				square.DrawCurrentAttemptLeft = false;
                square.IsValid = false;
			}

		}
		
		public void ResetPreviousSolution()
		{
            GeneticsSquare square = null;

			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
                square = ( GeneticsSquare )dicEnt.Value;
                if( square == null )
                    return;

/*                if( square.DrawPreviousSolutionUp == true 
                    || square.DrawPreviousSolutionRight == true 
                    || square.DrawPreviousSolutionDown == true
                    || square.DrawPreviousSolutionLeft == true 
                    || square.DrawBlocked == true )
                {
                    square.IsValid = false;
                    return;
                } */

				square.DrawPreviousSolutionUp = false;
				square.DrawPreviousSolutionRight = false;
				square.DrawPreviousSolutionDown = false;
				square.DrawPreviousSolutionLeft = false;
				square.DrawBlocked = false;
                square.IsValid = false; 
			}

 		}
		
		public void SetPreviousSolution( GeneticsPathString geneticsPathString )
		{
			if( geneticsPathString == null )
				return;
			
			GeneticsPathItem pathItem = null;
			GeneticsPathItem gsPreviousItem = null;
			GeneticsSquare square = null;
			SquareInformation squareInfo = null;
			string strCurrentSquare = GetStartSquare();
			
			if( ShowPreviousSolutionPath == true && tsTextBox != null )
			{
				tsTextBox.AppendTextWithColour( "Drawing Previous Solution " + geneticsPathString.ToString() + " Length Travelled = " + geneticsPathString.LengthTravelled.ToString(), Color.ForestGreen );
			}
			
			for( int i=0; i<geneticsPathString.LengthTravelled; i++ )
			{
				gsPreviousItem = pathItem;
				pathItem = ( GeneticsPathItem )geneticsPathString.GeneticsString[ i ];
				squareInfo = GetSquareInformation( strCurrentSquare );
				
				if( squareInfo == null )
				{
					return;
				}
				
				squareInfo.ClearDraw();
				
				if( pathItem == null )
				{
					return;
				}
				
				square = ( GeneticsSquare )GetSquare( strCurrentSquare );
				
				if( square == null )
				{
					return;
				}

             /*   if( square.Blocked == true )
                {
                    square.DrawBlocked = true;
                }
			*/	
				if( ShowPreviousSolutionPath == true && tsTextBox != null )
				{
					tsTextBox.AppendTextWithColour( "Drawing Square " + strCurrentSquare, Color.ForestGreen );
				}
				
				if( pathItem.Valid == true )
				{
					switch( pathItem.Direction )
					{
						case "Up":
							{
								square.DrawPreviousSolutionUp = true;
								try
								{
									strCurrentSquare = GetSquareAbove( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawUp = true;
							}break;
						case "Right":
							{
								square.DrawPreviousSolutionRight = true;
								try
								{
									strCurrentSquare = GetSquareToRight( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawRight = true;
								      
							}break;
						case "Down":
							{
								square.DrawPreviousSolutionDown = true;
								try
								{
									strCurrentSquare = GetSquareBelow( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawDown = true;
							}break;
						case "Left":
							{
								square.DrawPreviousSolutionLeft = true;
								try
								{
									strCurrentSquare = GetSquareToLeft( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawLeft = true;
							}break;
					}
					
					if( gsPreviousItem != null )
					{
						switch( gsPreviousItem.Direction )
						{
							case "Up": 
								{
									squareInfo.DrawDown = true;
								}break;
							case "Right":
								{
									squareInfo.DrawLeft = true;
								}break;
							case "Down":
								{
									squareInfo.DrawUp = true;
								}break;
							case "Left":
								{
									squareInfo.DrawRight = true;
								}break;
						}
					}
					
					if( squareInfo != null )
					{
						if( squareInfo.DrawUp == true )
							square.DrawPreviousSolutionUp = true;
						if( squareInfo.DrawRight == true )
							square.DrawPreviousSolutionRight = true;
						if( squareInfo.DrawDown == true )
							square.DrawPreviousSolutionDown = true;
						if( squareInfo.DrawLeft == true )
							square.DrawPreviousSolutionLeft = true;
					}
				}
			}
		}
		
		public void SetCurrentSolution( GeneticsPathString geneticsPathString )
		{
			if( geneticsPathString == null )
				return;
			
			GeneticsPathItem pathItem = null;
			GeneticsPathItem gsPreviousItem = null;
			GeneticsSquare square = null;
			SquareInformation squareInfo = null;
			string strCurrentSquare = GetStartSquare();
			
			if( ShowCurrentSolutionPath == true && tsTextBox != null )
			{
				tsTextBox.AppendTextWithColour( "Drawing Current Solution " + geneticsPathString.ToString() + " Length Travelled = " + geneticsPathString.LengthTravelled.ToString(), Color.ForestGreen );
			}
			
			for( int i=0; i<geneticsPathString.LengthTravelled; i++ )
			{
				gsPreviousItem = pathItem;
				pathItem = ( GeneticsPathItem )geneticsPathString.GeneticsString[ i ];
				squareInfo = GetSquareInformation( strCurrentSquare );
				
				if( squareInfo == null )
				{
					return;
				}
				
				squareInfo.ClearDraw();
				
				if( pathItem == null )
				{
					return;
				}
				
				square = ( GeneticsSquare )GetSquare( strCurrentSquare );
				
				if( square == null )
				{
					return;
				}
				
				if( ShowCurrentSolutionPath == true && tsTextBox != null )
				{
					tsTextBox.AppendTextWithColour( "Drawing Square " + strCurrentSquare, Color.ForestGreen );
				}
				
				if( pathItem.Valid == true )
				{
					switch( pathItem.Direction )
					{
						case "Up":
							{
								square.DrawCurrentSolutionUp = true;
								try
								{
									strCurrentSquare = GetSquareAbove( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawUp = true;
							}break;
						case "Right":
							{
								square.DrawCurrentSolutionRight = true;
								try
								{
									strCurrentSquare = GetSquareToRight( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawRight = true;
							}break;
						case "Down":
							{
								square.DrawCurrentSolutionDown = true;
								try
								{
									strCurrentSquare = GetSquareBelow( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawDown = true;
							}break;
						case "Left":
							{
								square.DrawCurrentSolutionLeft = true;
								try
								{
									strCurrentSquare = GetSquareToLeft( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawLeft = true;
							}break;
					}
					
					if( gsPreviousItem != null )
					{
						switch( gsPreviousItem.Direction )
						{
							case "Up": 
								{
									squareInfo.DrawDown = true;
								}break;
							case "Right":
								{
									squareInfo.DrawLeft = true;
								}break;
							case "Down":
								{
									squareInfo.DrawUp = true;
								}break;
							case "Left":
								{
									squareInfo.DrawRight = true;
								}break;
						}
					}
					
					if( squareInfo != null )
					{
						if( squareInfo.DrawUp == true )
							square.DrawCurrentSolutionUp = true;
						if( squareInfo.DrawRight == true )
							square.DrawCurrentSolutionRight = true;
						if( squareInfo.DrawDown == true )
							square.DrawCurrentSolutionDown = true;
						if( squareInfo.DrawLeft == true )
							square.DrawCurrentSolutionLeft = true;
					}
				}
			}
			
		}
		
		public void SetCurrentAttempt( GeneticsPathString geneticsPathString )
		{
			if( geneticsPathString == null )
				return;

			GeneticsPathItem gsItem = null;
			GeneticsPathItem gsPreviousItem = null;
			GeneticsSquare square = null; 
			SquareInformation squareInfo = null;
			string strCurrentSquare = GetStartSquare();
			
			if( ShowCurrentAttemptPath == true && tsTextBox != null )
			{
				tsTextBox.AppendTextWithColour( "Drawing Current Attempt " + geneticsPathString.ToString() + " Length Travelled = " + geneticsPathString.LengthTravelled.ToString(), Color.ForestGreen );
			}
			
			for( int i=0; i<geneticsPathString.LengthTravelled; i++ )
			{
				gsPreviousItem = gsItem;
				gsItem = ( GeneticsPathItem )geneticsPathString.GeneticsString[ i ];
				squareInfo = GetSquareInformation( strCurrentSquare );
				
				if( squareInfo == null )
				{
					return;
				}
				
				squareInfo.ClearDraw();
				
				if( gsItem == null )
				{
					return;
				}
				
				square = ( GeneticsSquare )GetSquare( strCurrentSquare );
				
				if( square == null )
				{
					return;
				}
				
				if( ShowCurrentAttemptPath == true && tsTextBox != null )
				{
					tsTextBox.AppendTextWithColour( "Drawing Square " + strCurrentSquare, Color.ForestGreen );
				}
				
				if( gsItem.Valid == true )
				{
					switch( gsItem.Direction )
					{
						case "Up":
							{
								try
								{
									strCurrentSquare = GetSquareAbove( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawUp = true;
							}break;
						case "Right":
							{
								try
								{
									strCurrentSquare = GetSquareToRight( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawRight = true;
								
							}break;
						case "Down":
							{
								try
								{
									strCurrentSquare = GetSquareBelow( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawDown = true;
							}break;
						case "Left":
							{
								try
								{
									strCurrentSquare = GetSquareToLeft( strCurrentSquare ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									continue;
								}
								
								squareInfo.DrawLeft = true;
							}break;
					}
					
					if( gsPreviousItem != null )
					{
						switch( gsPreviousItem.Direction )
						{
							case "Up": 
								{
									squareInfo.DrawDown = true;
								}break;
							case "Right":
								{
									squareInfo.DrawLeft = true;
								}break;
							case "Down":
								{
									squareInfo.DrawUp = true;
								}break;
							case "Left":
								{
									squareInfo.DrawRight = true;
								}break;
						}
					}
					
					if( squareInfo != null )
					{
						if( squareInfo.DrawUp == true )
							square.DrawCurrentAttemptUp = true;
						if( squareInfo.DrawRight == true )
							square.DrawCurrentAttemptRight = true;
						if( squareInfo.DrawDown == true )
							square.DrawCurrentAttemptDown = true;
						if( squareInfo.DrawLeft == true )
							square.DrawCurrentAttemptLeft = true;
					}
					
				}
			}
		}
	}
}
