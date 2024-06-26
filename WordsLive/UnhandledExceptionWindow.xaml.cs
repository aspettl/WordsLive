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
using System.Windows;

namespace WordsLive
{
	public partial class UnhandledExceptionWindow : Window
	{
		public Exception Exception
		{
			get;
			private set;
		}

		public bool CanAbort
		{
			get;
			private set;
		}


		public UnhandledExceptionWindow(Exception exception, bool canAbort = true)
		{
			InitializeComponent();

			this.CanAbort = canAbort;
			this.Exception = exception;
			this.DataContext = this;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}
	}
}
