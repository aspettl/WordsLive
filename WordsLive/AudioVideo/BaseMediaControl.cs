﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WordsLive.AudioVideo
{
	public abstract class BaseMediaControl : UserControl
	{
		public abstract TimeSpan Duration { get; }
		public abstract int Volume { get; set; }
		public abstract int Position { get; set; }
		public abstract bool Loop { get; set; }
		public abstract bool Autoplay { get; set; }
		public abstract void Load(string path);
		public abstract void Play();
		public abstract void Pause();
		public abstract void Stop();
		public abstract void Destroy();

		public abstract event Action MediaLoaded;
		public abstract event Action PlaybackEnded;
	}
}