using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using IGAE_GUI.Types;
using IGAE_GUI.Utils;

namespace IGAE_GUI.IGZ
{
    public partial class IGZ_GeneralForm : Form {
        TreeNode fixups = new("Fixups");
        TreeNode objects = new("Object List");
        IGZ_File _igz;

        private Dictionary<TreeNode, igObject> igObjectMap = new();
        List<TreeNode> filtered = new();
        List<TreeNode> unfiltered = new();

        public IGZ_GeneralForm(IGZ_File igz) {
            InitializeComponent();

            _igz = igz;

            if (_igz.ebr.BaseStream.GetType() == typeof(FileStream)) {
                btnSaveIGZ.Enabled = false;
                btnSaveIGZ.Visible = false;
            }

            if (_igz.fixups != null) Console.WriteLine("Fixups exist");
            foreach (IGZ_Fixup fixup in _igz.fixups) {
                if (igz.ebr._endianness == StreamHelper.Endianness.Big) {
                    fixups.Nodes.Add(
                        System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(fixup.magicNumber).Reverse()
                            .ToArray()));
                }
                else {
                    fixups.Nodes.Add(System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(fixup.magicNumber)));
                }

                switch (fixup.magicNumber) {
                    case 0x544D4554:
                        foreach (string item in (fixup as IGZ_TMET).typeNames) {
                            fixups.Nodes[fixups.Nodes.Count - 1].Nodes.Add(item);
                        }

                        break;
                    case 0x54535452:
                        foreach (string item in (fixup as IGZ_TSTR).strings) {
                            fixups.Nodes[fixups.Nodes.Count - 1].Nodes.Add(item);
                        }

                        break;
                    case 0x54444550:
                        foreach (string item in (fixup as IGZ_TDEP).dependancies) {
                            fixups.Nodes[fixups.Nodes.Count - 1].Nodes.Add(item);
                        }

                        break;
                    case 0x45584944:
                        IGZ_EXID exid = fixup as IGZ_EXID;
                        for (uint i = 0; i < exid.count; i++) {
                            string hashName = string.Empty;
                            string type;

                            if (Enum.IsDefined(typeof(IGZ_TextureFormat), (IGZ_TextureFormat)exid.hashes[i])) {
                                hashName = ((IGZ_TextureFormat)exid.hashes[i]).ToString().Split('.').Last();
                                if (hashName.EndsWith("_old")) {
                                    hashName = hashName.Substring(0, 4);
                                }
                            }
                            else {
                                hashName = "Unknown";
                            }

                            type = exid.types[i].ToString("X8");

                            fixups.Nodes[fixups.Nodes.Count - 1].Nodes
                                .Add($"{type}: {exid.hashes[i].ToString("X08")} ({hashName})");
                        }

                        break;
                    case 0x45584E4D:
                        IGZ_EXNM exnm = fixup as IGZ_EXNM;
                        IGZ_TSTR tstr = _igz.fixups.First(x => x.magicNumber == 0x54535452) as IGZ_TSTR;
                        for (uint i = 0; i < exnm.count; i++) {
                            fixups.Nodes[fixups.Nodes.Count - 1].Nodes
                                .Add($"{tstr.strings[exnm.types[i]]}: {tstr.strings[exnm.names[i]]}");
                        }

                        break;
                    case 0x52565442:
                        IGZ_RVTB rvtb = fixup as IGZ_RVTB;
                        for (uint i = 0; i < rvtb.count; i++) {
                            fixups.Nodes[fixups.Nodes.Count - 1].Nodes.Add($"{rvtb.offsets[i].ToString("X08")}");
                        }

                        break;
                    case 0x52545352:
                        IGZ_RSTR rstr = fixup as IGZ_RSTR;
                        for (uint i = 0; i < rstr.count; i++) {
                            fixups.Nodes[fixups.Nodes.Count - 1].Nodes.Add($"{rstr.offsets[i]:X08}");
                        }

                        break;
                    case 0x544D484E:
                        IGZ_TMHN tmhn = fixup as IGZ_TMHN;
                        for (uint i = 0; i < tmhn.count; i++) {
                            fixups.Nodes[fixups.Nodes.Count - 1].Nodes
                                .Add($"{tmhn.sizes[i].ToString("X08")}: {tmhn.offsets[i].ToString("X08")}");
                        }

                        break;
                    case 0x4D54535A:
                        IGZ_MTSZ mtsz = fixup as IGZ_MTSZ;
                        for (uint i = 0; i < mtsz.count; i++) {
                            fixups.Nodes[fixups.Nodes.Count - 1].Nodes.Add(mtsz.metaSizes[i].ToString("X08"));
                        }

                        break;
                }
            }

