using System.Windows;

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        public Book Book { get; private set; }

        public BookWindow(Book book)
        {
            InitializeComponent();
            Book = book;
            this.DataContext = Book;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
