﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Words.Presentation.Wpf;
using System.Windows.Controls;
using System.Windows;


namespace Words
{
	public class XpsPresentation : WpfPresentation<DocumentViewer>
	{
		public XpsPresentation()
		{
			this.Control.Style = Application.Current.FindResource("reducedDocumentViewer") as Style;
		}
		public void SetSourceDocument(XpsDocument doc)
		{
			this.Control.Document = doc.Document.GetFixedDocumentSequence();
		}
	}
}