using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using CefSharp;
//using CefSharp.WinForms;

namespace TrinityCreator
{
    public partial class BrowserControl : UserControl
    {
        public BrowserControl(string url)
        {
            InitializeComponent();/*
            Chromium = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill
            };
            Chromium.BackColor = Color.Black;

            Controls.Add(Chromium);*/
        }

        //public ChromiumWebBrowser Chromium { get; set; }
    }
}
