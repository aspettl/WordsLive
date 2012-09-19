﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WordsLive.Utils;

namespace WordsLive.Editor
{
    public partial class RenamePartWindow : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        SongNodeRoot song;
        SongNodePart part;

        public RenamePartWindow(SongNodeRoot song, SongNodePart part)
        {
            InitializeComponent();
            this.song = song;
            this.part = part;
            this.DataContext = this;
            if (this.part != null)
            {
                this.PartName = this.part.Title;
            }
        }

        private string partName;
        public string PartName
        {
            get
            {
                return partName;
            }
            set
            {
                partName = value;
                OnNotifyPropertyChanged("PartName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(partName))
            {
                this.newNameTextBox.Text = string.Empty;
            }

            if (!this.IsValid()) return;
            this.DialogResult = true;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox box = (ListBox)sender;
            this.newNameTextBox.Text = (string)box.SelectedItem;
            this.newNameTextBox.Focus();
            int i = this.newNameTextBox.Text.IndexOf('#');
            if (i >= 0)
            {
                this.newNameTextBox.Select(i, 1);
            }
            else
            {
                this.newNameTextBox.Select(this.newNameTextBox.Text.Length, 0);
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string name]
        {
            get
            {
                switch (name)
                { 
                    case "PartName":
                        if (string.IsNullOrEmpty(this.partName))
                            return WordsLive.Resources.Resource.rpMsgNameMustNotBeEmpty;

                        foreach (var part in this.song.Parts)
                        {
                            if (this.partName == part.Title && !(part == this.part && this.part != null))
                                return WordsLive.Resources.Resource.rpMsgNameAlreadyExists;
                        }
                        break;
                }

                return null;
            }
        }
    }
}