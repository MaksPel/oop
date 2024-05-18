using System;
using System.Windows;

namespace BinaryTreeCollection
{
    public partial class MainWindow : Window
    {
        private BinaryTree<int> binaryTree;

        public MainWindow()
        {
            InitializeComponent();

            binaryTree = new BinaryTree<int>();

            // Добавляем несколько элементов для демонстрации
            binaryTree.Add(50);
            binaryTree.Add(30);
            binaryTree.Add(70);
            binaryTree.Add(20);
            binaryTree.Add(40);
            binaryTree.Add(60);
            binaryTree.Add(80);

            // Отображаем элементы в ListBox
            foreach (var item in binaryTree)
            {
                listBoxItems.Items.Add(item);
            }
        }

        private void AddRandomNumber_Click(object sender, RoutedEventArgs e)
        {
            // Генерируем случайное число и добавляем его в дерево
            Random random = new Random();
            int randomNumber = random.Next(1, 100); // Генерируем число от 1 до 100
            binaryTree.Add(randomNumber);

            // Очищаем ListBox и обновляем его с новыми элементами
            UpdateListBox();
        }

        private void RemoveSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли какой-либо элемент в ListBox
            if (listBoxItems.SelectedItem != null)
            {
                // Удаляем выбранный элемент из коллекции
                binaryTree.Remove((int)listBoxItems.SelectedItem);

                // Очищаем ListBox и обновляем его с новыми элементами
                UpdateListBox();
            }
            else
            {
                MessageBox.Show("Please select an item to remove.", "No Item Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateListBox()
        {
            listBoxItems.Items.Clear();
            foreach (var item in binaryTree)
            {
                listBoxItems.Items.Add(item);
            }
        }
    }
}
