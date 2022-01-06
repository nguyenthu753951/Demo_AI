/*
 * Created by SharpDevelop.
 * User: pseudonym67
 * Date: 23/03/2006
 * Time: 11:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 
// wrapper to allow thread safe use of textboxes
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
using System.Windows.Forms;
using System.Drawing;

namespace UsefulClasses
{
	/// <summary>
	/// Description of ThreadSafeTextBox.
	/// </summary>
	public class ThreadSafeTextBox
	{
		private TextBox tbTextBox;
		private RichTextBox rtbTextBox;
		
		/// delegates
        /// 
        delegate void SetColorCallBack( Color color );
        delegate void SetTextCallBack( string text );

        public TextBox GetTextBox
        {
            get
            {
                return tbTextBox;
            }
        }

        public RichTextBox GetRichTextBox
        {
            get
            {
                return rtbTextBox;
            }
        }
		
		public ThreadSafeTextBox( TextBox textBox )
		{
			tbTextBox = textBox;
			rtbTextBox = null;
		}
		
		public ThreadSafeTextBox( RichTextBox textBox )
		{
			tbTextBox = null;
			rtbTextBox = textBox;
		}
		
		public void AppendText( string text )
		{
			if( tbTextBox != null )
			{
				if( tbTextBox.InvokeRequired == true )
				{
					SetTextCallBack t = new SetTextCallBack( SetText );
					tbTextBox.Invoke( t, new object[]{ text + "\n" } );
				}
				else
				{
					tbTextBox.AppendText( text + "\n" );
				}
			}
			
			if( rtbTextBox != null )
			{
				if( rtbTextBox.InvokeRequired == true )
				{
					SetTextCallBack t = new SetTextCallBack( SetText );
					rtbTextBox.Invoke( t, new object[]{ text + "\n" } );
				}
				else
				{
					rtbTextBox.AppendText( text + "\n" );
				}
			}
		}
		
		public void AppendTextWithColour( string text, Color color )
		{
			if( tbTextBox != null )
			{
				if( tbTextBox.InvokeRequired == true )
				{
					SetTextCallBack t = new SetTextCallBack( SetText );
					tbTextBox.Invoke( t, new object[]{ text + "\n" } );
				}
				else
				{
					tbTextBox.AppendText( text + "\n" );
				}
			}
			
			if( rtbTextBox != null )
			{
				if( rtbTextBox.InvokeRequired == true )
				{
					SetColorCallBack c = new SetColorCallBack( SetColour );
					rtbTextBox.Invoke( c, new object[]{ color } );
					SetTextCallBack t = new SetTextCallBack( SetText );
					rtbTextBox.Invoke( t, new object[]{ text + "\n" } );
				}
				else
				{
					rtbTextBox.SelectionColor = color;
					rtbTextBox.AppendText( text + "\n" );
				}
			}
		}
		
		public void AppendTextWithColour( string text, Color color, bool newLine )
		{
			if( tbTextBox != null )
			{
				if( tbTextBox.InvokeRequired == true )
				{
					SetTextCallBack t = new SetTextCallBack( SetText );
					if( newLine == true )
					{
						tbTextBox.Invoke( t, new object[]{ text + "\n" } );
					}
					else
						tbTextBox.Invoke( t, new object[]{ text } );
				}
				else
				{
					if( newLine == true )
					{
						tbTextBox.AppendText( text + "\n" );
					}
					else
						tbTextBox.AppendText( text );
				}
			}
			
			if( rtbTextBox != null )
			{
				if( rtbTextBox.InvokeRequired == true )
				{
					SetColorCallBack c = new SetColorCallBack( SetColour );
					rtbTextBox.Invoke( c, new object[]{ color } );
					SetTextCallBack t = new SetTextCallBack( SetText );
					if( newLine == true )
					{
						rtbTextBox.Invoke( t, new object[]{ text + "\n" } );
					}
					else
						rtbTextBox.Invoke( t, new object[]{ text } );
				}
				else
				{
					rtbTextBox.SelectionColor = color;
					if( newLine == true )
					{
						rtbTextBox.AppendText( text + "\n" );
					}
					else
						rtbTextBox.AppendText( text );
				}
			}
		}

        private void SetColour( Color color )
        {
        	if( rtbTextBox != null )
        		rtbTextBox.SelectionColor = color;
        }

        private void SetText( string text )
        {
        	if( tbTextBox != null )
        	{
        		tbTextBox.AppendText( text );
        	}
        	
        	if( rtbTextBox != null )
        	{
        		rtbTextBox.AppendText( text );
        	}
        }  
        
	}
}
