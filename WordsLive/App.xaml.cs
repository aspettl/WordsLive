﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Globalization;

namespace WordsLive
{
	/*
	 * TODO:
	 * - PDF support via pdf.js?
	 * - Shortcuts in the menu are shown as "Ctrl" instead of "Strg" in German language
	 * - Add alerts to Presentation.Wpf to be able to display messages over any WPF presentation
	 * - Improve audio/video support (look at DMediaPlayer) and support start/stop times
	 *   (see http://manual.openlp.org/creating_service.html#using-the-media-timer, but with better UI)
	 * - Maintain correct window z-order using SetWindowPos, especially for ImpressPresentation
	 *   (see http://msdn.microsoft.com/en-us/library/windows/desktop/ms633545%28v=vs.85%29.aspx)
	 * - Add "Edit" entry in context menu for songs
	 * - Put WordsLive.Slideshow (+Bridge) under GPLv3 and add COPYING.txt and README.txt
	 *   (with note that PowerpointViewerLib and WordsLive.Slideshow are under GPL) to distribution/installer
	 * - Refactor drag & drop (introduce helper class)
	 */

	public partial class App : Application
	{
		public static string StartupPortfolio { get; private set; }

		private void ApplicationStartup(object sender, StartupEventArgs e)
		{
			if (e.Args.Length > 0)
				StartupPortfolio = e.Args[0];

			//Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			//Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		}
	}
}