using System;
using System.Windows.Forms;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CsLib {
	public partial class LogWindow : Form {
		public static LogWindow _instance;

		public static LogWindow instance {
			get {
				if( _instance == null ) {
					_instance = new LogWindow();
					_instance.Show();
					( (Form) _instance ).Visible = false;
				}
				return _instance;
			}
		}

		LogWindow() {
			if( _instance != null ) throw new Exception( "LogWindow singleton" );

			InitializeComponent();
			_instance = this;
		}

		public static bool Visible {
			get {
				return ( (Form) instance ).Visible;
			}
			set {
				( (Form) instance ).Visible = value;
			}
		}

		public void AddLog( string txt ) {
			Invoke( new Action( () => {
				lock( richTextBox1 ) {
					richTextBox1.AppendText( txt );
					richTextBox1.AppendText( "\n" );
				}
			} ) );
		}

		private void LogWindow_FormClosing( object sender, FormClosingEventArgs e ) {
			if( e.CloseReason == CloseReason.UserClosing ) {
				e.Cancel = true;
				Visible = false;
			}
		}

		private void toolStripMenuItem1_Click( object sender, EventArgs e ) {
			richTextBox1.Clear();
		}
	}

	
}