            AddObject(igz.objectList._objects.ToArray());

            treeItems.Nodes.Add(fixups);
            treeItems.Nodes.Add(objects);
            objects.Nodes.AddRange(filtered.ToArray());

            Config config = Config.Read();

            if (config.darkMode) {
                foreach (Control control in Controls) {
                    Themes.SetControlToDark(control);
                }

                Themes.SetWindowControlToDark(this);
            }
            else {
                foreach (Control control in Controls) {
                    Themes.SetControlToLight(control);
                }

                Themes.SetControlToLight(this);
            }
        }

        void AddObject(igObject[] objs) {
            IGZ_RVTB rvtb = _igz.fixups.First(x => x.magicNumber == 0x52565442) as IGZ_RVTB;
            for (int i = 0; i < objs.Length; i++) {
                string objectType;
                var igObject = objs[i];

                if (_igz.version <= 0x09) {
                    IGZ_TMET types = _igz.fixups.First(x => x.magicNumber == 0x544D4554) as IGZ_TMET;
                    try {
                        objectType = types.typeNames[igObject.name];
                    }
                    catch (Exception) {
                        objectType = objs[(int)i].name.ToString("X08");
                    }
                }
                else {
                    IGZ_TSTR strings = _igz.fixups.First(x => x.magicNumber == 0x54535452) as IGZ_TSTR;
                    try {
                        objectType = strings.strings[igObject.name];
                    }
                    catch (Exception) {
                        objectType = objs[i].name.ToString("X08");
                    }
                }
                //potentialParentNode = objects.Nodes.Add($"{i.ToString("X04")} : {(rvtb.offsets[i+1]).ToString("X08")} : {objs[i].length.ToString("X08")} => {objectType}");

                long DeserializeOffset(int offset) {
                if(_igz.version <= 0x06) return (_igz.descriptors[(offset >> 0x18) + 1].offset + (offset & 0x00FFFFFF));
                                    return (_igz.descriptors[(offset >> 0x1B) + 1].offset + (offset & 0x07FFFFFF));
                }

                var name = igObject.offset.ToString("X04") + ": " + objectType;
                if (igObject is igImage2) {
                    try {
                        _igz.ebr.BaseStream.Seek(igObject.offset + 0x8, SeekOrigin.Begin); // seek to igImage 2 + 8
                        var namePointer =
                            DeserializeOffset((int)_igz.ebr.ReadUInt32()); // read a uint32 and deserialize that
                        _igz.ebr.BaseStream.Seek(namePointer, SeekOrigin.Begin); // seek to that offset
                        var tmpName = igObject.offset.ToString("X04") + ": " + _igz.ebr.ReadString(); // read a string
                        if (tmpName.Length < 5)
                        {
                        }
                        // Console.WriteLine(tmpName + " is probably a bad name. falling back to offsets");
                        else name = tmpName;
                    }
                    catch (Exception) {
                        // ignored
                    }
                }

                var treeNode = new TreeNode(name);
                igObjectMap.Add(treeNode, igObject);
                unfiltered.Add(treeNode);
                if (objectType == "igImage2") filtered.Add(treeNode);

                //Console.WriteLine(i.ToString("X04") + " : " + objs[i].children.Count);
                for (uint j = 0; j < igObject.children.Count; j++) {
                    AddObject(igObject.children.ToArray());
                }
            }
        }

        void SelectionChange(object sender, TreeViewEventArgs e) {
            if (treeItems.SelectedNode.Parent == objects) {
                var igObject = igObjectMap[treeItems.SelectedNode];

                if (igObject.GetType() == typeof(igImage2)) {
                    Console.WriteLine("igImage2 Selected");
                    MemoryStream msImage = new MemoryStream();
                    (igObject as igImage2).Extract(msImage);
                    if (msImage.Length == 0) return;
                    msImage.Seek(0x00, SeekOrigin.Begin);
                    pbTexturePreview.Image = TextureHelper.BitmapFromDDS(msImage);
                    pbTexturePreview.Visible = true;
                    btnTextureExtract.Visible = true;
                    btnTextureReplace.Visible = true;
                    msImage.Close();
                }
                else {
                    pbTexturePreview.Image = null;
                    pbTexturePreview.Visible = false;
                    btnTextureExtract.Visible = false;
                    btnTextureReplace.Visible = false;
                }
            }
            else {
                pbTexturePreview.Image = null;
                pbTexturePreview.Visible = false;
                btnTextureExtract.Visible = false;
                btnTextureReplace.Visible = false;
            }
        }

        void ExportTexture(object sender, EventArgs e) {
            using (SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.Filter = "DirectDraw Surface Files (*.dds)|*.dds|All Files (*.*)|*.*";
                sfd.RestoreDirectory = true;
                if (treeItems.SelectedNode.FullPath != "")
                    sfd.FileName =
                        treeItems.SelectedNode.FullPath.Replace("\\", "/")[
                                (treeItems.SelectedNode.FullPath.LastIndexOf("/", StringComparison.Ordinal) + 1)..]
                            .Replace(".png", ".dds");
                if (sfd.FileName.Contains("#"))
                {
                    string[] parts = sfd.FileName.Split("#");
                    sfd.FileName = parts[parts.Length - 2];
                }
                if (sfd.ShowDialog() == DialogResult.OK) {
                    FileStream ofs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.ReadWrite);
                    (igObjectMap[treeItems.SelectedNode] as igImage2)?.Extract(ofs);
                    ofs.Close();
                }
            }
        }

        private void ImportTexture(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.Filter = "All Files (*.*)|*.*|DirectDraw Surface Files (*.dds)|*.dds";

            if (ofd.ShowDialog() != DialogResult.OK) return;
            var ifs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.ReadWrite);
            (igObjectMap[treeItems.SelectedNode] as igImage2)!.Replace(ifs);
        }
        void ExportDDSTexture(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "DirectDraw Surface Files (*.dds)|*.dds|All Files (*.*)|*.*";
                sfd.RestoreDirectory = true;
                if (treeItems.SelectedNode.FullPath != "")
                    sfd.FileName =
                        treeItems.SelectedNode.FullPath.Replace("\\", "/")[
                                (treeItems.SelectedNode.FullPath.LastIndexOf("/", StringComparison.Ordinal) + 1)..]
                            .Replace(".png", ".dds");
                if (sfd.FileName.Contains("#"))
                {
                    string[] parts = sfd.FileName.Split("#");
                    sfd.FileName = parts[parts.Length - 2];
                }
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    FileStream ofs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.ReadWrite);
                    (igObjectMap[treeItems.SelectedNode] as igImage2)?.ExtractDDS(ofs);
                    ofs.Close();
                }
            }
        }

        private void ImportDDSTexture(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.Filter = "DirectDraw Surface Files (*.dds)|*.dds";

            if (ofd.ShowDialog() != DialogResult.OK) return;
            var ifs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.ReadWrite);
            (igObjectMap[treeItems.SelectedNode] as igImage2)!.ReplaceDDS(ifs);
        }

        private void ExportAllObjects(object sender, EventArgs e) {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            // FIXME: a exporting interface would make this work better but whatever. The tool will be dead sooner rather than later hopefully anyways

            var exportDir = fbd.SelectedPath;
            foreach (var key in unfiltered) {
                var nodeName = key.ToString().Substring("TreeNode: ".Length);
                var fileName = nodeName.Replace("\\", "/")[(nodeName.LastIndexOf("/", StringComparison.Ordinal) + 1)..];
                if (fileName.Contains("#"))
                {
                    string[] parts = fileName.Split("#");
                    fileName = parts[parts.Length - 2];
                    fileName += ".dds";
                }
                var fullPath = exportDir + "/" + fileName;

                // FIXME: the only exportable thing from igae is textures. R.I.P igae Hope the new tool does well
                try {
                    if (igObjectMap[key] is not igImage2 image2) continue;
                    var ofs = new FileStream(fullPath.Replace(".png", ".dds"), FileMode.Create, FileAccess.Write);
                    image2.Extract(ofs);
                    ofs.Close();
                }
                catch (Exception) {
                    var message = "Exception Caught when exporting " + fileName;
                    MessageBox.Show(message, "Export All Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            MessageBox.Show("Exported all objects", "Export All Progress", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        void Save(object sender, EventArgs e) {
            using (SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.Filter = "IGZ Files (*.igz)|*.igz;level.bld;*.pak;*.lang|All Files (*.*)|*.*";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK) {
                    FileStream ofs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.ReadWrite);
                    _igz.ebr.BaseStream.Seek(0x00, SeekOrigin.Begin);
                    _igz.ebr.BaseStream.CopyTo(ofs);
                    ofs.Flush();
                    ofs.Close();
                }
            }
        }

        void ChangeFilter(object sender, EventArgs e) {
            objects.Nodes.Clear();
            if (cbFilterImages.Checked) {
                objects.Nodes.AddRange(filtered.ToArray());
            }
            else {
                objects.Nodes.AddRange(unfiltered.ToArray());
            }
        }

        void Closing(object sender, EventArgs e) {
            _igz.ebr.Close();
        }
    }
}