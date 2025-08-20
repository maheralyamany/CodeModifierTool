using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

using DevComponents.DotNetBar;

namespace OpetraViews.Controls {
	/// <summary>Represents: FileExplorerTreeView</summary>
	[Designer(typeof(FileExplorerTreeViewDesigner))]
	public class FileExplorerTreeView : System.Windows.Forms.TreeView {
		private Dictionary<string, string> iconCache = new Dictionary<string, string>();
		/// <summary>Gets or sets: root tree node</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		public FileTreeNode RootTreeNode { get; set; } = new FileTreeNode();

		/// <summary>Initializes a new instance of the FileExplorerTreeView class</summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FileExplorerTreeView() {
			InitializeComponent();
			this.CheckBoxes = true;
			this.ImageList = imageList1;
			this.DrawMode = TreeViewDrawMode.Normal;
			//DrawNode += TreeView1_DrawNode;
			this.imageList1.Images.Add("file", SystemIcons.Application.ToBitmap());
		}

		private string excludedFolders;
		private string excludedExtensions;
		private string filterFilesExtensions;
		private List<string> filesExtensions = new List<string>();
		private List<string> excludedExtensionsList = new List<string>();
		private List<string> excludedFoldersList = new List<string>();
		private string rootDirectoryPath;
		/// <summary>Gets or sets: excluded folders</summary>
		[DefaultValue(null)]
		public string ExcludedFolders {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return excludedFolders;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (excludedFolders == value)
					return;
				if (!value.IsEmptyOrWhiteSpace())
					excludedFolders = value;
				else
					excludedFolders = null;
				excludedFoldersList = value.IsEmptyOrWhiteSpace() ? new List<string>() : value.SplitList(",");
				ReloadNodes();
			}
		}

		/// <summary>Gets or sets: filter files extensions</summary>
		[DefaultValue(null)]
		public string FilterFilesExtensions {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return filterFilesExtensions;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (filterFilesExtensions == value)
					return;
				if (!value.IsEmptyOrWhiteSpace())
					filterFilesExtensions = value;
				else
					filterFilesExtensions = null;
				filesExtensions = value.IsEmptyOrWhiteSpace() ? new List<string>() : value.SplitList(",");
				ReloadNodes();
			}
		}

		/// <summary>Gets or sets: excluded extensions</summary>
		[DefaultValue(null)]
		public string ExcludedExtensions {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return excludedExtensions;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (excludedExtensions == value)
					return;
				if (!value.IsEmptyOrWhiteSpace())
					excludedExtensions = value;
				else
					excludedExtensions = null;
				excludedExtensionsList = value.IsEmptyOrWhiteSpace() ? new List<string>() : value.SplitList(",");
				ReloadNodes();
			}
		}

		private bool includeHiddenFolders = false;
		/// <summary>Gets or sets: include hidden folders</summary>
		[DefaultValue(false)]
		public bool IncludeHiddenFolders {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return includeHiddenFolders;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (includeHiddenFolders == value)
					return;
				includeHiddenFolders = value;
				ReloadNodes();
			}
		}

		private int maxDirectoryLevel = -1;
		/// <summary>Gets or sets: max directory level</summary>
		[DefaultValue(-1)]
		public int MaxDirectoryLevel {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return maxDirectoryLevel;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (maxDirectoryLevel == value)
					return;
				maxDirectoryLevel = value;
				ReloadNodes();
			}
		}

		private bool expandOnClick = false;
		/// <summary>Gets or sets: expand on click</summary>
		[DefaultValue(false)]
		public bool ExpandOnClick {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return expandOnClick;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				//Toggle
				if (expandOnClick == value)
					return;
				expandOnClick = value;
			}
		}

		private bool singleCheckFolder = false;
		/// <summary>Gets or sets: single check folder</summary>
		[DefaultValue(false)]
		public bool SingleCheckFolder {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return singleCheckFolder;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (singleCheckFolder == value)
					return;
				singleCheckFolder = value;
			}
		}

		private bool includeFiles = true;
		/// <summary>Gets or sets: include files</summary>
		[DefaultValue(true)]
		public bool IncludeFiles {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return includeFiles;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (includeFiles == value)
					return;
				includeFiles = value;
				ReloadNodes();
			}
		}

		private bool expandAllNodes = false;
		/// <summary>Gets or sets: expand all nodes</summary>
		[DefaultValue(false)]
		public bool ExpandAllNodes {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return expandAllNodes;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (expandAllNodes == value)
					return;
				expandAllNodes = value;
				if (this.Nodes.Count > 0) {
					if (expandAllNodes)
						this.ExpandAll();
					else {
						this.CollapseAll();
						foreach (TreeNode node in this.Nodes) {
							if (node.Tag is string path) {
								if (path == rootDirectoryPath) {
									node.Expand();
									return;
								}
							}
						}
					}
				}
			}
		}

		/// <summary>Gets or sets: root path</summary>
		[DefaultValue(null)]
		[Editor(typeof(FileExplorerRootPathEditor), typeof(System.Drawing.Design.UITypeEditor))]
		//[TypeConverter(typeof(RootPathConverter))]
		public string RootPath {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return rootDirectoryPath;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				rootDirectoryPath = value;
				ReloadNodes();
			}
		}

		/// <summary>Sets excluded folders list</summary>
		/// <param name = "folders">The folders of type String</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetExcludedFoldersList(List<string> folders) {
			excludedFoldersList = folders;
		}

		// Public: Load new path dynamically
		/// <summary>Performs load root path</summary>
		/// <param name = "path">The path collection</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void LoadRootPath(string path) {
			try {
				Clear();
				if (!Directory.Exists(path)) {
					DoToast.Show("Invalid directory");
					return;
				}

				DirectoryInfo root = new DirectoryInfo(path);
				RootTreeNode = new FileTreeNode(root.FullName, root.Name) {
					Level = 0,
					Index = 0,
					ParentId = -1,
					ImageKey = "folder",
					SelectedImageKey = "openfolder",
				};
				LoadSubNodes(RootTreeNode);
				int currentIndex = 0;
				RootTreeNode.SetNodeIndex(ref currentIndex, -1);
				var rootNode = RootTreeNode.GetNodeWithSubNode();
				this.Nodes.Add(rootNode);
				rootNode.Expand();
				if (RootTreeNode.SubNodes.Count > 0) {
				}
			} catch (System.Exception ex) {

				DoToast.Show(ex.Message);
			}
		}

		/// <summary>Performs reload nodes</summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void ReloadNodes() {
			if (!rootDirectoryPath.IsEmptyOrWhiteSpace())
				LoadRootPath(rootDirectoryPath);
			else {
				Clear();
			}
		}

		/// <summary>Performs clear</summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Clear() {
			RootTreeNode.ClearAll();
			Nodes.Clear();
		}

		/// <summary>Gets: checked nodes</summary>
		[Browsable(false)]
		public List<TreeNode> CheckedNodes => GetAllCheckedNodes();

		/// <summary>Gets: checked file nodes</summary>
		[Browsable(false)]
		public List<FileTreeNode> CheckedFileNodes => GetAllCheckedFileNodes();

		/// <summary>Gets checked directories</summary>
		/// <param name = "level">The level</param>
		/// <returns>The retrieved checked directories collection</returns>
		/// <exception cref = "System.ArgumentNullException">Thrown when level is null</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public List<string> GetCheckedDirectories(int level = -1) => CheckedFileNodes.LWhereSelect(n => n != null && !n.IsFileType && (n.Level == level || level == -1), n => n.Path);
		/// <summary>Gets checked file tree nodes</summary>
		/// <returns>The retrieved checked file tree nodes collection</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public List<FileTreeNode> GetCheckedFileTreeNodes() => CheckedFileNodes;
		/// <summary>Gets: checked paths</summary>
		[Browsable(false)]
		public List<string> CheckedPaths => CheckedFileNodes.LSelect(n => n.Path);

		/*private FileTreeNode AddFileTreeNodeIndex(FileTreeNode node) {
        if (node.Index < 0)
        node.Index = currentIndex++;
        if (node.ParentId >= 0)
        node.AddParentId(node.ParentId);
        return node;
        }*/
		/// <summary>Gets file tree node</summary>
		/// <param name = "node">The node</param>
		/// <returns>The retrieved file tree node</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private FileTreeNode GetFileTreeNode(TreeNode node) {
			if (node.Tag == null)
				return null;
			var n = this.RootTreeNode.SearchFileTreeNode(node.Tag.ToString());
			return n;
		}

		/// <summary>Gets directories</summary>
		/// <param name = "path">The path collection</param>
		/// <returns>The retrieved directories collection</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private List<DirectoryInfo> GetDirectories(string path) {
			var dirs = Directory.GetDirectories(path).LSelectWhere(dir => new DirectoryInfo(dir), dirInfo => {
				var dirName = dirInfo.Name;
				return !((!includeHiddenFolders && dirName.MTrimStartsWith(".")) || (excludedFoldersList.Any(b => dirName.MStartsWith(b))));
			});
			return dirs;
		}

		/// <summary>Gets files</summary>
		/// <param name = "path">The path collection</param>
		/// <returns>The retrieved files collection</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private List<string> GetFiles(string path) {
			if (!includeFiles)
				return new List<string>();
			var files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).LWhere(file => {
				return !((filesExtensions.Count > 0 && !filesExtensions.Any(ext => file.MEndsWith(ext))) || excludedExtensionsList.Any(ext => file.MEndsWith(ext)));
			});
			return files;
		}

		/// <summary>Determines whether sub nodes</summary>
		/// <param name = "path">The path collection</param>
		/// <returns>Whether sub nodes</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool HasSubNodes(string path) {
			var directories = GetDirectories(path).Count;
			if (directories == 0)
				return GetFiles(path).Count > 0;
			return directories > 0;
		}

		/// <summary>Performs on before expand</summary>
		/// <param name = "e">The e</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected override void OnBeforeExpand(TreeViewCancelEventArgs e) {
			base.OnBeforeExpand(e);
			TreeView1_BeforeExpand(this, e);
		}

		/// <summary>Performs load file nodes</summary>
		/// <param name = "treeFile">The treeFile</param>
		/// <returns>The result of the load file nodes</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private FileTreeNode LoadFileNodes(FileTreeNode treeFile) {
			if (includeFiles) {
				var files = GetFiles(treeFile.Path);
				foreach (var file in files) {
					FileInfo fileInfo = new FileInfo(file);
					string ext = fileInfo.Extension.ToLower();
					if (!iconCache.ContainsKey(ext)) {
						try {
							if (!imageList1.Images.ContainsKey(ext)) {
								var icon = ShellIconHelper.GetSmallIcon(file);
								imageList1.Images.Add(ext, icon.ToBitmap());
							}

							iconCache[ext] = ext;
							//iconCache[ext] = "file";
						} catch {
							iconCache[ext] = "file";
						}
					}

					var n = new FileTreeNode(fileInfo.FullName, fileInfo.Name) {
						Level = treeFile.Level + 1,
						ParentId = treeFile.Index,
						FileType = TreeFileType.File,
						ImageKey = iconCache[ext],
						SelectedImageKey = iconCache[ext],
					};
					treeFile.AddSubNode(n);
				}
			}

			return treeFile;
		}

		/// <summary>Performs load sub nodes</summary>
		/// <param name = "treeFile">The treeFile</param>
		/// <returns>The result of the load sub nodes</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private FileTreeNode LoadSubNodes(FileTreeNode treeFile) {
			if (treeFile == null || treeFile.FileType == TreeFileType.File || (maxDirectoryLevel > -1 && treeFile.Level > maxDirectoryLevel))
				return treeFile;
			var directories = GetDirectories(treeFile.Path);
			foreach (var dirInfo in directories) {
				var dirName = dirInfo.Name;
				if ((!includeHiddenFolders && dirName.MTrimStartsWith(".")) || (excludedFoldersList.Any(b => dirName.MStartsWith(b))))
					continue;
				var n = new FileTreeNode(dirInfo.FullName, dirInfo.Name) {
					Level = treeFile.Level + 1,
					ParentId = treeFile.Index,
				};
				n = LoadSubNodes(n);
				n = LoadFileNodes(n);
				treeFile.AddSubNode(n);
			}

			treeFile = LoadFileNodes(treeFile);
			return treeFile;
		}

		/// <summary>Performs tree view1_ before expand</summary>
		/// <param name = "sender">The sender</param>
		/// <param name = "e">The e</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void TreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
			TreeNode node = e.Node;
			if (node.Nodes.Count == 1 && node.Nodes[0].Text == "Loading...") {
				node.Nodes.Clear();
				var treeFile = GetFileTreeNode(node);
				if (treeFile == null || treeFile.FileType == TreeFileType.File || (maxDirectoryLevel > -1 && treeFile.Level > maxDirectoryLevel))
					return;
				//
			}
		}

		/// <summary>Performs on mouse click</summary>
		/// <param name = "e">The e</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected override void OnMouseClick(MouseEventArgs e) {
			if (ExpandOnClick) {
				TreeNode tn = this.GetNodeAt(e.Location);
				if (tn != null) {
					if (tn.Level != 0) {
						this.SelectedNode = tn;
					}

					var l = e.Location;
					var b = tn.Bounds;
					Rectangle bounds = new Rectangle(tn.Bounds.Left - 17, tn.Bounds.Y, tn.Bounds.Width - 16, tn.Bounds.Height);
					if ((l.X >= b.Left && l.X <= b.Right && l.Y >= b.Top && l.Y <= b.Bottom) || bounds.Contains(e.Location) == false) {
						if (tn.IsExpanded == false)
							tn.Expand();
						else
							tn.Collapse();
					}
				}
			}

			base.OnMouseClick(e);
		}

		private bool lockAfterCheckEvent = false;
		//private Dictionary<object,bool> checkLock
		/// <summary>Performs on after check</summary>
		/// <param name = "e">The e</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected override void OnAfterCheck(TreeViewEventArgs e) {
			TreeView1_AfterCheck(e.Node, e);
			base.OnAfterCheck(e);
		}

		/// <summary>Performs tree view1_ after check</summary>
		/// <param name = "sender">The sender</param>
		/// <param name = "e">The e</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e) {
			if (lockAfterCheckEvent)
				return;
			lockAfterCheckEvent = true;
			try {
				TreeNode node = e.Node;
				var isChecked = node.Checked;
				var treeFile = GetFileTreeNode(node);
				if (treeFile != null) {
					if (SingleCheckFolder && isChecked) {
						if ((treeFile.IsFolderType) && treeFile.ParentId >= 0) {
							SetAllCheckedNodesRecursive(false, treeFile);
						}
					}

					treeFile.SetSubNodesCheckedRecursive(isChecked);
				}
			} finally {
				lockAfterCheckEvent = false;
			}
		}

		/// <summary>Sets all checked nodes recursive</summary>
		/// <param name = "isChecked">Whether checked exists/is true</param>
		/// <param name = "treeNode">The treeNode</param>
		/// <exception cref = "System.ArgumentNullException">Thrown when isChecked is null</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void SetAllCheckedNodesRecursive(bool isChecked, FileTreeNode treeNode) {
			foreach (var child in this.RootTreeNode.SubNodes) {
				SetCheckedNodesRecursive(child, isChecked, treeNode);
			}
		}

		/// <summary>Sets checked nodes recursive</summary>
		/// <param name = "node">The node</param>
		/// <param name = "isChecked">Whether checked exists/is true</param>
		/// <param name = "treeNode">The treeNode</param>
		/// <exception cref = "System.ArgumentNullException">Thrown when isChecked is null</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void SetCheckedNodesRecursive(FileTreeNode node, bool isChecked, FileTreeNode treeNode) {
			if (treeNode.Index != node.Index) {
				if (isChecked != node.Checked) {
					node.Checked = isChecked;
				}
			}

			if (treeNode.Index != node.Index || isChecked)
				foreach (var child in node.SubNodes) {
					SetCheckedNodesRecursive(child, isChecked, treeNode);
					/*if (!childFile.Path.MEquals(treeNode.Path) && (isChecked || !childFile.GetParentsIds().Contains(treeNode.Index))) {
																if (isChecked != child.Checked) {
																child.Checked = isChecked;
																}
																if (!childFile.IsFileType)
																SetCheckedNodesRecursive(child, isChecked, treeNode);
																}*/
				}
		}

		/// <summary>Performs on node mouse click</summary>
		/// <param name = "e">The e</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e) {
			base.OnNodeMouseClick(e);
			TreeView1_NodeMouseClick(this, e);
		}

		/// <summary>Performs tree view1_ node mouse click</summary>
		/// <param name = "sender">The sender</param>
		/// <param name = "e">The e</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
			SelectedNode = e.Node;
			//Invalidate();
		}

		/// <summary>Gets all checked nodes</summary>
		/// <returns>The retrieved all checked nodes collection</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private List<TreeNode> GetAllCheckedNodes() {
			List<TreeNode> result = new List<TreeNode>();
			GetCheckedNodesRecursive(this.RootTreeNode, result);
			return result;
		}

		/// <summary>Gets all checked file nodes</summary>
		/// <returns>The retrieved all checked file nodes collection</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public List<FileTreeNode> GetAllCheckedFileNodes() {
			List<FileTreeNode> result = new List<FileTreeNode>();
			GetCheckedNodesFileRecursive(this.RootTreeNode, result);
			return result;
		}

		/// <summary>Gets checked nodes file recursive</summary>
		/// <param name = "node">The node</param>
		/// <param name = "list">The list of type FileTreeNode</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void GetCheckedNodesFileRecursive(FileTreeNode node, List<FileTreeNode> list) {
			if (node.Checked) {
				/*var treeFile = GetFileTreeNode(node);
                if (treeFile != null)*/
				list.Add(node);
			}

			foreach (var child in node.SubNodes)
				GetCheckedNodesFileRecursive(child, list);
		}

		/// <summary>Gets checked nodes recursive</summary>
		/// <param name = "node">The node</param>
		/// <param name = "list">The list of type TreeNode</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void GetCheckedNodesRecursive(FileTreeNode node, List<TreeNode> list) {
			if (node.Checked)
				list.Add(node.GetNode());
			foreach (var child in node.SubNodes)
				GetCheckedNodesRecursive(child, list);
		}

		/// <summary>Gets: nodes</summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		[MergableProperty(false)]
		public new TreeNodeCollection Nodes {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return base.Nodes;
			}
		}

		/// <summary>Gets or sets: check boxes</summary>
		[DefaultValue(true)]
		public new bool CheckBoxes {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.CheckBoxes; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.CheckBoxes = value;
		}

		/// <summary>Gets or sets: hot tracking</summary>
		[DefaultValue(true)]
		public new bool HotTracking {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.HotTracking; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.HotTracking = value;
		}

		/// <summary>Gets or sets: image index</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		[DefaultValue(0)]
		public new int ImageIndex {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.ImageIndex; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.ImageIndex = value;
		}

		/// <summary>Gets or sets: image key</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		public new string ImageKey {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.ImageKey; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.ImageKey = value;
		}

		/// <summary>Gets or sets: path separator</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		public new string PathSeparator {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.PathSeparator; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.PathSeparator = value;
		}

		/// <summary>Gets or sets: selected image key</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		public new string SelectedImageKey {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.SelectedImageKey; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.SelectedImageKey = value;
		}

		/// <summary>Gets or sets: selected image index</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		[DefaultValue(0)]
		public new int SelectedImageIndex {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.SelectedImageIndex; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.SelectedImageIndex = value;
		}

		/// <summary>Gets or sets: image list</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		public new ImageList ImageList {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.ImageList; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.ImageList = value;
		}

		/// <summary>Gets or sets: state image list</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(false)]
		public new ImageList StateImageList {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.StateImageList; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.StateImageList = value;
		}

		/// <summary>Gets or sets: indent</summary>
		[DefaultValue(19)]
		public new int Indent {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.Indent; [MethodImpl(MethodImplOptions.NoInlining)]
			set => base.Indent = value;
		}

		/// <summary>Performs dispose</summary>
		/// <param name = "disposing">The disposing</param>
		/// <exception cref = "System.ArgumentNullException">Thrown when disposing is null</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// <summary>Performs initialize component</summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileExplorerTreeView));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			// 
			// imageList1
			//
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			/*this.imageList1.Images.SetKeyName(0, "openfolder");
            this.imageList1.Images.SetKeyName(1, "folder");
            this.imageList1.Images.SetKeyName(2, ".cs");
            this.imageList1.Images.SetKeyName(3, ".csproj");
            this.imageList1.Images.SetKeyName(4, "references");
            this.imageList1.Images.SetKeyName(5, ".ico");
            this.imageList1.Images.SetKeyName(6, ".dll");
            this.imageList1.Images.SetKeyName(7, "form");*/
			this.ShowLines = true;
			this.HotTracking = true;
			this.ImageIndex = 0;
			this.ImageList = this.imageList1;
			this.Indent = 19;
			this.SelectedImageIndex = 0;
			this.Size = new System.Drawing.Size(300, 400);
		}

		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ImageList imageList1;
	}
}
