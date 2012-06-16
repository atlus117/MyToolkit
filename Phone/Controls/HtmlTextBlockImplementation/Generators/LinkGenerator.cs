using System;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Phone.Tasks;
using MyToolkit.MVVM;
using MyToolkit.Paging;

namespace MyToolkit.Controls.HtmlTextBlockImplementation.Generators
{
	public class LinkGenerator : SingleControlGenerator
	{
		public override DependencyObject GenerateSingle(HtmlNode node, IHtmlTextBlock textBlock)
		{
			try
			{
				var link = node.Attributes["href"];
				var label = node.Children[0].Value;

				var hr = new Hyperlink();
				hr.TextDecorations = TextDecorations.Underline;
				hr.Inlines.Add(label);

				var action = CreateLinkAction(hr, link, textBlock);
				var origAction = action;
				action = delegate
				{
					if (NavigationState.TryBeginNavigating())
						origAction();
				};

				hr.Command = new RelayCommand(action);	
				return hr;
			}
			catch
			{
				return null;
			}
		}

		protected virtual Action CreateLinkAction(Hyperlink hyperlink, string link, IHtmlTextBlock textBlock)
		{
			if (link.StartsWith("mailto:"))
				return () => new EmailComposeTask {To = link.Substring(7)}.Show();

			var uri = link.StartsWith("http://") || link.StartsWith("https://") ?
				new Uri(link, UriKind.Absolute) : new Uri(textBlock.BaseUri, link);
			return () => new WebBrowserTask { Uri = uri }.Show();
		}
	}
}