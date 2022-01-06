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

namespace GeneticsDevTwo
{
	/// <summary>
	///  A class containing the specifications for drawing a square on the board
	///  
	/// </summary>
	public class SquareInformation
	{
		private string strSquare;
		private bool bStart;
		private bool bFinish;
		private bool bUp;
		private bool bRight;
		private bool bDown;
		private bool bLeft;
		
		/// drawing references
		/// 
		private static bool bDrawUp;
		private static bool bDrawRight;
		private static bool bDrawDown;
		private static bool bDrawLeft;

		public string Square
		{
			get
			{
				return strSquare;
			}
			set
			{
				strSquare = value;
			}
		}

		public bool Start
		{
			get
			{
				return bStart;
			}
			set
			{
				bStart = value;
			}
		}

		public bool Finish
		{
			get
			{
				return bFinish;
			}
			set
			{
				bFinish = value;
			}
		}

		public bool Up
		{
			get
			{
				return bUp;
			}
			set
			{
				bUp = value;
			}
		}

		public bool Right
		{
			get
			{
				return bRight;
			}
			set
			{
				bRight = value;
			}
		}

		public bool Down 
		{
			get
			{
				return bDown;
			}
			set
			{
				bDown = value;
			}
		}

		public bool Left
		{
			get
			{
				return bLeft;
			}
			set
			{
				bLeft = value;
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

		public SquareInformation()
		{

			strSquare = null;
			bStart = false;
			bFinish = false;
			bUp = false;
			bRight = false;
			bDown = false;
			bLeft = false;
			bDrawUp = false;
			bDrawRight = false;
			bDrawDown = false;
			bDrawLeft = false;
		}
		
		public void ClearDraw()
		{
			bDrawUp = false;
			bDrawRight = false;
			bDrawDown = false;
			bDrawLeft = false;
		}
	}
}
