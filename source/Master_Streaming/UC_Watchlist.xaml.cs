﻿using Class;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Master_Streaming
{
    /// <summary>
    /// Logique d'interaction pour UC_Watchlist.xaml
    /// </summary>
    public partial class UC_Watchlist : UserControl
    {
        public ProfilManager Manager => (App.Current as App).Pmanager;

        public UC_Watchlist()
        {
            InitializeComponent();
        }
    }
}