using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace Lab1
{
    public partial class MainWindow : Window
    {
        private List<TaxiEntry> taxiEntries;
        private string currentFilePath; // Track the current XML file path

        public MainWindow()
        {
            InitializeComponent();

            taxiEntries = new List<TaxiEntry>();
            dataGrid.ItemsSource = taxiEntries;

            // Subscribe to events
            dataGrid.CellEditEnding += DataGrid_CellEditEnding;
            dataGrid.LoadingRow += DataGrid_LoadingRow;
        }

        private void LoadXml_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(openFileDialog.FileName))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "taxi_entry")
                            {
                                TaxiEntry entry = new TaxiEntry();
                                reader.ReadToFollowing("car_number");
                                entry.CarNumber = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("car_brand");
                                entry.CarBrand = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("start_location");
                                reader.ReadToFollowing("latitude");
                                entry.StartLatitude = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("longitude");
                                entry.StartLongitude = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("destination");
                                reader.ReadToFollowing("latitude");
                                entry.DestLatitude = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("longitude");
                                entry.DestLongitude = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("distance");
                                entry.Distance = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("date");
                                entry.Date = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("time");
                                entry.Time = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("fuel_consumption");
                                entry.FuelConsumption = reader.ReadElementContentAsString();
                                reader.ReadToFollowing("fare");
                                entry.Fare = reader.ReadElementContentAsString();
                                taxiEntries.Add(entry);
                            }
                        }
                    }

                    dataGrid.Items.Refresh();
                    currentFilePath = openFileDialog.FileName; // Update the current file path
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading XML: " + ex.Message);
                }
            }
        }


        private void SaveXml_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                // If the current file path is not set, open a save dialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML files (*.xml)|*.xml";
                if (saveFileDialog.ShowDialog() == true)
                {
                    currentFilePath = saveFileDialog.FileName;
                }
                else
                {
                    return; // User canceled the save operation
                }
            }

            try
            {
                // Создание XML документа
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement rootElement = xmlDoc.CreateElement("taxi_journal");
                xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                xmlDoc.AppendChild(rootElement);

                foreach (TaxiEntry entry in taxiEntries)
                {
                    XmlElement entryElement = xmlDoc.CreateElement("taxi_entry");

                    XmlElement carNumberElement = xmlDoc.CreateElement("car_number");
                    carNumberElement.InnerText = entry.CarNumber;
                    entryElement.AppendChild(carNumberElement);

                    XmlElement carBrandElement = xmlDoc.CreateElement("car_brand");
                    carBrandElement.InnerText = entry.CarBrand;
                    entryElement.AppendChild(carBrandElement);

                    XmlElement startLocationElement = xmlDoc.CreateElement("start_location");
                    XmlElement startLatitudeElement = xmlDoc.CreateElement("latitude");
                    startLatitudeElement.InnerText = entry.StartLatitude;
                    startLocationElement.AppendChild(startLatitudeElement);
                    XmlElement startLongitudeElement = xmlDoc.CreateElement("longitude");
                    startLongitudeElement.InnerText = entry.StartLongitude;
                    startLocationElement.AppendChild(startLongitudeElement);
                    entryElement.AppendChild(startLocationElement);

                    XmlElement destinationElement = xmlDoc.CreateElement("destination");
                    XmlElement destLatitudeElement = xmlDoc.CreateElement("latitude");
                    destLatitudeElement.InnerText = entry.DestLatitude;
                    destinationElement.AppendChild(destLatitudeElement);
                    XmlElement destLongitudeElement = xmlDoc.CreateElement("longitude");
                    destLongitudeElement.InnerText = entry.DestLongitude;
                    destinationElement.AppendChild(destLongitudeElement);
                    entryElement.AppendChild(destinationElement);

                    XmlElement distanceElement = xmlDoc.CreateElement("distance");
                    distanceElement.InnerText = entry.Distance;
                    entryElement.AppendChild(distanceElement);

                    XmlElement dateElement = xmlDoc.CreateElement("date");
                    dateElement.InnerText = entry.Date;
                    entryElement.AppendChild(dateElement);

                    XmlElement timeElement = xmlDoc.CreateElement("time");
                    timeElement.InnerText = entry.Time;
                    entryElement.AppendChild(timeElement);

                    XmlElement fuelConsumptionElement = xmlDoc.CreateElement("fuel_consumption");
                    fuelConsumptionElement.InnerText = entry.FuelConsumption;
                    entryElement.AppendChild(fuelConsumptionElement);

                    XmlElement fareElement = xmlDoc.CreateElement("fare");
                    fareElement.InnerText = entry.Fare;
                    entryElement.AppendChild(fareElement);

                    rootElement.AppendChild(entryElement);
                }

                // Сохранение XML документа
                xmlDoc.Save(currentFilePath);
                MessageBox.Show("XML saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving XML: " + ex.Message);
            }
        }

        private void AddEntry_Click(object sender, RoutedEventArgs e)
        {
            taxiEntries.Add(new TaxiEntry());
            dataGrid.Items.Refresh();
        }

        private void DeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                taxiEntries.Remove((TaxiEntry)dataGrid.SelectedItem);
                dataGrid.Items.Refresh();
            }
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Update the edited value in the underlying list
            var editedEntry = (TaxiEntry)e.Row.Item;
            var column = e.Column as DataGridBoundColumn;
            if (column != null)
            {
                var bindingPath = (column.Binding as System.Windows.Data.Binding).Path.Path;
                var textBox = e.EditingElement as TextBox;
                if (textBox != null)
                {
                    var newValue = textBox.Text.Trim(); // Trim leading and trailing spaces
                    bool isValid = true;

                    if (string.IsNullOrEmpty(newValue))
                    {
                        isValid = false;
                    }
                    else
                    {
                        switch (bindingPath)
                        {
                            case "CarNumber":
                                editedEntry.CarNumber = newValue;
                                break;
                            case "CarBrand":
                                editedEntry.CarBrand = newValue;
                                break;
                            case "StartLatitude":
                                if (!double.TryParse(newValue, out double startLatitude))
                                    isValid = false;
                                else
                                    editedEntry.StartLatitude = startLatitude.ToString();
                                break;
                            case "StartLongitude":
                                if (!double.TryParse(newValue, out double startLongitude))
                                    isValid = false;
                                else
                                    editedEntry.StartLongitude = startLongitude.ToString();
                                break;
                            case "DestLatitude":
                                if (!double.TryParse(newValue, out double destLatitude))
                                    isValid = false;
                                else
                                    editedEntry.DestLatitude = destLatitude.ToString();
                                break;
                            case "DestLongitude":
                                if (!double.TryParse(newValue, out double destLongitude))
                                    isValid = false;
                                else
                                    editedEntry.DestLongitude = destLongitude.ToString();
                                break;
                            case "Distance":
                                if (!double.TryParse(newValue, out double distance))
                                    isValid = false;
                                else
                                    editedEntry.Distance = distance.ToString();
                                break;
                            case "Date":
                                editedEntry.Date = newValue;
                                break;
                            case "Time":
                                editedEntry.Time = newValue;
                                break;
                            case "FuelConsumption":
                                if (!double.TryParse(newValue, out double fuelConsumption))
                                    isValid = false;
                                else
                                    editedEntry.FuelConsumption = fuelConsumption.ToString();
                                break;
                            case "Fare":
                                if (!double.TryParse(newValue, out double fare))
                                    isValid = false;
                                else
                                    editedEntry.Fare = fare.ToString();
                                break;
                        }
                    }

                    // Highlight the cell in red if the data is invalid
                    if (!isValid)
                    {
                        textBox.Background = new SolidColorBrush(Colors.Red);
                        e.Cancel = true; // Cancel the edit operation
                        MessageBox.Show("Invalid input. Please enter a non-empty numeric value.");
                    }
                    else
                    {
                        textBox.ClearValue(TextBox.BackgroundProperty);
                    }
                }
            }
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // Set the data grid row header to the row index
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }


    }

}