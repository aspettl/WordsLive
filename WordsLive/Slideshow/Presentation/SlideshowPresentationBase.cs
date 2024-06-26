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
using System.Collections.Generic;
using WordsLive.Presentation;

namespace WordsLive.Slideshow.Presentation
{
	public abstract class SlideshowPresentationBase : ISlideshowPresentation
	{
		protected IPreviewProvider preview;

		public abstract void Load();

		public virtual bool UsesSamePresentationWindow(IPresentation presentation)
		{
			return false;
		}

		public virtual bool TransitionPossibleFrom(IPresentation presentation)
		{
			return false;
		}

		public virtual bool TransitionPossibleTo(IPresentation presentation)
		{
			return false;
		}

		public virtual void Init(PresentationArea area)
		{
			this.area = area;
		}

		private PresentationArea area;

		public PresentationArea Area
		{
			get
			{
				return area;
			}
		}

		public virtual void Show(int transitionDuration = 0, Action callback = null, IPresentation previous = null)
		{
			// NOTE: transition duration will be ignored (not possible with external presentations)
			Show();
			if (callback != null)
				callback();
		}

		public virtual void TransitionTo(IPresentation target, int transitionDuration, Action callback)
		{
			// not possible
		}

		protected abstract void LoadPreviewProvider();

		public IPreviewProvider Preview
		{
			get
			{
				return preview;
			}
		}

		public virtual void Close()
		{
			// do nothing
		}

		#region Events
		
		public event EventHandler SlideIndexChanged;

		protected virtual void OnSlideIndexChanged()
		{
			if (SlideIndexChanged != null)
				SlideIndexChanged(this, EventArgs.Empty);
		}

		public event EventHandler<SlideshowLoadedEventArgs> Loaded;

		protected virtual void OnLoaded(bool success)
		{
			if (Loaded != null)
				Loaded(this, new SlideshowLoadedEventArgs(success));
		}

		public event EventHandler ClosedExternally;

		protected virtual void OnClosedExternally()
		{
			if (ClosedExternally != null)
				ClosedExternally(this, EventArgs.Empty);
		}

		#endregion

		#region Abstract members

		public abstract void GotoSlide(int index);

		public abstract void NextStep();

		public abstract void PreviousStep();

		public abstract void Show();

		public abstract void Hide();

		public abstract IList<SlideThumbnail> Thumbnails { get; }

		public abstract bool IsEndless { get; }

		public abstract int SlideIndex { get; }

		#endregion
	}
}
