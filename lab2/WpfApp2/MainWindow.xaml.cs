using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace StreamDecoratorDemo
{
    // Интерфейс для декорирования потока
    public interface IDecoratedStream
    {
        void Write(byte[] buffer, int offset, int count);
        string Read(int count);
    }

    // Декоратор для получения последних пяти символов из потока
    public class LastFiveCharactersStreamDecorator : IDecoratedStream
    {
        private readonly MemoryStream _stream;
        private readonly StringBuilder _builder;

        public LastFiveCharactersStreamDecorator(Stream stream)
        {
            _stream = new MemoryStream();
            _builder = new StringBuilder();

            // Сохраняем данные из переданного потока во внутренний MemoryStream
            stream.CopyTo(_stream);
            _stream.Position = 0;

            // Считываем данные из потока в StringBuilder
            using (var reader = new StreamReader(stream))
            {
                _builder.Append(reader.ReadToEnd());
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            // Добавляем новые данные в StringBuilder
            _builder.Append(Encoding.UTF8.GetString(buffer, offset, count));

            // Если длина данных превышает пять символов, оставляем только последние пять
            if (_builder.Length > 5)
            {
                _builder.Remove(0, _builder.Length - 5);
            }

            // Записываем актуальные данные обратно в MemoryStream
            _stream.SetLength(0);
            var bytes = Encoding.UTF8.GetBytes(_builder.ToString());
            _stream.Write(bytes, 0, bytes.Length);
        }

        public string Read(int count)
        {
            long currentPosition = _stream.Position;
            byte[] buffer;

            // Определяем количество символов, которые мы можем прочитать
            long availableCharacters = _stream.Length - currentPosition;
            int length = (int)Math.Min(count, availableCharacters);

            // Читаем данные из текущей позиции
            buffer = new byte[length];
            _stream.Read(buffer, 0, length);

            // Возвращаем указатель обратно на исходное место
            _stream.Seek(currentPosition, SeekOrigin.Begin);

            // Если символов меньше пяти, добавляем символы из предыдущей записи
            if (length < 5)
            {
                byte[] prevBuffer = new byte[5 - length];
                _stream.Seek(-5, SeekOrigin.Current);
                _stream.Read(prevBuffer, 0, 5 - length);
                buffer = prevBuffer.Concat(buffer).ToArray();
                _stream.Seek(currentPosition, SeekOrigin.Begin);
            }

            return Encoding.UTF8.GetString(buffer);
        }
    }

    public partial class MainWindow : Window
    {
        private readonly IDecoratedStream _fileStream;
        private readonly IDecoratedStream _memoryStream;
        private readonly IDecoratedStream _bufferStream;

        public MainWindow()
        {
            InitializeComponent();

            _fileStream = new LastFiveCharactersStreamDecorator(new FileStream("test.txt", FileMode.Create, FileAccess.ReadWrite));
            _memoryStream = new LastFiveCharactersStreamDecorator(new MemoryStream());
            _bufferStream = new LastFiveCharactersStreamDecorator(new BufferedStream(new MemoryStream()));
        }

        private void WriteToFileStream_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string data = inputTextBox.Text;
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                // Записываем данные в FileStream
                _fileStream.Write(bytes, 0, bytes.Length);

                // Очищаем текстовое поле после записи
                inputTextBox.Text = "";

                MessageBox.Show("Data written to FileStream.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to FileStream: {ex.Message}");
            }
        }

        private void WriteToMemoryStream_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string data = inputTextBox.Text;
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                _memoryStream.Write(bytes, 0, bytes.Length);
                MessageBox.Show("Data written to MemoryStream.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to MemoryStream: {ex.Message}");
            }
        }

        private void WriteToBufferStream_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string data = inputTextBox.Text;
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                _bufferStream.Write(bytes, 0, bytes.Length);
                MessageBox.Show("Data written to BufferStream.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to BufferStream: {ex.Message}");
            }
        }

        private void GetLastFiveCharacters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string output = $"FileStream: {_fileStream.Read(5)}\n" +
                                $"MemoryStream: {_memoryStream.Read(5)}\n" +
                                $"BufferStream: {_bufferStream.Read(5)}";
                outputTextBox.Text = output;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting last five characters: {ex.Message}");
            }
        }
    }
}
