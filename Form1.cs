using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PopulateTreeView();
            new_file();
        }
        private void new_file()
        {
            string folder = @"C:\Users\Пользователь\Desktop\задание 1 файловый pdf менеджер\корневая папка";

            Directory.CreateDirectory(folder); // создание папки
        }
        private void PopulateTreeView()
        {
            TreeNode rootNode;
            DirectoryInfo info = new DirectoryInfo(@"../..");
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                treeView1.Nodes.Add(rootNode);
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs,
            TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node; // Сохраняем выбранный узел в переменной newSelected

            listView1.Items.Clear();
            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag; // Получаем информацию о директории из свойства Tag выбранного узла

            ListViewItem.ListViewSubItem[] subItems; // Создаем переменные для подэлементов ListViewItem
            ListViewItem item = null;
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "File"),
             new ListViewItem.ListViewSubItem(item,
                file.LastAccessTime.ToShortDateString())};

                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                if (file.Extension.ToLower() == ".pdf") // Проверяем, что файл имеет расширение .pdf
                {
                    item = new ListViewItem(file.Name, 2); // Создаем новый элемент с именем файла и иконкой "2" (или любой другой подходящей иконкой)
                    subItems = new ListViewItem.ListViewSubItem[]
                    {
                new ListViewItem.ListViewSubItem(item, "PDF"), // Добавляем подэлемент с типом "PDF"
                new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString()) // Добавляем подэлемент с датой последнего доступа
                    };
                    item.SubItems.AddRange(subItems); // Добавляем все подэлементы к элементу
                }
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize); // Автоматически подстраиваем размер колонок по содержимому
        }

        private void Copy_file(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder); //Создает все каталоги по заданному пути.
            }
            string folder = @"C:\Users\Пользователь\Desktop\задание 1 файловый pdf менеджер\корневая папка";
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file); // Возвращает имя и расширение файла из пути к файлу, представленного диапазоном символов только для чтения
                string dest = Path.Combine(folder, name); // Объединяет массив строк в путь.
                File.Copy(file, dest);
            }

            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string Folder in folders)
            {
                string name = Path.GetFileName(Folder);
                string dest = Path.Combine(folder, name);
                Copy_file(Folder, dest);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folder = @"C:\Users\Пользователь\Desktop\задание 1 файловый pdf менеджер\корневая папка";
            DirectoryInfo di = new DirectoryInfo(folder); //
            foreach (FileInfo file in di.GetFiles()) //
            {
                file.Delete();
            }
        }
        private string selectedFilePath;
        private bool IsPdfFile(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath).ToLower();
            return fileExtension == ".pdf";
        }


        private void axAcroPDF1_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedFilePath))
            {
                axAcroPDF1.LoadFile(selectedFilePath); // Используем сохраненный путь к файлу
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; ;
            }
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                if (Directory.Exists(file)) // Проверяем, что перетаскиваемый элемент является папкой
                {
                    string folder = @"C:\Users\Пользователь\Desktop\задание 1 файловый pdf менеджер\корневая папка";
                    Copy_file(file, folder);
                }
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e) // обработчик события при нажатии
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedFile = listView1.SelectedItems[0].Text; // Получаем имя выбранного файла из первого выбранного элемента в listView1
                string folderPath = @"C:\Users\Пользователь\Desktop\задание 1 файловый pdf менеджер\корневая папка";
                string filePath = Path.Combine(folderPath, selectedFile); // Создаем полный путь к выбранному файлу

                if (File.Exists(filePath) && Path.GetExtension(filePath).ToLower() == ".pdf")
                {
                    axAcroPDF1.src = filePath; // Загружаем выбранный PDF-файл в axAcroPDF1
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) // проверка выбранли файл
            {
                string selectedFile = listView1.SelectedItems[0].Text; // Получаем имя выбранного файла из первого выбранного элемента в listView1
                string folderPath = @"C:\Users\Пользователь\Desktop\задание 1 файловый pdf менеджер\корневая папка";
                string filePath = Path.Combine(folderPath, selectedFile); // Создаем полный путь к выбранному файлу
                DateTime date = File.GetCreationTime(filePath); // получает дату создания  с помощью File.GetCreationTime(filePath)
                FileInfo size_file = new FileInfo(filePath);
                long fileSizeInBytes = size_file.Length; // получает размер файла
                FileAttributes аttributes_file = size_file.Attributes; // получает атрибуты файла
                string print_properties = $"Дата: {date},   Размер файла: {fileSizeInBytes},   Тип файла: PDF,   Атрибуты:{аttributes_file}";
                label1.Text = print_properties;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            New_Files new_files_form = new New_Files();// создание формы
            new_files_form.Show();
        }
    }
}
