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

namespace WordsLive.Core.Songs
{
	/// <summary>
	/// Represents a slide in a song.
	/// </summary>
	public class SongSlide
	{
		private string text;

		/// <summary>
		/// Gets or sets the text on this slide.
		/// </summary>
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				TextWithoutChords = Chords.Chords.RemoveAll(text);
			}
		}

		/// <summary>
		/// Gets the text on this slide, but with chords removed.
		/// </summary>
		public string TextWithoutChords { get; private set; }

		/// <summary>
		/// Gets or sets the translation of the text on this slide.
		/// </summary>
		public string Translation { get; set; }

		/// <summary>
		/// Gets or sets the index pointing to the background of this slide.
		/// </summary>
		public int BackgroundIndex { get; set; }

		/// <summary>
		/// Gets or sets the font size of the text on this slide.
		/// </summary>
		public int Size { get; set; }
	}
}
