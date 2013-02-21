﻿using System;
using System.Windows.Controls;

namespace WordsLive.AudioVideo
{
	public partial class WpfWrapper : BaseMediaControl
	{
		public WpfWrapper()
		{
			InitializeComponent();
		}

		public override TimeSpan Duration
		{
			get { return mediaElement.NaturalDuration.TimeSpan; }
		}

		public override int Position
		{
			get
			{
				return (int)mediaElement.Position.TotalMilliseconds;
			}
			set
			{
				mediaElement.Position = new TimeSpan(0, 0, 0, 0, value);
			}
		}

		public override int Volume
		{
			get
			{
				return (int)(mediaElement.Volume * 100);
			}
			set
			{
				mediaElement.Volume = value / 100.0;
			}
		}

		public override bool Loop { get; set; }
		public override bool Autoplay { get; set; }

		public override event Action MediaLoaded;

		protected void OnMediaLoaded()
		{
			if (MediaLoaded != null)
				MediaLoaded();
		}

		public override event Action PlaybackEnded;

		protected void OnPlaybackEnded()
		{
			if (PlaybackEnded != null)
				PlaybackEnded();
		}

		public override event Action SeekStart;

		protected void OnSeekStart()
		{
			if (SeekStart != null)
				SeekStart();
		}

		public override void Load(Uri uri)
		{
			mediaElement.MediaOpened += (sender, args) =>
			{
				if (!Autoplay)
					mediaElement.Pause();
				else
					rect.Visibility = System.Windows.Visibility.Hidden;

				OnMediaLoaded();
			};

			mediaElement.MediaEnded += (sender, args) =>
			{
				if (!Loop)
				{
					Stop();
					OnPlaybackEnded();
				}
				else
				{
					mediaElement.Stop();
					OnSeekStart();
					mediaElement.Play();
				}
			};

			mediaElement.LoadedBehavior = MediaState.Manual;
			mediaElement.Source = uri;
			mediaElement.Volume = 1;
			mediaElement.Play();
		}

		public override void Play()
		{
			mediaElement.Play();
			rect.Visibility = System.Windows.Visibility.Hidden;
		}

		public override void Pause()
		{
			mediaElement.Pause();
		}

		public override void Stop()
		{
			mediaElement.Stop();
			OnSeekStart();
			rect.Visibility = System.Windows.Visibility.Visible;
		}

		public override void Destroy()
		{
			Stop();
		}
	}
}
