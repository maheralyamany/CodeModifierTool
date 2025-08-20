using System;
using System.Reflection;
using System.Windows.Forms;

namespace CodeModifierTool {
	public partial class CommentOptionsForm : Form {
		private Action<ProcessingOptions> Action;
		private ProcessingOptions options;
		public CommentOptionsForm(ProcessingOptions options, Action<ProcessingOptions> action) {
			InitializeComponent();

			listView.FullRowSelect = true;
			Action = action;
			columnHeader1.Width = listView.Width;
			this.options = options ?? new ProcessingOptions();
			listView.Items.Clear();
			listView.MouseClick += ListView_MouseClick;
			var properties = GetProperties();
			foreach (var prop in properties) {
				var value = (bool)prop.GetValue(this.options);

				var text = CodeFormatter.ToTitle(prop.Name);
				ListViewItem item = new ListViewItem(text) {
					Tag = prop.Name,
					Checked = value,
					IndentCount = 10
				};

				listView.Items.Add(item);
			}

		}

		private void ListView_MouseClick(object sender, MouseEventArgs e) {
			ListViewItem item = listView.GetItemAt(e.X, e.Y);
			if (item != null)
				item.Checked = !item.Checked;
		}

		private System.Collections.Generic.List<System.Reflection.PropertyInfo> GetProperties() {

			var properties = this.options.GetType().GetBasePropertiesList(p => p.PropertyType == typeof(bool));

			return properties;
		}

		private void BtnOk_Click(object sender, EventArgs e) {
			var properties = GetProperties().ToMDictionary(s => s.Name, s => s);

			foreach (ListViewItem item in listView.Items) {
				var tag = item.Tag.ToString();
				if (properties.TryGetValue(tag, out PropertyInfo prop)) {
					prop.SetValue(this.options, item.Checked);
				}
			}
			Action?.Invoke(this.options);
			this.Close();
		}
	}
}
