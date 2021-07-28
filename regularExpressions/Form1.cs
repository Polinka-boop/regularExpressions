using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace regularExpressions
{
    public partial class Form1 : Form
    {
        bool isFileNew;
        string currentPath;
        public Form1()
        {
            InitializeComponent();
            isFileNew = true;
            richTextBoxOutput.ReadOnly = true;

            ToolTip t1 = new ToolTip();
            t1.SetToolTip(buttonFileNew, "Создать");
            ToolTip t2 = new ToolTip();
            t2.SetToolTip(buttonOpen, "Открыть");
            ToolTip t3 = new ToolTip();
            t3.SetToolTip(buttonSave, "Сохранить");
            ToolTip t4 = new ToolTip();
            t4.SetToolTip(buttonBack, "Отменить");
            ToolTip t5 = new ToolTip();
            t5.SetToolTip(buttonForward, "Повторить");
            ToolTip t6 = new ToolTip();
            t6.SetToolTip(buttonCopy, "Копировать");
            ToolTip t7 = new ToolTip();
            t7.SetToolTip(buttonCut, "Вырезать");
            ToolTip t8 = new ToolTip();
            t8.SetToolTip(buttonPaste, "Вставить");
            ToolTip t9 = new ToolTip();
            t9.SetToolTip(buttonDeleteAll, "Удалить все");
            ToolTip t10 = new ToolTip();
            t10.SetToolTip(buttonPlay, "Пуск");
        }

        //СОХРАНИТЬ КАК
        private void SaveAs()
        {
            if (isFileNew)
                saveFileDialog1.FileName = "new file";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Save(saveFileDialog1.FileName);
            }

        }

        //СОХРАНИТЬ
        private void Save(string path)
        {
            try
            {
                var writer = new System.IO.StreamWriter(path, false, System.Text.Encoding.Default);
                writer.Write(richTextBoxInput.Text);
                writer.Close();
            }
            catch (Exception ex)
            { // отчет о других ошибках
                MessageBox.Show(ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Create()
        {
            isFileNew = true;
            richTextBoxInput.Clear();
        }

        //ОТКРЫТЬ
        private void Open()
        {
            isFileNew = false;
            //не открыт ли уже файл?
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    currentPath = filePath;
                    var fileStream = openFileDialog.OpenFile();
                    using (StreamReader reader = new StreamReader(fileStream, System.Text.Encoding.Default))
                    {
                        fileContent = reader.ReadToEnd();
                        richTextBoxInput.Text = fileContent;
                    }
                }
            }
        }

        //ВЫДЕЛИТЬ
        private void Select()
        {
            richTextBoxInput.Focus();
            richTextBoxInput.SelectAll();
        }

        //КОПИРОВАТЬ
        private void Copy()
        {
            if (richTextBoxInput.SelectionLength > 0)
            {
                richTextBoxInput.Copy();
            }
        }

        //ВЫРЕЗАТЬ
        private void Cut()
        {
            if (richTextBoxInput.SelectionLength > 0)
            {
                richTextBoxInput.Cut();
            }
        }

        //ВСТАВИТЬ
        private void Paste()
        {
            richTextBoxInput.Paste();
        }

        //УДАЛИТЬ
        private void Delete()
        {
            if (richTextBoxInput.SelectionLength > 0)
            {
                richTextBoxInput.SelectedText = "";
            }
        }

        //ЗАКРЫТЬ
        private bool CloseFile()
        {

            if (richTextBoxInput.Text == string.Empty)
                return true;
            System.Windows.Forms.DialogResult answer = MessageBox.Show("Сохранить перед закрытием?", "Compiler",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (answer == System.Windows.Forms.DialogResult.Yes)
            {
                if (isFileNew)
                    SaveAs();
                else
                    Save(currentPath);
            }

            if (answer == System.Windows.Forms.DialogResult.Cancel)
                return false;

            return true;
        }

        private void Redo()
        {
            if (richTextBoxInput.CanRedo == true)
            {
                richTextBoxInput.Redo();
            }
        }

        private void Undo()
        {
            if (richTextBoxInput.CanUndo == true)
            {
                richTextBoxInput.Undo();
            }
        }

        private void Reg()
        {
            string pattern = "(((?<=\\s)|^)[А-Я]{1}\\.\\s?[А-Я]{1}\\.\\s[А-Я]{1}[а-я]*(\\-[А-Я]{1}[а-я]*)?(\\s|(?=\\W)|$))|(((?<=\\s)|^)[А-Я]{1}[а-я]*(\\-[А-Я]{1}[а-я]+)?\\s[А-Я]{1}\\.\\s?[А-Я]{1}\\.(\\s|(?=\\W)|$))";
            richTextBoxOutput.Text = "Найденные совпадения ФИО:\n";
            foreach (Match match in Regex.Matches(richTextBoxInput.Text, pattern))
                richTextBoxOutput.Text = richTextBoxOutput.Text + "\n" + (match.Value);
        }

        private void ExampleReg()
        {
            richTextBoxInput.Text = "Данная программа правильно выводит написанные ФИО, а всякие петров И. И., Сидоров п. д. и ПетРов В. А., а тем более Ермаков И. В.пппп выводить не будет. Так же правильно выводятся двойные фамилии: Салтыков-Щедрин М. Е., Кузнецов-петров В. В., Гуляев-Смирнов П. в.. Также это работает и наоборот, когда сначала идут инициалы: В. В. Тягунова, А. А. Майнгардт и Ю. О. Оршулевич, а. д. Киль, Р. К. стародубов.";
        }

        /////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
            Create();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFileNew)
                SaveAs();
            else
                Save(currentPath);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void повторитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Select();
        }

        private void тестовыйПримерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExampleReg();
        }

        private void пускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reg();
        }

        private void buttonFileNew_Click(object sender, EventArgs e)
        {
            CloseFile();
            Create();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            CloseFile();
            Open();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (isFileNew)
                SaveAs();
            else
                Save(currentPath);
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            Reg();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void buttonCut_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            Select();
            Delete();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CloseFile())
                e.Cancel = true;
        }
    }
}
