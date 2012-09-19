﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WordsLive.Utils;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;

namespace WordsLive.Utils.ActivatableListBox
{
	public static class Activator
	{
		[AttachedPropertyBrowsableForType(typeof(ListBox))]
		public static bool GetIsActivatable(ListBox obj)
		{
			return (bool)obj.GetValue(IsActivatableProperty);
		}

		public static void SetIsActivatable(ListBox obj, bool value)
		{
			obj.SetValue(IsActivatableProperty, value);
		}

		public static readonly DependencyProperty IsActivatableProperty =
			DependencyProperty.RegisterAttached("IsActivatable", typeof(bool), typeof(Activator), new UIPropertyMetadata(false, OnIsActivatableChanged));

		private static void OnIsActivatableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var value = (bool)e.NewValue;
			var sender = d as ListBox;

			if (value)
			{
				sender.MouseDoubleClick += ListBox_MouseDoubleClick;
				sender.ItemContainerGenerator.StatusChanged += ListBox_ContainerGeneratorStatusChanged;
				sender.DataContextChanged += ListBox_DataContextChanged;
			}
			else
			{
				sender.MouseDoubleClick -= ListBox_MouseDoubleClick;
				sender.ItemContainerGenerator.StatusChanged -= ListBox_ContainerGeneratorStatusChanged;
				sender.DataContextChanged -= ListBox_DataContextChanged;
			}
		}

		#region Event handlers
		private static void ListBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var listBox = sender as ListBox;

			// remove old list-changed handler
			if (e.OldValue is IBindingList)
			{
				(e.OldValue as IBindingList).ListChanged -= (s, args) => ActivatableListBox_ListChanged(listBox, s, args);
			}
			else if (e.OldValue is INotifyCollectionChanged)
			{
				(e.OldValue as INotifyCollectionChanged).CollectionChanged -= (s, args) => ActivatableListBox_CollectionChanged(listBox, s, args);
			}

			// add new list-changed handler
			if (e.NewValue is IBindingList)
			{
				(e.NewValue as IBindingList).ListChanged += (s, args) => ActivatableListBox_ListChanged(listBox, s, args);
			}
			else if (e.NewValue is INotifyCollectionChanged)
			{
				(e.NewValue as INotifyCollectionChanged).CollectionChanged += (s, args) => ActivatableListBox_CollectionChanged(listBox, s, args);
			}
		}

		private static void ActivatableListBox_CollectionChanged(ListBox listBox, object sender, NotifyCollectionChangedEventArgs e)
		{
			// TODO: This is untested (binding to an ObservableCollection<T>)
			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				var list = sender as INotifyCollectionChanged;
				if (e.OldItems.Contains(GetActiveItem(listBox)))
					SetActiveItem(listBox, null);
			}
			else if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				if (!listBox.Items.Contains(GetActiveItem(listBox)))
					SetActiveItem(listBox, null);
			}

			UpdateActivatedIndex(listBox, listBox.GetActiveItem());
		}

		private static void ActivatableListBox_ListChanged(ListBox listBox, object sender, ListChangedEventArgs e)
		{
			if (e.ListChangedType == ListChangedType.ItemDeleted)
			{
				var list = sender as IBindingList;
				if (e.NewIndex == listBox.GetActiveIndex()) // removing active item
				{
					SetActiveItem(listBox, null);
				}
			}
			else if (e.ListChangedType == ListChangedType.Reset)
			{
				if (!listBox.Items.Contains(GetActiveItem(listBox)))
					SetActiveItem(listBox, null);
			}
			else if (e.ListChangedType == ListChangedType.ItemChanged)
			{
				UpdateActivatedItem(listBox, GetActiveItem(listBox));
			}

			UpdateActivatedIndex(listBox, listBox.GetActiveItem());
		}

		private static void ListBox_ContainerGeneratorStatusChanged(object sender, EventArgs e)
		{
			var generator = sender as ItemContainerGenerator;
			if (generator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
			{
				var first = generator.ContainerFromIndex(0) as DependencyObject;
				if (first != null)
				{
					// find parent ListBox from first container
					var listBox = first.FindVisualParent<ListBox>();
					var value = listBox.GetActiveItem();
					UpdateActivatedItem(listBox, value);
				}
			}
		}

		private static void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				// get ListBoxItem at mouse position
				var listBox = sender as ListBox;
				var element = listBox.InputHitTest(e.GetPosition(listBox)) as DependencyObject;
				var listBoxItem = element.FindVisualParent<ListBoxItem, ListBox>();
				if (listBoxItem != null)
				{
					var content = listBoxItem.GetValue(ListBoxItem.ContentProperty);
					if (content != null)
					{
						if (GetActiveItem(listBox) != content/* && content.Activate()*/)
						{
							SetActiveItem(listBox, content);
						}
					}
				}
			}
		}

		#endregion

		[AttachedPropertyBrowsableForType(typeof(ListBox))]
		public static object GetActiveItem(this ListBox obj)
		{
			return obj.GetValue(ActiveItemProperty);
		}

		public static void SetActiveItem(this ListBox obj, object value)
		{
			obj.SetValue(ActiveItemProperty, value);
		}

		public static readonly DependencyProperty ActiveItemProperty =
			DependencyProperty.RegisterAttached("ActiveItem", typeof(object), typeof(Activator), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnActiveItemChanged));

		private static void OnActiveItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var value = e.NewValue;
			var listBox = d as ListBox;
			UpdateActivatedIndex(listBox, value);
			UpdateActivatedItem(listBox, value);
		}

		[AttachedPropertyBrowsableForType(typeof(ListBox))]
		public static int GetActiveIndex(this ListBox obj)
		{
			return (int)obj.GetValue(ActiveIndexProperty);
		}

		public static readonly DependencyPropertyKey ActiveIndexPropertyKey =
			DependencyProperty.RegisterAttachedReadOnly("ActiveIndex", typeof(int), typeof(Activator), new UIPropertyMetadata(-1));

		public static readonly DependencyProperty ActiveIndexProperty =
			ActiveIndexPropertyKey.DependencyProperty;

		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		public static bool GetIsActive(ListBoxItem obj)
		{
			return (bool)obj.GetValue(IsActiveProperty);
		}

		public static readonly DependencyPropertyKey IsActivePropertyKey =
			DependencyProperty.RegisterAttachedReadOnly("IsActive", typeof(bool), typeof(Activator), new UIPropertyMetadata(false));

		public static readonly DependencyProperty IsActiveProperty =
			IsActivePropertyKey.DependencyProperty;

		private static void UpdateActivatedItem(ListBox listBox, object value)
		{
			for (int i = 0; i < listBox.Items.Count; i++)
			{
				var container = listBox.ItemContainerGenerator.ContainerFromIndex(i);
				if (container != null)
				{
					container.SetValue(IsActivePropertyKey, listBox.Items[i] == value);
				}
			}
		}

		private static void UpdateActivatedIndex(ListBox listBox, object value)
		{
			// find index of activated item
			int index;
			if (value == null)
			{
				index = -1;
			}
			else
			{
				index = 0;
				foreach (var item in listBox.Items)
				{
					if (item == value)
						break;
					index++;
				}
			}

			if (index >= listBox.Items.Count)
				index = -1;

			listBox.SetValue(ActiveIndexPropertyKey, index);
		}
	}
}