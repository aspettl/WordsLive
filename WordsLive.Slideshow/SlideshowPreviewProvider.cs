﻿using System;
using System.Windows;
using System.Windows.Controls;
using WordsLive.Presentation.Wpf;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace WordsLive.Slideshow
{
	public class SlideshowPreviewProvider : WpfPreviewProvider
	{
		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);
		
		private Image image = new Image();
		private DispatcherTimer timer = null;
		SlideshowPresentationBase presentation;

		public static BitmapSource ConvertBitmap(System.Drawing.Bitmap bmp)
		{
			BitmapSource result;
			IntPtr hBitmap = bmp.GetHbitmap();
			try
			{
				result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap,
					IntPtr.Zero, System.Windows.Int32Rect.Empty,
					BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));

				result.Freeze();
			}
			finally
			{
				DeleteObject(hBitmap);
				bmp.Dispose();
			}

			return result;
		}

		public SlideshowPreviewProvider(SlideshowPresentationBase presentation)
		{
			// assume that presentation has already been loaded

			this.presentation = presentation;

			if (presentation.CaptureWindow(100) != null)
			{
				timer = new DispatcherTimer();
				timer.Interval = new TimeSpan(0, 0, 0, 0, 500); // 2 seconds
				timer.Tick += (sender, args) =>
				{
					Update();
				};

				timer.Start();
			}

			presentation.SlideIndexChanged += presentation_SlideIndexChanged;
		}

		void presentation_SlideIndexChanged(object sender, EventArgs e)
		{
			Update();
		}

		protected override UIElement WpfControl
		{
			get
			{
				return image;
			}
		}

		public void Update()
		{
			if (timer != null && Controller.PresentationManager.CurrentPresentation == presentation && Controller.PresentationManager.Status == PresentationStatus.Show) // only use CaptureWindow() if timer is initialized
			{
				image.Dispatcher.Invoke(new Action(() => { image.Source = presentation.CaptureWindow((int)image.ActualWidth); }));
			}
			else
			{
				if (presentation.SlideIndex != -1)
				{
					image.Dispatcher.Invoke(new Action(() => { image.Source = presentation.Thumbnails[presentation.SlideIndex].Image; }));
				}
			}
		}

		public void Close()
		{
			if (timer != null)
			{
				timer.Stop();
				timer = null;
			}
			presentation.SlideIndexChanged -= presentation_SlideIndexChanged;
		}
	}
}
