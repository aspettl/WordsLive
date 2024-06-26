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
using System.IO;
using Xunit;

namespace WordsLive.Core.Tests
{
	public class DataTests
	{
		[Fact]
		public void TempDirectory()
		{
			var dir = new DataManager.TemporaryDirectory();
			DirectoryInfo info = dir.Directory;
			var stream = File.Create(Path.Combine(dir.Directory.FullName, "test.tmp"));
			stream.Close();
			dir = null;
			GC.Collect();
			Assert.False(info.Exists);
		}
	}
}
