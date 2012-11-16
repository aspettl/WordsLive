﻿/*
 * WordsLive - worship projection software
 * Copyright (c) 2012 Patrick Reisert
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonitoredUndo;

namespace WordsLive.Core.Songs.Undo
{
	internal static class ChangeFactory
	{
		/// <summary>
		/// Construct a Change instance with actions for undo / redo.
		/// </summary>
		/// <param name="instance">The instance that changed.</param>
		/// <param name="propertyName">The property name that changed. (Case sensitive, used by reflection.)</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		/// <returns>A Change that can be added to the UndoRoot's undo stack.</returns>
		public static Change GetChange(ISongElement instance, string propertyName, object oldValue, object newValue)
		{
			var change = new DelegateChange(instance,
								() => instance.GetType().GetProperty(propertyName).SetValue(instance, oldValue, null),
								() => instance.GetType().GetProperty(propertyName).SetValue(instance, newValue, null),
								new ChangeKey<object, string>(instance, propertyName)
							);

			return change;
		}

		/// <summary>
		/// Construct a Change instance with actions for undo / redo.
		/// </summary>
		/// <param name="instance">The instance that changed.</param>
		/// <param name="propertyName">The property name that changed. (Case sensitive, used by reflection.)</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		public static void OnChanging(ISongElement instance, string propertyName, object oldValue, object newValue)
		{
			OnChanging(instance, propertyName, oldValue, newValue, propertyName);
		}

		/// <summary>
		/// Construct a Change instance with actions for undo / redo.
		/// </summary>
		/// <param name="instance">The instance that changed.</param>
		/// <param name="propertyName">The property name that changed. (Case sensitive, used by reflection.)</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		/// <param name="descriptionOfChange">A description of this change.</param>
		public static void OnChanging(ISongElement instance, string propertyName, object oldValue, object newValue, string descriptionOfChange)
		{
			if (!instance.Root.IsUndoEnabled)
				return;

			Change change = GetChange(instance, propertyName, oldValue, newValue);
			UndoService.Current[instance.Root.UndoKey].AddChange(change, descriptionOfChange);
		}

		public static void OnChanging(ISongElement instance, Action undoAction, Action redoAction, string description)
		{
			if (!instance.Root.IsUndoEnabled)
				return;

			var ch = new DelegateChange(instance, undoAction, redoAction, new ChangeKey<object, string>(instance, description));
			UndoService.Current[instance.Root.UndoKey].AddChange(ch, description);
		}

		/// <summary>
		/// Construct a Change instance with actions for undo / redo
		/// and tries to merge it with the previous one if possible.
		/// </summary>
		/// <param name="instance">The instance that changed.</param>
		/// <param name="propertyName">The property name that changed. (Case sensitive, used by reflection.)</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		public static void OnChangingTryMerge(ISongElement instance, string propertyName, object oldValue, object newValue)
		{
			if (!instance.Root.IsUndoEnabled)
				return;

			var ch = DefaultChangeFactory.GetChange(instance, propertyName, oldValue, newValue);
			var x = ch.ChangeKey.GetType();
			if (instance.Root.IsModified &&
				UndoService.Current[instance.Root.UndoKey].UndoStack.Count() > 0 &&
				UndoService.Current[instance.Root.UndoKey].UndoStack.First().Changes.Count() > 0 &&
				UndoService.Current[instance.Root.UndoKey].UndoStack.First().Changes.First().Target == instance &&
				UndoService.Current[instance.Root.UndoKey].UndoStack.First().Changes.Count() == 1 &&
				((ChangeKey<object, string>)UndoService.Current[instance.Root.UndoKey].UndoStack.First().Changes.First().ChangeKey).Item2 == propertyName)
			{
				UndoService.Current[instance.Root.UndoKey].UndoStack.First().Changes.First().MergeWith(ch);
			}
			else
			{
				UndoService.Current[instance.Root.UndoKey].AddChange(ch, propertyName);
			}
		}

		public static IDisposable Batch(ISongElement instance, string description)
		{
			if (instance.Root.IsUndoEnabled)
				return new UndoBatch(instance.Root.UndoKey, description, false);
			else
				return new UndoBatchDummy();
		}

		public class UndoBatchDummy : IDisposable
		{
			public void Dispose()
			{
				// do nothing
			}
		}
	}
}
