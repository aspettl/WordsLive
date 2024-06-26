﻿/*
 * WordsLive - worship projection software
 * Copyright (c) 2013 Patrick Reisert
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

using System.Windows;
using System.Windows.Controls;

namespace WordsLive.Utils.ImageLoader
{
	/// <summary>
	/// Original code from http://dotnetlearning.wordpress.com/2011/01/27/loading-images-asynchronously-in-wpf/
	/// </summary>
	public static class Loader
	{
		[AttachedPropertyBrowsableForType(typeof(Image))]
		public static SourceType GetSourceType(Image obj)
		{
			return (SourceType)obj.GetValue(SourceTypeProperty);
		}

		public static void SetSourceType(Image obj, SourceType value)
		{
			obj.SetValue(SourceTypeProperty, value);
		}
		public static readonly DependencyProperty SourceTypeProperty =
			DependencyProperty.RegisterAttached("SourceType", typeof(SourceType), typeof(Loader), new UIPropertyMetadata(SourceType.LocalDisk));

		
		[AttachedPropertyBrowsableForType(typeof(Image))]
		public static object GetSource(Image obj)
		{
			return obj.GetValue(SourceProperty);
		}

		public static void SetSource(Image obj, object value)
		{
			obj.SetValue(SourceProperty, value);
		}
		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.RegisterAttached("Source", typeof(object), typeof(Loader), new UIPropertyMetadata(string.Empty, OnSourceChanged));

		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Manager.Instance.LoadImage(e.NewValue, d as Image);
			(d as Image).AddHandler(Image.UnloadedEvent, new RoutedEventHandler(Manager.Instance.OnImageUnloaded));
		}


		[AttachedPropertyBrowsableForType(typeof(Image))]
		public static bool GetDisplayWaitingAnimationDuringLoading(Image obj)
		{
			return (bool)obj.GetValue(DisplayWaitingAnimationDuringLoadingProperty);
		}

		public static void SetDisplayWaitingAnimationDuringLoading(Image obj, bool value)
		{
			obj.SetValue(DisplayWaitingAnimationDuringLoadingProperty, value);
		}
		public static readonly DependencyProperty DisplayWaitingAnimationDuringLoadingProperty =
			DependencyProperty.RegisterAttached("DisplayWaitingAnimationDuringLoading", typeof(bool), typeof(Loader), new UIPropertyMetadata(true));


		[AttachedPropertyBrowsableForType(typeof(Image))]
		public static bool GetDisplayErrorThumbnailOnError(Image obj)
		{
			return (bool)obj.GetValue(DisplayErrorThumbnailOnErrorProperty);
		}

		public static void SetDisplayErrorThumbnailOnError(Image obj, bool value)
		{
			obj.SetValue(DisplayErrorThumbnailOnErrorProperty, value);
		}
		public static readonly DependencyProperty DisplayErrorThumbnailOnErrorProperty =
			DependencyProperty.RegisterAttached("DisplayErrorThumbnailOnError", typeof(bool), typeof(Loader), new UIPropertyMetadata(true));




		[AttachedPropertyBrowsableForType(typeof(Image))]
		public static DisplayOptions GetDisplayOption(Image obj)
		{
			return (DisplayOptions)obj.GetValue(DisplayOptionProperty);
		}

		public static void SetDisplayOption(Image obj, DisplayOptions value)
		{
			obj.SetValue(DisplayOptionProperty, value);
		}
		public static readonly DependencyProperty DisplayOptionProperty =
			DependencyProperty.RegisterAttached("DisplayOption", typeof(DisplayOptions), typeof(Loader), new UIPropertyMetadata(DisplayOptions.Preview));


		[AttachedPropertyBrowsableForType(typeof(Image))]
		public static int GetMaxSize(Image obj)
		{
			return (int)obj.GetValue(MaxSizeProperty);
		}

		public static void SetMaxSize(Image obj, int value)
		{
			obj.SetValue(MaxSizeProperty, value);
		}
		public static readonly DependencyProperty MaxSizeProperty =
			DependencyProperty.RegisterAttached("MaxSize", typeof(int), typeof(Loader), new UIPropertyMetadata(0));



		[AttachedPropertyBrowsableForType(typeof(Image))]
		public static bool GetIsLoading(Image obj)
		{
			return (bool)obj.GetValue(IsLoadingProperty);
		}

		internal static void SetIsLoading(Image obj, bool value)
		{
			obj.SetValue(IsLoadingProperty, value);
		}
		public static readonly DependencyProperty IsLoadingProperty =
			DependencyProperty.RegisterAttached("IsLoading", typeof(bool), typeof(Loader), new UIPropertyMetadata(true));


		[AttachedPropertyBrowsableForType(typeof(Image))]
		public static bool GetErrorDetected(Image obj)
		{
			return (bool)obj.GetValue(ErrorDetectedProperty);
		}

		internal static void SetErrorDetected(Image obj, bool value)
		{
			obj.SetValue(ErrorDetectedProperty, value);
		}

		public static readonly DependencyProperty ErrorDetectedProperty =
			DependencyProperty.RegisterAttached("ErrorDetected", typeof(bool), typeof(Loader), new UIPropertyMetadata(false));
		
		public static readonly RoutedEvent LoadedEvent = EventManager.RegisterRoutedEvent("Loaded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Loader));
		public static void AddLoadedHandler(DependencyObject d, RoutedEventHandler handler)
		{
			UIElement uie = d as UIElement;
			if (uie != null)
			{
				uie.AddHandler(LoadedEvent, handler);
			}
		}
		public static void RemoveLoadedHandler(DependencyObject d, RoutedEventHandler handler)
		{
			UIElement uie = d as UIElement;
			if (uie != null)
			{
				uie.RemoveHandler(LoadedEvent, handler);
			}
		}
		
	}
}
