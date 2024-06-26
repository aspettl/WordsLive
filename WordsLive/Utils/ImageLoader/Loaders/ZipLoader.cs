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

using System;
using System.IO;
using Ionic.Zip;

namespace WordsLive.Utils.ImageLoader.Loaders
{
	internal class ZipLoader: ILoader
	{
		private readonly static object Lock = new Object();

		public Stream Load(object source)
		{
			if (source is ZipEntry)
			{
				ZipEntry entry = source as ZipEntry;
				lock(Lock)
				{
					MemoryStream stream = new MemoryStream();
					entry.Extract(stream);
					stream.Flush();
					stream.Seek(0, SeekOrigin.Begin);
					return stream;
				}
			}
			else
			{
				return null;
			}
		}
	}
}
