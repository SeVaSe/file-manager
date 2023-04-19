using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using EnvDTE;
using System.Drawing.Printing;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Printing;


namespace fail_manage
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Btn_Unic_Click(object sender, RoutedEventArgs e)
        {
            using (StreamReader reader = new StreamReader(@"D:/VISUAL_STUDIO_/Vs_Project/C#/PR_TITOV_/pr6/fail_manage/testik.txt", Encoding.UTF8))
            {
                string content = TxtBox_Input.Text;


                var wordPad = new System.Diagnostics.Process();
                wordPad.StartInfo.FileName = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";

                string newFilePath = System.IO.Path.GetTempFileName();
                File.WriteAllText(newFilePath, content, Encoding.UTF8);


                wordPad.StartInfo.Arguments = "\"" + newFilePath + "\"";

                wordPad.Start();
                wordPad.WaitForExit();
                // Удаляем временный файл
                File.Delete(newFilePath);
            }
        }

        private void Btn_Win_Click(object sender, RoutedEventArgs e)
        {
            using (StreamReader reader = new StreamReader(@"D:/VISUAL_STUDIO_/Vs_Project/C#/PR_TITOV_/pr6/fail_manage/testik.txt", Encoding.GetEncoding(1251)))
            {
                string utf8Content = TxtBox_Input.Text;
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(utf8Content);
                string win1251Content = Encoding.GetEncoding(1251).GetString(utf8Bytes);


                var wordPad = new System.Diagnostics.Process();
                wordPad.StartInfo.FileName = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";

                string newFilePath = System.IO.Path.GetTempFileName();
                File.WriteAllText(newFilePath, win1251Content, Encoding.GetEncoding(1251));


                wordPad.StartInfo.Arguments = $"\"{newFilePath}\"";

                wordPad.Start();
                wordPad.WaitForExit();
                // Удаляем временный файл
                File.Delete(newFilePath);
            }
        }

        private void Btn_RTF_Click(object sender, RoutedEventArgs e)
        {
            using (StreamReader reader = new StreamReader(@"D:/VISUAL_STUDIO_/Vs_Project/C#/PR_TITOV_/pr6/fail_manage/testik.txt", Encoding.Default))
            {
                string utfContent = TxtBox_Input.Text;
                byte[] utfBytes = Encoding.UTF8.GetBytes(utfContent);
                string win1251Content = Encoding.GetEncoding(1251).GetString(utfBytes);
                string rtfContent = "{\\rtf1\\ansi\\deff0{\\fonttbl{\\f0\\fcharset0 Times New Roman;}}\n\\f0\\fs24 " + win1251Content + "\n}";


                var wordPad = new System.Diagnostics.Process();
                wordPad.StartInfo.FileName = @"C:\Program Files\Windows NT\Accessories\wordpad.exe";

                string newFilePath = System.IO.Path.GetTempFileName();
                File.WriteAllText(newFilePath, rtfContent, Encoding.GetEncoding(1251));


                wordPad.StartInfo.Arguments = $"\"{newFilePath}\"";

                wordPad.Start();
                wordPad.WaitForExit();
                File.Delete(newFilePath);
            }
        }

        private void Btn_BINAR_Click(object sender, RoutedEventArgs e)
        {
            using (var reader = new BinaryReader(File.Open(@"D:/VISUAL_STUDIO_/Vs_Project/C#/PR_TITOV_/pr6/fail_manage/testik.txt", FileMode.Open)))
            {
                string content = TxtBox_Input.Text.Replace(" ", ""); // удаляем все пробелы из введенной строки
                if (content.Length % 2 != 0)
                {
                    // выводим сообщение об ошибке и прерываем выполнение метода
                    MessageBox.Show("Некорректное количество символов. Введите шестнадцатеричные значения байтов без пробелов.");
                    return;
                }
                // продолжаем выполнение метода, если введенная строка имеет корректный формат
                byte[] bytes = new byte[content.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    string hex = new string(new char[] { content[i * 2], content[i * 2 + 1] });
                    bytes[i] = Convert.ToByte(hex, 16);
                }
                // использование массива bytes
            }
        }

        private void Btn_SAVE_AS_Click(object sender, RoutedEventArgs e)
        {
            string content = TxtBox_Input.Text;

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|RTF files (*.rtf)|*.rtf";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                Encoding encoding = null;
                switch (saveFileDialog.FilterIndex)
                {
                    case 1: //Unic
                        encoding = Encoding.UTF8;
                        break;
                    case 2: //Win1251
                        encoding = Encoding.GetEncoding(1251);
                        break;
                    case 3:
                        var doc = new FlowDocument(new Paragraph(new Run(content)));
                        using (var fileStream = new FileStream(fileName, FileMode.Create))
                        {
                            var textRange = new System.Windows.Documents.TextRange(doc.ContentStart, doc.ContentEnd);
                            textRange.Save(fileStream, DataFormats.Rtf);
                        }
                        return;
                }

                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    byte[] bytes = encoding.GetBytes(content);
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        private void Btn_SAVE_BIN_Click(object sender, RoutedEventArgs e)
        {
            string content = TxtBox_Input.Text;

            // Создание экземпляра BinaryWriter для записи в бинарный файл
            using (BinaryWriter write = new BinaryWriter(File.Open("save_binary_file.bin", FileMode.Create)))
            {
                // Запись текста в бинарный файл
                write.Write(content);
            }
            MessageBox.Show("Текст успешно сохранен в бинарный файл.");
        }

        private void Btn_PRINT_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            // Открываем диалог печати
            if (printDialog.ShowDialog() == true)
            {
                // Получаем содержимое TextBox
                string textToPrint = TxtBox_Input.Text;

                // Создаем объект класса FlowDocument
                FlowDocument doc = new FlowDocument(new Paragraph(new Run(textToPrint)));

                // Устанавливаем размер страницы
                doc.PageWidth = printDialog.PrintableAreaWidth;
                doc.PageHeight = printDialog.PrintableAreaHeight;

                // Создаем объект класса IDocumentPaginatorSource на основе FlowDocument
                IDocumentPaginatorSource paginatorSource = doc;

                // Устанавливаем настройки печати
                printDialog.PrintTicket.PageOrientation = PageOrientation.Portrait;
                printDialog.PrintTicket.PageScalingFactor = 100;
                printDialog.PrintTicket.CopyCount = 1;

                // Печатаем документ
                printDialog.PrintDocument(paginatorSource.DocumentPaginator, "Printing Flow Document...");
            }

        }
    }
}
