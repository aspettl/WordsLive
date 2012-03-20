﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Words.Core.Songs;
using System.IO;
using System.ComponentModel;
using MonitoredUndo;

namespace Words.Editor
{
	public class EditorDocument : INotifyPropertyChanged
	{
		public EditorDocument(string file, Song song, EditorWindow parent)
		{
			if (String.IsNullOrEmpty(file))
			{
				this.File = null;
			}
			else
			{
				this.File = new FileInfo(file);
			}

			Song = song;

			Grid = new EditorGrid(song, parent);
			Grid.PreviewControl.ShowChords = parent.ShowChords;

			UndoService.Current[Grid.Node].Clear();

			UndoService.Current[Grid.Node].UndoStackChanged += (sender, args) =>
			{
				IsModified = true;
			};
		}

		public static void OnChangingTryMerge(ISupportsUndo instance, string propertyName, object oldValue, object newValue)
		{
			var root = instance.GetUndoRoot();
			var ch = DefaultChangeFactory.GetChange(instance, propertyName, oldValue, newValue);
			var x = ch.ChangeKey.GetType();
			if (UndoService.Current[root].UndoStack.Count() > 0 &&
				UndoService.Current[root].UndoStack.First().Changes.Count() > 0 &&
				UndoService.Current[root].UndoStack.First().Changes.First().Target == instance &&
				((ChangeKey<object, string>)UndoService.Current[root].UndoStack.First().Changes.First().ChangeKey).Item2 == propertyName)
			{
				UndoService.Current[root].UndoStack.First().Changes.First().MergeWith(ch);
			}
			else
			{
				UndoService.Current[root].AddChange(ch, propertyName);
			}
		}

		private string GetChangesetText(ChangeSet set)
		{
			if (set.Changes.Count() == 0)
			{
				return set.Description;
			}
			else
			{
				string name;
				object target = set.Changes.First().Target;
				if (target is SongNodeRoot)
					name = "Lied";
				else if (target is SongNodePart)
					name = "Liedteil";
				else if (target is SongNodeSlide)
					name = "Folie";
				else if (target is SongNodeSource)
					name = "Quelle";
				else if (target is SongNodeMetadata)
					name = (target as SongNodeMetadata).Title;
				else
					throw new InvalidOperationException();

				return set.Description + " in " + name;
			}
		}

		private bool isModified;

		public bool IsModified
		{
			get
			{
				return isModified;
			}
			private set
			{
				isModified = value;
				OnPropertyChanged("IsModified");
			}
		}

		public void Save()
		{
			this.Song.SavePowerpraise(this.File.FullName);
			IsModified = false;
		}

		public void SaveAs(string file)
		{
			File = new FileInfo(file);
			this.Song.SavePowerpraise(this.File.FullName);
			IsModified = false;
		}

		public void Undo()
		{
			UndoService.Current[this.Grid.Node].Undo();
		}

		public void Redo()
		{
			UndoService.Current[this.Grid.Node].Redo();
		}

		public bool CanUndo
		{
			get
			{
				return UndoService.Current[this.Grid.Node].CanUndo;
			}
		}

		public bool CanRedo
		{
			get
			{
				return UndoService.Current[this.Grid.Node].CanRedo;
			}
		}

		public void UpdateFormatting(SongFormatting formatting)
		{
			SongFormatting oldFormatting = this.Song.Formatting;

			Action redo = () =>
			{
				this.Song.Formatting = formatting;
				this.Grid.PreviewControl.UpdateStyle();
				this.Grid.PreviewControl.Update();
			};
			Action undo = () =>
			{
				this.Song.Formatting = oldFormatting;
				this.Grid.PreviewControl.UpdateStyle();
				this.Grid.PreviewControl.Update();
			};

			var ch = new MonitoredUndo.DelegateChange(this.Grid.Node, undo, redo, new MonitoredUndo.ChangeKey<object, string>(this.Grid.Node, "SongFormatting"));
			MonitoredUndo.UndoService.Current[this.Grid.Node].AddChange(ch, "SongFormatting");

			redo();		
		}

		public Song Song
		{
			get;
			private set;
		}

		public EditorGrid Grid
		{
			get;
			private set;
		}

		private FileInfo file;

		public FileInfo File
		{
			get
			{
				return file;
			}
			protected set
			{
				file = value;
				OnPropertyChanged("File");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
