﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using WordsLive.Core;
using WordsLive.Core.Data;

namespace WordsLive.Images
{
	public class ImagesFileHandler : MediaFileHandler
	{
		public override IEnumerable<string> Extensions
		{
			get { return ImagesMedia.ImageExtensions.Concat(ImagesMedia.SlideshowExtensions); }
		}

		public override string Description
		{
			get { return "Diashows & Bilder"; } // TODO: localize
		}

		public override Media TryHandle(string path, IMediaDataProvider provider)
		{
			var media = new ImagesMedia(path, provider);
			return media;
		}

		public override IEnumerable<Media> TryHandleMultiple(IEnumerable<string> paths, IMediaDataProvider provider)
		{
			// make sure it's not a slideshow
			if (paths.All(p => ImagesMedia.ImageExtensions.Contains(Path.GetExtension(p).ToLower())))
			{
				Controller.FocusMainWindow();
				// TODO: localize
				var res = MessageBox.Show("Sie haben mehrere Bilddateien ausgewählt. Wollen Sie diese als Diashow hinzufügen? Bei „Ja“ müssen Sie als nächstes auswählen, wohin die Diashow gespeichert werden soll. Bei „Nein“ werden die Bilder einzeln hinzugefügt.", "Slideshow erstellen?", MessageBoxButton.YesNoCancel);
				if (res == MessageBoxResult.Yes)
				{
					// TODO: support other providers (don't use SaveFileDialog then)
					// maybe move save filename selection to DataProvider implementations?
					if (provider != DataManager.LocalFiles)
						throw new InvalidOperationException("Unsupported DataProvider: Can only save to local files.");

					Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
					dlg.DefaultExt = ".show";
					dlg.Filter = "Diashow|*.show";

					if (dlg.ShowDialog() == true)
					{
						var media = new ImagesMedia(dlg.FileName, provider);
						media.CreateSlideshow(paths);
						return new Media[] { media };
					}
					else
					{
						return new Media[] { };
					}
				}
				else if (res == MessageBoxResult.Cancel)
				{
					return new Media[] { };
				}
				else if (res == MessageBoxResult.No)
				{
					return null; // add them one by one
				}
			}

			return null;
		}
	}
}
