// Genetics Dev Two main form showing examples of genetics programming.
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.IO;
using System.Threading;
using UsefulClasses;
using RandomNumber;
using System.Diagnostics;
using System.Text;

namespace GeneticsDevTwo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
    {
       	private IContainer components;
		private GeneticsDevTwo.GeneticsBoard geneticsBoard1;
		private System.Windows.Forms.TabControl geneticsTabControl;
		private System.Windows.Forms.TabPage tabGeneticOptionsPage;
		private System.Windows.Forms.TabPage tabMapPage;
		private System.Windows.Forms.TabPage colorOptionsTab;
		private System.Windows.Forms.TabPage ExampleOnePage;
		private System.Windows.Forms.TabPage ExampleTwoPage;
		private System.Windows.Forms.TabPage ExampleThreePage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private string strMap;

        /// Threading stuff
        /// 

        private ThreadStart tsExampleOne;
        private ThreadStart tsExampleTwo;
        private ThreadStart tsExampleThree;
        private Thread tExampleOne;
        private Thread tExampleTwo;
        private Thread tExampleThree;
        private bool bStopThreadOne;
        private bool bStopThreadTwo;
        private bool bStopThreadThree;
        private bool bAllowClose;
        
        /// Threadsafe text box wrapper
        /// 
        
        private ThreadSafeTextBox tsTextBox;

        /// Threadsafe calls to the genetics board
        /// 

        private delegate void InvalidateCallback();
        private delegate void UpdateCallback();
        private delegate void ResetCurrentSolutionCallback();
        private delegate void ResetCurrentAttemptCallback();
        private delegate void ResetPreviousSolutionCallback();
        private delegate void SetCurrentSolutionCallback( GeneticsPathString geneticsPathString );
        private delegate void SetCurrentAttemptCallback( GeneticsPathString geneticsPathString );
        private delegate void SetPreviousSolutionCallback( GeneticsPathString geneticsPathString );
        private delegate bool ShowCurrentAttemptPathCallback();
        private delegate bool ShowCurrentSolutionPathCallback();
        private delegate bool ShowPreviousSolutionPathCallback();
	
        /// Threadsafe calls to the form
        /// 
        private delegate void StopExampleOneCallback( object sender, EventArgs e );
        private delegate void StopExampleTwoCallback( object sender, EventArgs e );
        private delegate void StopExampleThreeCallback( object sender, EventArgs e );
        private delegate void StartExampleTwoCallback( object sender, EventArgs e );

        /// genetic algorythm objects
        /// 

        private GeneticsPath gpExampleOne = null;
        private GeneticsPath gpExampleTwo = null;
        private GeneticsPath gpExampleThree = null;
        
        /// progress print out stuff
        /// 
    
        private bool bPrintCurrentAttemptFinds = false;
        private bool bPrintTimerMessage = false;
        private bool bPrintRunMessages = true; 
        private bool bPrintGenerationMessages = true;
        private bool bPrintSampleDebugString = true;
        private bool bPrintFullBestString = true;
        private bool bDebugProgress = false;
        private bool bDebugSetValid = false;
        
        /// debugging stuff
        /// 
        private bool bDrawFirstString = false;
        private bool bDrawBestGenerationString = true;
       
        
        /// Thread controls
        /// 
        
        private static Mutex currentAttemptMutex = new Mutex();
        private static Mutex geneticsArrayMutex = new Mutex();
        
        /// general control stuff
        /// 
        
        private bool bExampleOneRunning;
        private bool bExampleTwoRunning;
        private bool bExampleThreeRunning;
        private bool bInitialised;
        private bool bIsFinishDrawn;

                            
        /// <summary>
        /// the best path so far
        /// </summary>
        private GeneticsPathString gpsMaxString = null;
        private GeneticsPathString gpsPreviousSolutionString = null;
        private GeneticsPathString gpsCurrentAttemptString = null;
        private GeneticsPathString gpsCurrentSolutionString = null;
        private bool bFinishIsSet;
        private GroupBox groupBox1;
        private Label label5;
        private Label label4;
        private NumericUpDown numberOFCycles;
        private NumericUpDown exampleOnePopulationSize;
        private Label label7;
        private Label label6;
        private NumericUpDown exampleOneStartStringSize;
        private Button button1;
        private SaveFileDialog saveOutputFileDlg;
        private System.Windows.Forms.Timer guiTimer;
        private GroupBox groupBox2;
        private Label label8;
        private NumericUpDown displayDelayUpDown;
        private CheckBox useDelayCheckBox;
        private Button loadMapButton;
        private ListBox mapListBox;
        private Label label9;
	
        private RandomNumberGenerator random = null;
        private const string strMapStart = "";

        private bool bDrawCurrentAttempt = true;
        private bool bDrawCurrentSolution = true;
        private bool bDrawPreviousSolution = true;
        private bool bDrawCurrentAttemptPath = true;
        private bool bDrawCurrentSolutionPath = true;
        private bool bDrawPreviousSolutionPath = true;
        private NumericUpDown exampleTwoPopulationSize;
        private Label label10;
        private Label label11;
        private NumericUpDown exampleTwoStartStringSize;
        private Button startExampleTwoButton;
        private NumericUpDown exampleThreePopulationSize;
        private Label label12;
        private Label label13;
        private NumericUpDown exampleThreeStartStringSize;
        private Button stopExampleOneButton;
        private Button startExampleOneButton;
        private Button stopExampleThreeButton;
        private Button startExampleThreeButton;
        private Button stopExampleTwoButton;
        


        public bool PrintCurrentAttemptFinds
        {
        	get
        	{
        		return bPrintCurrentAttemptFinds;
        	}
        	set
        	{
        		bPrintCurrentAttemptFinds = value;
        	}
        }
        


		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


///			strMap = "..\\..\\MapTwo.xml";

            strMap = "MapTwo.xml";


            random = new RandomNumberGenerator( true );

			this.SuspendLayout();

			tsTextBox = new ThreadSafeTextBox( textBox );
			geneticsBoard1 = new GeneticsBoard( tsTextBox );
			
			// 
			// geneticsBoard1
			// 
			this.geneticsBoard1.AllowDrop = true;
			this.geneticsBoard1.BackColor = System.Drawing.SystemColors.Control;
			this.geneticsBoard1.BoardHeight = 640;
			this.geneticsBoard1.BoardWidth = 640;
			this.geneticsBoard1.DragDropFrom = null;
			this.geneticsBoard1.DragDropImage = null;
			this.geneticsBoard1.DragNoDropImage = null;
			this.geneticsBoard1.DragOverImage = null;
			this.geneticsBoard1.HorizontalSquares = 8;
			this.geneticsBoard1.ImplementDragDrop = false;
			this.geneticsBoard1.LegendColor = System.Drawing.Color.SandyBrown;
			this.geneticsBoard1.LegendWidth = 10;
			this.geneticsBoard1.Location = new System.Drawing.Point(24, 16);
			this.geneticsBoard1.Name = "geneticsBoard1";
			this.geneticsBoard1.ShowLegend = true;
			this.geneticsBoard1.Size = new System.Drawing.Size(608, 584);
			this.geneticsBoard1.SquareHeight = 60;
			this.geneticsBoard1.SquareWidth = 60;
			this.geneticsBoard1.TabIndex = 0;
			this.geneticsBoard1.TestPaint = false;
			this.geneticsBoard1.VerticalSquares = 8;

			this.Controls.Add( geneticsBoard1 );

			this.ResumeLayout( false );

			bAllowClose = true;
			
			geneticsBoard1.BoardWidth = geneticsBoard1.HorizontalSquares * geneticsBoard1.SquareWidth;
			geneticsBoard1.BoardHeight = geneticsBoard1.VerticalSquares * geneticsBoard1.SquareHeight;
			geneticsBoard1.InitialiseBoard();
			geneticsBoard1.DrawCurrentAttempt( bDrawCurrentAttempt );
       		geneticsBoard1.DrawCurrentSolution( bDrawCurrentSolution );
       		geneticsBoard1.DrawPreviousSolution( bDrawPreviousSolution );
			geneticsBoard1.ShowCurrentAttemptPath = bDrawCurrentAttemptPath;
       		geneticsBoard1.ShowCurrentSolutionPath = bDrawCurrentSolutionPath;
       		geneticsBoard1.ShowPreviousSolutionPath = bDrawPreviousSolutionPath;

			LoadMap();
			geneticsBoard1.DrawBoard();

	        tsExampleOne = new ThreadStart ( RunExampleOne );
	        tsExampleTwo = new ThreadStart( RunExampleTwo );
	        tsExampleThree = new ThreadStart( RunExampleThree );
	            
	        bExampleOneRunning = false;
	        bExampleTwoRunning = false;
	        bExampleThreeRunning = false;
		            
            bInitialised = false;

            gpExampleOne = new GeneticsPath( ref geneticsBoard1, tsTextBox, false );
            gpExampleTwo = new GeneticsPath( ref geneticsBoard1, tsTextBox, false );
            gpExampleThree = new GeneticsPath( ref geneticsBoard1, tsTextBox, false );

            bIsFinishDrawn = false;
            guiTimer.Start();
            bFinishIsSet = false;

            GetMaps();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.geneticsTabControl = new System.Windows.Forms.TabControl();
            this.tabGeneticOptionsPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.useDelayCheckBox = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.displayDelayUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numberOFCycles = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numberOfGenerations = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tabMapPage = new System.Windows.Forms.TabPage();
            this.loadMapButton = new System.Windows.Forms.Button();
            this.mapListBox = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ExampleOnePage = new System.Windows.Forms.TabPage();
            this.startExampleOneButton = new System.Windows.Forms.Button();
            this.stopExampleOneButton = new System.Windows.Forms.Button();
            this.exampleOnePopulationSize = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.exampleOneStartStringSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ExampleTwoPage = new System.Windows.Forms.TabPage();
            this.startExampleTwoButton = new System.Windows.Forms.Button();
            this.stopExampleTwoButton = new System.Windows.Forms.Button();
            this.exampleTwoPopulationSize = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.exampleTwoStartStringSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ExampleThreePage = new System.Windows.Forms.TabPage();
            this.startExampleThreeButton = new System.Windows.Forms.Button();
            this.stopExampleThreeButton = new System.Windows.Forms.Button();
            this.exampleThreePopulationSize = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.exampleThreeStartStringSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.colorOptionsTab = new System.Windows.Forms.TabPage();
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.saveOutputFileDlg = new System.Windows.Forms.SaveFileDialog();
            this.guiTimer = new System.Windows.Forms.Timer(this.components);
            this.geneticsTabControl.SuspendLayout();
            this.tabGeneticOptionsPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.displayDelayUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberOFCycles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfGenerations)).BeginInit();
            this.tabMapPage.SuspendLayout();
            this.ExampleOnePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exampleOnePopulationSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exampleOneStartStringSize)).BeginInit();
            this.ExampleTwoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exampleTwoPopulationSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exampleTwoStartStringSize)).BeginInit();
            this.ExampleThreePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exampleThreePopulationSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exampleThreeStartStringSize)).BeginInit();
            this.SuspendLayout();
            // 
            // geneticsTabControl
            // 
            this.geneticsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.geneticsTabControl.Controls.Add(this.tabGeneticOptionsPage);
            this.geneticsTabControl.Controls.Add(this.tabMapPage);
            this.geneticsTabControl.Controls.Add(this.ExampleOnePage);
            this.geneticsTabControl.Controls.Add(this.ExampleTwoPage);
            this.geneticsTabControl.Controls.Add(this.ExampleThreePage);
            this.geneticsTabControl.Location = new System.Drawing.Point(555, 104);
            this.geneticsTabControl.Name = "geneticsTabControl";
            this.geneticsTabControl.SelectedIndex = 0;
            this.geneticsTabControl.Size = new System.Drawing.Size(575, 395);
            this.geneticsTabControl.TabIndex = 1;
            // 
            // tabGeneticOptionsPage
            // 
            this.tabGeneticOptionsPage.Controls.Add(this.groupBox2);
            this.tabGeneticOptionsPage.Controls.Add(this.groupBox1);
            this.tabGeneticOptionsPage.Location = new System.Drawing.Point(4, 22);
            this.tabGeneticOptionsPage.Name = "tabGeneticOptionsPage";
            this.tabGeneticOptionsPage.Size = new System.Drawing.Size(567, 236);
            this.tabGeneticOptionsPage.TabIndex = 0;
            this.tabGeneticOptionsPage.Text = "Genetic Options";
            this.tabGeneticOptionsPage.Click += new System.EventHandler(this.OnPositionBasedClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.useDelayCheckBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.displayDelayUpDown);
            this.groupBox2.Location = new System.Drawing.Point(142, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 45);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Delay";
            // 
            // useDelayCheckBox
            // 
            this.useDelayCheckBox.AutoSize = true;
            this.useDelayCheckBox.Checked = true;
            this.useDelayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useDelayCheckBox.Location = new System.Drawing.Point(7, 21);
            this.useDelayCheckBox.Name = "useDelayCheckBox";
            this.useDelayCheckBox.Size = new System.Drawing.Size(75, 17);
            this.useDelayCheckBox.TabIndex = 2;
            this.useDelayCheckBox.Text = "Use Delay";
            this.useDelayCheckBox.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(239, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Seconds";
            // 
            // displayDelayUpDown
            // 
            this.displayDelayUpDown.Location = new System.Drawing.Point(173, 19);
            this.displayDelayUpDown.Name = "displayDelayUpDown";
            this.displayDelayUpDown.Size = new System.Drawing.Size(60, 20);
            this.displayDelayUpDown.TabIndex = 0;
            this.displayDelayUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numberOFCycles);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numberOfGenerations);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(2, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(562, 47);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generations";
            // 
            // numberOFCycles
            // 
            this.numberOFCycles.Location = new System.Drawing.Point(322, 17);
            this.numberOFCycles.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numberOFCycles.Name = "numberOFCycles";
            this.numberOFCycles.Size = new System.Drawing.Size(49, 20);
            this.numberOFCycles.TabIndex = 3;
            this.numberOFCycles.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(199, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Number Of Cycles";
            // 
            // numberOfGenerations
            // 
            this.numberOfGenerations.Location = new System.Drawing.Point(130, 17);
            this.numberOfGenerations.Name = "numberOfGenerations";
            this.numberOfGenerations.Size = new System.Drawing.Size(51, 20);
            this.numberOfGenerations.TabIndex = 1;
            this.numberOfGenerations.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Number Of Generations";
            // 
            // tabMapPage
            // 
            this.tabMapPage.Controls.Add(this.loadMapButton);
            this.tabMapPage.Controls.Add(this.mapListBox);
            this.tabMapPage.Controls.Add(this.label9);
            this.tabMapPage.Location = new System.Drawing.Point(4, 22);
            this.tabMapPage.Name = "tabMapPage";
            this.tabMapPage.Size = new System.Drawing.Size(567, 85);
            this.tabMapPage.TabIndex = 1;
            this.tabMapPage.Text = "Map Options";
            // 
            // loadMapButton
            // 
            this.loadMapButton.Location = new System.Drawing.Point(481, 153);
            this.loadMapButton.Name = "loadMapButton";
            this.loadMapButton.Size = new System.Drawing.Size(75, 23);
            this.loadMapButton.TabIndex = 2;
            this.loadMapButton.Text = "Load Map";
            this.loadMapButton.UseVisualStyleBackColor = true;
            this.loadMapButton.Click += new System.EventHandler(this.OnLoadMap);
            // 
            // mapListBox
            // 
            this.mapListBox.FormattingEnabled = true;
            this.mapListBox.Location = new System.Drawing.Point(109, 42);
            this.mapListBox.Name = "mapListBox";
            this.mapListBox.Size = new System.Drawing.Size(165, 134);
            this.mapListBox.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(70, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Maps";
            // 
            // ExampleOnePage
            // 
            this.ExampleOnePage.Controls.Add(this.startExampleOneButton);
            this.ExampleOnePage.Controls.Add(this.stopExampleOneButton);
            this.ExampleOnePage.Controls.Add(this.exampleOnePopulationSize);
            this.ExampleOnePage.Controls.Add(this.label7);
            this.ExampleOnePage.Controls.Add(this.label6);
            this.ExampleOnePage.Controls.Add(this.exampleOneStartStringSize);
            this.ExampleOnePage.Controls.Add(this.label1);
            this.ExampleOnePage.Location = new System.Drawing.Point(4, 22);
            this.ExampleOnePage.Name = "ExampleOnePage";
            this.ExampleOnePage.Size = new System.Drawing.Size(567, 85);
            this.ExampleOnePage.TabIndex = 2;
            this.ExampleOnePage.Text = "Example One";
            // 
            // startExampleOneButton
            // 
            this.startExampleOneButton.Location = new System.Drawing.Point(390, 150);
            this.startExampleOneButton.Name = "startExampleOneButton";
            this.startExampleOneButton.Size = new System.Drawing.Size(75, 23);
            this.startExampleOneButton.TabIndex = 8;
            this.startExampleOneButton.Text = "Start";
            this.startExampleOneButton.UseVisualStyleBackColor = true;
            this.startExampleOneButton.Click += new System.EventHandler(this.OnStartExampleOneButton);
            // 
            // stopExampleOneButton
            // 
            this.stopExampleOneButton.Enabled = false;
            this.stopExampleOneButton.Location = new System.Drawing.Point(481, 150);
            this.stopExampleOneButton.Name = "stopExampleOneButton";
            this.stopExampleOneButton.Size = new System.Drawing.Size(75, 23);
            this.stopExampleOneButton.TabIndex = 7;
            this.stopExampleOneButton.Text = "Stop";
            this.stopExampleOneButton.UseVisualStyleBackColor = true;
            this.stopExampleOneButton.Click += new System.EventHandler(this.OnStopExampleOneButton);
            // 
            // exampleOnePopulationSize
            // 
            this.exampleOnePopulationSize.Location = new System.Drawing.Point(336, 23);
            this.exampleOnePopulationSize.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.exampleOnePopulationSize.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.exampleOnePopulationSize.Name = "exampleOnePopulationSize";
            this.exampleOnePopulationSize.Size = new System.Drawing.Size(53, 20);
            this.exampleOnePopulationSize.TabIndex = 6;
            this.exampleOnePopulationSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(229, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Population Size";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Initial String Size";
            // 
            // exampleOneStartStringSize
            // 
            this.exampleOneStartStringSize.Location = new System.Drawing.Point(118, 23);
            this.exampleOneStartStringSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.exampleOneStartStringSize.Name = "exampleOneStartStringSize";
            this.exampleOneStartStringSize.Size = new System.Drawing.Size(56, 20);
            this.exampleOneStartStringSize.TabIndex = 3;
            this.exampleOneStartStringSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(554, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Example One Demonstrates PathFinding using an Expanding Genetics Array";
            // 
            // ExampleTwoPage
            // 
            this.ExampleTwoPage.Controls.Add(this.startExampleTwoButton);
            this.ExampleTwoPage.Controls.Add(this.stopExampleTwoButton);
            this.ExampleTwoPage.Controls.Add(this.exampleTwoPopulationSize);
            this.ExampleTwoPage.Controls.Add(this.label10);
            this.ExampleTwoPage.Controls.Add(this.label11);
            this.ExampleTwoPage.Controls.Add(this.exampleTwoStartStringSize);
            this.ExampleTwoPage.Controls.Add(this.label2);
            this.ExampleTwoPage.Location = new System.Drawing.Point(4, 22);
            this.ExampleTwoPage.Name = "ExampleTwoPage";
            this.ExampleTwoPage.Size = new System.Drawing.Size(567, 369);
            this.ExampleTwoPage.TabIndex = 3;
            this.ExampleTwoPage.Text = "Example Two";
            // 
            // startExampleTwoButton
            // 
            this.startExampleTwoButton.Location = new System.Drawing.Point(390, 150);
            this.startExampleTwoButton.Name = "startExampleTwoButton";
            this.startExampleTwoButton.Size = new System.Drawing.Size(75, 23);
            this.startExampleTwoButton.TabIndex = 12;
            this.startExampleTwoButton.Text = "Start";
            this.startExampleTwoButton.UseVisualStyleBackColor = true;
            this.startExampleTwoButton.Click += new System.EventHandler(this.OnStartExampleTwo);
            // 
            // stopExampleTwoButton
            // 
            this.stopExampleTwoButton.Enabled = false;
            this.stopExampleTwoButton.Location = new System.Drawing.Point(481, 150);
            this.stopExampleTwoButton.Name = "stopExampleTwoButton";
            this.stopExampleTwoButton.Size = new System.Drawing.Size(75, 23);
            this.stopExampleTwoButton.TabIndex = 11;
            this.stopExampleTwoButton.Text = "Stop";
            this.stopExampleTwoButton.UseVisualStyleBackColor = true;
            this.stopExampleTwoButton.Click += new System.EventHandler(this.OnStopExampleTwoButton);
            // 
            // exampleTwoPopulationSize
            // 
            this.exampleTwoPopulationSize.Location = new System.Drawing.Point(343, 43);
            this.exampleTwoPopulationSize.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.exampleTwoPopulationSize.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.exampleTwoPopulationSize.Name = "exampleTwoPopulationSize";
            this.exampleTwoPopulationSize.Size = new System.Drawing.Size(53, 20);
            this.exampleTwoPopulationSize.TabIndex = 10;
            this.exampleTwoPopulationSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(236, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Population Size";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Initial String Size";
            // 
            // exampleTwoStartStringSize
            // 
            this.exampleTwoStartStringSize.Location = new System.Drawing.Point(125, 43);
            this.exampleTwoStartStringSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.exampleTwoStartStringSize.Name = "exampleTwoStartStringSize";
            this.exampleTwoStartStringSize.Size = new System.Drawing.Size(56, 20);
            this.exampleTwoStartStringSize.TabIndex = 7;
            this.exampleTwoStartStringSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(2, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(565, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Example Two Demonstrates A Program Dynamically Improving Its Performance by findi" +
    "ng better/quicker ways to achieve the same result ";
            // 
            // ExampleThreePage
            // 
            this.ExampleThreePage.Controls.Add(this.startExampleThreeButton);
            this.ExampleThreePage.Controls.Add(this.stopExampleThreeButton);
            this.ExampleThreePage.Controls.Add(this.exampleThreePopulationSize);
            this.ExampleThreePage.Controls.Add(this.label12);
            this.ExampleThreePage.Controls.Add(this.label13);
            this.ExampleThreePage.Controls.Add(this.exampleThreeStartStringSize);
            this.ExampleThreePage.Controls.Add(this.label3);
            this.ExampleThreePage.Location = new System.Drawing.Point(4, 22);
            this.ExampleThreePage.Name = "ExampleThreePage";
            this.ExampleThreePage.Size = new System.Drawing.Size(567, 185);
            this.ExampleThreePage.TabIndex = 4;
            this.ExampleThreePage.Text = "Example Three";
            // 
            // startExampleThreeButton
            // 
            this.startExampleThreeButton.Location = new System.Drawing.Point(390, 150);
            this.startExampleThreeButton.Name = "startExampleThreeButton";
            this.startExampleThreeButton.Size = new System.Drawing.Size(75, 23);
            this.startExampleThreeButton.TabIndex = 18;
            this.startExampleThreeButton.Text = "Start";
            this.startExampleThreeButton.UseVisualStyleBackColor = true;
            this.startExampleThreeButton.Click += new System.EventHandler(this.OnStartExampleThree);
            // 
            // stopExampleThreeButton
            // 
            this.stopExampleThreeButton.Enabled = false;
            this.stopExampleThreeButton.Location = new System.Drawing.Point(481, 150);
            this.stopExampleThreeButton.Name = "stopExampleThreeButton";
            this.stopExampleThreeButton.Size = new System.Drawing.Size(75, 23);
            this.stopExampleThreeButton.TabIndex = 17;
            this.stopExampleThreeButton.Text = "Stop";
            this.stopExampleThreeButton.UseVisualStyleBackColor = true;
            this.stopExampleThreeButton.Click += new System.EventHandler(this.OnStopExampleThreeButton);
            // 
            // exampleThreePopulationSize
            // 
            this.exampleThreePopulationSize.Location = new System.Drawing.Point(364, 25);
            this.exampleThreePopulationSize.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.exampleThreePopulationSize.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.exampleThreePopulationSize.Name = "exampleThreePopulationSize";
            this.exampleThreePopulationSize.Size = new System.Drawing.Size(53, 20);
            this.exampleThreePopulationSize.TabIndex = 14;
            this.exampleThreePopulationSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(257, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Population Size";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(39, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Initial String Size";
            // 
            // exampleThreeStartStringSize
            // 
            this.exampleThreeStartStringSize.Location = new System.Drawing.Point(146, 25);
            this.exampleThreeStartStringSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.exampleThreeStartStringSize.Name = "exampleThreeStartStringSize";
            this.exampleThreeStartStringSize.Size = new System.Drawing.Size(56, 20);
            this.exampleThreeStartStringSize.TabIndex = 11;
            this.exampleThreeStartStringSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(2, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(565, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Example Three Demonstrates a program reconfiguring itself to continue working aft" +
    "er catastrophic failure";
            // 
            // colorOptionsTab
            // 
            this.colorOptionsTab.Location = new System.Drawing.Point(0, 0);
            this.colorOptionsTab.Name = "colorOptionsTab";
            this.colorOptionsTab.Size = new System.Drawing.Size(200, 100);
            this.colorOptionsTab.TabIndex = 0;
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Location = new System.Drawing.Point(559, 12);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(567, 63);
            this.textBox.TabIndex = 2;
            this.textBox.Text = "";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1051, 81);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save Output";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnSaveOutput);
            // 
            // saveOutputFileDlg
            // 
            this.saveOutputFileDlg.Filter = "Text files (*.txt)|*.txt";
            // 
            // guiTimer
            // 
            this.guiTimer.Interval = 1000;
            this.guiTimer.Tick += new System.EventHandler(this.OnGuiTimer);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1138, 533);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.geneticsTabControl);
            this.Name = "Form1";
            this.Text = "Genetics Dev Two";
            this.Activated += new System.EventHandler(this.OnActivated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.geneticsTabControl.ResumeLayout(false);
            this.tabGeneticOptionsPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.displayDelayUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberOFCycles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfGenerations)).EndInit();
            this.tabMapPage.ResumeLayout(false);
            this.tabMapPage.PerformLayout();
            this.ExampleOnePage.ResumeLayout(false);
            this.ExampleOnePage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exampleOnePopulationSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exampleOneStartStringSize)).EndInit();
            this.ExampleTwoPage.ResumeLayout(false);
            this.ExampleTwoPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exampleTwoPopulationSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exampleTwoStartStringSize)).EndInit();
            this.ExampleThreePage.ResumeLayout(false);
            this.ExampleThreePage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exampleThreePopulationSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exampleThreeStartStringSize)).EndInit();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.NumericUpDown numberOfGenerations;
        private System.Windows.Forms.RichTextBox textBox;
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void OnActivated(object sender, System.EventArgs e)
		{
			geneticsBoard1.InvalidateBoard();
		}

		private void LoadMap()
		{
			if( strMap == null )
			{
				MessageBox.Show( "Error no map name is set" );
				return;
			}

            geneticsBoard1.ClearSquareInformation();

			XmlTextReader xmlReader = null;

			try
			{
				StreamReader stream = new StreamReader( strMap );
				xmlReader = new XmlTextReader( stream );
			}
			catch( ArgumentException argExp )
			{
				MessageBox.Show( "Error Argument Exception thrown, message "  + argExp.Message );
				return;
			}
			catch( FileNotFoundException fnfExp )
			{
				MessageBox.Show( "Error File Not Found Exception thrown, message " + fnfExp.Message );
				return;
			}
			catch( DirectoryNotFoundException dnfExp )
			{
				MessageBox.Show( "Error Directory Not Found Exception thrown, message " + dnfExp.Message );
				return;
			}


			for( int i=0; i<64; i++ )
			{
				SquareInformation squareInfo = new SquareInformation();

				while( xmlReader.Name != "Square" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				squareInfo.Square = xmlReader.Value;

				while( xmlReader.Name != "Start" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				squareInfo.Start = XmlConvert.ToBoolean( xmlReader.Value );

				while( xmlReader.Name != "Finish" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				squareInfo.Finish = XmlConvert.ToBoolean( xmlReader.Value );

				while( xmlReader.Name != "Up" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				squareInfo.Up = XmlConvert.ToBoolean( xmlReader.Value );

				while( xmlReader.Name != "Right" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				squareInfo.Right = XmlConvert.ToBoolean( xmlReader.Value );

				while( xmlReader.Name != "Down" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				squareInfo.Down = XmlConvert.ToBoolean( xmlReader.Value );

				while( xmlReader.Name != "Left" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				squareInfo.Left = XmlConvert.ToBoolean( xmlReader.Value );

				geneticsBoard1.AddSquareInformation( squareInfo );

			}

			xmlReader.Close();
				
		}

        private void Settings( GeneticsPath geneticsPath )
        {
            geneticsPath.NumberOfGenerations = ( int )numberOfGenerations.Value;
            tsTextBox.AppendTextWithColour( "Number Of Generations Set To " + geneticsPath.NumberOfGenerations.ToString(), Color.Black );
            geneticsPath.NumberOfCyclesPerGeneration = ( int )numberOFCycles.Value;
            tsTextBox.AppendTextWithColour( "Number Of Cycles Per Generation Set To " + geneticsPath.NumberOfCyclesPerGeneration.ToString(), Color.Black );

///            if( rouletteWheelCheckBox.Checked == true ) 
///            {
                geneticsPath.SelectionMethod = GeneticsLibrary.SELECTIONMETHODS.ROULETTEWHEEL;
                tsTextBox.AppendTextWithColour( "Selection Method Set To Roulette Wheel Selection", Color.Blue );
 /*           }

            if( fixedPointCheckBox.Checked == true )
            {
                geneticsPath.SelectionMethod = GeneticsLibrary.SELECTIONMETHODS.FIXEDPOINTSELECTION;
                tsTextBox.AppendTextWithColour( "Selection Method Set To Fixed Point Selection", Color.Blue );
            }
*/
///            if( singlePointCheckBox.Checked == true )
///            {
                geneticsPath.CrossOverMethod = GeneticsLibrary.CROSSOVERMETHODS.SINGLEPOINTCROSSOVER;
                tsTextBox.AppendTextWithColour( "Crossover Method Set To Single Point Crossover", Color.BlueViolet );
                geneticsPath.SingleCrossOverPoint = geneticsPath.StartingLength /2;
                if( geneticsPath.StartingLength/2 == 0 )
                	MessageBox.Show( "The Single Point Crossover length has been screwed up in the Settings function" );
 /*           }

            if( dualPointCheckBox.Checked == true )
            {
                geneticsPath.CrossOverMethod = GeneticsLibrary.CROSSOVERMETHODS.DUALCROSSOVER;
                tsTextBox.AppendTextWithColour( "Crossover Method Set To Dual Crossover", Color.BlueViolet );
            }

            if( randomPointCheckBox.Checked == true )
            {
                geneticsPath.CrossOverMethod = GeneticsLibrary.CROSSOVERMETHODS.RANDOMCROSSOVER;
                tsTextBox.AppendTextWithColour( "Crossover Method Set To Random Crossover", Color.BlueViolet );
            }

            if( positionBasedCheckBox.Checked == true )
            {
                geneticsPath.CrossOverMethod = GeneticsLibrary.CROSSOVERMETHODS.POSITIONBASEDCROSSOVER;
                tsTextBox.AppendTextWithColour( "Crossover Method Set To Position Based Crossover", Color.BlueViolet );
            }
            */
        }

        private void RunExampleOne()
        {
            
        	Monitor.Enter( tExampleOne );
            bExampleOneRunning = true;
            bIsFinishDrawn = false;
            Monitor.Exit( tExampleOne );
        	
            bAllowClose = false;
        	
        	tsTextBox.AppendTextWithColour( "Starting Example One Thread", Color.Black );
        
        	gpExampleOne.SetExandingGeneticsArray( 100, ( int )exampleOneStartStringSize.Value, 10 );
        	
            Settings( gpExampleOne );
            GeneticsPathString.InitialChromosomeLength = ( int )exampleOneStartStringSize.Value;
            gpExampleOne.PopulationSize = ( int )exampleOnePopulationSize.Value;
            
            /// set up display strings
            /// 
            gpExampleOne.ShowCreationStrings = false;

            tsTextBox.AppendTextWithColour( "Beginning Example One Initialisation", Color.Blue );

            /// must be set before calling initialise
            /// 
            gpExampleOne.UseLocalInformation = false;
            gpExampleOne.Initialise();
            gpExampleOne.GeneticsArrayMutex = geneticsArrayMutex;
            
            Monitor.Enter( tExampleOne );
            bInitialised = true;
            Monitor.Exit( tExampleOne );
            
            /// if application is closed while initialising
            /// 
            Monitor.Enter( tExampleOne );
            if( bStopThreadOne == true )
            {
            	tsTextBox.AppendTextWithColour( "Example One Stopped", Color.Black );
            	bExampleOneRunning = false;
            	return;
            }
            Monitor.Exit( tExampleOne );
            
            tsTextBox.AppendTextWithColour( "Example One Initialised", Color.Blue );

            bool bStop = false;
            int nRunCount = 0;
            int nGenerationCount = 0;

            gpsPreviousSolutionString = null;
            
            while( bStop == false )
            {
                if( useDelayCheckBox.Checked == true )
                {
                	System.Threading.Thread.Sleep( ( int )( displayDelayUpDown.Value * 100 ) );
                }

                Monitor.Enter( tExampleOne );
                if( bStopThreadOne == true )
                {
                    Monitor.Exit( tExampleOne );
                    bStop = true;
                    geneticsBoard1.Invalidate();
        	///		geneticsBoard1.Update();
                    continue;
                    
                }
                Monitor.Exit( tExampleOne );
                
                gpExampleOne.IsFinishFound = false;

                if( bPrintRunMessages == true )
                {
                	tsTextBox.AppendTextWithColour( "\n Run Number " + nRunCount.ToString(), Color.BlueViolet );
                }

                /// At this point we are checking to see if any of the strings in the population 
                /// have reached our end point criteria which in this case is the end of the map
                /// 
                
                SetValidPaths( gpExampleOne );
                if( bDebugProgress == true )
                	Debug.WriteLine( "Set Valid Paths finished for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );

                if( bStop == false )
                {	
                	/// do fitness and run
                	/// 
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Entering Fitness function for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );
                	
                	ExampleOneFitness( gpExampleOne, tExampleOne, bStopThreadOne );

                    if( bFinishIsSet == true )
                    {
                        tsTextBox.AppendTextWithColour( "At generation number " + nGenerationCount.ToString() + " run number " + nRunCount.ToString() + " the algorythm found the finish.", Color.Red );
                        geneticsBoard1.SetCurrentSolution( gpsCurrentAttemptString );
                        OnGuiTimer( this, new EventArgs() );
                        bStop = true;
                        break;
                    }
                	
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Fitness Function finished, entering run for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );
                	
                	gpExampleOne.Run();
                	
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Run finished for run " + nRunCount.ToString() );
                	
                	nRunCount++;
                	
                	if( nGenerationCount == gpExampleOne.NumberOfGenerations )
                	{
                		if( bPrintGenerationMessages == true )
                		{
                			tsTextBox.AppendTextWithColour( "Number Of Generations Reached, Halting Algorythm", Color.BlueViolet );
                		}
                		
                		bStop = true;
                        break;
                	}

                	/// draw the best previous attempt
                	/// 
                	int nMaxSize = 0;
                    GeneticsPathString gpsTest = null;
                       	
                    for( int i=0; i<gpExampleOne.PopulationSize; i++ )
                    {
                       	gpsTest = ( GeneticsPathString )gpExampleOne.GeneticsArray[ i ];

                       	if( gpsTest.LengthTravelled > nMaxSize )
                       	{
                           	nMaxSize = gpsTest.LengthTravelled;
                           	gpsMaxString = gpsTest;
                       	}
                    }
                    
                	if( nRunCount == gpExampleOne.NumberOfCyclesPerGeneration )
                	{
                		if( gpsCurrentAttemptString.HasReachedFinish == true && gpsCurrentAttemptString.LengthTravelled >= gpsMaxString.LengthTravelled )
                		{
                			geneticsBoard1.ResetPreviousSolution();
                       		geneticsBoard1.SetPreviousSolution( gpsMaxString );
                       		gpsPreviousSolutionString = gpsMaxString;
                		}
                		
                		if( bPrintGenerationMessages == true )
                		{
                			tsTextBox.AppendTextWithColour( "Number Of Cycles Run For Generation " + nGenerationCount.ToString() + ", Reinitialising Genetics Array", Color.BlueViolet );
                		}
                		
                		gpExampleOne.Initialise();	
                        gpExampleOne.SingleCrossOverPoint = gpExampleOne.StartingLength /2;
                        if( gpExampleOne.SingleCrossOverPoint == 0 )
                        	Debug.WriteLine( "The Single Point CrossOver has been screwed up in run after the initialisation" );
                		nGenerationCount++;
                		nRunCount = 0;		
                	}
                	
                	if( bDrawBestGenerationString == true )
                    {
                       nMaxSize = 0;
                       gpsTest = null;

                       for( int i=0; i<gpExampleOne.PopulationSize; i++ )
                       {
                          gpsTest = ( GeneticsPathString )gpExampleOne.GeneticsArray[ i ];

                          if( gpsTest.LengthTravelled >= nMaxSize )
                          {
                             nMaxSize = gpsTest.LengthTravelled;
                             gpsMaxString = gpsTest;
                          }
                       }

    ///                   geneticsBoard1.SetCurrentSolution( gpsMaxString );
    /// 
    				   if( bFinishIsSet == false )
    				      gpsCurrentAttemptString = gpsMaxString;
                       
                       if( bPrintFullBestString == true )
                       {
                       	  tsTextBox.AppendTextWithColour( "Full Best Debug String :- " + gpsMaxString.FullDebugString(), Color.Brown );
                       }

                    }
                	
                	if( bPrintSampleDebugString == true )
                	{
						tsTextBox.AppendTextWithColour( "Sample Debug String :- " + ( ( GeneticsPathString )gpExampleOne.GeneticsArray[ 0 ] ).FullDebugString(), Color.Brown );
                	}
                }

            }
            
            tsTextBox.AppendTextWithColour( "Example One Stopped", Color.Black );
            bAllowClose = true;
            Monitor.Enter( tExampleOne );
            bExampleOneRunning = false;
            Monitor.Exit( tExampleOne );
            StopExampleOneCallback c = new StopExampleOneCallback( this.OnStopExampleOneButton );
            Invoke( c, new object[]{ this, new EventArgs() } );
           /// OnStopExampleOneButton( this, new EventArgs() );
        }

        private void RunExampleTwo()
        {
        	StopExampleTwoCallback c = new StopExampleTwoCallback( this.OnStopExampleTwoButton );

        	Monitor.Enter( tExampleTwo );
            bExampleTwoRunning = true;
            bIsFinishDrawn = false;
            Monitor.Exit( tExampleTwo );
        	
            bAllowClose = false;
        	
        	tsTextBox.AppendTextWithColour( "Starting Example Two Thread", Color.Black );
        
        	/// @TODO Allow these to be set through the gui later
        	/// 
        	gpExampleTwo.SetExandingGeneticsArray( 100, 10, 10 );
        	
            Settings( gpExampleTwo );
            GeneticsPathString.InitialChromosomeLength = ( int )exampleTwoStartStringSize.Value;
            gpExampleTwo.PopulationSize = ( int )exampleTwoPopulationSize.Value;
            
            /// set up display strings
            /// 
            gpExampleTwo.ShowCreationStrings = false;

            tsTextBox.AppendTextWithColour( "Beginning Example Two Initialisation", Color.Blue );

            /// must be set before calling initialise
            /// 
            gpExampleTwo.UseLocalInformation = false;
            gpExampleTwo.Initialise();
            gpExampleTwo.GeneticsArrayMutex = geneticsArrayMutex;
            
            Monitor.Enter( tExampleTwo );
            bInitialised = true;
            Monitor.Exit( tExampleTwo );
            
            /// if application is closed while initialising
            /// 
            Monitor.Enter( tExampleTwo );
            if( bStopThreadTwo == true )
            {
            	tsTextBox.AppendTextWithColour( "Example Two Stopped", Color.Black );
            	bExampleTwoRunning = false;
            	return;
            }
            Monitor.Exit( tExampleTwo );
            
            tsTextBox.AppendTextWithColour( "Example Two Initialised", Color.Blue );

            /// now see if there is a saved path for the current map
            /// 

            StringBuilder strMapName = new StringBuilder( strMap );

            strMapName = strMapName.Replace( ".xml", "ExampleTwo.sav" );
            
            if( File.Exists( strMapName.ToString() ) == true )
            {
                LoadPreviousSolutionString( strMapName.ToString() );
            }
            else
            {
                /// if the file is not found we don't have a valid file saved for this map.
                /// 
                while( RunInitialSearch( gpExampleTwo, tExampleTwo, ref bStopThreadTwo ) == false );

                SavePreviousSolutionString( strMapName.ToString() );

                Settings( gpExampleTwo );
                GeneticsPathString.InitialChromosomeLength = ( int )exampleTwoStartStringSize.Value;
                gpExampleTwo.PopulationSize = ( int )exampleTwoPopulationSize.Value;
                /// clear the board
                /// 
                geneticsBoard1.ResetCurrentAttempt();
                geneticsBoard1.ResetCurrentSolution();

                gpExampleTwo.Initialise();

                Monitor.Enter( tExampleTwo );
                bFinishIsSet = false;
                Monitor.Exit( tExampleTwo );

            }
            
            /// <summary>
            ///  We've now got a previous example of how to find the finish for this map
            ///  So run the code until we find a route that does it quicker.
            ///  This is a simple Technique and in a production environment there would
            ///  probably more suitable criteria to define what is the better set of routines
            ///  to perform a specific task but it serves for demonstration purposes.
            /// </summary>

            geneticsBoard1.SetPreviousSolution( gpsPreviousSolutionString );

            bool bStop = false;
            int nRunCount = 0;
            int nGenerationCount = 0;
            
            while( bStop == false )
            {
                if( useDelayCheckBox.Checked == true )
                {
                	System.Threading.Thread.Sleep( ( int )( displayDelayUpDown.Value * 100 ) );
                }

                Monitor.Enter( tExampleTwo );
                if( bStopThreadTwo == true )
                {
                    Monitor.Exit( tExampleTwo );
                    bStop = true;
                    geneticsBoard1.Invalidate();
        	///		geneticsBoard1.Update();
                    continue;
                    
                }
                Monitor.Exit( tExampleTwo );
                
                gpExampleTwo.IsFinishFound = false;

                if( bPrintRunMessages == true )
                {
                	tsTextBox.AppendTextWithColour( "\n Run Number " + nRunCount.ToString(), Color.BlueViolet );
                }

                /// At this point we are checking to see if any of the strings in the population 
                /// have reached our end point criteria which in this case is the end of the map
                /// 
                
                SetValidPaths( gpExampleTwo );
                if( bDebugProgress == true )
                	Debug.WriteLine( "Set Valid Paths finished for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );

                if( bStop == false )
                {	
                	/// do fitness and run
                	/// 
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Entering Fitness function for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );
                	
                	ExampleOneFitness( gpExampleTwo, tExampleTwo, bStopThreadTwo );

                    if( bFinishIsSet == true )
	               	{
	                	tsTextBox.AppendTextWithColour( "At generation number " + nGenerationCount.ToString() + " run number " + nRunCount.ToString() + " the algorythm found the finish.", Color.Red );
	                    
                        /// This is where we check that the length of the completed string is less than the length of the 
                        /// previous solution. It should be noted that for this project this is just a simple check and 
                        /// therefore not that accurate. Though by definition if you keep running it on the same map it should in
                        /// time find the obviousely shortest route.
                        /// 
                        
                        if( gpsCurrentAttemptString.LengthTravelled <= gpsPreviousSolutionString.LengthTravelled )
                        {
                            /// Of course it's a short step from here to repeatedly running the program in the 
                            /// style used on the RunInitialSearch function to find the shortest route.
                            /// 

                            gpsPreviousSolutionString = gpsCurrentAttemptString;
                            SavePreviousSolutionString( strMapName.ToString() );
                        
                            geneticsBoard1.SetCurrentSolution( gpsCurrentAttemptString );
	                	    OnGuiTimer( this, new EventArgs() );
	                        bStop = true;
	                        break;
                        }
                        else
                        {
                            tsTextBox.AppendTextWithColour( "The found Solution is not a valid solution", Color.Red );
                            bFinishIsSet = false;
                        }

	               	}
                	
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Fitness Function finished, entering run for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );
                	
                	gpExampleTwo.Run();
                	
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Run finished for run " + nRunCount.ToString() );
                	
                	nRunCount++;
                	
                	if( nGenerationCount == gpExampleTwo.NumberOfGenerations )
                	{
                		if( bPrintGenerationMessages == true )
                		{
                			tsTextBox.AppendTextWithColour( "Number Of Generations Reached, Halting Algorythm", Color.BlueViolet );
                		}
                		
                		bStop = true;
                        	break;
                	}

                	/// draw the best previous attempt
                	/// 
                    int nMaxSize = 0;
                    GeneticsPathString gpsTest = null;
                       	
                    for( int i=0; i<gpExampleTwo.PopulationSize; i++ )
                    {
                       	gpsTest = ( GeneticsPathString )gpExampleTwo.GeneticsArray[ i ];

                       	if( gpsTest.LengthTravelled > nMaxSize )
                       	{
                           	nMaxSize = gpsTest.LengthTravelled;
                           	gpsMaxString = gpsTest;
                       	}
                    }
                    
                	if( nRunCount == gpExampleTwo.NumberOfCyclesPerGeneration )
                	{
                        /// draw the longest current solution string if finish not found
                        /// 

                        if( bFinishIsSet == false )
                        {
                            nMaxSize = 0;
                       	    gpsTest = null;
                            
                            for( int i=0; i<gpExampleTwo.PopulationSize; i++ )
                       	    {
                         	    gpsTest = ( GeneticsPathString )gpExampleTwo.GeneticsArray[ i ];

                          	    if( gpsTest.LengthTravelled >= nMaxSize )
                          	    {
                             	    nMaxSize = gpsTest.LengthTravelled;
                             	    gpsMaxString = gpsTest;
                          	    }
                       	    }
                           	
                       	    if( bFinishIsSet == false )
    				          gpsCurrentSolutionString = gpsMaxString;
                        }
		
                		if( bPrintGenerationMessages == true )
                		{
                			tsTextBox.AppendTextWithColour( "Number Of Cycles Run For Generation " + nGenerationCount.ToString() + ", Reinitialising Genetics Array", Color.BlueViolet );
                		}
                		
                		gpExampleTwo.Initialise();	
                        gpExampleTwo.SingleCrossOverPoint = gpExampleTwo.StartingLength /2;
                        if( gpExampleTwo.SingleCrossOverPoint == 0 )
                        	Debug.WriteLine( "The Single Point CrossOver has been screwed up in run after the initialisation" );
                	///	SetValidPaths( gpExampleOne );
                		nGenerationCount++;
                		nRunCount = 0;		
                	}
                	
                 	if( bDrawBestGenerationString == true )
                    {
                      	nMaxSize = 0;
                       	gpsTest = null;

                       	for( int i=0; i<gpExampleTwo.PopulationSize; i++ )
                       	{
                         	gpsTest = ( GeneticsPathString )gpExampleTwo.GeneticsArray[ i ];

                          	if( gpsTest.LengthTravelled >= nMaxSize )
                          	{
                             	nMaxSize = gpsTest.LengthTravelled;
                             	gpsMaxString = gpsTest;
                          	}
                       	}
                       	
                       	if( bFinishIsSet == false )
    				      gpsCurrentAttemptString = gpsMaxString;
                       
                       	if( bPrintFullBestString == true )
                       	{
                       	  	tsTextBox.AppendTextWithColour( "Full Best Debug String :- " + gpsMaxString.FullDebugString(), Color.Brown );
                       	}

                    }
                	
                	if( bPrintSampleDebugString == true )
                	{
						tsTextBox.AppendTextWithColour( "Sample Debug String :- " + ( ( GeneticsPathString )gpExampleTwo.GeneticsArray[ 0 ] ).FullDebugString(), Color.Brown );
                	}
                }

            }

            
            tsTextBox.AppendTextWithColour( "Example Two Stopped", Color.Black );
            bAllowClose = true;
            Monitor.Enter( tExampleTwo );
            bExampleTwoRunning = false;
            Monitor.Exit( tExampleTwo );
            Invoke( c, new object[]{ this, new EventArgs() } );
           /// OnStopExampleOneButton( this, new EventArgs() );


        	bAllowClose = true;
        }

        private void RunExampleThree()
        {
        	bAllowClose = false;
        	StopExampleThreeCallback c = new StopExampleThreeCallback( this.OnStopExampleThreeButton );

        	Monitor.Enter( tExampleThree );
            bExampleThreeRunning = true;
            bIsFinishDrawn = false;
            Monitor.Exit( tExampleThree );
        	
            bAllowClose = false;
        	
        	tsTextBox.AppendTextWithColour( "Starting Example Three Thread", Color.Black );
        
        	/// @TODO Allow these to be set through the gui later
        	/// 
        	gpExampleThree.SetExandingGeneticsArray( 100, 10, 10 );
        	
            Settings( gpExampleThree );
            GeneticsPathString.InitialChromosomeLength = ( int )exampleThreeStartStringSize.Value;
            gpExampleThree.PopulationSize = ( int )exampleThreePopulationSize.Value;
            
            /// set up display strings
            /// 
            gpExampleThree.ShowCreationStrings = false;

            tsTextBox.AppendTextWithColour( "Beginning Example Three Initialisation", Color.Blue );

            /// must be set before calling initialise
            /// 
            gpExampleThree.UseLocalInformation = false;
            gpExampleThree.Initialise();
            gpExampleThree.GeneticsArrayMutex = geneticsArrayMutex;
            
            Monitor.Enter( tExampleThree );
            bInitialised = true;
            Monitor.Exit( tExampleThree );
            
            /// if application is closed while initialising
            /// 
            Monitor.Enter( tExampleThree );
            if( bStopThreadThree == true )
            {
            	tsTextBox.AppendTextWithColour( "Example Three Stopped", Color.Black );
            	bExampleThreeRunning = false;
            	return;
            }
            Monitor.Exit( tExampleThree );
            
            tsTextBox.AppendTextWithColour( "Example Three Initialised", Color.Blue );

            /// now see if there is a saved path for the current map
            /// 

            StringBuilder strMapName = new StringBuilder( strMap );

            strMapName = strMapName.Replace( ".xml", "ExampleThree.sav" );
            
            if( File.Exists( strMapName.ToString() ) == true )
            {
                LoadPreviousSolutionString( strMapName.ToString() );
            }
            else
            {
                /// if the file is not found we don't have a valid file saved for this map.
                /// 
                while( RunInitialSearch( gpExampleThree, tExampleThree, ref bStopThreadThree ) == false );

                SavePreviousSolutionString( strMapName.ToString() );

                Settings( gpExampleThree );
                GeneticsPathString.InitialChromosomeLength = ( int )exampleThreeStartStringSize.Value;
                gpExampleThree.PopulationSize = ( int )exampleThreePopulationSize.Value;
                /// clear the board
                /// 
                geneticsBoard1.ResetCurrentAttempt();
                geneticsBoard1.ResetCurrentSolution();

                gpExampleThree.Initialise();

                Monitor.Enter( tExampleThree );
                bFinishIsSet = false;
                Monitor.Exit( tExampleThree );

            }
            
            /// <summary>
            /// Now that we have the path 
            /// block it and try to find a way around it.
            /// </summary>
            
            int nBlockSquare = random.Random( gpsPreviousSolutionString.LengthTravelled -10 );
            /// give it a bit of space at the start
            /// ( Note this wont always be effective as the current code allows the algorythm to move back on itself )
            /// 
            if( nBlockSquare < 10 )
            {
                nBlockSquare += 10;
            }
            

            /// clear any previously blocked squares
            /// 
            string strSquare = geneticsBoard1.GetStartSquare();
            
            GeneticsSquare gsSearchSquare = ( GeneticsSquare )geneticsBoard1.GetSquare( strSquare );
            GeneticsSquare gsTempSquare = null;
            GeneticsPathItem gpiPathItem = null;

            foreach( DictionaryEntry dicEnt in geneticsBoard1.GetHashTable )
            {
                gsTempSquare = ( GeneticsSquare )dicEnt.Value;

                if( gsTempSquare.Blocked == true )
                {
                    gsTempSquare.Blocked = false;
                    gsTempSquare.DrawBlocked = false;
                }
            }

            if( geneticsBoard1.InvokeRequired == true )
            {
                InvalidateCallback d = new InvalidateCallback( Invalidate );
                geneticsBoard1.Invoke( d, new object[]{} );

                UpdateCallback f = new UpdateCallback( Update );
                geneticsBoard1.Invoke( f, new object[]{} );
            }
            else
            {
        	   geneticsBoard1.Invalidate();
        	   geneticsBoard1.Update();
            }  
            
            /// now find the new square and block it.
            /// 

            for( int i=0; i<nBlockSquare; i++ )
            {
                gpiPathItem = ( GeneticsPathItem )gpsPreviousSolutionString.GeneticsString[ i ];
                if( gpiPathItem == null )
                {
                    MessageBox.Show( "Error, trying to get item " + i.ToString() + " from the previous solution string" );
                    return;
                }

                switch( gpiPathItem.Direction )
                {
                    case "Up": gsTempSquare = ( GeneticsSquare )geneticsBoard1.GetSquareAbove( gsSearchSquare.Identifier ); break;
                    case "Right": gsTempSquare = ( GeneticsSquare )geneticsBoard1.GetSquareToRight( gsSearchSquare.Identifier ); break;
                    case "Down": gsTempSquare = ( GeneticsSquare )geneticsBoard1.GetSquareBelow( gsSearchSquare.Identifier ); break;
                    case "Left": gsTempSquare = ( GeneticsSquare )geneticsBoard1.GetSquareToLeft( gsSearchSquare.Identifier ); break;
                }
                
                if( gsTempSquare != null )
                {
                	gsSearchSquare = gsTempSquare;
                }
            }

            gsSearchSquare.Blocked = true;
            gsSearchSquare.DrawBlocked = true;


           
            bool bStop = false;
            int nRunCount = 0;
            int nGenerationCount = 0;
            
            while( bStop == false )
            {
                if( useDelayCheckBox.Checked == true )
                {
                	System.Threading.Thread.Sleep( ( int )( displayDelayUpDown.Value * 100 ) );
                }

                Monitor.Enter( tExampleThree );
                if( bStopThreadThree == true )
                {
                    Monitor.Exit( tExampleThree );
                    bStop = true;
                    geneticsBoard1.Invalidate();
                    continue;
                    
                }
                Monitor.Exit( tExampleThree );
                
                gpExampleThree.IsFinishFound = false;

                if( bPrintRunMessages == true )
                {
                	tsTextBox.AppendTextWithColour( "\n Run Number " + nRunCount.ToString(), Color.BlueViolet );
                }

                /// At this point we are checking to see if any of the strings in the population 
                /// have reached our end point criteria which in this case is the end of the map
                /// 
                
                SetValidPaths( gpExampleThree );
                if( bDebugProgress == true )
                	Debug.WriteLine( "Set Valid Paths finished for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );

                if( bStop == false )
                {	
                	/// do fitness and run
                	/// 
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Entering Fitness function for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );
                	
                	ExampleOneFitness( gpExampleThree, tExampleThree, bStopThreadThree );

                    if( bFinishIsSet == true )
	               	{
                        /// note the example one code is used here to set the finish as we only
                        /// care if a valid finish has been reached not about the size
                        /// 

	                	tsTextBox.AppendTextWithColour( "At generation number " + nGenerationCount.ToString() + " run number " + nRunCount.ToString() + " the algorythm found the finish.", Color.Red );
                        geneticsBoard1.SetCurrentSolution( gpsCurrentAttemptString );
                        gpsPreviousSolutionString = gpsCurrentAttemptString;
                        SavePreviousSolutionString( strMapName.ToString() );
                        OnGuiTimer( this, new EventArgs() );
                        bStop = true;
                        break;
	               	}
                	
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Fitness Function finished, entering run for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );
                	
                	gpExampleThree.Run();
                	
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Run finished for run " + nRunCount.ToString() );
                	
                	nRunCount++;
                	
                	if( nGenerationCount == gpExampleThree.NumberOfGenerations )
                	{
                		if( bPrintGenerationMessages == true )
                		{
                			tsTextBox.AppendTextWithColour( "Number Of Generations Reached, Halting Algorythm", Color.BlueViolet );
                		}
                		
                		bStop = true;
                        	break;
                	}

                	/// draw the best previous attempt
                	/// 
                    int nMaxSize = 0;
                    GeneticsPathString gpsTest = null;
                       	
                    for( int i=0; i<gpExampleThree.PopulationSize; i++ )
                    {
                       	gpsTest = ( GeneticsPathString )gpExampleThree.GeneticsArray[ i ];

                       	if( gpsTest.LengthTravelled > nMaxSize )
                       	{
                           	nMaxSize = gpsTest.LengthTravelled;
                           	gpsMaxString = gpsTest;
                       	}
                    }
                    
                	if( nRunCount == gpExampleThree.NumberOfCyclesPerGeneration )
                	{
                        /// draw the longest current solution string if finish not found
                        /// 

                        if( bFinishIsSet == false )
                        {
                            nMaxSize = 0;
                       	    gpsTest = null;
                            
                            for( int i=0; i<gpExampleThree.PopulationSize; i++ )
                       	    {
                         	    gpsTest = ( GeneticsPathString )gpExampleThree.GeneticsArray[ i ];

                          	    if( gpsTest.LengthTravelled >= nMaxSize )
                          	    {
                             	    nMaxSize = gpsTest.LengthTravelled;
                             	    gpsMaxString = gpsTest;
                          	    }
                       	    }
                           	
                       	    if( bFinishIsSet == false )
    				          gpsCurrentSolutionString = gpsMaxString;
                        }
		
                		if( bPrintGenerationMessages == true )
                		{
                			tsTextBox.AppendTextWithColour( "Number Of Cycles Run For Generation " + nGenerationCount.ToString() + ", Reinitialising Genetics Array", Color.BlueViolet );
                		}
                		
                		gpExampleThree.Initialise();	
                        gpExampleThree.SingleCrossOverPoint = gpExampleThree.StartingLength /2;
                        if( gpExampleThree.SingleCrossOverPoint == 0 )
                        	Debug.WriteLine( "The Single Point CrossOver has been screwed up in run after the initialisation" );
                		nGenerationCount++;
                		nRunCount = 0;		
                	}
                	
                 	if( bDrawBestGenerationString == true )
                    {
                      	nMaxSize = 0;
                       	gpsTest = null;

                       	for( int i=0; i<gpExampleThree.PopulationSize; i++ )
                       	{
                         	gpsTest = ( GeneticsPathString )gpExampleThree.GeneticsArray[ i ];

                          	if( gpsTest.LengthTravelled >= nMaxSize )
                          	{
                             	nMaxSize = gpsTest.LengthTravelled;
                             	gpsMaxString = gpsTest;
                          	}
                       	}
                       	
                       	if( bFinishIsSet == false )
    				      gpsCurrentAttemptString = gpsMaxString;
                       
                       	if( bPrintFullBestString == true )
                       	{
                       	  	tsTextBox.AppendTextWithColour( "Full Best Debug String :- " + gpsMaxString.FullDebugString(), Color.Brown );
                       	}

                    }
                	
                	if( bPrintSampleDebugString == true )
                	{
						tsTextBox.AppendTextWithColour( "Sample Debug String :- " + ( ( GeneticsPathString )gpExampleThree.GeneticsArray[ 0 ] ).FullDebugString(), Color.Brown );
                	}
                }

            }

            
            tsTextBox.AppendTextWithColour( "Example Two Stopped", Color.Black );
            bAllowClose = true;
            Monitor.Enter( tExampleThree );
            bExampleTwoRunning = false;
            Monitor.Exit( tExampleThree );
            Invoke( c, new object[]{ this, new EventArgs() } );

        	
        	bAllowClose = true;
        }

        private void OnStartExampleOneButton( object sender, EventArgs e )
        {
			bFinishIsSet = false;
            tExampleOne = new Thread ( tsExampleOne );
        	
            Monitor.Enter( tExampleOne );
        	bInitialised = false;
        	Monitor.Exit( tExampleOne );
        	
            startExampleOneButton.Enabled = false;
            startExampleTwoButton.Enabled = false;
            startExampleThreeButton.Enabled = false;
            stopExampleOneButton.Enabled = true;
            stopExampleTwoButton.Enabled = false;
            stopExampleThreeButton.Enabled = false;
            loadMapButton.Enabled = false;

            tExampleOne.Start();
            
            Monitor.Enter( tExampleOne );
            bStopThreadOne = false;
            Monitor.Exit( tExampleOne );

            tsTextBox.GetRichTextBox.Focus();

            /// clear the board
            /// 
            geneticsBoard1.ResetCurrentAttempt();
            geneticsBoard1.ResetCurrentSolution();
            geneticsBoard1.ResetPreviousSolution();

        }

        private void OnStartExampleTwo( object sender, EventArgs e )
        {
        	bFinishIsSet = false;
            tExampleTwo = new Thread ( tsExampleTwo );
            
            Monitor.Enter( tExampleTwo );
            bInitialised = false;
            Monitor.Exit( tExampleTwo );

            startExampleOneButton.Enabled = false;
            startExampleTwoButton.Enabled = false;
            startExampleThreeButton.Enabled = false;
            stopExampleOneButton.Enabled = false;
            stopExampleTwoButton.Enabled = true;
            stopExampleThreeButton.Enabled = false;
            loadMapButton.Enabled = false;

            tExampleTwo.Start();
            
            Monitor.Enter( tExampleTwo );
            bStopThreadTwo = false;
            Monitor.Exit( tExampleTwo );

            tsTextBox.GetRichTextBox.Focus();

            /// clear the board
            /// 
            geneticsBoard1.ResetCurrentAttempt();
            geneticsBoard1.ResetCurrentSolution();
            geneticsBoard1.ResetPreviousSolution();
        }

        private void OnStartExampleThree( object sender, EventArgs e )
        {
        	bFinishIsSet = false;
            tExampleThree = new Thread ( tsExampleThree );
            
            Monitor.Enter( tExampleThree );
            bInitialised = false;
            Monitor.Exit( tExampleThree );

            startExampleOneButton.Enabled = false;
            startExampleTwoButton.Enabled = false;
            startExampleThreeButton.Enabled = false;
            stopExampleOneButton.Enabled = false;
            stopExampleTwoButton.Enabled = false;
            stopExampleThreeButton.Enabled = true;
            loadMapButton.Enabled = false;

            tExampleThree.Start();
            
            Monitor.Enter( tExampleThree );
            bStopThreadThree = false;
            Monitor.Exit( tExampleThree );

            tsTextBox.GetRichTextBox.Focus();

            /// clear the board
            /// 
            geneticsBoard1.ResetCurrentAttempt();
            geneticsBoard1.ResetCurrentSolution();
            geneticsBoard1.ResetPreviousSolution();

            if( geneticsBoard1.InvokeRequired == true )
            {
                InvalidateCallback d = new InvalidateCallback( Invalidate );
                geneticsBoard1.Invoke( d, new object[]{} );

                UpdateCallback f = new UpdateCallback( Update );
                geneticsBoard1.Invoke( f, new object[]{} );
            }
            else
            {
        	   geneticsBoard1.Invalidate();
        	   geneticsBoard1.Update();
            } 
        }

        private void OnStopExampleTwoButton( object sender, EventArgs e )
        {
            startExampleOneButton.Enabled = true;
            startExampleTwoButton.Enabled = true;
            startExampleThreeButton.Enabled = true;
            stopExampleOneButton.Enabled = false;
            stopExampleTwoButton.Enabled = false;
            stopExampleThreeButton.Enabled = false;
            loadMapButton.Enabled = true;
            
            Monitor.Enter( tExampleTwo );
            bStopThreadTwo = true;
            Monitor.Exit( tExampleTwo );

            if( bExampleTwoRunning == true )
            	tsTextBox.AppendTextWithColour( "Example Two Thread Set To Stop", Color.Black );
            else
            	tExampleOne = null;        }

        private void OnStopExampleOneButton( object sender, EventArgs e )
        {
            startExampleOneButton.Enabled = true;
            startExampleTwoButton.Enabled = true;
            startExampleThreeButton.Enabled = true;
            stopExampleOneButton.Enabled = false;
            stopExampleTwoButton.Enabled = false;
            stopExampleThreeButton.Enabled = false;
            loadMapButton.Enabled = true;

            Monitor.Enter( tExampleOne );
            bStopThreadOne = true;
            Monitor.Exit( tExampleOne );

            if( bExampleOneRunning == true )
            	tsTextBox.AppendTextWithColour( "Example One Thread Set To Stop", Color.Black );
            else
            	tExampleOne = null;
        }

        private void OnStopExampleThreeButton( object sender, EventArgs e )
        {
            startExampleOneButton.Enabled = true;
            startExampleTwoButton.Enabled = true;
            startExampleThreeButton.Enabled = true;
            stopExampleOneButton.Enabled = false;
            stopExampleTwoButton.Enabled = false;
            stopExampleThreeButton.Enabled = false;
            loadMapButton.Enabled = true;

            Monitor.Enter( tExampleThree );
            bStopThreadThree = true;
            Monitor.Exit( tExampleThree );

            if( bExampleThreeRunning == true )
            	tsTextBox.AppendTextWithColour( "Example Three Thread Set To Stop", Color.Black );
            else
            	tExampleOne = null;       
        }

        private void OnRouletteWheelClick( object sender, EventArgs e )
        {
      /*      if( rouletteWheelCheckBox.Checked == true )
            {
                rouletteWheelCheckBox.Checked = false;
                fixedPointCheckBox.Checked = true;
            }
            else
            {
                rouletteWheelCheckBox.Checked = true;
                fixedPointCheckBox.Checked = false;
            }
       */ 
        }
		
		void OnFixedPointClick(object sender, System.EventArgs e)
		{
         /*   if( fixedPointCheckBox.Checked == true )
            {
                rouletteWheelCheckBox.Checked = true;
                fixedPointCheckBox.Checked = false;
            }
            else
            {
                rouletteWheelCheckBox.Checked = false;
                fixedPointCheckBox.Checked = true;
            }
          */ 
		}
		
		void OnSinglePointClick(object sender, System.EventArgs e)
		{
		/*	if( singlePointCheckBox.Checked == true )
			{
				singlePointCheckBox.Checked = false;
				randomPointCheckBox.Checked = true;
				dualPointCheckBox.Checked = false;
				positionBasedCheckBox.Checked = false;
			}
			else
			{
				singlePointCheckBox.Checked = true;
				randomPointCheckBox.Checked = false;
				dualPointCheckBox.Checked = false;
				positionBasedCheckBox.Checked = false;
			}
         */ 
		}
		
		void OnRandomPointClick(object sender, System.EventArgs e)
		{
		/*	if( randomPointCheckBox.Checked == true )
			{
				singlePointCheckBox.Checked = false;
				randomPointCheckBox.Checked = false;
				dualPointCheckBox.Checked = true;
				positionBasedCheckBox.Checked = false;
			}
			else
			{
				singlePointCheckBox.Checked = false;
				randomPointCheckBox.Checked = true;
				dualPointCheckBox.Checked = false;
				positionBasedCheckBox.Checked = false;
			}
         */ 
		}
		
		void OnDualPointClick(object sender, System.EventArgs e)
		{
		/*	if( dualPointCheckBox.Checked == true )
			{
				singlePointCheckBox.Checked = false;
				randomPointCheckBox.Checked = false;
				dualPointCheckBox.Checked = false;
				positionBasedCheckBox.Checked = true;
			}
			else
			{
				singlePointCheckBox.Checked = false;
				randomPointCheckBox.Checked = false;
				dualPointCheckBox.Checked = true;
				positionBasedCheckBox.Checked = false;
			}
		*/	
		}
		
		void OnPositionBasedClick(object sender, System.EventArgs e)
		{
		/*	if( positionBasedCheckBox.Checked == true )
			{
				singlePointCheckBox.Checked = true;
				randomPointCheckBox.Checked = false;
				dualPointCheckBox.Checked = false;
				positionBasedCheckBox.Checked = false;
			}
			else
			{
				singlePointCheckBox.Checked = false;
				randomPointCheckBox.Checked = false;
				dualPointCheckBox.Checked = false;
				positionBasedCheckBox.Checked = true;
			}
		*/	
		}

        private void OnFormClosing( object sender, FormClosingEventArgs e )
        {
        	/// don't allow closing until threads have exited
        	/// 
        	if( bAllowClose == false )
        	{
        		bStopThreadOne = true;
        		bStopThreadTwo = true;
        		bStopThreadThree = true;     		
        	}
        	
        	guiTimer.Stop();

        }

        private void OnSaveOutput( object sender, EventArgs e )
        {
        	saveOutputFileDlg = new SaveFileDialog();
        	saveOutputFileDlg.Filter = "Text Files (*.txt)|*.txt";
        	saveOutputFileDlg.InitialDirectory = Directory.GetCurrentDirectory();

        	if( saveOutputFileDlg.ShowDialog() == DialogResult.OK )
        	{ 
        		StreamWriter stream = File.CreateText( saveOutputFileDlg.FileName );
        		if( stream == null )
        		{
        			MessageBox.Show( "Error unable to open file" );
        			return;
        		}
        		
        		stream.Write( textBox.Text );
        		
        		stream.Close();
        	}
        }

        private void OnGuiTimer( object sender, EventArgs e )
        {
        	if( bInitialised == false )
        		return;
        	
            if( bIsFinishDrawn == true || bFinishIsSet == true )
        	{
        		/// occiasionally the code manages to finish without drawing the
        		/// final string so try to get it to draw it here.
        		///
        		
                if( geneticsBoard1.InvokeRequired == true )
                {
                    InvalidateCallback d = new InvalidateCallback( Invalidate );
                    geneticsBoard1.Invoke( d, new object[]{} );

                    UpdateCallback f = new UpdateCallback( Update );
                    geneticsBoard1.Invoke( f, new object[]{} );
                }
                else
                {
        		    geneticsBoard1.Invalidate();
        		    geneticsBoard1.Update();
                }

        		return;
        	}

            if( bAllowClose == true )
                return;
				
        
        	/// prevent the genetics array from being 
        	/// accessed for display while doing a run
        	/// 
         	bool bWait = geneticsArrayMutex.WaitOne( 1000, false );
        	
	        GeneticsPathString geneticsPathString = null;
            GeneticsPathString gpsTestString = null;
	        bool bFound = false;
	   
            if( bFinishIsSet == false )
            {
                /// So much for readable code
                /// 

                if( geneticsBoard1.InvokeRequired == true )
                {
                    ResetCurrentAttemptCallback d = new ResetCurrentAttemptCallback( geneticsBoard1.ResetCurrentAttempt );
                    ResetCurrentSolutionCallback f = new ResetCurrentSolutionCallback( geneticsBoard1.ResetCurrentSolution );
                    ResetPreviousSolutionCallback g = new ResetPreviousSolutionCallback( geneticsBoard1.ResetPreviousSolution );

                    if( geneticsBoard1.ShowCurrentAttemptPath == true )
                        geneticsBoard1.Invoke( d, new object[]{} );
                    if( geneticsBoard1.ShowCurrentSolutionPath == true )
                        geneticsBoard1.Invoke( f, new object[]{} );
                    if( geneticsBoard1.ShowPreviousSolutionPath == true )
                        geneticsBoard1.Invoke( g, new object[]{} );
                }
                else
                {
        	        if( geneticsBoard1.ShowCurrentAttemptPath == true )
        		        geneticsBoard1.ResetCurrentAttempt();
        	        if( geneticsBoard1.ShowCurrentSolutionPath == true )
        		        geneticsBoard1.ResetCurrentSolution();
                	if( geneticsBoard1.ShowPreviousSolutionPath == true )
             			geneticsBoard1.ResetPreviousSolution();
                }
            
	            if( bExampleOneRunning == true || bStopThreadOne == false )
	            {
                    
                    if( bDrawFirstString == true )
                    {
                	    if( gpExampleOne.GeneticsArray != null && gpExampleOne.GeneticsArray.Count > 0 )
                	    {
                		    geneticsPathString = ( GeneticsPathString )gpExampleOne.GeneticsArray[ 0 ];
                		    bFound = true;
                	    }
                	    else
                	    {
                		    if( bWait )
                                geneticsArrayMutex.ReleaseMutex();
                		    return;
                	    }
                    }
                    else
                    {
                	    int nMaxSize = 0;

                        for( int i=0; i<gpExampleOne.GeneticsArray.Count; i++ )
                        {
///                            geneticsArrayMutex.WaitOne();
                    	    gpsTestString = ( GeneticsPathString )gpExampleOne.GeneticsArray[ i ];
///                    	    geneticsArrayMutex.ReleaseMutex();

                       	    if( gpsTestString.LengthTravelled >= nMaxSize )
                       	    {
                                geneticsPathString = gpsTestString;
                       		    nMaxSize = geneticsPathString.LengthTravelled;
                       		    bFound = true;
                       	    }
                           	
                       	    if( gpsTestString.HasReachedFinish == true )
                       	    {
                       		    geneticsPathString = gpsTestString;
                       		    bFound = true;
                       		    tsTextBox.AppendTextWithColour( "Drawing Finished String ", Color.Crimson );
                       		    tsTextBox.AppendTextWithColour( "String = " + gpsTestString.FullDebugString(), Color.Crimson );
                       		    bIsFinishDrawn = true;
                       		    break;
                       	    }
                        }
                    }
	            }
                
                if( bExampleTwoRunning == true || bStopThreadTwo == false )
	            {
                    
                    if( bDrawFirstString == true )
                    {
                	    if( gpExampleTwo.GeneticsArray != null && gpExampleTwo.GeneticsArray.Count > 0 )
                	    {
                		    geneticsPathString = ( GeneticsPathString )gpExampleTwo.GeneticsArray[ 0 ];
                		    bFound = true;
                	    }
                	    else
                	    {
                		    if( bWait )
                                geneticsArrayMutex.ReleaseMutex();
                		    return;
                	    }
                    }
                    else
                    {
                	    int nMaxSize = 0;

                        for( int i=0; i<gpExampleTwo.GeneticsArray.Count; i++ )
                        {
                    	    gpsTestString = ( GeneticsPathString )gpExampleTwo.GeneticsArray[ i ];

                       	    if( gpsTestString.LengthTravelled >= nMaxSize )
                       	    {
                                geneticsPathString = gpsTestString;
                       		    nMaxSize = geneticsPathString.LengthTravelled;
                       		    bFound = true;
                       	    }
                           	
                       	    if( gpsTestString.HasReachedFinish == true )
                       	    {
                       		    geneticsPathString = gpsTestString;
                       		    bFound = true;
                       		    tsTextBox.AppendTextWithColour( "Drawing Finished String ", Color.Crimson );
                       		    tsTextBox.AppendTextWithColour( "String = " + gpsTestString.FullDebugString(), Color.Crimson );
                       		    bIsFinishDrawn = true;
                       		    break;
                       	    }
                        }
                    }
	            }

                if( bExampleThreeRunning == true || bStopThreadThree == false )
	            {
                    
                    if( bDrawFirstString == true )
                    {
                	    if( gpExampleThree.GeneticsArray != null && gpExampleThree.GeneticsArray.Count > 0 )
                	    {
                		    geneticsPathString = ( GeneticsPathString )gpExampleThree.GeneticsArray[ 0 ];
                		    bFound = true;
                	    }
                	    else
                	    {
                		    if( bWait )
                                geneticsArrayMutex.ReleaseMutex();
                		    return;
                	    }
                    }
                    else
                    {
                	    int nMaxSize = 0;

                        for( int i=0; i<gpExampleThree.GeneticsArray.Count; i++ )
                        {
                    	    gpsTestString = ( GeneticsPathString )gpExampleThree.GeneticsArray[ i ];

                       	    if( gpsTestString.LengthTravelled >= nMaxSize )
                       	    {
                                geneticsPathString = gpsTestString;
                       		    nMaxSize = geneticsPathString.LengthTravelled;
                       		    bFound = true;
                       	    }
                           	
                       	    if( gpsTestString.HasReachedFinish == true )
                       	    {
                       		    geneticsPathString = gpsTestString;
                       		    bFound = true;
                       		    tsTextBox.AppendTextWithColour( "Drawing Finished String ", Color.Crimson );
                       		    tsTextBox.AppendTextWithColour( "String = " + gpsTestString.FullDebugString(), Color.Crimson );
                       		    bIsFinishDrawn = true;
                       		    break;
                       	    }
                        }
                    }
	            }


            }
	        else 
	            geneticsPathString = null;
	        
	    
	        if( bWait )
                geneticsArrayMutex.ReleaseMutex();
	        
	        if( tExampleOne != null )
	        {
	        	if( bExampleOneRunning == false )
	       			OnStopExampleOneButton( this, null );
	        }
            if( tExampleTwo != null )
            {
                if( bExampleTwoRunning == false )
                    OnStopExampleTwoButton( this, null );
            }
            if( tExampleThree != null )
            {
                if( bExampleThreeRunning == false )
                    OnStopExampleThreeButton( this, null );
            }

	        if( geneticsPathString != null || bFound != false )
	        {
	        
	        	SetValidPath( geneticsPathString );
	        
	        	if( geneticsBoard1.InvokeRequired == true )
	            {
                    ResetCurrentAttemptCallback l = new ResetCurrentAttemptCallback( geneticsBoard1.ResetCurrentAttempt );
                    geneticsBoard1.Invoke( l, new object[]{} );

	                SetCurrentAttemptCallback k = new SetCurrentAttemptCallback( geneticsBoard1.SetCurrentAttempt );
	                geneticsBoard1.Invoke( k, new object[]{ geneticsPathString } );
	            }
	            else
	            {
	                geneticsBoard1.SetCurrentAttempt( geneticsPathString );
	            }
	
	      	    if( gpsMaxString != null )
	            {
	            	if( bFinishIsSet == false )
	            		gpsCurrentAttemptString = gpsMaxString;
	            } 
	            else
	                gpsCurrentAttemptString = geneticsPathString;
		     }
	        	
        	if( bPrintTimerMessage == true )
        	{
        		tsTextBox.AppendTextWithColour( "GUI Timer called", Color.Purple );
        	}
        	
        	if( geneticsBoard1.ShowCurrentSolutionPath == true )
        	{       		
        		if( geneticsBoard1.InvokeRequired == true )
	            {
	                ResetCurrentSolutionCallback g = new ResetCurrentSolutionCallback( geneticsBoard1.ResetCurrentSolution );
	                geneticsBoard1.Invoke( g, new object[]{} );
	                    
	                SetCurrentSolutionCallback h = new SetCurrentSolutionCallback( geneticsBoard1.SetCurrentSolution );
	                geneticsBoard1.Invoke( h, new object[]{ gpsCurrentSolutionString } );
	            }
	            else
	            {
	                geneticsBoard1.ResetCurrentSolution();
	        		geneticsBoard1.SetCurrentSolution( gpsCurrentSolutionString );
	            }
        	}

            if( gpsPreviousSolutionString != null )
            {
                if( geneticsBoard1.ShowPreviousSolutionPath == true )
                {
                    if( geneticsBoard1.InvokeRequired == true )
                    {
                        ResetPreviousSolutionCallback m = new ResetPreviousSolutionCallback( geneticsBoard1.ResetPreviousSolution );
                        geneticsBoard1.Invoke( m, new object[]{} );

                        SetPreviousSolutionCallback n = new SetPreviousSolutionCallback( geneticsBoard1.SetPreviousSolution );
                        geneticsBoard1.Invoke( n, new object[]{ gpsPreviousSolutionString } );
                    }
                    else
                    {
                        geneticsBoard1.ResetPreviousSolution();
                        geneticsBoard1.SetPreviousSolution( gpsPreviousSolutionString );
                    }
                }
            }

            if( geneticsBoard1.InvokeRequired == true )
            {
                InvalidateCallback d = new InvalidateCallback( Invalidate );
                geneticsBoard1.Invoke( d, new object[]{} );

                UpdateCallback f = new UpdateCallback( Update );
                geneticsBoard1.Invoke( f, new object[]{} );
            }
            else
            {
        	   geneticsBoard1.Invalidate();
        	   geneticsBoard1.Update();
            }        	
        }
        
        /// <summary>
        /// Update the path information for each item in the genetics path
        /// </summary>
        /// <param name="geneticsPath"></param>
        private void SetValidPaths( GeneticsPath geneticsPath )
        {
       		/// find the valid paths
            /// 
                
            string strStartSquare = geneticsBoard1.GetStartSquare();
            if( strStartSquare == null )
            {
             	tsTextBox.AppendTextWithColour( "Error everythings wrong the map has no start square", Color.Red );
               	return;
            }
                
     		GeneticsPathString strTemp = null;
            
            /// reset valid for the entire population
            /// 
            geneticsPath.ResetValid();
                
            for( int i=0; i<geneticsPath.PopulationSize; i++ )
            {
                	
    	       	strTemp = ( GeneticsPathString )geneticsPath.GeneticsArray[ i ];

               	strTemp.LengthTravelled = 0;
               	
               	SetValidPath( strTemp );                	
           }
        	
        }
        
        /// <summary>
        /// Update the path information for each item in the genetics string
        /// </summary>
        /// <param name="geneticsString"></param>
        private bool SetValidPath( GeneticsPathString geneticsString )
        {
        	currentAttemptMutex.WaitOne( 1000, true );
        	bool bNoMansLand = false;
        	
        	if( geneticsString == null )
        	{
        		tsTextBox.AppendTextWithColour( "The Genetics String passed to set valid path is null", Color.Red );
        		currentAttemptMutex.ReleaseMutex();
        		return false;
        	}
       		/// find the valid paths
            /// 
                
            string strStartSquare = geneticsBoard1.GetStartSquare();
            if( strStartSquare == null )
            {
             	tsTextBox.AppendTextWithColour( "Error everythings wrong the map has no start square", Color.Red );
          ///   	currentAttemptMutex.ReleaseMutex();
             	return false;
            }
                
            string strCurrentSquare = strStartSquare;
            SquareInformation squareInfo = null;
            GeneticsPathItem pathItem = null;               
                	
           	strCurrentSquare = strStartSquare;
 
           	geneticsString.LengthTravelled = 0;
                	
			for( int j=0;j<geneticsString.GeneticsString.Count; j++ )
			{
				squareInfo = geneticsBoard1.GetSquareInformation( strCurrentSquare );
				pathItem = ( GeneticsPathItem )geneticsString.GeneticsString[ j ];
				( ( GeneticsSquare )geneticsBoard1.GetSquare( strCurrentSquare ) ).ClearCurrentAttempt();
				
				if( bNoMansLand == true )
				{
					pathItem.Valid = false;
					continue;
				}
				
				switch( pathItem.Direction )
				{
					case "Up": 
						{
							if( squareInfo.Up == true )
							{								
								pathItem.Valid = true;
																	
								geneticsString.LengthTravelled++;
								
								/// it is possible that the strings will go outside the board area
								/// 
								try
								{
									strCurrentSquare = ( ( GeneticsSquare )geneticsBoard1.GetSquareAbove( strCurrentSquare ) ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									/// carry on
									/// 
									bNoMansLand = true;
									continue;
								}
										
										
								if( PrintCurrentAttemptFinds == true )
								{
									tsTextBox.AppendTextWithColour( "Up valid for Genetics String " + geneticsString.ToString(), Color.BlueViolet );
								}

				                             if( bDebugSetValid == true )
				                             {
				                                    Debug.WriteLine( "Up valid for square " + squareInfo.Square );
				                             }
				
				                             if( squareInfo.Finish == true )
				                             {
				                                    currentAttemptMutex.ReleaseMutex();
				                                    return true;
				                              }

							}
							else
							{
								pathItem.Valid = false;
								j = geneticsString.GeneticsString.Count;
								continue;
							}
						}break;
					case "Right":
						{
							if( squareInfo.Right == true )
							{
								pathItem.Valid = true;
										
								geneticsString.LengthTravelled++;
								try
								{
									strCurrentSquare = ( ( GeneticsSquare )geneticsBoard1.GetSquareToRight( strCurrentSquare ) ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									/// carry on
									/// 
									bNoMansLand = true;
									continue;
								}
									
								if( PrintCurrentAttemptFinds == true )
								{
									tsTextBox.AppendTextWithColour( "Right valid for Genetics String " + geneticsString.ToString(), Color.BlueViolet );
								}

				                             if( bDebugSetValid == true )
				                             {
				                                    Debug.WriteLine( "Right valid for square " + squareInfo.Square );
				                             }
				
				                             if( squareInfo.Finish == true )
				                             {
				                                    currentAttemptMutex.ReleaseMutex();
				                                    return true;
				                             }
							}
							else
							{
								pathItem.Valid = false;
								j = geneticsString.GeneticsString.Count;
								continue;
							}
						}break;
					case "Down":
						{
							if( squareInfo.Down == true )
							{
								pathItem.Valid = true;
										
								geneticsString.LengthTravelled++;
								try
								{
									strCurrentSquare = ( ( GeneticsSquare )geneticsBoard1.GetSquareBelow( strCurrentSquare ) ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									/// carry on
									/// 
									bNoMansLand = true;
									continue;
								}
									
								if( PrintCurrentAttemptFinds == true )
								{
									tsTextBox.AppendTextWithColour( "Down valid for Genetics String " + geneticsString.ToString(), Color.BlueViolet );
								}

				                             if( bDebugSetValid == true )
				                             {
				                                    Debug.WriteLine( "Down valid for square " + squareInfo.Square );
				                             }
				
				                             if( squareInfo.Finish == true )
				                             {
				                                    currentAttemptMutex.ReleaseMutex();
				                                    return true;
				                             }
							}
							else
							{
								pathItem.Valid = false;
								j = geneticsString.GeneticsString.Count;
								continue;
							}
						}break;
					case "Left":
						{
							if( squareInfo.Left == true )
							{
								pathItem.Valid = true;

										
								geneticsString.LengthTravelled++;
								try
								{
									strCurrentSquare = ( ( GeneticsSquare )geneticsBoard1.GetSquareToLeft( strCurrentSquare ) ).Identifier;
								}
								catch( NullReferenceException nullExp )
								{
									string strWaste = nullExp.Message;
									/// carry on
									/// 
									bNoMansLand = true;
									continue;
								}
										
								if( PrintCurrentAttemptFinds == true )
								{
									tsTextBox.AppendTextWithColour( "Left valid for Genetics String " + geneticsString.ToString(), Color.BlueViolet );
								}

                                if( bDebugSetValid == true )
                                {
                                    Debug.WriteLine( "Left valid for square " + squareInfo.Square );
                                }

                                if( squareInfo.Finish == true )
                                {
                                    currentAttemptMutex.ReleaseMutex();
                                    return true;
                                }
							}
							else
							{
								pathItem.Valid = false;
								j = geneticsString.GeneticsString.Count;
								continue;
							}
						}break;	
					}						
			}	
			
			currentAttemptMutex.ReleaseMutex();

            return false;
        }
        
        /// <summary>
        /// The fitness criteria for example one is if the search has reached the end
        /// At this point there is no penalty for a string that goes back over previously covered ground
        /// so a string that reaches the finish is all we are looking for we don't care at this point if another 
        /// string can reach the finish using less moves.
        /// </summary>
        private void ExampleOneFitness( GeneticsPath geneticsPath, Thread geneticsThread, bool stopThread )
        {
        	if( geneticsPath == null )
        		return;
        	
    	///	int nFurthestThrough = 0;
		///	int nCurrentNumber = 0;
			
			ArrayList holdingList = new ArrayList();
			GeneticsPathString geneticsString = null;
			GeneticsPathItem pathItem = null;
			SquareInformation squareInfo = null;
			string strCurrentSquare = null;
			
			/// see if any string has found the finish
			/// 
			bool bNoMansLand = false;
			
			for( int i=0; i<geneticsPath.PopulationSize; i++ )
			{
				strCurrentSquare = geneticsBoard1.GetStartSquare();
				holdingList.Clear();
				
				geneticsString = ( GeneticsPathString )geneticsPath.GeneticsArray[ i ];
				
				for( int j=0; j<geneticsString.GeneticsString.Count; j++ )
				{
					holdingList.Add( strCurrentSquare );
					squareInfo = geneticsBoard1.GetSquareInformation( strCurrentSquare );
					pathItem =  ( GeneticsPathItem )geneticsString.GeneticsString[ j ];
					
					if( bNoMansLand == true )
						break;
					
					/// Check for a blocked square for example three this wont impact earlier examples
					/// 
					GeneticsSquare square = ( GeneticsSquare )geneticsBoard1.GetSquare( strCurrentSquare );
					if( square != null && square.Blocked == true )
						break;
					
					if( pathItem.Valid == true )
					{
						switch( pathItem.Direction )
						{
							case "Up":
								{
									if( squareInfo.Up == true )
									{
										try
										{
											strCurrentSquare = ( ( GeneticsSquare )geneticsBoard1.GetSquareAbove( strCurrentSquare ) ).Identifier;
										}
										catch( NullReferenceException nullExp )
										{
											string strWaste = nullExp.Message;
											/// carry on
											/// 
											bNoMansLand = true;
											continue;
										}
									}
								}break;
							case "Right":
								{
									if( squareInfo.Right == true )
									{
										try
										{
											strCurrentSquare = ( ( GeneticsSquare )geneticsBoard1.GetSquareToRight( strCurrentSquare ) ).Identifier;
										}
										catch( NullReferenceException nullExp )
										{
											string strWaste = nullExp.Message;
											bNoMansLand = true;
											continue;
										}
									}
								}break;
							case "Down":
								{
									if( squareInfo.Down == true )
									{
										try
										{
											strCurrentSquare = ( ( GeneticsSquare )geneticsBoard1.GetSquareBelow( strCurrentSquare ) ).Identifier;
										}
										catch( NullReferenceException nullExp )
										{
											string strWaste = nullExp.Message;
											bNoMansLand = true;
											continue;
										}
									}
								}break;
							case "Left":
								{
									if( squareInfo.Left == true )
									{
										try
										{
											strCurrentSquare = ( ( GeneticsSquare )geneticsBoard1.GetSquareToLeft( strCurrentSquare ) ).Identifier;
										}
										catch( NullReferenceException nullExp )
										{
											string strWaste = nullExp.Message;
											bNoMansLand = true;
											continue;
										}
									}
								}break;
								
						}
						
						/// This is where the finish is set
						/// 
						if( squareInfo.Finish == true )
						{
			                /// check that is a valid path to the finish
			                /// 
			                if( this.SetValidPath( geneticsString ) != true )
			                    return;
			
							/// print out the path taken
							/// 
							tsTextBox.AppendTextWithColour( "Finished String Path", Color.Crimson );
							for( int u=0; u<holdingList.Count; u++ )
							{
								tsTextBox.AppendTextWithColour( holdingList[ u ].ToString(), Color.Crimson );
							}
										
							gpExampleOne.IsFinishFound = true;
			                gpsCurrentAttemptString = geneticsString;
							geneticsString.HasReachedFinish = true;
			
			                geneticsBoard1.ResetCurrentAttempt();
			                bFinishIsSet = true;
			
			                /// for Example One this is the finish
							///
									
						     Monitor.Enter( geneticsThread );
						     stopThread = true;
						     Monitor.Exit( geneticsThread );
			
			                 return;
						    
						}
					}
					else
					{
						break;
	   				}
				}
			}
			
            /*
			if( gpExampleOne.IsFinishFound == true )
			{	
				/// The finish has been found so find the string which has reached the finish and draw it
				/// 
					
				for( int i=0; i<gpExampleOne.PopulationSize; i++ )
				{
					geneticsString = ( GeneticsPathString )gpExampleOne.GeneticsArray[ i ];
					
					if( geneticsString.HasReachedFinish == true )
					{
						Debug.WriteLine( "Drawing finished string" );
						/// shouldn't be needed here but good habit to get into
						/// 
						geneticsBoard1.ResetCurrentSolution();
						gpsCurrentSolutionString = geneticsString;
						bFinishIsSet = true;
						

					}
				}
			}
             */
        
        }

        private void OnLoadMap( object sender, EventArgs e )
        {
            geneticsBoard1.Clear();
            strMap = strMapStart + ( string )mapListBox.SelectedItem;
            LoadMap();
            geneticsBoard1.InitialiseBoard();
            geneticsBoard1.DrawCurrentAttempt( bDrawCurrentAttempt );
            geneticsBoard1.DrawCurrentSolution( bDrawCurrentSolution );
            geneticsBoard1.DrawPreviousSolution( bDrawPreviousSolution );
			geneticsBoard1.ShowCurrentAttemptPath = bDrawCurrentAttemptPath;
            geneticsBoard1.ShowCurrentSolutionPath = bDrawCurrentSolutionPath;
            geneticsBoard1.ShowPreviousSolutionPath = bDrawPreviousSolutionPath;
            geneticsBoard1.DrawBoard();
        }

        private void GetMaps()
        {
            Directory.SetCurrentDirectory( Directory.GetCurrentDirectory() );
            string[] files = Directory.GetFiles( Directory.GetCurrentDirectory(), "*.xml" );

            string strTest = null;
            foreach( string strFile in files )
            {
                strTest = strFile.Remove( 0, Directory.GetCurrentDirectory().Length + 1 );
                mapListBox.Items.Add( strTest );
            }
        }

        private bool RunInitialSearch( GeneticsPath examplePath, Thread exampleThread, ref bool stopThread )
        {
            bool bFinished = false;
            bool bStop = false;
            int nRunCount = 0;
            int nGenerationCount = 0;

            gpsPreviousSolutionString = null;
            
            while( bStop == false )
            {
                if( useDelayCheckBox.Checked == true )
                {
                	System.Threading.Thread.Sleep( ( int )( displayDelayUpDown.Value * 100 ) );
                }

                Monitor.Enter( exampleThread );
                if( stopThread == true )
                {
                    Monitor.Exit( exampleThread );
                    bStop = true;
                    geneticsBoard1.Invalidate();
                    continue;
                    
                }
                Monitor.Exit( exampleThread );
                
                examplePath.IsFinishFound = false;

                if( bPrintRunMessages == true )
                {
                	tsTextBox.AppendTextWithColour( "\n Run Number " + nRunCount.ToString(), Color.BlueViolet );
                }

                /// At this point we are checking to see if any of the strings in the population 
                /// have reached our end point criteria which in this case is the end of the map
                /// 
                
                SetValidPaths( examplePath );
                if( bDebugProgress == true )
                	Debug.WriteLine( "Set Valid Paths finished for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );

                if( bStop == false )
                {	
                	/// do fitness and run
                	/// 
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Entering Fitness function for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );
                	
                    /// Note we are effectively redoing the example one portion of the code here so the example one fitness counts here
                    /// 
                	ExampleOneFitness( examplePath, exampleThread, stopThread );

                  	if( bFinishIsSet == true )
                	{
                    	tsTextBox.AppendTextWithColour( "At generation number " + nGenerationCount.ToString() + " run number " + nRunCount.ToString() + " the algorythm found the finish.", Color.Red );
                    	/// Note previous Solution is set as this function is only called to 
                        /// get a path for one of the examples that continues after it has a working solution.
                        /// 
                        geneticsBoard1.SetPreviousSolution( gpsCurrentAttemptString );
                        gpsPreviousSolutionString = gpsCurrentAttemptString;
                   	    OnGuiTimer( this, new EventArgs() );
                    	bStop = true;
                    	bFinished = true;
                    	break;
                	}
                	
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Fitness Function finished, entering run for generation " + nGenerationCount.ToString() + " run " + nRunCount.ToString() );
                	
                	examplePath.Run();
                	
                	if( bDebugProgress == true )
                		Debug.WriteLine( "Run finished for run " + nRunCount.ToString() );
                	
                	nRunCount++;
                	
                	if( nGenerationCount == examplePath.NumberOfGenerations )
                	{
                		if( bPrintGenerationMessages == true )
                		{
                			tsTextBox.AppendTextWithColour( "Number Of Generations Reached, Halting Algorythm", Color.BlueViolet );
                		}
                		
                		bStop = true;
                        break;
                	}

                	/// draw the best previous attempt
                	/// 
                	int nMaxSize = 0;
                	GeneticsPathString gpsTest = null;
                   	
                	for( int i=0; i<examplePath.PopulationSize; i++ )
                	{
                       	gpsTest = ( GeneticsPathString )examplePath.GeneticsArray[ i ];

                       	if( gpsTest.LengthTravelled > nMaxSize )
                       	{
                           	nMaxSize = gpsTest.LengthTravelled;
                           	gpsMaxString = gpsTest;
                       	}
                	}
                    
                	if( nRunCount == examplePath.NumberOfCyclesPerGeneration )
                	{
               /// 		if( gpsPreviousSolutionString == null )
                ///			gpsPreviousSolutionString = gpsMaxString;
                		
                		if( gpsCurrentAttemptString.HasReachedFinish == true && gpsCurrentAttemptString.LengthTravelled >= gpsMaxString.LengthTravelled )
                		{
                			geneticsBoard1.ResetPreviousSolution();
                       		geneticsBoard1.SetPreviousSolution( gpsMaxString );
                       		gpsPreviousSolutionString = gpsMaxString;
                		}
                		
                		if( bPrintGenerationMessages == true )
                		{
                			tsTextBox.AppendTextWithColour( "Number Of Cycles Run For Generation " + nGenerationCount.ToString() + ", Reinitialising Genetics Array", Color.BlueViolet );
                		}
                		
                		examplePath.Initialise();	
                    	examplePath.SingleCrossOverPoint = examplePath.StartingLength /2;
                    	if( examplePath.SingleCrossOverPoint == 0 )
                    		Debug.WriteLine( "The Single Point CrossOver has been screwed up in run after the initialisation" );
                		nGenerationCount++;
                		nRunCount = 0;		
                	}
                	
                	if( bDrawBestGenerationString == true )
                   	 {
                      	 nMaxSize = 0;
                      	 gpsTest = null;

                       	for( int i=0; i<examplePath.PopulationSize; i++ )
                       	{
                          		gpsTest = ( GeneticsPathString )examplePath.GeneticsArray[ i ];

                          		if( gpsTest.LengthTravelled >= nMaxSize )
                          		{
                            		 nMaxSize = gpsTest.LengthTravelled;
                           			 gpsMaxString = gpsTest;
                          		}
                       	}

    				    if( bFinishIsSet == false )
    				      gpsCurrentAttemptString = gpsMaxString;
                       
                       	if( bPrintFullBestString == true )
                       	{
                       	  	tsTextBox.AppendTextWithColour( "Full Best Debug String :- " + gpsMaxString.FullDebugString(), Color.Brown );
                       	}

                    	}
                	
                	if( bPrintSampleDebugString == true )
                	{
						tsTextBox.AppendTextWithColour( "Sample Debug String :- " + ( ( GeneticsPathString )examplePath.GeneticsArray[ 0 ] ).FullDebugString(), Color.Brown );
                	}
                }

            }


            return bFinished;
        }
        
       	private void LoadPreviousSolutionString( string file )
        {
    		if( file == null )
			{
				MessageBox.Show( "Error no map name is set" );
				return;
			}

			XmlTextReader xmlReader = null;

			try
			{
				StreamReader stream = new StreamReader( file );
				xmlReader = new XmlTextReader( stream );
			}
			catch( ArgumentException argExp )
			{
				MessageBox.Show( "Error Argument Exception thrown, message "  + argExp.Message );
				return;
			}
			catch( FileNotFoundException fnfExp )
			{
				MessageBox.Show( "Error File Not Found Exception thrown, message " + fnfExp.Message );
				return;
			}
			catch( DirectoryNotFoundException dnfExp )
			{
				MessageBox.Show( "Error Directory Not Found Exception thrown, message " + dnfExp.Message );
				return;
			}
			
			int nCount = 0;
			
			while( xmlReader.Name != "Length" )
			{
				xmlReader.Read();
			}
			
			xmlReader.Read();
			nCount = Int32.Parse( xmlReader.Value );
			
			gpsPreviousSolutionString = new GeneticsPathString( false );
            gpsPreviousSolutionString.LengthTravelled = nCount;

			string strDirection;
			string strIdentifier;

			for( int i=0; i<nCount; i++ )
			{
				while( xmlReader.Name != "Direction" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				strDirection = xmlReader.Value;

				while( xmlReader.Name != "Identifier" )
				{
					xmlReader.Read();
				}

				xmlReader.Read();
				strIdentifier = xmlReader.Value;

				gpsPreviousSolutionString.GeneticsString.Add( new GeneticsPathItem( strDirection, true, strIdentifier ) );
			}

			xmlReader.Close();
		}
        
        private void SavePreviousSolutionString( string file )
        {
        	if( file == null )
			{
				MessageBox.Show( "Error no map name is set" );
				return;
			}

			XmlTextWriter xmlWriter = null;

			try
			{
				StreamWriter stream = new StreamWriter( file );
				xmlWriter = new XmlTextWriter( stream );
			}
			catch( ArgumentException argExp )
			{
				MessageBox.Show( "Error Argument Exception thrown, message "  + argExp.Message );
				return;
			}
			catch( FileNotFoundException fnfExp )
			{
				MessageBox.Show( "Error File Not Found Exception thrown, message " + fnfExp.Message );
				return;
			}
			catch( DirectoryNotFoundException dnfExp )
			{
				MessageBox.Show( "Error Directory Not Found Exception thrown, message " + dnfExp.Message );
				return;
			}
			
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement( "GeneticsPathItems" );
			xmlWriter.WriteElementString( "Length", gpsPreviousSolutionString.LengthTravelled.ToString() );
			xmlWriter.WriteStartElement( "Items" );
			
			for( int i=0; i<gpsPreviousSolutionString.LengthTravelled; i++ )
			{
				GeneticsPathItem item = ( GeneticsPathItem )gpsPreviousSolutionString.GeneticsString[ i ];
				
				xmlWriter.WriteStartElement( "GeneticsPathItem" );
				xmlWriter.WriteElementString( "Direction", item.Direction );
				xmlWriter.WriteElementString( "Identifier", item.Identifier );
				xmlWriter.WriteEndElement();
				
			}
			
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			xmlWriter.Close();
    	
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
	}
}
