﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using Microsoft.Win32;
using System.Runtime.CompilerServices;
using System.IO;

namespace Lab4
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        RelayCommand saveDb;
        RelayCommand showHelp;
        RelayCommand exit;
        IEnumerable<Book> books;
 
        public IEnumerable<Book> Books
        {
            get { return books; }
            set
            {
                books = value;
                OnPropertyChanged("Books");
            }
        }
 
        public ApplicationViewModel()
        {
            db = new ApplicationContext();
            db.Books.Load();
            Books = db.Books.Local.ToBindingList();
        }
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      BookWindow bookWindow = new BookWindow(new Book());
                      if (bookWindow.ShowDialog() == true)
                      {
                          Book book = bookWindow.Book;
                          db.Books.Add(book);
                          db.SaveChanges();
                      }
                  }));
            }
        }
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      Book book = selectedItem as Book;
 
                      Book vm = new Book()
                      {
                          Id = book.Id,
                          Author = book.Author,
                          Year = book.Year,
                          Title = book.Title
                      };
                      BookWindow bookWindow = new BookWindow(vm);
 
 
                      if (bookWindow.ShowDialog() == true)
                      {
                          book = db.Books.Find(bookWindow.Book.Id);
                          if (book != null)
                          {
                              book.Author = bookWindow.Book.Author;
                              book.Title = bookWindow.Book.Title;
                              book.Year = bookWindow.Book.Year;
                              db.Entry(book).State = EntityState.Modified;
                              db.SaveChanges();
                          }
                      }
                  }));
            }
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      Book book = selectedItem as Book;
                      db.Books.Remove(book);
                      db.SaveChanges();
                  }));
            }
        }

        public RelayCommand SaveDb
        {
            get
            {
                return saveDb ??
                    (saveDb = new RelayCommand(obj =>
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        try
                        {
                            if (saveFileDialog.ShowDialog() == true)
                            {
                                StreamWriter sr = new StreamWriter(saveFileDialog.FileName + ".txt");
                                foreach (var book in db.Books.Local.ToBindingList())
                                {
                                    sr.WriteLine(book.Id + " " + book.Author + " " + book.Title + " " + book.Year);
                                }
                                sr.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(":D");
                        }
                    }));
            }
        }
 
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}