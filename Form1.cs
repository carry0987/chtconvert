using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace toTraditionalC
{
    public partial class Form1 : Form
    {
        private enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Form1()
        {
            InitializeComponent();

            comboBox1.Sorted = true;
            comboBox1.DataSource = Enum.GetValues(typeof(KeyModifier));
            comboBox1.SelectedItem = KeyModifier.None;
            comboBox1.SelectedValueChanged += ChangeHotKey;
            
            comboBox2.Sorted = true;
            comboBox2.DataSource = Enum.GetValues(typeof (Keys));
            comboBox2.SelectedItem = Keys.F9;
            comboBox2.SelectedValueChanged += ChangeHotKey;

            RegisterHotKey(Handle, 0, (int)comboBox1.SelectedItem, ((Keys)comboBox2.SelectedItem).GetHashCode()); 
        }

        private void ChangeHotKey(object sender, EventArgs e)
        {
            UnregisterHotKey(Handle, 0);
            RegisterHotKey(Handle, 0, (int)comboBox1.SelectedItem, ((Keys)comboBox2.SelectedItem).GetHashCode()); 
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                for(int i = 0; i < 10; i++)
                    try
                    {
                        TranslateSelectText();
                        break;
                    }
                    catch (Exception)
                    {
                    }
            }
        }

        private void TranslateSelectText()
        {
            SendKeys.SendWait("^c");
            Thread.Sleep(100);
            Clipboard.SetText(Translator.Translate(Clipboard.GetText()));
            SendKeys.SendWait("^v");
        }

        
    }
}