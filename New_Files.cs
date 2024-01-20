using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Windows.Forms;
namespace WindowsFormsApp9
{
    public partial class New_Files : Form
    {
        public New_Files()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 menu_pdf = new Form1();
            this.Close();
        }
        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {

            if (textBox1.Text == "Введите имя PDF файла")
            {
                textBox1.Text = "";
                textBox1.Text = textBox1.Text;
                //name_path = textBox1.Text + ".pdf";
                //PdfWriter.GetInstance(doc, new FileStream(file_path += name_path, FileMode.Create));

            }
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Введите имя PDF файла";
            }
        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (richTextBox1.Text == "Введите текст")
            {
                richTextBox1.Text = "";
                richTextBox1.Text = richTextBox1.Text;
            }
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                richTextBox1.Text = "Введите текст";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file_path = @"C:\Users\Пользователь\Desktop\задание 1 файловый pdf менеджер\корневая папка";
            string name_path = textBox1.Text;
            string fullFilePath = Path.Combine(file_path, name_path + ".pdf"); // Составляем полный путь к файлу PDF
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(fullFilePath, FileMode.Create));
            doc.Open();
            doc.Add(new Paragraph(richTextBox1.Text));
            doc.Close();
        }
    }
}
