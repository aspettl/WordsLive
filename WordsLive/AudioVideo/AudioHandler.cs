﻿using System;
using System.Collections.Generic;
using System.Linq;
using WordsLive.Core;

namespace WordsLive.AudioVideo
{
	public class AudioHandler : MediaTypeHandler
	{
		private string[] vlcExtensions = new string[] { ".ogg", ".flac", ".oga", ".mka", ".opus" };
		private string[] extensions = new string[] { ".mp3", ".wav", ".aac", ".wma", ".m4a" };

		public override IEnumerable<string> Extensions
		{
			get
			{
				if (Properties.Settings.Default.UseVlc && VlcController.IsAvailable)
				{
					return extensions.Concat(vlcExtensions);
				}
				else
				{
					return extensions;
				}
			}
		}

		public override string Description
		{
			get { return "Audio-Dateien"; } // TODO: localize
		}

		public override int Test(Uri uri)
		{
			return CheckExtension(uri) ? 100 : -1;
		}

		public override Media Handle(Uri uri, Dictionary<string, string> options)
		{
			return new AudioMedia(uri, options);
		}
	}
}
