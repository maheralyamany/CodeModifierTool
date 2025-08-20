using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace OpetraViews.Controls
{
    /// <summary>Represents: FileTreeNode</summary>
    public class FileTreeNode
    {
        /// <summary>Gets or sets: checked</summary>
        public bool Checked {[MethodImpl(MethodImplOptions.NoInlining)]
            get => GetNode().Checked; [MethodImpl(MethodImplOptions.NoInlining)]
            set => GetNode().Checked = value; }
        /// <summary>Gets or sets: path</summary>
        public string Path { get; set; }
        /// <summary>Gets or sets: index</summary>
        public int Index { get; set; } = -1;
        /// <summary>Gets or sets: level</summary>
        public int Level { get; set; } = 0;
        /// <summary>Gets or sets: parent id</summary>
        public int ParentId { get; set; } = -1;

        /// <summary>The node field</summary>
        private TreeNode node;
        /// <summary>Gets or sets: node text</summary>
        public string NodeText { get; set; }
        /// <summary>Gets or sets: image key</summary>
        public string ImageKey { get; set; } = "folder";
        /// <summary>Gets or sets: selected image key</summary>
        public string SelectedImageKey { get; set; } = "openfolder";
        /// <summary>Gets or sets: file type</summary>
        public TreeFileType FileType { get; set; } = TreeFileType.Folder;
        /// <summary>Gets or sets: sub nodes</summary>
        public List<FileTreeNode> SubNodes { get; set; } = new List<FileTreeNode>();

        /// <summary>Initializes a new instance of the FileTreeNode class</summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileTreeNode()
        {
            Path = "";
            NodeText = "";
        }

        /// <summary>Initializes a new instance of the FileTreeNode class</summary>
        /// <param name = "path">The path collection</param>
        /// <param name = "nodeText">The nodeText collection</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileTreeNode(string path, string nodeText)
        {
            Path = path;
            NodeText = nodeText;
        }

        /// <summary>Gets node</summary>
        /// <returns>The retrieved node</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public TreeNode GetNode()
        {
            if (node == null)
            {
                node = new TreeNode(this.NodeText)
                {
                    Tag = this.Path,
                    ImageKey = this.ImageKey,
                    SelectedImageKey = this.SelectedImageKey,
                    StateImageKey = this.SelectedImageKey,
                };
            }

            return node;
        }

        /// <summary>Gets node with sub node</summary>
        /// <returns>The retrieved node with sub node</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public TreeNode GetNodeWithSubNode()
        {
            GetNode();
            node.Nodes.Clear();
            foreach (var sub in SubNodes)
            {
                var subNode = sub.GetNodeWithSubNode();
                node.Nodes.Add(subNode);
            }

            return node;
        }

        /// <summary>Performs clear</summary>
        /// <returns>The result of the clear</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileTreeNode Clear()
        {
            foreach (var sub in SubNodes)
            {
                sub.Clear();
            }

            if (node != null)
                node.Nodes.Clear();
            node = null;
            return this;
        }

        /// <summary>Performs clear all</summary>
        /// <returns>The result of the clear all</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileTreeNode ClearAll()
        {
            foreach (var sub in SubNodes)
            {
                sub.ClearAll();
            }

            if (node != null)
                node.Nodes.Clear();
            node = null;
            SubNodes.Clear();
            return this;
        }

        /// <summary>Sets node index</summary>
        /// <param name = "index">The index</param>
        /// <param name = "parentId">The ID of the parent</param>
        /// <returns>The result of the set node index</returns>
        /// <exception cref = "System.ArgumentNullException">Thrown when index is null</exception>
        /// <exception cref = "System.ArgumentNullException">Thrown when parentId is null</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileTreeNode SetNodeIndex(ref int index, int parentId)
        {
            this.Index = index++;
            this.ParentId = parentId;
            for (int i = 0; i < SubNodes.Count; i++)
            {
                var nod = SubNodes[i];
                nod.SetNodeIndex(ref index, this.Index);
                SubNodes[i] = nod;
            }

            return this;
        }

        /// <summary>Performs add sub node</summary>
        /// <param name = "node">The node</param>
        /// <returns>The result of the add sub node</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileTreeNode AddSubNode(FileTreeNode node)
        {
            SubNodes.Add(node);
            return this;
        }

        /// <summary>Determines whether equals path</summary>
        /// <param name = "path">The path collection</param>
        /// <returns>Whether equals path</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool IsEqualsPath(string path)
        {
            if (path == null)
                return false;
            return this.Path.MEquals(path);
        }

        /// <summary>Determines whether parent path</summary>
        /// <param name = "path">The path collection</param>
        /// <returns>Whether parent path</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool IsParentPath(string path)
        {
            if (path == null || this.Path.Equals(path))
                return false;
            return path.StartsWith(this.Path) && SubNodes.Count > 0;
        }

        /// <summary>Finds search file tree node</summary>
        /// <param name = "path">The path collection</param>
        /// <returns>The found search file tree node</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileTreeNode SearchFileTreeNode(string path)
        {
            if (path == null)
                return null;
            if (this.IsEqualsPath(path))
                return this;
            if (this.IsParentPath(path))
            {
                foreach (var sub in SubNodes)
                {
                    var n = sub.SearchFileTreeNode(path);
                    if (n != null)
                        return n;
                }
            }

            return null;
        }

        /// <summary>Sets sub nodes checked recursive</summary>
        /// <param name = "isChecked">Whether checked exists/is true</param>
        /// <exception cref = "System.ArgumentNullException">Thrown when isChecked is null</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void SetSubNodesCheckedRecursive(bool isChecked)
        {
            foreach (var child in this.SubNodes)
            {
                if (child.Checked != isChecked)
                    child.Checked = isChecked;
                child.SetSubNodesCheckedRecursive(isChecked);
            }
        }

        /// <summary>Gets: is file type</summary>
        public bool IsFileType => FileType == TreeFileType.File;
        /// <summary>Gets: is folder type</summary>
        public bool IsFolderType => FileType == TreeFileType.Folder;
        /// <summary>Gets: nodes</summary>
        public TreeNodeCollection Nodes => GetNode().Nodes;
    }

    /// <summary>Represents: FileExplorerDataSource</summary>
    public class FileExplorerDataSource : List<FileTreeNode>
    {
        /// <summary>Performs add new</summary>
        /// <param name = "item">The item</param>
        /// <returns>The result of the add new collection</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileExplorerDataSource AddNew(FileTreeNode item)
        {
            this.Add(item);
            return this;
        }
    }
}