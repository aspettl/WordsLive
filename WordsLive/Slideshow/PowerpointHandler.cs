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
using WordsLive.Core;
using WordsLive.Slideshow.Presentation;

namespace WordsLive.Slideshow
{
	// TODO: This currently only works with PowerpointViewer 2007 (older versions untested), not with 2010
	// and Visual C++ 2012 Redistributable must be installed for pptviewlib.dll to work
	public class PowerpointHandler : MediaTypeHandler
	{
		public override IEnumerable<string> Extensions
		{
			get { return new string[] { ".ppt", ".pptx" }; }
		}

		public override string Description
		{
			get { return "Powerpoint-Präsentationen"; }
		}

		public override int Test(Uri uri)
		{
			if (!SlideshowPresentationFactory.IsTargetAvailable(SlideshowPresentationTarget.PowerpointViewer) &&
				!SlideshowPresentationFactory.IsTargetAvailable(SlideshowPresentationTarget.Powerpoint) &&
				!SlideshowPresentationFactory.IsTargetAvailable(SlideshowPresentationTarget.Impress))
				return -1;
			else
				return CheckExtension(uri) ? 100 : -1;
		}

		public override Media Handle(Uri uri, Dictionary<string, string> options)
		{
			return new PowerpointMedia(uri);
		}
	}
}
