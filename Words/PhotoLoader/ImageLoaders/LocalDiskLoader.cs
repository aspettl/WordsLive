﻿using System.IO;

namespace Words.PhotoLoader.ImageLoaders
{
	internal class LocalDiskLoader: ILoader
	{
		public Stream Load(object source)
		{
			if (source is string)
				return File.OpenRead(source as string);
			else if (source is FileInfo)
				return File.OpenRead((source as FileInfo).FullName);
			else
				return null;
		}
	}
}
