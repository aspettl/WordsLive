﻿/*
 * WordsLive - worship projection software
 * Copyright (c) 2014 Patrick Reisert
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

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WordsLive.Utils
{
	/// <summary>
	/// Collection of various extension methods.
	/// </summary>
	public static class ExtensionMethods
	{
		/// <summary>
		/// Validate all dependency objects in a window
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static bool IsValid(this DependencyObject node)
		{
			// Check if dependency object was passed
			if (node != null)
			{
				// Check if dependency object is valid.
				// NOTE: Validation.GetHasError works for controls that have validation rules attached 
				bool isValid = !Validation.GetHasError(node);
				if (!isValid)
				{
					// If the object is a child of a TabItem, select that item
					var tab = node.FindLogicalParent<TabItem>();
					if (tab != null)
					{
						var tabControl = tab.FindLogicalParent<TabControl>();
						tabControl.SelectedItem = tab;
					}

					// If the dependency object is invalid, and it can receive the focus,
					// set the focus
					if (node is IInputElement) Keyboard.Focus((IInputElement)node);
					return false;
				}
			}

			// If this dependency object is valid, check all child dependency objects
			foreach (object subnode in LogicalTreeHelper.GetChildren(node))
			{
				if (subnode is DependencyObject)
				{
					// If a child dependency object is invalid, return false immediately,
					// otherwise keep checking
					if (IsValid((DependencyObject)subnode) == false) return false;
				}
			}

			// All dependency objects are valid
			return true;
		}

		public static T FindLogicalParent<T>(this DependencyObject source) where T : DependencyObject
		{
			while (source != null && source.GetType() != typeof(T))
				source = LogicalTreeHelper.GetParent(source);

			return (T)source;
		}

		public static T FindVisualParent<T>(this DependencyObject source) where T : DependencyObject
		{
			while (source != null && source.GetType() != typeof(T))
				source = VisualTreeHelper.GetParent(source);

			return (T)source;
		}

		public static T FindVisualParent<T, TLimit>(this DependencyObject source) where T : DependencyObject
		{
			while (source != null && source.GetType() != typeof(T) && source.GetType() != typeof(TLimit))
				source = VisualTreeHelper.GetParent(source);

			if (source == null || source.GetType() != typeof(T))
				return null;
			else
				return (T)source;
		}

		public static T FindVisualChild<T>(this DependencyObject source, int index = 0) where T : DependencyObject
		{
			int count = 0;
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(source); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(source, i);

				if (child != null && child is T)
				{
					if (count == index)
						return (T)child;
					else
						count++;
				}
				else
				{
					T childOfChild = FindVisualChild<T>(child, index);
					if (childOfChild != null)
						return childOfChild;
				}
			}

			return null;
		}

		public static int GetIndexAtPosition(this ListBox listBox, Point relativePosition)
		{
			var item = (listBox.InputHitTest(relativePosition) as DependencyObject).FindVisualParent<ListBoxItem, ListBox>();
			if (item != null)
			{
				return listBox.ItemContainerGenerator.IndexFromContainer(item);
			}
			else
			{
				return -1;
			}
		}

		public static int GetIndexAtPosition(this ListView listView, Point relativePosition)
		{
			var item = (listView.InputHitTest(relativePosition) as DependencyObject).FindVisualParent<ListViewItem, ListView>();
			if (item != null)
			{
				return listView.ItemContainerGenerator.IndexFromContainer(item);
			}
			else
			{
				return -1;
			}
		}

		public static TreeViewItem GetItemAtPosition(this TreeView treeView, Point relativePosition)
		{
			return (treeView.InputHitTest(relativePosition) as DependencyObject).FindVisualParent<TreeViewItem, TreeView>();
		}

		public static bool ExceedsMinimumDragDistance(this Point point, Point other)
		{
			return (Math.Abs(point.X - other.X) > SystemParameters.MinimumHorizontalDragDistance || 
				Math.Abs(point.Y - other.Y) > SystemParameters.MinimumVerticalDragDistance);
		}

		public static bool IsInFirstHalf(this Point clickedPoint, FrameworkElement container, bool hasVerticalOrientation)
		{
			if (hasVerticalOrientation)
			{
				return clickedPoint.Y < container.ActualHeight / 2;
			}
			return clickedPoint.X < container.ActualWidth / 2;
		}
		
		public static void CreateRecursive(this DirectoryInfo dirInfo)
		{
			if (dirInfo.Parent != null && !dirInfo.Exists)
				CreateRecursive(dirInfo.Parent);

			if (!dirInfo.Exists)
			{
				dirInfo.Create();
				dirInfo.Refresh();
			}
		}
	}
}
