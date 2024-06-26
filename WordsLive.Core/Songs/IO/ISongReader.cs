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

using System.IO;

namespace WordsLive.Core.Songs.IO
{
	public interface ISongReader
	{
		/// <summary>
		/// Gets a value indicating whether in order to read a song with this reader
		/// the template needs to be loaded first.
		/// </summary>
		bool NeedsTemplate { get; }

		/// <summary>
		/// Reads the song data from a stream.
		/// </summary>
		/// <param name="song">The song.</param>
		/// <param name="stream">The stream.</param>
		void Read(Song song, Stream stream);
	}
}
