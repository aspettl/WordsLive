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

using WordsLive.Core.Songs;

namespace WordsLive.Editor.Nodes
{
	/// <summary>
	/// Represents an abstract base class for metadata nodes
	/// in the song structure tree.
	/// </summary>
	public abstract class MetadataNode : ISongElement
	{
		/// <summary>
		/// Gets the song this metadata belongs to.
		/// </summary>
		public Song Root { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataNode"/> class.
		/// </summary>
		/// <param name="song">The song.</param>
		public MetadataNode(Song song)
		{
			this.Root = song;
		}
	}
}
